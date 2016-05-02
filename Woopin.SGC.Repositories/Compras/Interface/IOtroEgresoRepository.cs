using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Repositories.Compras
{
    public interface IOtroEgresoRepository : IRepository<OtroEgreso>
    {

        int GetProximoNumeroReferencia();

        OtroEgreso GetCompleto(int Id);

        IList<OtroEgreso> GetAllByProveedor(int IdProveedor, DateTime _start, DateTime _end);
    }
}
