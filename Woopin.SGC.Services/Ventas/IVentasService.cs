using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Services
{
    public interface IVentasService
    {
        #region ComprobanteVenta
        void AddComprobanteVenta(ComprobanteVenta Comprobante);
        void AddComprobanteVentaNT(ComprobanteVenta ComprobanteVenta);
        ComprobanteVenta GetComprobanteVenta(int Id);
        ComprobanteVenta GetComprobanteVentaCompleto(int Id);
        IList<ComprobanteVenta> GetAllComprobantesVentas();
        string GetProximoComprobante(string Letra, int TipoComprobante, int Talonario);
        IList<ComprobanteVenta> GetComprobantesVentasACobrar(int IdCliente);
        IList<ComprobanteVenta> GetAllComprobantesVentasByCliente(int IdCliente, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, Model.Common.CuentaCorrienteFilter filter);
        void AnularComprobante(int IdComprobante);
        void UpdateObservacion(int IdComprobante, string Observacion);
        void UpdateEnvioEmail(int IdComprobante);
        void EliminarComprobante(int IdComprobante);
        void SetearCAEComprobanteVenta(int IdComprobante, string CAE, string Vencimiento);
        IList<ComprobanteVenta> GetAllByClienteFilterNC(int? IdCliente, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Cobrada);

        #endregion

        #region Cobranza
        void AddCobranza(Cobranza Cobranza);
        IList<Cobranza> GetAllCobranzas();
        Cobranza GetCobranza(int Id);
        string GetProximoRecibo(string talonario);
        int GetProximoIdCobranza();
        Cobranza GetCobranzaCompleto(int Id);
        IList<Cobranza> GetAllCobranzaByCliente(int IdCliente, DateTime? start, DateTime? end);

        void AnularCobranza(int IdCobranza);

        #endregion

        #region CuentaCorriente
        VentasCuentaCorriente LoadCtaCorrienteHead(int id, DateTime? start, DateTime? end, Model.Common.CuentaCorrienteFilter filter);
        List<CuentaCorrienteItem> GetCuentaCorrienteByDates(DateTime? start, DateTime? end, int id, Model.Common.CuentaCorrienteFilter filter);
        #endregion

        #region Imputaciones
        void AddImputacion(ImputacionVenta Imputacion);
        void AddImputacionNT(ImputacionVenta Imputacion);
        void AddImputaciones(IList<ImputacionVenta> Imputaciones);
        ImputacionVenta GetImputacion(int Id);
        IList<ImputacionVenta> GetAllImputaciones();
        ImputacionVenta GetImputacionCompleto(int Id);
        void UpdateImputacion(ImputacionVenta Imputacion);
        IList<ImputacionVenta> GetAllImputacionesByCliente(int Id, DateTime? start, DateTime? end);
        void AnularImputacion(int IdImputacion);
        void DeleteImputacion(int Id);
        void DeleteImputacionNT(int Id);

        #endregion


        
    }
}
