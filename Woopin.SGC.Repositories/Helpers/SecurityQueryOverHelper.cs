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

namespace Woopin.SGC.Repositories.Helpers
{
    public static class SecurityQueryOverHelper
    {

        public static IQueryOver<T, T> GetFilterBySecurity<T>(this IQueryOver<T, T> query) where T : class, ISecuredEntity
        {
            Organizacion o = Security.GetOrganizacion();
            if (o != null)
            {
                return query.Where(x => x.Organizacion.Id == o.Id);
            }
            return query;
        }

    }
}
