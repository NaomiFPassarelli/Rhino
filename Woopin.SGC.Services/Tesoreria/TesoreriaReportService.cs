using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Contabilidad;
using Woopin.SGC.Repositories.Tesoreria;

namespace Woopin.SGC.Services
{
    public class TesoreriaReportService : ITesoreriaReportService
    {
        #region Variables y Constructor

        private readonly IValorIngresadoRepository ValorIngresadoRepository;
        private readonly IComprobanteRetencionRepository ComprobanteRetencionRepository;

        public TesoreriaReportService(IValorIngresadoRepository ValorIngresadoRepository, IComprobanteRetencionRepository ComprobanteRetencionRepository)
        {
            this.ValorIngresadoRepository = ValorIngresadoRepository;
            this.ComprobanteRetencionRepository = ComprobanteRetencionRepository;
        }

        #endregion

        public IList<ReporteTesoreria> GetIngresosByDates(DateTime start, DateTime end)
        {
            IList<ReporteTesoreria> IngresosReporteTesoreria = null;
            this.ValorIngresadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                IngresosReporteTesoreria = this.ValorIngresadoRepository.GetIngresosByDates(start,end);
            });
            return IngresosReporteTesoreria;
        }

        public IList<ReporteTesoreria> GetEgresosByDates(DateTime start, DateTime end)
        {
            IList<ReporteTesoreria> EgresosReporteTesoreria = null;
            this.ValorIngresadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                EgresosReporteTesoreria = this.ValorIngresadoRepository.GetEgresosByDates(start, end);
            });
            return EgresosReporteTesoreria;
        }

        public IList<ComprobanteRetencion> GetComprobantesRetencionesByDates(DateTime start, DateTime end)
        {
            IList<ComprobanteRetencion> ComprobantesRetencionesReporteTesoreria = null;
            this.ComprobanteRetencionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ComprobantesRetencionesReporteTesoreria = this.ComprobanteRetencionRepository.GetRetencionFilter(0,0,0,start, end);
            });
            return ComprobantesRetencionesReporteTesoreria;
        }

    }
}
