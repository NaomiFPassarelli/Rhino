using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public interface IChequePropioRepository : IRepository<ChequePropio>
    {
        IList<ChequePropio> GetAllByFilter(int IdProveedor, int IdCuenta, DateTime start, DateTime end, FilterCheque filter);

        IList<ChequePropio> GetAllInChequera(int IdCuentaBancaria, int NumeroDesde, int NumeroHasta);

        ChequePropio GetByFilter(int IdCuentaBancaria, int Numero);

    }
}
