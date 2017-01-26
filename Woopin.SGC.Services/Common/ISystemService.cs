using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Services
{
    public interface ISystemService
    {
        void InitializeSessionData(Usuario user);
        void InitializeSessionData(JobHeader header);

        #region Usuario
        void AddUsuario(Usuario Usuario);
        Usuario GetUsuario(int Id);
        Usuario GetUsuarioByUsername(string Username);
        void UpdateUsuario(Usuario Usuario);
        IList<Usuario> GetAllUsuarios();
        void DeleteUsuarios(List<int> Ids);
        void CambiarActivo(List<int> Ids, bool Estado);
        SelectCombo GetAllUsuariosByFilterCombo(SelectComboRequest req);


        /// <summary>
        /// Consulta para traer todos los usuarios para la administracion de organizaciones.
        /// Si se le manda Id de Organizacion, filtrara por esa organizacion.
        /// </summary>
        /// <param name="IdOrganizacion">Id de la Organizacion a filtrar, 0 no filtra</param>
        /// <returns>Devuelve todos los usuarios que cumplan con los criterios, menos el que esta loggeado.</returns>
        IList<Usuario> GetAllUsuariosByOrganizacion(int IdOrganizacion);

        /// <summary>
        /// Busqueda de usuarios conocidos por compartir alguna organizacion con el usuario loggeado.
        /// </summary>
        /// <returns>Listado de usuarios que conoce el usuario loggeado.</returns>
        IList<Usuario> GetAllUsuariosMisOrganizaciones(int IdOrganizacion);

        /// <summary>
        /// Elimina la relacion usuario-organizacion, para los usuarios seleccionados de la organizacion dada.
        /// </summary>
        /// <param name="Ids">Ids de los usuarios a remover</param>
        /// <param name="IdOrganizacion">Id de la organización a limpiar</param>
        void RemoverUsuariosOrganizacion(List<int> Ids, int IdOrganizacion);

        /// <summary>
        /// Agregar las relaciones usuario-organizacion, para los usuarios seleccionados de la organizacion dada.
        /// </summary>
        /// <param name="Ids">Ids de usuarios a agregar</param>
        /// <param name="IdOrganizacion">Id de la organizacion</param>
        void AgregarUsuariosOrganizacion(List<int> Ids, int IdOrganizacion);

        /// <summary>
        /// Setea la organizacion actual del usuario loggeado.
        /// </summary>
        /// <param name="Id">ID de la organizacion que desea operar</param>
        void SetCurrentOrganizacion(int Id);

        #endregion

        #region Organizacion
        void AddOrganizacion(Organizacion Organizacion);
        Organizacion GetOrganizacion(int Id);
        void UpdateOrganizacion(Organizacion Organizacion);
        IList<Organizacion> GetAllOrganizaciones();

        /// <summary>
        /// Busqueda de las organizaciones en las que participa  el usuario loggeado.
        /// </summary>
        /// <returns>Listado de organizaciones a las cual pertenece el usuario loggeado.</returns>
        IList<Organizacion> GetMisOrganizaciones();
        void DeleteOrganizaciones(List<int> Ids);

        /// <summary>
        /// Obtiene la organizacion que tiene seteada el usuario.
        /// </summary>
        /// <returns>Organización actual del usuario loggeado</returns>
        Organizacion GetCurrentOrganizacion();
        #endregion

        #region Common
        /// <summary>
        /// Consulta para los indicadores del Dashboard iniciales.
        /// </summary>
        /// <returns>Objeto con todos los valroes de los indicadores del dashboard.</returns>
        Dashboard GetDashboard();

        /// <summary>
        /// Listado de logs para la administración del sistema.
        /// Los parametros restringiran la busqueda.
        /// </summary>
        /// <param name="IdUsuario">Usuario que genero el log</param>
        /// <param name="IdOrganizacion">Organizacion del usuario que se desea filtrar</param>
        /// <param name="start">Fecha de comienzo de busqueda</param>
        /// <param name="end">Fecha de finalizacion de busqueda</param>
        /// <returns></returns>
        IList<Log> GetAllLogsByDates(int IdUsuario, int IdOrganizacion, DateTime start, DateTime end);

        #endregion

        #region OrganizacionModulo

        /// <summary>
        /// Consulta para traer todos los modulos de organizacion actual.
        /// Si se le manda Id de Organizacion, filtrara por esa organizacion.
        /// </summary>
        /// <param name="IdOrganizacion">Id de la Organizacion a filtrar, 0 no filtra</param>
        /// <returns>Devuelve todos los modulos que cumplan con los criterios.</returns>
        IList<ModulosSistemaGestion> GetAllModulosByOrganizacion(int IdOrganizacion);

        /// <summary>
        /// Elimina la relacion modulo-organizacion, para los modulos seleccionados de la organizacion dada.
        /// </summary>
        /// <param name="Ids">Ids de los modulos a remover</param>
        /// <param name="IdOrganizacion">Id de la organización a limpiar</param>
        void RemoverModulosOrganizacion(List<int> Ids, int IdOrganizacion);

        /// <summary>
        /// Agregar las relaciones modulos-organizacion, para los modulos seleccionados de la organizacion dada.
        /// </summary>
        /// <param name="Ids">Ids de modulos a agregar</param>
        /// <param name="IdOrganizacion">Id de la organizacion</param>
        void AgregarModulosOrganizacion(List<int> Ids, int IdOrganizacion);


        #endregion



    }
}

