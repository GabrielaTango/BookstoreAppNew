using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Informes
{
    public  class IvaVentasService
    {
        private readonly QueryService qService = new QueryService("");

        public DataTable GetData(Dictionary<string,object> parameters)
        {
            string sql = @"SELECT comp.fecha, comp.numeroComprobante, cli.Nombre,cli.NroDocumento, comp.total
                            FROM comprobantes comp
                            INNER JOIN comprobante_detalle cd
                            ON comp.id = cd.factura_id
                            INNER JOIN clientes cli
                            ON cli.id = comp.cliente_id
                            @parameters
                            ORDER BY comp.fecha asc

            ";

            if (parameters != null)
                sql = sql.Replace("@parameters", "WHERE fecha between @fechaDesde AND @fechaHasta");


            return qService.EjecutarQuery(sql, parameters);
        }
    }
}
