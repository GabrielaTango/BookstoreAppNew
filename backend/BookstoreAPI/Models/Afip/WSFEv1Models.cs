using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Afip.WsfeV1
{
    public static class Ns
    {
        public const string Soap = "http://www.w3.org/2003/05/soap-envelope";
        public const string Ar = "http://ar.gov.afip.dif.FEV1/";
    }

    // Root Envelope
    [XmlRoot("Envelope", Namespace = Ns.Soap)]
    public class Envelope
    {
        [XmlElement("Header", Namespace = Ns.Soap)]
        public Header Header { get; set; } = new Header();

        [XmlElement("Body", Namespace = Ns.Soap)]
        public Body Body { get; set; }
    }

    public class Header
    {
        // Vacío según el ejemplo
    }

    public class Body
    {
        [XmlElement("FECAESolicitar", Namespace = Ns.Ar)]
        public FECAESolicitar FECAESolicitar { get; set; }
    }

    public class FECAESolicitar
    {
        [XmlElement("Auth", Namespace = Ns.Ar)]
        public Auth Auth { get; set; }

        [XmlElement("FeCAEReq", Namespace = Ns.Ar)]
        public FeCAEReq FeCAEReq { get; set; }
    }

    public class Auth
    {
        [XmlElement("Token", Namespace = Ns.Ar)]
        public string Token { get; set; }

        [XmlElement("Sign", Namespace = Ns.Ar)]
        public string Sign { get; set; }

        [XmlElement("Cuit", Namespace = Ns.Ar)]
        public string Cuit { get; set; }
    }

    public class FeCAEReq
    {
        [XmlElement("FeCabReq", Namespace = Ns.Ar)]
        public FeCabReq FeCabReq { get; set; }

        [XmlElement("FeDetReq", Namespace = Ns.Ar)]
        public FeDetReq FeDetReq { get; set; }
    }

    public class FeCabReq
    {
        [XmlElement("CantReg", Namespace = Ns.Ar)]
        public int? CantReg { get; set; }

        [XmlElement("PtoVta", Namespace = Ns.Ar)]
        public int? PtoVta { get; set; }

        [XmlElement("CbteTipo", Namespace = Ns.Ar)]
        public int? CbteTipo { get; set; }
    }

    public class FeDetReq
    {
        // Zero or more FECAEDetRequest
        [XmlElement("FECAEDetRequest", Namespace = Ns.Ar)]
        public List<FECAEDetRequest> FECAEDetRequest { get; set; } = new List<FECAEDetRequest>();
    }

    public class FECAEDetRequest
    {
        [XmlElement("Concepto", Namespace = Ns.Ar)]
        public int? Concepto { get; set; }

        [XmlElement("DocTipo", Namespace = Ns.Ar)]
        public int? DocTipo { get; set; }

        // DocNro puede ser grande -> usar string o long?; uso string para evitar pérdidas con guiones/puntos
        [XmlElement("DocNro", Namespace = Ns.Ar)]
        public string DocNro { get; set; }

        [XmlElement("CbteDesde", Namespace = Ns.Ar)]
        public long? CbteDesde { get; set; }

        [XmlElement("CbteHasta", Namespace = Ns.Ar)]
        public long? CbteHasta { get; set; }

        // Fecha como string (AFIP: usualmente "yyyyMMdd")
        [XmlElement("CbteFch", Namespace = Ns.Ar)]
        public string CbteFch { get; set; }

        [XmlElement("ImpTotal", Namespace = Ns.Ar)]
        public string ImpTotal { get; set; }

        [XmlElement("ImpTotConc", Namespace = Ns.Ar)]
        public decimal? ImpTotConc { get; set; }

        [XmlElement("ImpNeto", Namespace = Ns.Ar)]
        public string ImpNeto { get; set; }

        [XmlElement("ImpOpEx", Namespace = Ns.Ar)]
        public decimal? ImpOpEx { get; set; }

        [XmlElement("ImpTrib", Namespace = Ns.Ar)]
        public decimal? ImpTrib { get; set; }

        [XmlElement("ImpIVA", Namespace = Ns.Ar)]
        public decimal? ImpIVA { get; set; }

        [XmlElement("FchServDesde", Namespace = Ns.Ar)]
        public string FchServDesde { get; set; }

        [XmlElement("FchServHasta", Namespace = Ns.Ar)]
        public string FchServHasta { get; set; }

        [XmlElement("FchVtoPago", Namespace = Ns.Ar)]
        public string FchVtoPago { get; set; }

        [XmlElement("MonId", Namespace = Ns.Ar)]
        public string MonId { get; set; }

        [XmlElement("MonCotiz", Namespace = Ns.Ar)]
        public decimal? MonCotiz { get; set; }

        [XmlElement("CanMisMonExt", Namespace = Ns.Ar)]
        public string CanMisMonExt { get; set; } 

        [XmlElement("CondicionIVAReceptorId", Namespace = Ns.Ar)]
        public int? CondicionIVAReceptorId { get; set; }

        // CbtesAsoc -> zero or more CbteAsoc
        [XmlArray("CbtesAsoc", Namespace = Ns.Ar)]
        [XmlArrayItem("CbteAsoc", Namespace = Ns.Ar)]
        public List<CbteAsoc> CbtesAsoc { get; set; }

        // Tributos -> zero or more Tributo
        [XmlArray("Tributos", Namespace = Ns.Ar)]
        [XmlArrayItem("Tributo", Namespace = Ns.Ar)]
        public List<Tributo> Tributos { get; set; } 

        // Iva -> zero or more AlicIva
        [XmlArray("Iva", Namespace = Ns.Ar)]
        [XmlArrayItem("AlicIva", Namespace = Ns.Ar)]
        public List<AlicIva> Iva { get; set; }

        // Opcionales -> zero or more Opcional
        [XmlArray("Opcionales", Namespace = Ns.Ar)]
        [XmlArrayItem("Opcional", Namespace = Ns.Ar)]
        public List<Opcional> Opcionales { get; set; } 

        // Compradores -> zero or more Comprador
        [XmlArray("Compradores", Namespace = Ns.Ar)]
        [XmlArrayItem("Comprador", Namespace = Ns.Ar)]
        public List<Comprador> Compradores { get; set; } 

        // PeriodoAsoc
        [XmlElement("PeriodoAsoc", Namespace = Ns.Ar)]
        public PeriodoAsoc PeriodoAsoc { get; set; }

        // Actividades -> zero or more Actividad
        [XmlArray("Actividades", Namespace = Ns.Ar)]
        [XmlArrayItem("Actividad", Namespace = Ns.Ar)]
        public List<Actividad> Actividades { get; set; } 
    }

    public class CbteAsoc
    {
        [XmlElement("Tipo", Namespace = Ns.Ar)]
        public int? Tipo { get; set; }

        [XmlElement("PtoVta", Namespace = Ns.Ar)]
        public int? PtoVta { get; set; }

        [XmlElement("Nro", Namespace = Ns.Ar)]
        public long? Nro { get; set; }

        [XmlElement("Cuit", Namespace = Ns.Ar)]
        public string Cuit { get; set; }

        [XmlElement("CbteFch", Namespace = Ns.Ar)]
        public string CbteFch { get; set; }
    }

    public class Tributo
    {
        [XmlElement("Id", Namespace = Ns.Ar)]
        public int? Id { get; set; }

        [XmlElement("Desc", Namespace = Ns.Ar)]
        public string Desc { get; set; }

        [XmlElement("BaseImp", Namespace = Ns.Ar)]
        public decimal? BaseImp { get; set; }

        [XmlElement("Alic", Namespace = Ns.Ar)]
        public decimal? Alic { get; set; }

        [XmlElement("Importe", Namespace = Ns.Ar)]
        public decimal? Importe { get; set; }
    }

    public class AlicIva
    {
        [XmlElement("Id", Namespace = Ns.Ar)]
        public int? Id { get; set; }

        [XmlElement("BaseImp", Namespace = Ns.Ar)]
        public decimal? BaseImp { get; set; }

        [XmlElement("Importe", Namespace = Ns.Ar)]
        public decimal? Importe { get; set; }
    }

    public class Opcional
    {
        [XmlElement("Id", Namespace = Ns.Ar)]
        public int? Id { get; set; }

        [XmlElement("Valor", Namespace = Ns.Ar)]
        public string Valor { get; set; }
    }

    public class Comprador
    {
        [XmlElement("DocTipo", Namespace = Ns.Ar)]
        public int? DocTipo { get; set; }

        [XmlElement("DocNro", Namespace = Ns.Ar)]
        public string DocNro { get; set; }

        [XmlElement("Porcentaje", Namespace = Ns.Ar)]
        public decimal? Porcentaje { get; set; }
    }

    public class PeriodoAsoc
    {
        [XmlElement("FchDesde", Namespace = Ns.Ar)]
        public string FchDesde { get; set; }

        [XmlElement("FchHasta", Namespace = Ns.Ar)]
        public string FchHasta { get; set; }
    }

    public class Actividad
    {
        [XmlElement("Id", Namespace = Ns.Ar)]
        public int? Id { get; set; }
    }

    // --- Serializer helper ---
    public static class SoapXmlHelper
    {
        public static string Serialize(Envelope envelope, bool omitXmlDeclaration = true)
        {
            var serializer = new XmlSerializer(typeof(Envelope));

            var ns = new XmlSerializerNamespaces();
            ns.Add("soap", Ns.Soap);
            ns.Add("ar", Ns.Ar);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitXmlDeclaration,
                Encoding = new UTF8Encoding(false),
                Indent = true
            };

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, settings))
            {
                serializer.Serialize(writer, envelope, ns);
            }

            return sb.ToString();
        }

        public static Envelope Deserialize(string xml)
        {
            var serializer = new XmlSerializer(typeof(Envelope));
            using var sr = new System.IO.StringReader(xml);
            return (Envelope)serializer.Deserialize(sr);
        }
    }
}
