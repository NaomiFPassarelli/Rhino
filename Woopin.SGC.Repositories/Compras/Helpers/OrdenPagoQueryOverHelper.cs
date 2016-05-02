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
    public static class OrdenPagoQueryOverHelper
    {

        public static IQueryOver<OrdenPago, OrdenPago> GetByPermissions(this IQueryOver<OrdenPago, OrdenPago> query)
        {
            if (Roles.IsUserInRole("Secretaria"))
            {
                return query.Where(x => x.Usuario.Id == Security.GetCurrentUser().Id);
            }
            return query;
        }
    }
}
