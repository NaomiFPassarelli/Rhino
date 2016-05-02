using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Services
{
    public interface IContabilidadConfigService
    {
        #region Cuentas
        void AddCuenta(Cuenta cuenta);
        void AddCuentaNT(Cuenta cuenta);
        Cuenta GetCuenta(int Id);
        IList<Cuenta> GetAllCuentas();
        IList<Cuenta> GetRubros();
        IList<Cuenta> GetCorrientes(int Rubro);
        IList<Cuenta> GetSubRubros(int Rubro, int Corriente);
        SelectCombo GetCuentaIngresosCombo();
        SelectCombo GetCuentaEgresosCombo();
        IList<Cuenta> GetSubRubrosEgresosCombo();
        void UpdateCuenta(Cuenta cuenta);
        SelectCombo GetAllCuentasByFilterCombo(SelectComboRequest req);

        #endregion

        #region EjerciciosContables
        void AddEjercicio(Ejercicio ejercicio);
        Ejercicio GetEjercicio(int Id);
        IList<Ejercicio> GetAllEjercicios();
        IList<Ejercicio> GetAllAvailableEjercicios();
        void UpdateEjercicio(Ejercicio ejercicio);
        
        void DeleteEjercicio(int Ids);
        Ejercicio GetEjercicioCompleto(int Id);
        void EjercicioCambiarCerrado(int Id, bool p);

        #endregion

        #region Bloqueos
        void AddBloqueoContable(BloqueoContable bloqueo);
        void UpdateBloqueoContable(int Id, bool Cerrado);

        #endregion

        #region Retenciones
        Retencion GetRetencion(int Id);
        void AddRetencion(Retencion Retencion);
        void UpdateRetencion(Retencion Retencion);
        IList<Retencion> GetAllRetenciones();
        IList<Retencion> GetAllRetencionesValor();
        void DeleteRetenciones(List<int> Ids);
        SelectCombo GetRetencionCombos(SelectComboRequest req);
        #endregion


        
    }
}
