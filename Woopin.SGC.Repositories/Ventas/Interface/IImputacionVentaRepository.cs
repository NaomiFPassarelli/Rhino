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
    public interface IImputacionVentaRepository : IRepository<ImputacionVenta>
    {
        ImputacionVenta GetCompleto(int Id);
        IList<ImputacionVenta> GetAllByCliente(int Id, DateTime start, DateTime end);
        IList<ImputacionVenta> GetAllByComprobante(int IdComprobante);
    }
}
