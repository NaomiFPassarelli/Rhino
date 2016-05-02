using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Repositories.Compras
{
    public interface IOrdenPagoRepository : IRepository<OrdenPago>
    {

        string GetProximoComprobante();

        int GetProximoNumeroReferencia();

        OrdenPago GetCompleto(int Id);

        IList<OrdenPago> GetAllByProveedor(int IdProveedor, DateTime _start, DateTime _end);

    }
}
