using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Reporting;

namespace Woopin.SGC.Repositories.Reporting
{
    public interface IGrupoIngresoRepository : IRepository<GrupoIngreso>
    {

        IList<GrupoIngreso> GetAllTree(DateTime start, DateTime end);
        IList<GrupoIngreso> GetAllGruposIngresosNoHoja(SelectComboRequest req);
        int GetLastRaiz();

        IList<GrupoIngreso> GetAllHijos(int Id);

    }
}
