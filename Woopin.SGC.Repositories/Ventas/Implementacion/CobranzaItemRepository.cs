using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Repositories.Ventas
{
    public class CobranzaItemRepository : BaseRepository<CobranzaItem>, ICobranzaItemRepository
    {
        public CobranzaItemRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<CobranzaItem> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<CobranzaItem>().List();
        }
    }
}
