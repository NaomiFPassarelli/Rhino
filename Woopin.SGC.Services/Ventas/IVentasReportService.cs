using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Services
{
    public interface IVentasReportService
    {
        IList<ReporteVenta> GetVencimientosACobrar();
        IList<ReporteVentasArticulo> GetReporteVentasArticulo(int IdCliente, DateTime start, DateTime end);
        IList<Cliente> GetClientesDeudores();
        IList<ComprobanteVenta> GetAllComprobantesPendientes();

        /// <summary>
        /// Busca los resultados de los comprobantes para mostrar el reporte citi ventas.
        /// </summary>
        /// <returns>Listado de ReporteCitiItem filtrado</returns>
        IList<ReporteCitiItem> GetCitiVentas(DateTime start, DateTime end);

        /// <summary>
        /// Devuelve un listado de los clientes con las ventas acumuladas en el periodo.
        /// </summary>
        /// <param name="start">Fecha de inicio del filtro</param>
        /// <param name="end">Fecha de fin del filtro</param>
        /// <returns>Listado de ReporteAcumulado</returns>
        IList<ReporteAcumulado> GetVentasPorClientes(DateTime start, DateTime end);
    }
}
