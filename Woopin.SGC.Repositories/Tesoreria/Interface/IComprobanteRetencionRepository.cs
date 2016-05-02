using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public interface IComprobanteRetencionRepository : IRepository<ComprobanteRetencion>
    {
        IList<ComprobanteRetencion> GetRetencionFilter(int TipoRetencion, int IdProveedor, int IdCliente, DateTime start, DateTime end);

        IList<ComprobanteRetencionReporte> GetRetencionFilterReporte(int TipoRetencion, int IdProveedor, int IdCliente, DateTime start, DateTime end);
    }
}
