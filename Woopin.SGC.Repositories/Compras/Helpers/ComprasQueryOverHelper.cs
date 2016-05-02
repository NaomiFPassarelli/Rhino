using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Repositories.Compras.Helpers
{
    public static class ComprasQueryOverHelper
    {
        public static IQueryOver<ComprobanteCompra, ComprobanteCompra> GetFiltroCuentaCorriente(this IQueryOver<ComprobanteCompra, ComprobanteCompra> query, CuentaCorrienteFilter filter)
        {
            DateTime hoy = DateTime.Now;

            switch (filter)
            {
                case CuentaCorrienteFilter.Todos:
                    break;
                case CuentaCorrienteFilter.Pendientes:
                    query.Where(x => x.TotalPagado != x.Total);
                    break;
                case CuentaCorrienteFilter.Vencidos:
                    query.Where(x => x.FechaVencimiento < hoy && x.TotalPagado != x.Total);
                    break;
                default:
                    break;
            }


            return query;
        }

        public static IQueryOver<ComprobanteCompra, ComprobanteCompra> GetByPermissions(this IQueryOver<ComprobanteCompra, ComprobanteCompra> query)
        {
            if (Roles.IsUserInRole("Secretaria"))
            {
                return query.Where(x => x.Usuario.Id == Security.GetCurrentUser().Id);
            }
            return query;
        }
    }
}
