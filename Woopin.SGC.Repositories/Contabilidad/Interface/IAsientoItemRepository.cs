using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public interface IAsientoItemRepository : IRepository<AsientoItem>
    {
        IList<LibroIVA> GetLibroIVACompras(DateTime start, DateTime end);

        IList<LibroIVA> GetLibroIVAVentas(DateTime start, DateTime end);

        IList<MayorItem> GetAllMayorProveedores(DateTime start, DateTime end);

        IList<MayorItem> GetAllMayorClientes(DateTime start, DateTime end);

        IList<SumaSaldo> GetAllSumasYSaldos( DateTime start, DateTime end);

        IList<SumaSaldo> GetAllSumasYSaldosTree(DateTime start, DateTime end);

        IList<SumaSaldo> GetBalance(DateTime start, DateTime end);
    }
}
