using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public class BloqueoContableRepository : BaseSecuredRepository<BloqueoContable>, IBloqueoContableRepository
    {
        public BloqueoContableRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public IList<BloqueoContable> GetByDates(DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<BloqueoContable>()
                                                                        .Where(x => ((x.Inicio <= start && x.Final >= start) ||
                                                                                        (x.Inicio <= end && x.Final >= end)) && x.Activo)
                                                                        .GetFilterBySecurity()
                                                                        .List();
        }
    }
}
