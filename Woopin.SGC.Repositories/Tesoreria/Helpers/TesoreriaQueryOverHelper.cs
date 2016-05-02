using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria.Helpers
{
    public static class TesoreriaQueryOverHelper
    {
        public static IQueryOver<PagoTarjeta, PagoTarjeta> GetFiltroPagosTarjetas(this IQueryOver<PagoTarjeta, PagoTarjeta> query, PagoTarjetaFilter filter)
        {
            DateTime hoy = DateTime.Now;

            switch (filter)
            {
                case PagoTarjetaFilter.Todos:
                    break;
                case PagoTarjetaFilter.Pendientes:
                    query.Where(x => x.TotalCancelado < x.Total);
                    break;
                case PagoTarjetaFilter.Cancelados:
                    query.Where(x => x.TotalCancelado >= x.Total);
                    break;
                default:
                    break;
            }


            return query;
        }

    }
}
