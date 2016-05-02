using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public static class ComprasQueryOverHelper
    {
        public static IQueryOver<ComprobanteVenta, ComprobanteVenta> GetFiltroCuentaCorriente(this IQueryOver<ComprobanteVenta, ComprobanteVenta> query, CuentaCorrienteFilter filter)
        {
            DateTime hoy = DateTime.Now;

            switch (filter)
            {
                case CuentaCorrienteFilter.Todos:
                    break;
                case CuentaCorrienteFilter.Pendientes:
                    query.Where(x => x.TotalCobrado != x.Total);
                    break;
                case CuentaCorrienteFilter.Vencidos:
                    query.Where(x => x.FechaVencimiento < hoy && x.TotalCobrado != x.Total);
                    break;
                default:
                    break;
            }


            return query;
        }
        public static IQueryOver<ComprobanteVenta, ComprobanteVenta> GetByPermissions(this IQueryOver<ComprobanteVenta, ComprobanteVenta> query)
        {
            if (Roles.IsUserInRole("Secretaria"))
            {
                return query.Where(x => x.Usuario.Id == Security.GetCurrentUser().Id);
            }
            return query;
        }

        public static IQueryOver<ComprobanteVenta, ComprobanteVenta> GetFiltroByTipoComprobante(this IQueryOver<ComprobanteVenta, ComprobanteVenta> query, int tipoComprobante, string Letra)
        {
            switch (tipoComprobante)
            {
                case ComprobanteVentaHelper.Factura:
                case ComprobanteVentaHelper.NotaCredito:
                case ComprobanteVentaHelper.NotaDebito:
                    List<int> tiposValidos = new List<int>();
                    tiposValidos.Add(ComprobanteVentaHelper.NotaCredito);
                    tiposValidos.Add(ComprobanteVentaHelper.NotaDebito);
                    tiposValidos.Add(ComprobanteVentaHelper.Factura);
                    query.Where(x => x.Letra == Letra)
                         .WhereRestrictionOn(x => x.Tipo.Id).IsIn(tiposValidos);
                    break;
                case ComprobanteVentaHelper.FEFacturaA:
                case ComprobanteVentaHelper.FEFacturaB:
                case ComprobanteVentaHelper.FEFacturaC:
                case ComprobanteVentaHelper.FENotaCreditoB:
                case ComprobanteVentaHelper.FENotaCreditoA:
                case ComprobanteVentaHelper.FENotaCreditoC:
                case ComprobanteVentaHelper.FENotaDebitoA:
                case ComprobanteVentaHelper.FENotaDebitoB:
                case ComprobanteVentaHelper.FENotaDebitoC:
                    query.Where(x => x.Letra == Letra && x.Tipo.Id == tipoComprobante);
                    break;
                default:
                    break;
            }


            return query;
        }
    }
}
