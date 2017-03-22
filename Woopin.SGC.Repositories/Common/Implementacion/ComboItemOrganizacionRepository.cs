using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class ComboItemOrganizacionRepository : BaseRepository<ComboItemOrganizacion>, IComboItemOrganizacionRepository
    {
        public ComboItemOrganizacionRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<ComboItemOrganizacion> GetAll()
        {
            ComboOrganizacion cc = null;
            return this.GetSessionFactory().GetSession().QueryOver<ComboItemOrganizacion>()
                                                        .JoinAlias(c => c.Combo, () => cc)
                                                        .Where(c => c.Activo)
                                                        .And(c => cc.Organizacion.Id == Security.GetOrganizacion().Id)
                                                        .OrderBy(x => x.Data).Asc
                                                        .List();
        }

        public IList<ComboItemOrganizacion> GetItemsByComboId(int ComboId)
        {
            ComboOrganizacion cc = null;
            return this.GetSessionFactory().GetSession().QueryOver<ComboItemOrganizacion>()
                                                        .JoinAlias(x => x.Combo, () => cc)
                                                        .Where(x => x.Combo.Id == ComboId && x.Activo)
                                                        .And(x => cc.Organizacion.Id == Security.GetOrganizacion().Id)
                                                        .List();
        }

        public ComboItemOrganizacion GetByComboAndName(ComboType combo, string data)
        {
            int ComboId = (int)combo;
            ComboOrganizacion cc = null;
            return this.GetSessionFactory().GetSession().QueryOver<ComboItemOrganizacion>()
                                                        .JoinAlias(x => x.Combo, () => cc)
                                                        .Where(x => x.Combo.Id == ComboId && x.Activo && x.Data == data)
                                                        .And(x => cc.Organizacion.Id == Security.GetOrganizacion().Id)
                                                        .SingleOrDefault();
        }
    }
}
