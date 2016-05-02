using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Services
{
    public interface IComprasService
    {
        #region ComprobanteCompra
        void AddComprobanteCompra(ComprobanteCompra Comprobante);
        ComprobanteCompra GetComprobanteCompra(int Id);
        IList<ComprobanteCompra> GetAllComprobantesCompras();
        IList<ComprobanteCompra> GetAllComprobantesCompraByProveedor(int Id, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, Model.Common.CuentaCorrienteFilter filter);
        IList<ComprobanteCompra> GetAllComprobanteCompraAPagarByProv(int IdProveedor);
        ComprobanteCompra GetComprobanteCompraCompleto(int Id);
        int GetProximoNumeroReferencia();
        void UpdateComprobanteCompra(ComprobanteCompra Comprobante);
        void UpdateComprobanteCompraNT(ComprobanteCompra Comprobante);
        void AnularComprobante(int IdComprobante);
        IList<CuentaCorrienteItem> GetAllCCAcumulados(int IdProveedor, int IdRubro, DateTime? start, DateTime? end, Model.Common.CuentaCorrienteFilter filter);
        
        void EliminarComprobante(int IdComprobante);
        IList<ComprobanteCompra> GetAllByProvFilterNC(int? IdProveedor, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Pagada);
        
        #endregion

        #region Cuenta Corriente
        IList<CuentaCorrienteItem> GetCuentaCorrienteByDates(DateTime? start, DateTime? end, int id, Model.Common.CuentaCorrienteFilter filter);
        ComprasCuentaCorriente LoadCtaCorrienteHead(int id, DateTime? start, DateTime? end, Model.Common.CuentaCorrienteFilter filter);
        ComprasCuentaCorriente AcumuladosHead(int id, int idRubro, DateTime? start, DateTime? end, Model.Common.CuentaCorrienteFilter filter);
        #endregion

        #region OrdenPago
        void AddOrdenPago(OrdenPago OrdenPago);
        OrdenPago GetOrdenPago(int Id);
        IList<OrdenPago> GetAllOrdenPagos();
        IList<OrdenPago> GetAllOrdenPagoByProveedor(int Id, DateTime? start, DateTime? end);
        IList<OrdenPago> GetAllOrdenPagoByDates(DateTime? start, DateTime? end);
        string GetProximoOrdenPago();
        OrdenPago GetOrdenPagoCompleto(int Id);
        int GetOrdenPagoProximoNumeroReferencia();
        void CancelarOrdenPago(int IdOrdenPago);
        #endregion

        #region Otro Egreso
        void AddOtroEgreso(OtroEgreso OtroEgreso);
        OtroEgreso GetOtroEgreso(int Id);
        IList<OtroEgreso> GetAllOtroEgresos();
        IList<OtroEgreso> GetAllOtroEgresoByProveedor(int Id, DateTime? start, DateTime? end);
        OtroEgreso GetOtroEgresoCompleto(int Id);
        int GetOtroEgresoProximoNumeroReferencia();

        void AnularOE(int IdOE);
        #endregion

        #region Imputaciones

        void AddImputacion(ImputacionCompra Imputacion);
        void AddImputacionNT(ImputacionCompra Imputacion);
        void AddImputaciones(IList<ImputacionCompra> Imputaciones);
        ImputacionCompra GetImputacion(int Id);
        IList<ImputacionCompra> GetAllImputaciones();
        ImputacionCompra GetImputacionCompleto(int Id);
        void UpdateImputacion(ImputacionCompra Imputacion);
        IList<ImputacionCompra> GetAllImputacionesByProveedor(int Id, DateTime? start, DateTime? end);
        void AnularImputacion(int IdImputacion);
        void DeleteImputacion(int Id);
        void DeleteImputacionNT(int Id);
        
        #endregion

    }
}
