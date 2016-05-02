using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public interface IBloqueoContableRepository : IRepository<BloqueoContable>
    {
        IList<BloqueoContable> GetByDates(DateTime start, DateTime end);
    }
}
