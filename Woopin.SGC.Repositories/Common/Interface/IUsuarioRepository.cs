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
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        /// <summary>
        /// Busqueda de usuario por el nombre de usuario.
        /// </summary>
        /// <param name="username">Nombre del usuario</param>
        /// <returns>Objeto usuario completo, sin lazyload.</returns>
        Usuario GetByUsername(string username);


        /// <summary>
        /// Busqueda de los usuarios que coumplan con los criterios preseleccionados.
        /// </summary>
        /// <param name="req">Pedido de combo, envia el where con lo que se desea filtrar.</param>
        /// <returns>Listado de usuarios que cumplen la condición del where.</returns>
        IList<Usuario> GetAllByFilter(SelectComboRequest req);


        /// <summary>
        /// Consulta para traer todos los usuarios para la administracion de organizaciones.
        /// Si se le manda Id de Organizacion, filtrara por esa organizacion.
        /// Si se le manda Id de Usuario, filtrara por los usuarios que el pueda administrar en las distintas orgs.
        /// </summary>
        /// <param name="IdOrganizacion">Id de la Organizacion a filtrar, 0 no filtra</param>
        /// <param name="IdUsuario">Id del usuario a filtrar, 0 no filtra</param>
        /// <returns>Devuelve todos los usuarios que cumplan con los criterios, menos el que esta loggeado.</returns>
        IList<Usuario> GetAllByOrganizacion(int IdOrganizacion, int IdUsuario);

        /// <summary>
        /// Busqueda de usuarios conocidos por compartir alguna organizacion con el usuario loggeado.
        /// </summary>
        /// <param name="IdUsuario">Id del usuario a filtrar, 0 no filtra</param>
        /// <param name="IdOrganizacion">Id de la organizacion a no traer</param>
        /// <returns>Listado de usuarios que conoce el usuario loggeado.</returns>
        IList<Usuario> GetAlMisOrganizaciones(int IdUsuario, int IdOrganizacion);


        /// <summary>
        /// Busqueda de usuario por el Id y trae el usuario completo
        /// </summary>
        /// <returns>Objeto usuario completo</returns>
        Usuario GetCompleto(int Id);
    }
}
