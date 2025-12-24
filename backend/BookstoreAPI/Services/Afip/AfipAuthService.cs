using BookstoreAPI.Models.Afip;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace BookstoreAPI.Services.Afip
{
    public class AfipAuthService : IAfipAuthService
    {
        private readonly AfipConfig _config;
        private readonly ILogger<AfipAuthService> _logger;
        private static AfipTicketAcceso? _cachedTicket;
        private static readonly object _lock = new object();
        private const string TA_FILE = "ta.xml";

        public AfipAuthService(IOptions<AfipConfig> config, ILogger<AfipAuthService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task<AfipTicketAcceso> GetTicketAccesoAsync()
        {
            // Intentar cargar desde archivo si existe
            if (_cachedTicket == null && File.Exists(TA_FILE))
            {
                try
                {
                    _cachedTicket = CargarCredencialesDesdeArchivo();
                    _logger.LogInformation("TA cargado desde archivo. Expira: {ExpirationTime}", _cachedTicket.ExpirationTime);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "No se pudo cargar TA desde archivo");
                }
            }

            lock (_lock)
            {
                if (_cachedTicket != null && _cachedTicket.IsValid())
                {
                    _logger.LogInformation("Usando ticket de acceso cacheado");
                    return _cachedTicket;
                }
            }

            _logger.LogInformation("Solicitando nuevo ticket de acceso a AFIP");

            try
            {
                // Generar Login Ticket Request
                var loginTicketRequest = GenerateLoginTicketRequest();
                _logger.LogInformation("LoginTicketRequest generado (longitud: {Length}): {Request}", loginTicketRequest.Length, loginTicketRequest);

                // Cargar certificado
                var cert = LoadCertificateFromPfx(_config.PfxPath, _config.PfxPassword);
                _logger.LogInformation("Certificado cargado. Subject: {Subject}, Expira: {NotAfter}, HasPrivateKey: {HasPrivateKey}",
                    cert.Subject, cert.NotAfter, cert.HasPrivateKey);

                // Convertir el Login Ticket Request a bytes, firmar y convertir a Base64
                Encoding encodedMsg = Encoding.UTF8;
                byte[] msgBytes = encodedMsg.GetBytes(loginTicketRequest);
                byte[] encodedSignedCms = FirmaBytesMensaje(msgBytes, cert);
                var cmsFirmadoBase64 = Convert.ToBase64String(encodedSignedCms);

                _logger.LogInformation("LoginTicketRequest firmado exitosamente. Tamaño base64: {Size} caracteres", cmsFirmadoBase64.Length);

                // Enviar a WSAA y obtener respuesta
                var ticket = await SendToWSAAAsync(cmsFirmadoBase64);

                // Guardar en archivo
                GuardarCredencialesEnArchivo(ticket);

                lock (_lock)
                {
                    _cachedTicket = ticket;
                }

                return ticket;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ticket de acceso de AFIP");
                throw;
            }
        }

        private AfipTicketAcceso CargarCredencialesDesdeArchivo()
        {
            XmlDocument taDoc = new XmlDocument();
            taDoc.Load(TA_FILE);

            string token = taDoc.SelectSingleNode("//token")?.InnerText ?? throw new Exception("Token no encontrado en TA");
            string sign = taDoc.SelectSingleNode("//sign")?.InnerText ?? throw new Exception("Sign no encontrado en TA");
            string expirationText = taDoc.SelectSingleNode("//expirationTime")?.InnerText ?? throw new Exception("ExpirationTime no encontrado en TA");
            string generationText = taDoc.SelectSingleNode("//generationTime")?.InnerText ?? throw new Exception("GenerationTime no encontrado en TA");

            return new AfipTicketAcceso
            {
                Token = token,
                Sign = sign,
                ExpirationTime = DateTime.Parse(expirationText, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                GenerationTime = DateTime.Parse(generationText, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
            };
        }

        private void GuardarCredencialesEnArchivo(AfipTicketAcceso ticket)
        {
            try
            {
                var xml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<loginTicketResponse>
    <header>
        <source>CN=wsaahomo, O=AFIP, C=AR, SERIALNUMBER=CUIT 33693450239</source>
        <destination>SERIALNUMBER=CUIT {_config.CUIT}</destination>
        <uniqueId>0</uniqueId>
        <generationTime>{ticket.GenerationTime:s}</generationTime>
        <expirationTime>{ticket.ExpirationTime:s}</expirationTime>
    </header>
    <credentials>
        <token>{ticket.Token}</token>
        <sign>{ticket.Sign}</sign>
    </credentials>
</loginTicketResponse>";

                File.WriteAllText(TA_FILE, xml);
                _logger.LogInformation("TA guardado en archivo {File}", TA_FILE);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo guardar TA en archivo");
            }
        }

        public static byte[] FirmaBytesMensaje(byte[] argBytesMsg, X509Certificate2 argCertFirmante)
        {
            try
            {
                // Poner el mensaje en un objeto ContentInfo (requerido para construir SignedCms)
                ContentInfo infoContenido = new ContentInfo(argBytesMsg);
                SignedCms cmsFirmado = new SignedCms(infoContenido);

                // Crear objeto CmsSigner con las características del firmante
                CmsSigner cmsFirmante = new CmsSigner(argCertFirmante);
                cmsFirmante.IncludeOption = X509IncludeOption.EndCertOnly;

                // Firmar el mensaje PKCS #7
                cmsFirmado.ComputeSignature(cmsFirmante);

                // Encodear el mensaje PKCS #7
                return cmsFirmado.Encode();
            }
            catch (Exception excepcionAlFirmar)
            {
                throw new Exception("Error al firmar: " + excepcionAlFirmar.Message, excepcionAlFirmar);
            }
        }

        public static X509Certificate2 LoadCertificateFromPfx(string rutaPfx, string password)
        {
            var cert = new X509Certificate2(rutaPfx, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);

            if (!cert.HasPrivateKey)
            {
                throw new Exception("El certificado PFX no contiene la clave privada");
            }

            return cert;
        }

        private static int _globalUniqueID = 0;

        private static DateTime GetBuenosAiresTime()
        {
            var buenosAiresZone = TimeZoneInfo.FindSystemTimeZoneById("America/Buenos_Aires");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, buenosAiresZone);
        }

        private string GenerateLoginTicketRequest()
        {
            string xmlTemplate = $@"<loginTicketRequest><header><source>SERIALNUMBER=CUIT {_config.CUIT}, CN=prueba2025</source><destination>CN=wsaahomo, O=AFIP, C=AR, SERIALNUMBER=CUIT 33693450239</destination><uniqueId></uniqueId><generationTime></generationTime><expirationTime></expirationTime></header><service></service></loginTicketRequest>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlTemplate);

            _globalUniqueID += 1;

            var xmlNodoUniqueId = xmlDoc.SelectSingleNode("//uniqueId") ?? throw new Exception("No se encontró nodo uniqueId");
            var xmlNodoGenerationTime = xmlDoc.SelectSingleNode("//generationTime") ?? throw new Exception("No se encontró nodo generationTime");
            var xmlNodoExpirationTime = xmlDoc.SelectSingleNode("//expirationTime") ?? throw new Exception("No se encontró nodo expirationTime");
            var xmlNodoService = xmlDoc.SelectSingleNode("//service") ?? throw new Exception("No se encontró nodo service");

            var buenosAiresNow = GetBuenosAiresTime();
            xmlNodoGenerationTime.InnerText = buenosAiresNow.AddMinutes(-10).ToString("s");
            xmlNodoExpirationTime.InnerText = buenosAiresNow.AddMinutes(+10).ToString("s");          

            xmlNodoUniqueId.InnerText = Convert.ToString(_globalUniqueID);
            xmlNodoService.InnerText = "wsfe";

            var xml = xmlDoc.OuterXml;
            _logger.LogInformation("XML generado: {Xml}", xml);
            return xml;
        }

        private async Task<AfipTicketAcceso> SendToWSAAAsync(string signedRequestBase64)
        {
            try
            {
                using var client = new HttpClient();

                var soapRequest = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:wsaa=""http://wsaa.view.sua.dvadac.desein.afip.gov"">
<soapenv:Header/>
<soapenv:Body>
    <wsaa:loginCms>
        <wsaa:in0>{System.Security.SecurityElement.Escape(signedRequestBase64)}</wsaa:in0>
    </wsaa:loginCms>
</soapenv:Body>
</soapenv:Envelope>";

                _logger.LogInformation("Enviando request a WSAA: {Url}", _config.WsaaUrl);

                var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                content.Headers.Add("SOAPAction", "");

                var response = await client.PostAsync(_config.WsaaUrl, content);
                var responseXml = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Respuesta de WSAA (StatusCode: {StatusCode})", response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error en respuesta WSAA: {StatusCode} - {Response}", response.StatusCode, responseXml);
                    throw new Exception($"Error en WSAA: {response.StatusCode} - {responseXml}");
                }

                _logger.LogInformation("Respuesta exitosa de WSAA");
                return ParseWSAAResponse(responseXml);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar request a WSAA");
                throw;
            }
        }

        private AfipTicketAcceso ParseWSAAResponse(string responseXml)
        {
            try
            {
                _logger.LogInformation("Parseando respuesta WSAA");

                // Extraer el TA (es un XML en string)
                var doc = new XmlDocument();
                doc.LoadXml(responseXml);
                var taNode = doc.GetElementsByTagName("loginCmsReturn")[0];

                if (taNode == null || string.IsNullOrEmpty(taNode.InnerText))
                {
                    _logger.LogError("No se encontró loginCmsReturn en la respuesta. XML: {Xml}", responseXml);
                    throw new Exception("No se pudo obtener el TA");
                }

                var credentialsXml = taNode.InnerText;
                _logger.LogInformation("TA XML extraído exitosamente");

                // Parsear el XML del TA
                var credentialsDoc = new XmlDocument();
                credentialsDoc.LoadXml(credentialsXml);

                var token = credentialsDoc.SelectSingleNode("//token")?.InnerText ?? throw new Exception("Token no encontrado");
                var sign = credentialsDoc.SelectSingleNode("//sign")?.InnerText ?? throw new Exception("Sign no encontrado");
                var expirationTime = credentialsDoc.SelectSingleNode("//expirationTime")?.InnerText ?? throw new Exception("ExpirationTime no encontrado");
                var generationTime = credentialsDoc.SelectSingleNode("//generationTime")?.InnerText ?? throw new Exception("GenerationTime no encontrado");

                _logger.LogInformation("Ticket de acceso parseado exitosamente. Expira: {ExpirationTime}", expirationTime);

                return new AfipTicketAcceso
                {
                    Token = token,
                    Sign = sign,
                    ExpirationTime = DateTime.Parse(expirationTime, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                    GenerationTime = DateTime.Parse(generationTime, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al parsear respuesta WSAA");
                throw new Exception("Error al parsear respuesta de WSAA", ex);
            }
        }
    }
}
