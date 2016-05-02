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
    public interface IIngresoStockRepository : IRepository<IngresoStock>
    {
        IList<IngresoStock> GetAllByFilter(SelectComboRequest req);
        IList<IngresoStock> GetAllByFilter(PagingRequest req);
        IList<IngresoStock> GetAllByArticulo(int IdArticulo, DateTime _start, DateTime _end);
    }
}
