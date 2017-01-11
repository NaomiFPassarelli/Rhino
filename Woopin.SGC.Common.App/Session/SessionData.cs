using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.CommonApp.Session
{
    /// <summary>
    /// Representa los datos generales a toda la aplicacion cargados en tiempo de ejecución.
    /// </summary>
    public class SessionData
    {
        /// <summary>
        /// Devuelve el identificador univoco de la sesion instanciada.
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Devuelve la organización que esta siendo usada por el usuario.
        /// </summary>
        public Organizacion CurrentOrganizacion { get; set; }

        /// <summary>
        /// Devuelve toda la información del usuario.
        /// </summary>
        public Usuario CurrentUser { get; set; }

        /// <summary>
        /// Devuelve los modulos de la organizacion
        /// </summary>
        public IList<ModulosSistemaGestion> CurrentsModuls { get; set; }

        /// <summary>
        /// Devuelve una nueva instancia de la clase.
        /// </summary>
        public SessionData()
        {
            this.SessionId = Guid.NewGuid();
        }

    }
}
