using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Repositories.Compras
{
    public interface IImputacionCompraRepository : IRepository<ImputacionCompra>
    {
        ImputacionCompra GetCompleto(int Id);
        IList<ImputacionCompra> GetAllByProveedor(int Id, DateTime start, DateTime end);
        IList<ImputacionCompra> GetAllByComprobante(int IdComprobante);
    }
}
