using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public interface IAsientoRepository : IRepository<Asiento>
    {

        int GetProximoIdAsiento();

        Asiento GetCompleto(int Id);

        IList<Asiento> GetAsientosFilter( DateTime _start, DateTime _end);

        IList<LibroMayor> GetAsientosFilterCuenta(int id, DateTime _start, DateTime _end);

        LibroMayorHeader GetAsientosHeaderFilterCuenta(int id, DateTime _start, DateTime _end);

    }
}
