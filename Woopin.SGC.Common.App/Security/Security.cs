using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMatrix.WebData;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.CommonApp.Security
{
    public class Security
    {

        /// <summary>
        /// Devuelve el usuario loggeado solo con el Id para asociaciones de entidades
        /// antes de guardarlo en la BD.
        /// </summary>
        /// <returns>Usuario solo con Id</returns>
        public static Usuario GetCurrentUser()
        {
            int currentUserId = SessionDataManager.GetUsuario().Id;
            return new Usuario() { Id = currentUserId };
        }

        /// <summary>
        /// Devuelve el nombre completo del usuario loggeado.
        /// </summary>
        /// <returns>Nombre completo del usuario</returns>
        public static string GetUserNombre()
        {
            SessionData s = SessionDataManager.Get();
            if (s == null || s.CurrentUser == null) return "";
            return s.CurrentUser.NombreCompleto;
        }

        /// <summary>
        /// Busca la organizacion que actualmente tiene seteado el usuario.
        /// </summary>
        /// <returns>La organizacion actual</returns>
        public static Organizacion GetOrganizacion()
        {
            return SessionDataManager.GetOrganizacion();
        }

        /// <summary>
        /// Actualiza la información del usuario loggeado.
        /// </summary>
        /// <param name="org">Nuevo objeto usuario</param>
        public static void SetUsuario(Usuario user)
        {
            SessionDataManager.SetUsuario(user);
        }

        /// <summary>
        /// Cambia la organizacion actual del usuario de la sesion
        /// </summary>
        /// <param name="org">Nueva organizacion actual</param>
        public static void SetOrganizacion(Organizacion org)
        {
            SessionDataManager.SetOrganizacion(org);
        }

        /// <summary>
        /// Consulta para saber si el usuario debe o no loggear.
        /// </summary>
        /// <returns>Si el usuario debe o no loggear</returns>
        public static bool IsUserDebugging()
        {
            return SessionDataManager.Get().CurrentUser.IsDebugging;
        }


        /// <summary>
        /// Devuelvela cabecera para un job request por hangfire.
        /// </summary>
        /// <returns>JobHeader request</returns>
        public static JobHeader GetJobHeader()
        {
            return new JobHeader()
            {
                IdOrganizacion = SessionDataManager.GetOrganizacion().Id,
                IdUsuario = SessionDataManager.GetUsuario().Id
            };
        }
    }
}
