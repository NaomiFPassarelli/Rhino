using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class HistorialCuentaBancariaRepository : BaseSecuredRepository<HistorialCuentaBancaria>, IHistorialCuentaBancariaRepository
    {
        public HistorialCuentaBancariaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public IList<HistorialCuentaBancaria> GetAllByDates(int Id,DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<HistorialCuentaBancaria>()
                                                        .Where(X => X.Fecha >= start && X.Fecha <= end && (X.CuentaBancaria.Id == Id || Id == 0))
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Id).Desc
                                                        .List();
        }
    }
}
