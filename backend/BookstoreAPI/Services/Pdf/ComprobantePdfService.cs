using BookstoreAPI.Models;
using BookstoreAPI.Models.Afip;
using BookstoreAPI.Services.Afip;
using Microsoft.Extensions.Options;
using MySqlX.XDevAPI;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BookstoreAPI.Services.Pdf
{
    public class ComprobantePdfService : IComprobantePdfService
    {
        private readonly AfipConfig _config;
        private readonly IAfipQrService _qrService;
        private readonly ILogger<ComprobantePdfService> _logger;

        public ComprobantePdfService(
            IOptions<AfipConfig> config,
            IAfipQrService qrService,
            ILogger<ComprobantePdfService> logger)
        {
            _config = config.Value;
            _qrService = qrService;
            _logger = logger;

            // Configurar licencia de QuestPDF (Community para uso no comercial)
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerarPdf(Comprobante comprobante, Cliente cliente, List<ComprobanteDetalle> detalles)
        {
            try
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.DefaultTextStyle(x => x.FontSize(10));

                        page.Header().Element(ComposeHeader);
                        page.Content().Element(c => ComposeContent(c, comprobante, cliente, detalles));
                        page.Footer().AlignCenter().Text(text =>
                        {
                            text.Span("Página ");
                            text.CurrentPageNumber();
                            text.Span(" de ");
                            text.TotalPages();
                        });
                    });
                });

                return document.GeneratePdf();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar PDF del comprobante");
                throw;
            }
        }

        private void ComposeHeader(IContainer container)
        {


            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    // Tipo de comprobante
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().PaddingVertical(10).Border(1).Background(Colors.Grey.Lighten3)
                                .Text(DeterminarTipoComprobanteTexto("")).FontSize(16).Bold();
                        });
                    });
                    column.Item().Width(100).Image("./Images/LiberLogo.png");
                    column.Item().Text($"CUIT: {_config.CUIT}").FontSize(10);
                    column.Item().Text("Dirección: Calle Falsa 123").FontSize(9);
                    column.Item().Text("Tel: (011) 1234-5678").FontSize(9);
                });
            });
        }

        private void ComposeContent(IContainer container, Comprobante comprobante, Cliente cliente, List<ComprobanteDetalle> detalles)
        {
            container.Column(column =>
            {
                column.Item().PaddingVertical(10);

                // Información del comprobante y cliente
                column.Item().Row(row =>
                {
                    // Datos del comprobante
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("DATOS DEL COMPROBANTE").FontSize(12).Bold();
                        col.Item().PaddingTop(5).Text($"Número: {comprobante.NumeroComprobante}");
                        col.Item().Text($"Fecha: {comprobante.Fecha:dd/MM/yyyy}");

                        if (!string.IsNullOrEmpty(comprobante.CAE))
                        {
                            col.Item().Text($"CAE: {comprobante.CAE}");
                            col.Item().Text($"Vto. CAE: {comprobante.VTO:dd/MM/yyyy}");
                        }
                    });

                    // Datos del cliente
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("DATOS DEL CLIENTE").FontSize(12).Bold();
                        col.Item().PaddingTop(5).Text($"Nombre: {cliente.Nombre}");
                        col.Item().Text($"Documento: {cliente.NroDocumento ?? "-"}");
                        col.Item().Text($"Dirección: {cliente.DomicilioComercial ?? cliente.DomicilioParticular ?? "-"}");
                        col.Item().Text($"Teléfono: {cliente.Telefono ?? cliente.TelefonoMovil ?? "-"}");
                        col.Item().Text($"Email: {cliente.EMail ?? "-"}");
                    });
                });

                column.Item().PaddingVertical(15);

                // Tabla de detalles
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(50);    // Cantidad
                        columns.RelativeColumn(3);     // Descripción
                        columns.ConstantColumn(80);    // Precio Unit.
                        columns.ConstantColumn(80);    // Subtotal
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Cant.").Bold();
                        header.Cell().Element(CellStyle).Text("Descripción").Bold();
                        header.Cell().Element(CellStyle).AlignRight().Text("Precio Unit.").Bold();
                        header.Cell().Element(CellStyle).AlignRight().Text("Subtotal").Bold();

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5)
                                .BorderBottom(1).BorderColor(Colors.Black);
                        }
                    });

                    // Detalles
                    foreach (var detalle in detalles)
                    {
                        table.Cell().Element(CellStyle).Text(detalle.Cantidad.ToString());
                        table.Cell().Element(CellStyle).Text($"Artículo ID: {detalle.Articulo_Id}");
                        table.Cell().Element(CellStyle).AlignRight().Text($"${detalle.Precio_Unitario:N2}");
                        table.Cell().Element(CellStyle).AlignRight().Text($"${detalle.Subtotal:N2}");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                        }
                    }
                });

                column.Item().PaddingVertical(10);

                // Totales y QR
                column.Item().Row(row =>
                {
                    // QR Code
                    row.ConstantItem(150).Column(col =>
                    {
                        if (!string.IsNullOrEmpty(comprobante.CAE))
                        {
                            try
                            {
                                var qrBytes = _qrService.GenerarQrBytes(comprobante, cliente);
                                col.Item().Image(qrBytes);
                                col.Item().AlignCenter().Text("Escanear QR para verificar").FontSize(8);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "No se pudo generar QR en PDF");
                                col.Item().Text("QR no disponible");
                            }
                        }
                    });

                    row.RelativeItem();

                    // Totales
                    row.ConstantItem(200).Column(col =>
                    {
                        var subtotal = detalles.Sum(d => d.Subtotal);
                        var iva = comprobante.Total - subtotal;

                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Subtotal:");
                            r.ConstantItem(80).AlignRight().Text($"${subtotal:N2}");
                        });

                        if (iva > 0)
                        {
                            col.Item().Row(r =>
                            {
                                r.RelativeItem().Text("IVA (21%):");
                                r.ConstantItem(80).AlignRight().Text($"${iva:N2}");
                            });
                        }

                        col.Item().PaddingTop(5).Row(r =>
                        {
                            r.RelativeItem().Text("TOTAL:").FontSize(14).Bold();
                            r.ConstantItem(80).AlignRight().Text($"${comprobante.Total:N2}").FontSize(14).Bold();
                        });
                    });
                });
            });
        }

        private string DeterminarTipoComprobanteTexto(string? categoriaIva)
        {
            return categoriaIva?.ToUpper() switch
            {
                "RESPONSABLE INSCRIPTO" => "A",
                "MONOTRIBUTO" => "B",
                "CONSUMIDOR FINAL" => "C",
                "EXENTO" => "B",
                _ => "C"
            };
        }
    }
}
