using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Reporting;

namespace Woopin.SGC.Repositories.Reporting
{
    public interface IGrupoEgresoRepository : IRepository<GrupoEgreso>
    {

        IList<GrupoEgreso> GetAllTree(DateTime start, DateTime end);
        IList<GrupoEgreso> GetAllGruposEgresosNoHoja(SelectComboRequest req);
        int GetLastRaiz();

        IList<GrupoEgreso> GetAllHijos(int Id);
    }
}
