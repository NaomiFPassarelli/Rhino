using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class CuentaBancariaRepository : BaseSecuredRepository<CuentaBancaria>, ICuentaBancariaRepository
    {
        public CuentaBancariaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<CuentaBancaria> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<CuentaBancaria>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<CuentaBancaria> GetAllEmiteCheque()
        {
            return this.GetSessionFactory().GetSession().QueryOver<CuentaBancaria>().Where(c => c.Activo && c.EmiteCheques).GetFilterBySecurity().List();
        }

    }
}
