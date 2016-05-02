using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public interface IComprobanteVentaRepository : IRepository<ComprobanteVenta>
    {
        string GetProximoComprobante(string Letra, int TipoComprobante, int Talonario);

        ComprobanteVenta GetByLetrayNumero(string LetraYNumero);

        ComprobanteVenta GetComprobanteVentaCompleto(int Id);

        #region Find by Filters
        IList<ComprobanteVenta> GetComprobantesVentasACobrar(int IdCliente);
        IList<ComprobanteVenta> GetAllByCliente(int IdCliente, DateTime _start, DateTime _end, DateTime _startvenc, DateTime _endvenc, Model.Common.CuentaCorrienteFilter filter);
        IList<ComprobanteVenta> GetAllByClienteFilterNC(int _idCliente, int _tipo, int _notipo, ComprobantesACancelarFilter Cobrada);
        #endregion

        #region CuentaCorriente
        VentasCuentaCorriente LoadCtaCorrienteHead(int id, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter);
        List<CuentaCorrienteItem> GetCuentaCorrienteByDates(int id, DateTime _start, DateTime _end, CuentaCorrienteFilter filter);
        #endregion

        #region Reportes
        IList<ReporteVenta> GetVencimientosACobrar();
        IList<ReporteVentasArticulo> GetReporteVentasArticulo(int IdCliente, DateTime start, DateTime end);
        IList<Cliente> GetAllClientesDeudores();
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
        #endregion







    }
}
