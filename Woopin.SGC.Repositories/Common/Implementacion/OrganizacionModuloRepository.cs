using NHibernate.Criterion;
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
    public class OrganizacionModuloRepository : BaseRepository<OrganizacionModulo>, IOrganizacionModuloRepository
    {
        public OrganizacionModuloRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }


        /// <summary>
        /// Devuelve el objeto modulo-Organizacion dado los ids de ambas entidades.
        /// </summary>
        /// <param name="Id">Id del modulo</param>
        /// <param name="IdOrganizacion">Id de la organizacion</param>
        /// <returns></returns>
        public OrganizacionModulo GetByIDs(int Id, int IdOrganizacion)
        {
            return this.GetSessionFactory().GetSession().QueryOver<OrganizacionModulo>()
                                                        .Where(x => (int)x.ModulosSistemaGestion == Id && x.Organizacion.Id == IdOrganizacion)
                                                        .SingleOrDefault();
        }
    }
}
