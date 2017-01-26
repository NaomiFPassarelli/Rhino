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

        /// <summary>
        /// Consulta para traer todos los modulos de organizacion actual.
        /// Si se le manda Id de Organizacion, filtrara por esa organizacion.
        /// </summary>
        /// <param name="IdOrganizacion">Id de la Organizacion a filtrar, 0 no filtra</param>
        /// <returns>Devuelve todos los modulos que cumplan con los criterios.</returns>
        public IList<ModulosSistemaGestion> GetAllModulosByOrganizacion(int IdOrganizacion)
        {
            return this.GetSessionFactory().GetSession().QueryOver<OrganizacionModulo>()
                                                        .Where(x => x.Organizacion.Id == IdOrganizacion || IdOrganizacion == 0)
                                                        .Select(x => x.ModulosSistemaGestion)
                                                        .List<ModulosSistemaGestion>();
        }

    }
}
