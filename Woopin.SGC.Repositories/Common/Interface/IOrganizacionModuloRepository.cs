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
    public interface IOrganizacionModuloRepository : IRepository<OrganizacionModulo>
    {

        /// <summary>
        /// Devuelve el objeto Modulo-Organizacion dado los ids de ambas entidades.
        /// </summary>
        /// <param name="Id">Id del modulo</param>
        /// <param name="IdOrganizacion">Id de la organizacion</param>
        /// <returns></returns>
        OrganizacionModulo GetByIDs(int Id, int IdOrganizacion);
    }
}
