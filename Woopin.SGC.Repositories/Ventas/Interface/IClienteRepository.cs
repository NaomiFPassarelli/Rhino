using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        IList<Cliente> GetAllByFilter(SelectComboRequest req);
        IList<Cliente> GetAllByFilter(PagingRequest req);
        bool ExistCUIT(string cuit, int? IdUpdate);
    }
}
