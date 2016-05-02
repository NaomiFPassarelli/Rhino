using NHibernate.Criterion;
using NHibernate.Transform;
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
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        /// <summary>
        /// Busqueda de usuario por el nombre de usuario.
        /// </summary>
        /// <param name="username">Nombre del usuario</param>
        /// <returns>Objeto usuario completo</returns>
        public Usuario GetByUsername(string username)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Usuario>()
                                                        .Where(x => x.Username == username)
                                                        .SingleOrDefault();
        }
        public override IList<Usuario> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Usuario>()
                                                        .Where(m => m.Activo)
                                                        .OrderBy(x => x.NombreCompleto).Asc
                                                        .List();
        }


        /// <summary>
        /// Busqueda de los usuarios que coumplan con los criterios preseleccionados.
        /// </summary>
        /// <param name="req">Pedido de combo, envia el where con lo que se desea filtrar.</param>
        /// <returns>Listado de usuarios que cumplen la condición del where.</returns>
        public IList<Usuario> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Usuario>()
                                                        .Where(Restrictions.On<Usuario>(x => x.NombreCompleto).IsLike('%' + req.where + '%'))
                                                        .And(Expression.Eq("Activo", true))
                                                        .List();
        }

        /// <summary>
        /// Consulta para traer todos los usuarios para la administracion de organizaciones.
        /// Si se le manda Id de Organizacion, filtrara por esa organizacion.
        /// </summary>
        /// <param name="IdOrganizacion">Id de la Organizacion a filtrar, 0 no filtra</param>
        /// <param name="IdUsuario">Id del usuario a filtrar, 0 no filtra</param>
        /// <returns>Devuelve todos los usuarios que cumplan con los criterios, menos el que esta loggeado.</returns>
        public IList<Usuario> GetAllByOrganizacion(int IdOrganizacion, int IdUsuario)
        {
            UsuarioOrganizacion uoAlias = null;
            Usuario uAlias = null;


            return this.GetSessionFactory().GetSession().QueryOver<UsuarioOrganizacion>( () => uoAlias)
                                                        .JoinAlias( () => uoAlias.Usuario,() => uAlias )
                                                        .Where(() => uoAlias.Organizacion.Id == IdOrganizacion && uAlias.Id != IdUsuario && uAlias.Activo)
                                                        .Select(x => uoAlias.Usuario)
                                                        .OrderBy(x => uAlias.NombreCompleto).Asc
                                                        .List<Usuario>();
        }


        /// <summary>
        /// Busqueda de usuarios conocidos por compartir alguna organizacion con el usuario loggeado.
        /// </summary>
        /// <param name="IdUsuario">Id del usuario a filtrar, 0 no filtra</param>
        /// <returns>Listado de usuarios que conoce el usuario loggeado.</returns>
        public IList<Usuario> GetAlMisOrganizaciones(int IdUsuario, int IdOrganizacion)
        {
            UsuarioOrganizacion uoAlias = null;
            Usuario uAlias = null;

            IList<int> misOrganizaciones = this.GetSessionFactory().GetSession().QueryOver<UsuarioOrganizacion>()
                                                                                        .Where(x => x.Usuario.Id == IdUsuario)
                                                                                        .Select(x => x.Organizacion.Id)
                                                                                        .TransformUsing(Transformers.DistinctRootEntity)
                                                                                        .List<int>();

            IList<int> usuariosEnOrganizacion = this.GetSessionFactory().GetSession().QueryOver<UsuarioOrganizacion>()
                                                                                        .Where(x => x.Organizacion.Id == IdOrganizacion)
                                                                                        .Select(x => x.Usuario.Id)
                                                                                        .List<int>();


            return this.GetSessionFactory().GetSession().QueryOver<UsuarioOrganizacion>( () => uoAlias)
                                                        .JoinAlias( () => uoAlias.Usuario,() => uAlias )
                                                        .Where(() => uAlias.Id != IdUsuario && uAlias.Activo)
                                                        .WhereRestrictionOn(() => uoAlias.Organizacion.Id).IsIn(misOrganizaciones.ToList())
                                                        .WhereRestrictionOn(() => uAlias.Id).Not.IsIn(usuariosEnOrganizacion.ToList())
                                                        .Fetch(x => x.Usuario).Eager
                                                        .TransformUsing(Transformers.DistinctRootEntity)
                                                        .Select(x => x.Usuario)
                                                        .List<Usuario>();
        }


        /// <summary>
        /// Busqueda de usuario por el Id y trae el usuario completo
        /// </summary>
        /// <returns>Objeto usuario completo</returns>
        public Usuario GetCompleto(int Id)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Usuario>()
                                                        .Where(x => x.Id == Id)
                                                        .Fetch(x => x.OrganizacionActual).Eager
                                                        .SingleOrDefault();
        }
    }
}
