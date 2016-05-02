using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.Repositories.Stock
{
    public interface IRubroArticuloRepository : IRepository<RubroArticulo>
    {
        IList<RubroArticulo> GetAllByFilter(SelectComboRequest req);
        IList<RubroArticulo> GetAllByFilter(PagingRequest req);
    }
}
