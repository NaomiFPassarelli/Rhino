using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.Services
{
    public interface ISueldosConfigService
    {
        #region Empleado
        void AddEmpleado(Empleado Empleado);
        void AddEmpleadoNT(Empleado Empleado);
        Empleado GetEmpleado(int Id);
        void UpdateEmpleado(Empleado Empleado);
        IList<Empleado> GetAllEmpleados();
        void DeleteEmpleados(List<int> Ids);
        SelectCombo GetAllEmpleadosByFilterCombo(SelectComboRequest req);
        SelectCombo GetEmpleadoCombos();
        bool ExistCUITNT(string cuit, int? IdUpdate);
        void ImportEmpleado(Empleado c);
        void ImportEmpleadoNT(Empleado c);
        void ImportEmpleados(List<Empleado> Empleados);
        #endregion

        #region Adicional
        void AddAdicional(Adicional Adicional);
        void AddAdicionalNT(Adicional Adicional);
        Adicional GetAdicional(int Id);
        Adicional GetAdicionalNT(int Id);
        void UpdateAdicional(Adicional Adicional, IList<AdicionalAdicionales> Adicionales = null);
        IList<Adicional> GetAllAdicionales();
        void DeleteAdicionales(List<int> Ids);
        SelectCombo GetAllAdicionalesByFilterCombo(SelectComboRequest req);
        SelectCombo GetAdicionalCombos();
        void AddAdicionalConAdicionales(Adicional Adicional, IList<Adicional> Adicionales);
        #endregion


        #region AdicionalAdicionales
        void AddAdicionalAdicionales(AdicionalAdicionales AdicionalAdicionales);
        void AddAdicionalAdicionalesNT(AdicionalAdicionales AdicionalAdicionales);
        IList<AdicionalAdicionales> GetAllAdicionalAdicionaleses();
        AdicionalAdicionales GetAdicionalAdicionales(int Id);
        IList<AdicionalAdicionales> GetAdicionalAdicionalesByAdicional(int Id);
        //void UpdateAdicionalAdicionales(AdicionalAdicionales AdicionalAdicionales);
        //void DeleteAdicionalAdicionaleses(List<int> Ids);
        //SelectCombo GetAllAdicionalAdicionalesesByFilterCombo(SelectComboRequest req);
        //SelectCombo GetAdicionalAdicionalesCombos();
        #endregion



    }
}
