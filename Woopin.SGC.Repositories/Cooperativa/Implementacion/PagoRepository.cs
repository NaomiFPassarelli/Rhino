using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Common.HtmlModel;
using NHibernate.Criterion;
using Woopin.SGC.Repositories.Helpers;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.Repositories.Cooperativa
{
    public class PagoRepository : BaseSecuredRepository<Pago>, IPagoRepository
    {
        public PagoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }
            public IList<Pago> GetAllPagosByAsociado(int IdAsociado)
            {
                return this.GetSessionFactory().GetSession().QueryOver<Pago>().Where(c => c.Asociado.Id == IdAsociado).GetFilterBySecurity().List();
            }

    }
}
