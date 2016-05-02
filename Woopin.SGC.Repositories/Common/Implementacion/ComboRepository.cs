using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class ComboRepository : BaseRepository<Combo>, IComboRepository
    {
        public ComboRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Combo> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Combo>().OrderBy(x => x.Nombre).Asc.List();
        }
    }
}
