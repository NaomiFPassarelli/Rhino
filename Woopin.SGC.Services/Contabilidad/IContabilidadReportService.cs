using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Services
{
    public interface IContabilidadReportService
    {
        IList<LibroIVA> GetAllLibroIVACompras(DateTime start, DateTime end);
        IList<LibroIVA> GetAllLibroIVAVentas(DateTime start, DateTime end);
        IList<MayorItem> GetAllMayorProveedores(DateTime start, DateTime end);
        IList<MayorItem> GetAllMayorClientes(DateTime start, DateTime end);

        IList<SumaSaldo> GetAllSumasYSaldos(DateTime start, DateTime end);

        IList<SumaSaldo> GetAllSumasYSaldosTree(DateTime start, DateTime end);

        IList<SumaSaldo> GetBalance(DateTime start, DateTime end);
    }
}
