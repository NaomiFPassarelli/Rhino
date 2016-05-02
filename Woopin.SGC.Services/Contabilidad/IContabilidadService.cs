using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Services
{
    public interface IContabilidadService
    {
        #region Asientos

        Asiento NuevoAsientoVentaNT(ComprobanteVenta comprobante);
        Asiento NuevoAsientoCompraNT(ComprobanteCompra Comprobante);
        Asiento NuevoAsientoMovimientoFondos(MovimientoFondo movimiento);
        Asiento NuevoAsientoOP(OrdenPago op);
        Asiento NuevoAsientoOE(OtroEgreso op);
        Asiento NuevoAsientoCancelacionTarjeta(CancelacionTarjeta cancelacion);
        Asiento NuevoAsientoCobranza(Cobranza Cobranza);
        Asiento NuevoAsientoDeposito(Deposito Deposito);
        Asiento NuevoAsientoCanjeCheques(ChequePropio newCheque, ChequePropio chequeAnterior);


        void AddAsiento(Asiento asiento);
        int GetProximoIdAsiento();
        Asiento GetAsientoCompleto(int Id);
        IList<Asiento> GetAsientosFilter( DateTime? start, DateTime? end);

        IList<LibroMayor> GetAsientosFilterCuenta(int id, DateTime? start, DateTime? end);

        LibroMayorHeader GetAsientosHeaderFilterCuenta(int id, DateTime? start, DateTime? end);
        void UpdateAsiento(Asiento Asiento);
        void UpdateAsientoNT(Asiento Asiento);
        void DeleteAsiento(int Id);
        void DeleteAsientoNT(int Id);
        #endregion

        #region Controles de Ejercicios Contables
        bool ControlarIngresoNT(DateTime fechaContable);
        bool ControlarIngreso(DateTime fechaContable);
        void TryControlarIngresoNT(DateTime fechaContable);

        #endregion

    }
}
