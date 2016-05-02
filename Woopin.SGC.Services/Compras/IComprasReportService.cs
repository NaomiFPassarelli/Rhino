using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Services
{
    public interface IComprasReportService
    {
        IList<ReporteCompra> GetVencimientosAPagar();
        IList<ReporteComprasRubros> GetReporteRubros(int IdProveedor, DateTime start, DateTime end);
        IList<ReporteCitiItem> GetCitiCompras(DateTime from, DateTime to);
    }
}
