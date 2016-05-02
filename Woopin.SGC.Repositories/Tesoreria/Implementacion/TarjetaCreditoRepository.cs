using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class TarjetaCreditoRepository : BaseSecuredRepository<TarjetaCredito>, ITarjetaCreditoRepository
    {
        public TarjetaCreditoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<TarjetaCredito> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<TarjetaCredito>()
                                                        .Where(c => c.Estado != EstadoTarjeta.Cancelada && c.Estado != EstadoTarjeta.Eliminada)
                                                        .GetFilterBySecurity()
                                                        .List();
        }
    }
}
