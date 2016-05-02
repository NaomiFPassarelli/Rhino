using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public interface IDepositoRepository : IRepository<Deposito>
    {
        int GetProximoNumeroReferencia();

        Deposito GetDepositoCompleto(int Id);

        IList<Deposito> GetDepositoFilter(int idCuentaBancaria, DateTime start, DateTime end);

    }
}
