using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public interface IPagoTarjetaRepository : IRepository<PagoTarjeta>
    {
        IList<PagoTarjeta> GetAllByDates(int Id, DateTime _start, DateTime _end, PagoTarjetaFilter filter);
        PagoTarjeta GetCompleto(int Id);
    }
}
