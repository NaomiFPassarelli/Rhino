using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class LogRepository : BaseSecuredRepository<Log>, ILogRepository
    {
        public LogRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public IList<Log> GetAllByDates(int IdUsuario,int IdOrganizacion, DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Log>()
                                                        .Where(l => (l.Usuario.Id == IdUsuario || IdUsuario == 0) && (l.Organizacion.Id == IdOrganizacion || IdOrganizacion == 0) && l.Date >= start && l.Date <= end)
                                                        .Fetch(x => x.Usuario).Eager
                                                        .OrderBy(x => x.Date).Desc
                                                        .List();
        }
    }
}
