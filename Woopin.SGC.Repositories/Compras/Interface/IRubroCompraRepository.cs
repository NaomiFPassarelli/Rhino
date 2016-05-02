using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Repositories.Compras
{
    public interface IRubroCompraRepository : IRepository<RubroCompra>
    {
        IList<RubroCompra> GetAllByFilter(PagingRequest req);
        IList<RubroCompra> GetAllByFilter(SelectComboRequest req);
        IList<RubroCompra> GetAllSinPerceByFilter(SelectComboRequest req);
    }
}
