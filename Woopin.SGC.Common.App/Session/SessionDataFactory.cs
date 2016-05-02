using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Woopin.SGC.CommonApp.Session
{
    public class SessionDataFactory
    {
        /// <summary>
        /// Devuelve la key con la que se guarda el identificador de sesion.
        /// </summary>
        public static string SESSION_KEY = "sessionDataId";

        /// <summary>
        /// Devuelve el objeto de sincronizacion entre threads.
        /// </summary>
        static object syncObject = new object();

        /// <summary>
        /// Devuelve el Diccionario utilizado para almacenar las sesiones instanciadas.
        /// </summary>
        static Dictionary<Guid, SessionData> registeredSessionData = new Dictionary<Guid, SessionData>();

        /// <summary>
        /// Devuelve el objeto con los datos de sesion por su identificador.
        /// </summary>
        /// <param name="guid">El identificador del objeto de los datos de sesion.</param>
        /// <returns>El objeto que encapsula los datos de sesion por su identificador.</returns>
        public static SessionData GetSessionData(Guid guid)
        {
            SessionData returnValue = null;

            if (SessionDataFactory.registeredSessionData.ContainsKey(guid))
                returnValue = SessionDataFactory.registeredSessionData[guid];

            return returnValue;
        }

        /// <summary>
        /// Registra el objeto con los datos de session en el diccionario utilizado para almacenar las sesiones instanciadas.
        /// </summary>
        /// <param name="sessionData">El objeto con los datos de session.</param>
        public static void RegisterSessionData(SessionData sessionData)
        {
            Monitor.Enter(SessionDataFactory.syncObject);

            try
            {
                if (sessionData != null && !SessionDataFactory.registeredSessionData.ContainsKey(sessionData.SessionId))
                    SessionDataFactory.registeredSessionData[sessionData.SessionId] = sessionData;
                else
                    throw new Exception("RegisterSessionData Failed!");
            }
            catch (Exception)
            {
                Monitor.Exit(SessionDataFactory.syncObject);
                throw;
            }

            Monitor.Exit(SessionDataFactory.syncObject);
        }

        /// <summary>
        /// Remueve el objeto con los datos de sesion en el diccionario utilizado para almacenar las sesiones instanciadas.
        /// </summary>
        /// <param name="sessionData">El objeto con los datos de sesion.</param>
        public static void UnRegisterSessionData(SessionData sessionData)
        {
            Monitor.Enter(SessionDataFactory.syncObject);

            try
            {
                if (sessionData != null && SessionDataFactory.registeredSessionData.ContainsKey(sessionData.SessionId))
                    SessionDataFactory.registeredSessionData.Remove(sessionData.SessionId);
                else
                    throw new Exception("UnRegisterSessionData Failed!");
            }
            catch (Exception)
            {
                Monitor.Exit(SessionDataFactory.syncObject);
                throw;
            }

            Monitor.Exit(SessionDataFactory.syncObject);
        }

        /// <summary>
        /// Remueve el objeto con los datos de sesion en el diccionario utilizado para almacenar las sesiones instanciadas
        /// por su identificador.
        /// </summary>
        /// <param name="guid">El identificador del objeto con los datos de sesion.</param>
        public static void UnRegisterSessionData(Guid guid)
        {
            Monitor.Enter(SessionDataFactory.syncObject);

            try
            {
                if (SessionDataFactory.registeredSessionData.ContainsKey(guid))
                    SessionDataFactory.registeredSessionData.Remove(guid);
                else
                    throw new Exception("UnRegisterSessionData Failed!");
            }
            catch (Exception)
            {
                Monitor.Exit(SessionDataFactory.syncObject);
                throw;
            }

            Monitor.Exit(SessionDataFactory.syncObject);
        }
    }
}
