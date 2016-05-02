using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class TipoBancoRepository : BaseRepository<TipoBanco>, ITipoBancoRepository
    {
        public TipoBancoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<TipoBanco> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<TipoBanco>().Where(c => c.Activo).List();
        }


    }
}
