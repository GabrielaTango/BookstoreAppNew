using Models;
using Models.Comprobantes;
using Mysqlx.Crud;
using MySqlX.XDevAPI.Relational;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using System.Globalization;


namespace BL.Informes
{
    public class CuponDto
    {
        public string Cliente { get; set; }
        public int NumeroCuota { get; set; }
        public decimal Monto { get; set; }
        public DateTime Vencimiento { get; set; }
    }

    public class CuponReportBuilder
    {
        static string archivo = "archivo.pdf";

        private static EntidadService _provinService = new EntidadService("provincias");
        private static EntidadService _zonaService = new EntidadService("zonas");
        private static EntidadService _vendedorService = new EntidadService("vendedores");

        private int fontSize = 12;

        public static void GenerarCuponesPDF(Cliente cliente, Comprobante comp,List<Cuotas> cuotas)
        {

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(GenerarHeader(cliente, comp, cuotas));
                    page.Content().Element(GenerarDetalle(cliente, comp, cuotas));

                });
            }).GeneratePdfAndShow(); //.GeneratePdf(archivo);

            //ReportHelper.MostrarPdf(archivo);
        }

        private static Action<IContainer> GenerarDetalle(Cliente cliente, Comprobante comp, List<Cuotas> cuotas) => container =>
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                //for(int i=0;i < 10; i++)
                int nro = 1;
                foreach (var cuota in cuotas)
                {
                    RenderCuponCell(table,cliente, comp, cuota, nro);
                    nro++;
                }


                    //for(int i=0; i < 10/4; i++)
                    //    RenderEmptyCell(table);
                });
        };

        private static Action<IContainer> GenerarHeader(Cliente cliente,Comprobante comp, List<Cuotas> cuotas) => container =>
        {
            container.Column(column =>
            {
                column.Item().Border(1).Padding(10).Row(row =>
                {
                    row.RelativeItem()
                        .Column(c =>
                        {
                            c.Item().Text(text =>
                            {
                                text.Span($"Cliente: {cliente.Nombre} - {cliente.Id}").Bold();
                            });
                            c.Item().Text($"Domicilio Com: {cliente.Nombre}").FontSize(12);
                            c.Item().Text($"Dirección Part: {cliente.DomicilioComercial}");
                            c.Item().Text($"{cliente.Telefono} -  {_provinService.GetById(cliente.Provincia_Id.Value).Descripcion}");
                        });

                    row.RelativeItem()
                        .AlignRight()
                        .Column(c =>
                        {
                            c.Item().Text($"Zona: {_zonaService.GetById(cliente.Zona_Id.Value).Descripcion}");
                            c.Item().Text($"Vendedor: {_vendedorService.GetById(cliente.Vendedor_Id.Value).Descripcion}");
                            c.Item().Text($"Tel.:: {cliente.Telefono}");
                            c.Item().Text($"Factura {comp.NumeroComprobante}");
                         
                        });
                });

                column.Item().Border(1).Padding(1).Text($"Cuota Inicial ${comp.Anticipo:N2} y {cuotas.Count} Cuotas de ${cuotas.First().Importe:N2}").FontSize(12).Bold().AlignCenter();
            });
        };

        private static void RenderCuponCell(TableDescriptor table, Cliente cliente, Comprobante comp, Cuotas cuota, int nro)
        {
            table.Cell().Padding(0).Element(container =>
            {
                container.Border(1).Padding(2).Column(c =>
                {
                    c.Spacing(0);
                    c.Item().Border(1).Image("logo.png");
                    c.Item().Text($"Sr/a: {cliente.Nombre}");
                    c.Item().Text($"Mes: {cuota.Fecha.ToString("MM/yyyy")}");
                    c.Item().Row(row =>
                    {
                        row.RelativeItem(70)
                            .Column(c =>
                            {
                                c.Item().Text($"Cuota:${cuota.Importe:N2}");
                            });
                        row.RelativeItem(30)
                            .Column(c =>
                            {
                                c.Item().AlignRight().Padding(4).Text($"{nro}").FontSize(14);
                            });
                    });
                });
            });
        }

        //private static void RenderEmptyCell(TableDescriptor table)
        //{
        //    table.Cell().Padding(0).Element(container =>
        //    {
        //        container.Border(1).Padding(0)
        //                 .MinHeight(60); // algo de altura para mantener la fila
        //    });
        //}
    }
}
