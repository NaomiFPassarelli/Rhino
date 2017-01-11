using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using System.Threading;
using Woopin.SGC.Common.App.Session;

namespace Woopin.SGC.CommonApp.Session
{
    public class SessionDataManager
    {

        public static SessionData Get()
        {
            Guid sessionId = Guid.NewGuid();

            // Recupera el Guid de la sesion 
            if (HttpContext.Current == null)
            {
                sessionId = JobSession.SessionId;
            }
            else
            {
                sessionId = (Guid) HttpContext.Current.Session[SessionDataFactory.SESSION_KEY];
            }
            return SessionDataFactory.GetSessionData(sessionId);
        }



        /// <summary>
        /// Busca de la sesión, la organizacion que actualmente tiene seteado el usuario.
        /// </summary>
        /// <returns>La organizacion actual</returns>
        public static Organizacion GetOrganizacion()
        {
            return SessionDataManager.Get().CurrentOrganizacion;
        }

        /// <summary>
        /// Cambia la organizacion actual del usuario de la sesion
        /// </summary>
        /// <param name="org">Nueva organizacion actual</param>
        public static void SetOrganizacion(Organizacion org)
        {
            SessionDataManager.Get().CurrentOrganizacion = org;
        }

        /// <summary>
        /// Cambia los modulos de la organizacion actual del usuario de la sesion
        /// </summary>
        /// <param name="modulos">Nuevos modulos de la organizacion actual</param>
        public static void SetModulos(IList<ModulosSistemaGestion> modulos)
        {
            SessionDataManager.Get().CurrentsModuls = modulos;
        }


        /// <summary>
        /// Busca de la sesión, toda la información del usuario.
        /// </summary>
        /// <returns>Objeto usuario loggeado</returns>
        public static Usuario GetUsuario()
        {
            return SessionDataManager.Get().CurrentUser;
        }

        /// <summary>
        /// Actualiza la información del usuario loggeado.
        /// </summary>
        /// <param name="org">Nuevo objeto usuario</param>
        public static void SetUsuario(Usuario user)
        {
            SessionDataManager.Get().CurrentUser = user;
        }


        /// <summary>
        /// Activa o desactiva el loggeo de debugs.
        /// </summary>
        public static void ToggleUserDebugging()
        {
            SessionDataManager.Get().CurrentUser.IsDebugging = !SessionDataManager.Get().CurrentUser.IsDebugging;
        }


        /// <summary>
        /// Busca de la sesión, los modulos que actualmente tiene seteado la organizacion
        /// </summary>
        /// <returns>Los modulos de la organizacion</returns>
        public static IList<ModulosSistemaGestion> GetModulosByOrganizacion()
        {
            return SessionDataManager.Get().CurrentsModuls;
        }

    }
}