using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.Repositories.Bolos
{
    public interface IEscalafonRepository : IRepository<Escalafon>
    {
        IList<Escalafon> GetAllByFilter(SelectComboRequest req);
        IList<Escalafon> GetAllByFilter(PagingRequest req);
        //Escalafon GetCompleto(int IdEscalafon);
    }
}
