using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public interface IChequeraRepository : IRepository<Chequera>
    {
        IList<Chequera> FindChequera(int CuentaBancaria, int NumeroDesde, int NumeroHasta);

        Chequera ControlChequePropioChequera(int IdCuentaBancaria, int Numero);
        
    }
}
