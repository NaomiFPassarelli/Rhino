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
    public interface IArticuloRepository : IRepository<Articulo>
    {
        IList<Articulo> GetAllByFilter(SelectComboRequest req);
        IList<Articulo> GetAllByFilter(PagingRequest req);
        IList<Articulo> GetAllConStockByFilter(SelectComboRequest req);
        Articulo GetConStock(int IdArticulo);
        IList<Articulo> GetAllByRubro(int IdRubro);
    }
}
