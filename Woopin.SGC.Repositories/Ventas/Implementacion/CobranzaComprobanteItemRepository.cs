using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public class CobranzaComprobanteItemRepository : BaseRepository<CobranzaComprobanteItem>, ICobranzaComprobanteItemRepository
    {
        public CobranzaComprobanteItemRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

    }
}
