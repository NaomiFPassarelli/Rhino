using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class ComboItemRepository : BaseRepository<ComboItem>, IComboItemRepository
    {
        public ComboItemRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<ComboItem> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<ComboItem>()
                                                        .Where(c => c.Activo)
                                                        .OrderBy(x => x.Data).Asc
                                                        .List();
        }

        public IList<ComboItem> GetItemsByComboId(int ComboId)
        {
            return this.GetSessionFactory().GetSession().QueryOver<ComboItem>()
                                                        .Where(x => x.Combo.Id == ComboId && x.Activo)
                                                        .List();
        }

        public ComboItem GetByComboAndName(ComboType combo, string data)
        {
            int ComboId = (int)combo;
            return this.GetSessionFactory().GetSession().QueryOver<ComboItem>()
                                                        .Where(x => x.Combo.Id == ComboId && x.Activo && x.Data == data)
                                                        .SingleOrDefault();
        }
    }
}
