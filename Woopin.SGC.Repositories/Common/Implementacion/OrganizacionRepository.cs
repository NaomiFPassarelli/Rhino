using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class OrganizacionRepository : BaseRepository<Organizacion>, IOrganizacionRepository
    {
        public OrganizacionRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }
        public override IList<Organizacion> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Organizacion>()
                                                        .Where(m => m.Activo)
                                                        .OrderBy(x => x.RazonSocial).Asc
                                                        .List();
        }

        public IList<Organizacion> GetAllMine()
        {
            IList<int> misOrganizaciones = this.GetSessionFactory().GetSession().QueryOver<UsuarioOrganizacion>()
                                                                                        .Where(x => x.Usuario.Id == Security.GetCurrentUser().Id)
                                                                                        .Select(x => x.Organizacion.Id)
                                                                                        .TransformUsing(Transformers.DistinctRootEntity)
                                                                                        .List<int>();

            return this.GetSessionFactory().GetSession().QueryOver<Organizacion>()
                                                        .Where(m => m.Activo)
                                                        .WhereRestrictionOn(m => m.Id).IsIn(misOrganizaciones.ToList())
                                                        .OrderBy(x => x.RazonSocial).Asc
                                                        .List();
        }
        


    }
}
