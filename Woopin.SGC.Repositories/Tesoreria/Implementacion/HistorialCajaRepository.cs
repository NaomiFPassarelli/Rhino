using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class HistorialCajaRepository : BaseSecuredRepository<HistorialCaja>, IHistorialCajaRepository
    {
        public HistorialCajaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public IList<HistorialCaja> GetAllByDates(int IdCaja, DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<HistorialCaja>()
                                                        .Where(X => X.Fecha >= start && X.Fecha <= end && (X.Caja.Id == IdCaja || IdCaja == 0))
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Id).Desc
                                                        .List();
        }

    }
}
