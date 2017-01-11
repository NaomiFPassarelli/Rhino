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
        /// <summary>
        /// Consulta para traer todos los usuarios para la administracion de organizaciones.
        /// Si se le manda Id de Organizacion, filtrara por esa organizacion.
        /// </summary>
        /// <param name="IdOrganizacion">Id de la Organizacion a filtrar, 0 no filtra</param>
        /// <param name="IdUsuario">Id del usuario a filtrar, 0 no filtra</param>
        /// <returns>Devuelve todos los usuarios que cumplan con los criterios, menos el que esta loggeado.</returns>
        public IList<ModulosSistemaGestion> GetAllModulosByOrganizacion(int IdOrganizacion, int IdModulo)
        {
            //OrganizacionModulo uoAlias = null;
            //int uAlias = 0;


            //return this.GetSessionFactory().GetSession().QueryOver<OrganizacionModulo>( () => uoAlias)
            //                                            .JoinAlias( () => uoAlias.ModulosSistemaGestion,() => uAlias )
            //                                            .Where(() => uoAlias.Organizacion.Id == IdOrganizacion)
            //                                            //.Where(() => uoAlias.Organizacion.Id == IdOrganizacion && (int)uAlias != IdModulo)
            //                                            .Select(x => uoAlias.ModulosSistemaGestion)
            //                                            .List<ModulosSistemaGestion>();



            return this.GetSessionFactory().GetSession().QueryOver<OrganizacionModulo>()
                                                        .Where(x => x.Organizacion.Id == IdOrganizacion)
                                                        .Select(x => x.ModulosSistemaGestion)
                                                        .List<ModulosSistemaGestion>();
        }


    }
}
