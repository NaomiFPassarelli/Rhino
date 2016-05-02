using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Desarrollos;

namespace Woopin.SGC.Repositories.Desarrollos
{
    public class DesarrolloRepository : BaseRepository<Desarrollo>, IDesarrolloRepository
    {
        public DesarrolloRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public IList<Desarrollo> GetAllByFilter(int IdUsuario, int IdCliente, DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Desarrollo>().Where(x => (x.Profesional.Id == IdUsuario || IdUsuario == 0)
                                                                                                && (x.Empresa.Id == IdCliente || IdCliente == 0)
                                                                                                && x.Fecha >= start && x.Fecha <= end)
                                                                                  .Fetch(x => x.Empresa).Eager
                                                                                  .Fetch(x => x.Profesional).Eager
                                                                                  .OrderBy(x => x.NombreDesarrollado).Asc
                                                                                  .List();
        }
    
    }
}
