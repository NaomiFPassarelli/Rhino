using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Compras;
using Woopin.SGC.Repositories.Contabilidad;

namespace Woopin.SGC.Services
{
    public class ComprasReportService : IComprasReportService
    {
        #region VariablesyConstructor
        private readonly IComprobanteCompraRepository ComprobanteCompraRepository;
        public ComprasReportService(IComprobanteCompraRepository ComprobanteCompraRepository)
        {
            this.ComprobanteCompraRepository = ComprobanteCompraRepository;
        }

        #endregion

        #region ComprobanteCompra
        public IList<ReporteComprasRubros> GetReporteRubros(int IdProveedor, DateTime start, DateTime end)
        {
            IList<ReporteComprasRubros> Comprobantes = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobantes = this.ComprobanteCompraRepository.GetReporteRubros(IdProveedor,start,end);
            });
            return Comprobantes;
        }
        #endregion

        public IList<ReporteCompra> GetVencimientosAPagar()
        {
            IList<ReporteCompra> Comprobantes = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobantes = this.ComprobanteCompraRepository.GetVencimientosAPagar();
            });
            return Comprobantes;
        }

        public IList<ReporteCitiItem> GetCitiCompras(DateTime start, DateTime end)
        {
            IList<ReporteCitiItem> reporte = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                reporte = this.ComprobanteCompraRepository.GetCitiCompras(start, end);
            });
            return reporte;
        }
    }
}
