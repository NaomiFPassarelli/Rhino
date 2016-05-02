using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Services
{
    public interface ITesoreriaReportService
    {
        IList<ReporteTesoreria> GetIngresosByDates(DateTime start, DateTime end);
        IList<ReporteTesoreria> GetEgresosByDates(DateTime start, DateTime end);
        IList<ComprobanteRetencion> GetComprobantesRetencionesByDates(DateTime start, DateTime end);
    }
}
