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
    public interface ICooperativaConfigService
    {
        #region Asociado
        void AddAsociado(Asociado Asociado);
        void AddAsociadoNT(Asociado Asociado);
        Asociado GetAsociado(int Id);
        Asociado GetAsociadoCompleto(int Id);
        IList<Asociado> GetAsociados(IList<int> Ids);
        void UpdateAsociado(Asociado Asociado);
        IList<Asociado> GetAllAsociados();
        void DeleteAsociados(List<int> Ids);
        SelectCombo GetAllAsociadosByFilterCombo(SelectComboRequest req);
        SelectCombo GetAsociadoCombos();
        bool ExistCUITNT(string cuit, int? IdUpdate);
        void ImportAsociado(Asociado c);
        void ImportAsociadoNT(Asociado c);
        void ImportAsociados(List<Asociado> Asociados);
        IList<Asociado> GetAsociadosMes(int Mes, int Año);
        IList<Asociado> GetAsociadosMesEgreso(int Mes, int Año);
        void BajarAsociado(Asociado Asociado);
        void ActualizarAltaAsociados(Asociado Asociado, int Mes, int Año);
        void ActualizarBajaAsociados(Asociado Asociado, int Mes, int Año);
        //Asociado LoadHeader();
        int GetProximoNumeroReferencia();

        #endregion

        #region Concepto
        void AddConcepto(Concepto Concepto);
        void AddConceptoNT(Concepto Concepto);
        Concepto GetConcepto(int Id);
        void UpdateConcepto(Concepto Concepto);
        IList<Concepto> GetAllConceptos();
        void DeleteConceptos(List<int> Ids);
        SelectCombo GetAllConceptosByFilterCombo(SelectComboRequest req);
        SelectCombo GetConceptoCombos();
        //void AddConceptoConConceptos(Concepto Concepto, IList<Concepto> Conceptos);
        #endregion

        #region AdicionalPago
        void AddAdicionalPago(AdicionalPago AdicionalPago);
        void AddAdicionalPagoNT(AdicionalPago AdicionalPago);
        AdicionalPago GetAdicionalPago(int Id);
        AdicionalPago GetAdicionalPagoNT(int Id);
        //void UpdateAdicionalPago(AdicionalPago AdicionalPago, IList<AdicionalAdicionales> Adicionales = null);
        //IList<AdicionalPago> GetAllAdicionalPagoes();
        void DeleteAdicionalPagos(List<int> Ids);
        //SelectCombo GetAllAdicionalPagoesByFilterCombo(SelectComboRequest req);
        //SelectCombo GetAdicionalPagoCombos();
        //void AddAdicionalPagoConAdicionalPagoes(AdicionalPago AdicionalPago, IList<AdicionalPago> AdicionalPagoes);
        //IList<AdicionalPago> GetAdicionalesDelPeriodoByEmpleado(string Periodo, int IdEmpleado);

        #endregion


    }
}
