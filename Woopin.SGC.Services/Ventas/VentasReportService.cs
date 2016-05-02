using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Ventas;

namespace Woopin.SGC.Services
{
    public class VentasReportService : IVentasReportService
    {

        #region VariablesyConstructor

        private readonly IComprobanteVentaRepository ComprobanteVentaRepository;


        public VentasReportService(IComprobanteVentaRepository ComprobanteVentaRepository)
        {
            this.ComprobanteVentaRepository = ComprobanteVentaRepository;
        }

        #endregion

        public IList<ReporteVenta> GetVencimientosACobrar()
        {
            IList<ReporteVenta> Comprobantes = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobantes = this.ComprobanteVentaRepository.GetVencimientosACobrar();
            });
            return Comprobantes;
        }
        public IList<ReporteVentasArticulo> GetReporteVentasArticulo(int IdCliente, DateTime start, DateTime end)
        {
            IList<ReporteVentasArticulo> reporte = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                reporte = this.ComprobanteVentaRepository.GetReporteVentasArticulo(IdCliente, start, end);
            });
            return reporte;
        }
        public IList<Cliente> GetClientesDeudores()
        {
            IList<Cliente> Clientes = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Clientes = this.ComprobanteVentaRepository.GetAllClientesDeudores();
            });
            return Clientes;
        }
        public IList<ComprobanteVenta> GetAllComprobantesPendientes()
        {
            IList<ComprobanteVenta> ComprobanteVenta = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ComprobanteVenta = this.ComprobanteVentaRepository.GetAllComprobantesPendientes();
            });
            return ComprobanteVenta;
        }

        public IList<ReporteCitiItem> GetCitiVentas(DateTime start, DateTime end)
        {
            IList<ReporteCitiItem> reporte = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                reporte = this.ComprobanteVentaRepository.GetCitiVentas(start,end);
            });
            return reporte;
        }

        public IList<ReporteAcumulado> GetVentasPorClientes(DateTime start, DateTime end)
        {
            IList<ReporteAcumulado> reporte = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                reporte = this.ComprobanteVentaRepository.GetVentasPorClientes(start, end);
            });
            return reporte;
        }
    }
}
