using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.Repositories.Cooperativa
{
    public interface IActaRepository : IRepository<Acta>
    {
        Acta GetActaByFecha(DateTime endOfMonth);
        Acta GetActaCompleta(int ActaId);
        IList<Acta> GetAllActasCompletas();
        IList<Acta> GetActas(IList<int> Ids);

    }
}
