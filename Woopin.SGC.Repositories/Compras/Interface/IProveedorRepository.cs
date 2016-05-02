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
    public interface IProveedorRepository : IRepository<Proveedor>
    {
        IList<Proveedor> GetAllByFilter(SelectComboRequest req);
        IList<Proveedor> GetAllByFilter(PagingRequest req);
        bool ExistCUIT(string cuit, int? IdUpdate);
    }
}
