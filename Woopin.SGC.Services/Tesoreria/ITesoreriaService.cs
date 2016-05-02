using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Services
{
    public interface ITesoreriaService
    {
        #region MovimientoFondo
        void AddMovimientoFondo(MovimientoFondo MovimientoFondo);
        MovimientoFondo GetMovimientoFondo(int Id);
        void UpdateMovimientoFondo(MovimientoFondo MovimientoFondo);
        IList<MovimientoFondo> GetAllMovimientosFondos();
        IList<MovimientoFondo> GetAllMovimientosFondosByDates(DateTime? start, DateTime? end);
        #endregion

        #region Historiales Caja y Banco
        IList<HistorialCaja> GetAllHistorialCajaByDates(int IdCaja, DateTime? start, DateTime? end);
        IList<HistorialCuentaBancaria> GetAllHistorialCuentasByDates(int Id,DateTime? start, DateTime? end);
        #endregion

        #region Cheque
        void AddCheque(Cheque Cheque);
        Cheque GetCheque(int Id);
        void UpdateCheque(Cheque Cheque);
        IList<Cheque> GetAllCheques();
        IList<Cheque> GetAllChequesEnCartera();
        IList<Cheque> GetChequeFilter(int IdCliente, int IdBanco, DateTime? start, DateTime? end, FilterCheque filter);
        #endregion

        #region ChequePropio
        void AddChequePropio(ChequePropio ChequePropio);
        ChequePropio GetChequePropio(int Id);
        void UpdateChequePropio(ChequePropio ChequePropio);
        IList<ChequePropio> GetAllChequesPropios();
        IList<ChequePropio> GetChequePropioFilter(int IdProveedor, int IdCuenta, DateTime? start, DateTime? end,FilterCheque filter);
        void CanjeCheque(int IdAnterior, ChequePropio NuevoCheque);
        void DeleteChequesPropios(List<int> Ids);
        void AnularChequePropio(int IdCuentaBancaria, int Numero);
        void ConfirmarPagoChequePropio(int IdChequePropio, DateTime fechaPago);
        #endregion

        #region Transferencia
        void AddTransferencia(Transferencia t);
        IList<Transferencia> GetAllTransferencias();
        void UpdateTransferencia(Transferencia transferencia);
        Transferencia GetTransferencia(int Id);
        IList<Transferencia> GetTransferenciaFilter(int IdCuentaBancaria, int IdProveedor, int IdCliente, DateTime? start, DateTime? end);
        #endregion

        #region Pago con Tarjeta
        void AddPagoTarjeta(PagoTarjeta p);
        IList<PagoTarjeta> GetAllPagoTarjetas();
        void UpdatePagoTarjeta(PagoTarjeta p);
        PagoTarjeta GetPagoTarjeta(int Id);
        PagoTarjeta GetPagoTarjetaCompleta(int Id);
        IList<PagoTarjeta> GetAllPagoTarjetasByDates(int Id, DateTime? start, DateTime? end, PagoTarjetaFilter filter);
        #endregion

        #region Cancelacion de Tarjeta
        void AddCancelacionTarjeta(CancelacionTarjeta c);
        IList<CancelacionTarjeta> GetAllCancelacionTarjetas();
        void UpdateCancelacionTarjeta(CancelacionTarjeta p);
        CancelacionTarjeta GetCancelacionTarjeta(int Id);
        #endregion

        #region Ingreso de Valores
        void RegistrarIngresosNT(IList<ValorIngresado> Valores, TipoIngreso tipo, string Concepto, string NumeroRetencion);
        void GetCuentaContableForValor(ValorIngresado valor, TipoIngreso tipo);
        Caja RegistrarMovCajaNT(int IdCaja, decimal Monto, string Concepto);
        void CancelarIngresosNT(IList<ValorIngresado> valores, TipoIngreso tipoIngreso, string Concepto);
        #endregion

        #region Deposito
        void AddDeposito(Deposito Deposito);
        Deposito GetDeposito(int Id);
        void UpdateDeposito(Deposito Deposito);
        IList<Deposito> GetDepositoFilter(int idCuentaBancaria, DateTime? start, DateTime? end);
        int GetProximoNumeroReferenciaDeposito();

        Deposito GetDepositoCompleto(int Id);
        #endregion

        #region Comprobante de Retenciones
        ComprobanteRetencion GetComprobanteRetencion(int Id);
        IList<ComprobanteRetencion> GetAllComprobanteRetenciones();
        void AddComprobanteRetencion(ComprobanteRetencion ComprobanteRetencion);
        void UpdateComprobanteRetencion(ComprobanteRetencion ComprobanteRetencion);
        IList<ComprobanteRetencion> GetRetencionFilter(int TipoRetencion, int IdProveedor, int IdCliente, DateTime? start, DateTime? end);
        IList<ComprobanteRetencionReporte> GetRetencionFilterReporte(int TipoRetencion, int IdProveedor, int IdCliente, DateTime? start, DateTime? end);
        
        #endregion



    }
}
