using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class BancoRepository : BaseSecuredRepository<Banco>, IBancoRepository
    {
        public BancoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Banco> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Banco>().Where(c => c.Activo).GetFilterBySecurity().List();
        }


    }
}
