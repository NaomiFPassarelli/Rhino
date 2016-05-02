using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class CajaRepository : BaseSecuredRepository<Caja>, ICajaRepository
    {
        public CajaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Caja> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Caja>().Where(c => c.Activo).GetFilterBySecurity().List();
        }


    }
}
