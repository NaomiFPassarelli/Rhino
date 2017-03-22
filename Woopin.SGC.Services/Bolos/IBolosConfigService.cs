using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.Services
{
    public interface IBolosConfigService
    {
        #region Trabajador
        void AddTrabajador(Trabajador Trabajador);
        void AddTrabajadorNT(Trabajador Trabajador);
        Trabajador GetTrabajador(int Id);
        //Trabajador GetTrabajadorCompleto(int Id);
        void UpdateTrabajador(Trabajador Trabajador);
        IList<Trabajador> GetAllTrabajadores();
        int GetProximoNumeroReferencia();

        void DeleteTrabajadores(List<int> Ids);
        SelectCombo GetAllTrabajadoresByFilterCombo(SelectComboRequest req);
        SelectCombo GetTrabajadorCombos();
        bool ExistCUITNT(string cuit, int? IdUpdate);
        void ImportTrabajador(Trabajador c);
        void ImportTrabajadorNT(Trabajador c);
        void ImportTrabajadores(List<Trabajador> Trabajadores);

        #endregion

        #region Escalafon
        void AddEscalafon(Escalafon Escalafon);
        Escalafon GetEscalafon(int Id);
        //Escalafon GetEscalafonCompleto(int Id);
        void UpdateEscalafon(Escalafon Escalafon);
        IList<Escalafon> GetAllEscalafones();

        void DeleteEscalafones(List<int> Ids);
        //SelectCombo GetAllEscalafonesByFilterCombo(SelectComboRequest req);
        //SelectCombo GetEscalafonCombos();

        #endregion


        //#region Adicional
        //void AddAdicional(Adicional Adicional);
        //void AddAdicionalNT(Adicional Adicional);
        //Adicional GetAdicional(int Id, int IdSindicato, bool OnlyManual);
        //void UpdateAdicional(Adicional Adicional, IList<AdicionalAdicionales> Adicionales = null);
        //IList<Adicional> GetAllAdicionales();
        //void DeleteAdicionales(List<int> Ids);
        //SelectCombo GetAllAdicionalesByFilterCombo(SelectComboRequest req, int IdSindicato, bool OnlyManual);
        //SelectCombo GetAdicionalCombos();
        //void AddAdicionalConAdicionales(Adicional Adicional, IList<Adicional> Adicionales);
        //#endregion

        //#region AdicionalRecibo
        //void AddAdicionalRecibo(AdicionalRecibo AdicionalRecibo);
        //void AddAdicionalReciboNT(AdicionalRecibo AdicionalRecibo);
        //AdicionalRecibo GetAdicionalRecibo(int Id);
        //AdicionalRecibo GetAdicionalReciboNT(int Id);
        ////void UpdateAdicionalRecibo(AdicionalRecibo AdicionalRecibo, IList<AdicionalAdicionales> Adicionales = null);
        ////IList<AdicionalRecibo> GetAllAdicionalReciboes();
        //void DeleteAdicionalRecibos(List<int> Ids);
        ////SelectCombo GetAllAdicionalReciboesByFilterCombo(SelectComboRequest req);
        ////SelectCombo GetAdicionalReciboCombos();
        ////void AddAdicionalReciboConAdicionalReciboes(AdicionalRecibo AdicionalRecibo, IList<AdicionalRecibo> AdicionalReciboes);
        //IList<AdicionalRecibo> GetAdicionalesDelPeriodoByEscalafon(string Periodo, int IdEscalafon);
        
        //#endregion



        //#region AdicionalAdicionales
        //void AddAdicionalAdicionales(AdicionalAdicionales AdicionalAdicionales);
        //void AddAdicionalAdicionalesNT(AdicionalAdicionales AdicionalAdicionales);
        //IList<AdicionalAdicionales> GetAllAdicionalAdicionaleses();
        //AdicionalAdicionales GetAdicionalAdicionales(int Id);
        //IList<AdicionalAdicionales> GetAdicionalAdicionalesByAdicional(int Id);
        ////void UpdateAdicionalAdicionales(AdicionalAdicionales AdicionalAdicionales);
        ////void DeleteAdicionalAdicionaleses(List<int> Ids);
        ////SelectCombo GetAllAdicionalAdicionalesesByFilterCombo(SelectComboRequest req);
        ////SelectCombo GetAdicionalAdicionalesCombos();
        //#endregion


        #region Empresa
        void AddEmpresa(Empresa Empresa);
        Empresa GetEmpresa(int Id);
        //void UpdateEmpresa(Empresa Empresa);
        //IList<Empresa> GetAllEmpresas();
        //void DeleteEmpresas(List<int> Ids);
        #endregion

        #region ConceptoBolo
        void AddConceptoBolo(ConceptoBolo ConceptoBolo);
        ConceptoBolo GetConceptoBolo(int Id);
        void UpdateConceptoBolo(ConceptoBolo ConceptoBolo);
        IList<ConceptoBolo> GetAllConceptosBolo();
        void DeleteConceptosBolo(List<int> Ids);
        #endregion


        #region TrabajadorBoloEscalafon
        void AddTrabajadorBoloEscalafon(TrabajadorBoloEscalafon TrabajadorBoloEscalafon);
        TrabajadorBoloEscalafon GetTrabajadorBoloEscalafon(int Id);
        void UpdateTrabajadorBoloEscalafon(TrabajadorBoloEscalafon TrabajadorBoloEscalafon);
        IList<TrabajadorBoloEscalafon> GetAllTrabajadoresBoloEscalafon();
        void DeleteTrabajadoresBoloEscalafon(List<int> Ids);
        #endregion


        #region Bolo
        void AddBolo(Bolo Bolo);
        Bolo GetBolo(int Id);
        void UpdateBolo(Bolo Bolo);
        IList<Bolo> GetAllBolos();
        void DeleteBolos(List<int> Ids);
        SelectCombo GetAllBolosByFilterCombo(SelectComboRequest req);
        #endregion


    }
}
