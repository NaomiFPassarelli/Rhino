using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public class CobranzaValorItemRepository : BaseRepository<CobranzaValorItem>, ICobranzaValorItemRepository
    {
        public CobranzaValorItemRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

    }
}
