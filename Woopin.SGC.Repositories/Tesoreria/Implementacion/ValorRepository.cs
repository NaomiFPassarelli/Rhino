using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class ValorRepository : BaseSecuredRepository<Valor>, IValorRepository
    {
        public ValorRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }


        public override IList<Valor> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Valor>().Where(c => c.Activo).GetFilterBySecurity().List();
        }
    }
}
