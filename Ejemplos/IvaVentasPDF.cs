using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Data;

namespace BL.Informes
{
    public class IvaVentasPDF
    {
        private static DataTable data;

        static string archivo = "archivo.pdf";
        public static void Generar(Dictionary<string,object> parameters)
        {
            IvaVentasService ivaVentasService = new IvaVentasService();

            data = ivaVentasService.GetData(parameters);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Element(GenerarHeader());
                    page.Content().Element(GenerarDetalle());

                });
            }).GeneratePdf(archivo);

            ReportHelper.MostrarPdf(archivo);
        }

        private static Action<IContainer> GenerarDetalle() => container =>
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();

                });

                table.Header(header =>
                {
                    header.Cell().BorderBottom(1).Padding(5).Text("Fecha").Bold();
                    header.Cell().BorderBottom(1).Padding(5).Text("Numero").Bold();
                    header.Cell().BorderBottom(1).Padding(5).Text("Cliente").Bold();
                    header.Cell().BorderBottom(1).Padding(5).Text("Cuit").Bold();
                    header.Cell().BorderBottom(1).Padding(5).Text("Debitos").Bold();
                    header.Cell().BorderBottom(1).Padding(5).Text("Creditos").Bold();
                });

                if(data.Rows.Count < 1)
                    table.Cell().ColumnSpan(5).Element(CellStyleBody).PaddingTop(2).Text($"No se encontro informacion!");

                int cnt = 0;
                int cantidad = 49;
                for (int i = 1; i < data.Rows.Count; i++)
                {
                    var debito = 0M;
                    var credito = 0M;

                    debito = (decimal)data.Rows[i]["total"];
                    
                    if (cnt == cantidad)
                    {
                        table.Cell().Text($"");
                        table.Cell().Text($"");
                        table.Cell().Text($"Transporte");
                        table.Cell().Text($"{debito}");
                        table.Cell().Text($"{debito}");
                        table.Cell().Text($"{credito}");
                        cnt = 0;
                        cantidad = 48;
                    } else
                    {
                        table.Cell().Element(CellStyleBody).Text($"{((DateTime)data.Rows[i]["fecha"]).ToString("dd/MM/yyyy")}");
                        table.Cell().Element(CellStyleBody).Text($"{data.Rows[i]["numeroComprobante"].ToString()}");
                        table.Cell().Element(CellStyleBody).Text($"{data.Rows[i]["Nombre"].ToString()}");
                        table.Cell().Element(CellStyleBody).Text($"{data.Rows[i]["NroDocumento"].ToString()}");
                        table.Cell().Element(CellStyleBody).Text($"{debito}");
                        table.Cell().Element(CellStyleBody).Text($"{credito}");
                        cnt++;
                    }
                }

            });
        };

        private static Action<IContainer> GenerarHeader() => container =>
        {
            container.Column(column =>
            {
                column.Item().BorderTop(2).BorderBottom(2).Row(row =>
                {
                    row.RelativeItem(80).Padding(2)
                        .Column(c =>
                        {
                            c.Item().Text("EDICIONES LIBER de Roberto Passarelli y Marcos E. Passarelli S.H").FontSize(12);
                            c.Item().Text("Av. Asamblea 1442 P 7 Dto 20 - CP: C1406HVR - C.A.B.A.");
                            c.Item().Text("C.U.I.T. 30-71417888-8    LIBRO DE I.V.A - VENTAS");
                        });

                    row.RelativeItem(20).Padding(2)
                        .AlignRight()
                        .Column(c =>
                        {
                            c.Item().Text(text =>
                            {
                                text.Span("Pagina:");
                                text.CurrentPageNumber();
                            });
                        });
                });               
            });
        };
        static IContainer CellStyleBody(IContainer container) =>
        container
            .DefaultTextStyle(x => x.FontSize(9));

    }
}
