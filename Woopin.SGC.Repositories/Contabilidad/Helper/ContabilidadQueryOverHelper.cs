using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Contabilidad.Helper
{
    public static class ContabilidadQueryOverHelper
    {
        public static IQueryOver<AsientoItem, AsientoItem> GetFiltroCuentaContable(this IQueryOver<AsientoItem, AsientoItem> query, Cuenta cuentaContable)
        {
            Cuenta cInnerAlias = null;

            // ooooooo
            query = query.Where(() => cInnerAlias.Rubro == cuentaContable.Rubro);
            if (cuentaContable.Corriente > 0)
            {
                query = query.Where(() => cInnerAlias.Corriente == cuentaContable.Corriente);
            }
            if (cuentaContable.SubRubro > 0)
            {
                return query.Where(() => cInnerAlias.SubRubro == cuentaContable.SubRubro);
            }
            if (cuentaContable.Numero > 0)
            {
                return query.Where(() => cInnerAlias.Numero == cuentaContable.Numero);
            }

            return query;
        }
    }
}
