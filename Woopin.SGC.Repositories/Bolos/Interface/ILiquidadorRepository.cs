using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.Repositories.Bolos
{
    public interface ILiquidadorRepository : IRepository<Liquidador>
    {
        IList<Liquidador> GetAllByFilter(SelectComboRequest req);
        int GetProximoNumeroReferencia();
        IList<Liquidador> GetAllByFilter(PagingRequest req);
        Liquidador GetCompleto(int Id);
        Liquidador GetLiquidadorAnterior();
        IList<Liquidador> GetLiquidadores(IList<int> Ids);

        IList<Liquidador> GetAllByDates(DateTime _start, DateTime _end);
        //IList<Liquidador> GetAllByBolo(int IdBolo, DateTime start, DateTime end, DateTime startvenc, DateTime endvenc, Model.Common.CuentaCorrienteFilter filter);
        IList<Liquidador> GetAllByBolo(int IdBolo, DateTime start, DateTime end);

        //decimal GetMejorRemuneracion(int IdEmpleado);
        //decimal[] GetPromedioRemunerativo(int IdEmpleado);
    }
}
