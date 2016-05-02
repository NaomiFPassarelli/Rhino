using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Reporting;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Services
{
    public interface IReportingService
    {
        
        #region Grupo Ingreso
        void AddGrupoIngreso(GrupoIngreso GrupoIngreso);
        GrupoIngreso GetGrupoIngreso(int Id);
        IList<GrupoIngreso> GetAllGruposIngresosTree(DateTime start, DateTime end);
        void UpdateGrupoIngreso(GrupoIngreso GrupoIngreso);

        SelectCombo GetAllGruposIngresosNoHoja(SelectComboRequest req);
        void DeleteGrupoIngreso(int Id);
        #endregion

        #region Grupo Egreso
        void AddGrupoEgreso(GrupoEgreso GrupoEgreso);
        GrupoEgreso GetGrupoEgreso(int Id);
        IList<GrupoEgreso> GetAllGruposEgresosTree(DateTime start, DateTime end);
        void UpdateGrupoEgreso(GrupoEgreso GrupoEgreso);

        SelectCombo GetAllGruposEgresosNoHoja(SelectComboRequest req);
        void DeleteGrupoEgreso(int Id);
        #endregion

    }
}
