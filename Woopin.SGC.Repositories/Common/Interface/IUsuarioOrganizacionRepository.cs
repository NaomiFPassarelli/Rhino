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
    public interface IUsuarioOrganizacionRepository : IRepository<UsuarioOrganizacion>
    {

        /// <summary>
        /// Devuelve el objeto Usuario-Organizacion dado los ids de ambas entidades.
        /// </summary>
        /// <param name="Id">Id del usuario</param>
        /// <param name="IdOrganizacion">Id de la organizacion</param>
        /// <returns></returns>
        UsuarioOrganizacion GetByIDs(int Id, int IdOrganizacion);
    }
}
