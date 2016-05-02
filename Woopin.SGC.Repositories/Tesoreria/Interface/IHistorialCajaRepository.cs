using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public interface IHistorialCajaRepository : IRepository<HistorialCaja>
    {
        IList<HistorialCaja> GetAllByDates(int IdCaja,DateTime _start, DateTime _end);
    }
}
