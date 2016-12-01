using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.Services
{
    public interface ICooperativaService
    {
        #region Pago
        //void AddPagos(Pago Pago, int CantidadCuotasAPagar);
        void AddPago(Pago Pago);
        void AddPagoNT(Pago Pago);
        Pago GetPago(int Id);
        IList<Pago> GetPagos(IList<int> Ids);
        IList<Pago> GetAllPagos(DateTime? start, DateTime? end);
        IList<Asociado> GetAllPorVencer();
        IList<Asociado> GetAllVencidos();
        void DeletePagos(List<int> Ids);
        //SelectCombo GetAllPagosByFilterCombo(SelectComboRequest req);
        //SelectCombo GetPagoCombos();
        IList<Pago> GetAllPagosByAsociado(int IdAsociado);
        int GetProximoNumeroReferenciaPago();

        #endregion

        #region Aporte
        void AddAportes(Aporte Aporte, int CantidadCuotasAPagar);
        void AddAporte(Aporte Aporte);
        void AddAporteNT(Aporte Aporte);
        Aporte GetAporte(int Id);
        IList<Aporte> GetAllAportes(DateTime? start, DateTime? end);
        //IList<Asociado> GetAllPorVencer();
        //IList<Asociado> GetAllVencidos();
        void DeleteAportes(List<int> Ids);
        //SelectCombo GetAllAportesByFilterCombo(SelectComboRequest req);
        //SelectCombo GetAporteCombos();
        IList<Aporte> GetAllAportesByAsociado(int IdAsociado);
        int GetProximoNumeroReferenciaAporte();

        #endregion


        #region Acta
        void AddActa(Acta Acta);
        Acta GetActa(int Id);
        IList<Acta> GetActas(IList<int> Ids);

        IList<Acta> GetAllActas();
        IList<Acta> GetAllActasCompletas();
        Acta GetActaCompleta(int ActaId);
        Acta GetActaByFecha(DateTime endOfMonth);
        IList<ActaPuntoExtra> GetActaPuntoExtraByActa(int ActaId);
        void DeleteActas(List<int> Ids);

        #endregion

    }
}
