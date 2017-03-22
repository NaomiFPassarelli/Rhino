using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class ComboOrganizacionRepository : BaseSecuredRepository<ComboOrganizacion>, IComboOrganizacionRepository
    {
        public ComboOrganizacionRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<ComboOrganizacion> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<ComboOrganizacion>().OrderBy(x => x.Nombre).Asc.List();
        }
    }
}
