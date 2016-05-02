using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Repositories.Compras
{
    public interface IComprobanteCompraRepository : IRepository<ComprobanteCompra>
    {

        int GetProximoNumeroReferencia();
        ComprobanteCompra GetCompleto(int Id);
        IList<ComprobanteCompra> GetAllByProveedor(int IdProveedor, DateTime _start, DateTime _end, DateTime _startvenc, DateTime _endvenc, Model.Common.CuentaCorrienteFilter filter);
        IList<CuentaCorrienteItem> GetAllAcumulados(int IdProveedor, int IdRubro, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter);
        ComprasCuentaCorriente LoadCtaCorrienteHead(int id, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter);
        ComprasCuentaCorriente AcumuladosHead(int id,int idRubro, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter);
        List<CuentaCorrienteItem> GetCuentaCorrienteByDates(int id, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter);
        IList<ComprobanteCompra> GetAllAPagarByProv(int IdProveedor);
        IList<ReporteComprasRubros> GetReporteRubros(int IdProveedor, DateTime start, DateTime end);
        IList<ReporteCompra> GetVencimientosAPagar();
        ComprobanteCompra GetByLetrayNumero(string LetraYNumero);
        IList<ComprobanteCompra> GetAllByProvFilterNC(int IdProveedor, int Tipo, int NoTipo, ComprobantesACancelarFilter Pagada);
        ComprobanteCompra GetComprobanteByInfo(int IdProveedor, string Letra, string Numero, int tipoComprobante);
        IList<ReporteCitiItem> GetCitiCompras(DateTime start, DateTime end);
    }
}
