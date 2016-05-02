using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Repositories.Compras
{
    public class OrdenPagoComprobanteItemRepository : BaseRepository<OrdenPagoComprobanteItem>, IOrdenPagoComprobanteItemRepository
    {
        public OrdenPagoComprobanteItemRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }
    }
}
