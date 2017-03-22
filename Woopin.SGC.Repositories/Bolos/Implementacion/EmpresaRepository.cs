using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Bolos;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Bolos
{
    public class EmpresaRepository : BaseSecuredRepository<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }
        //public override IList<Empresa> GetAll()
        //{
        //    return this.GetSessionFactory().GetSession().QueryOver<Empresa>()
        //                                                .Where(m => m.Activo)
        //                                                .OrderBy(x => x.RazonSocial).Asc
        //                                                .List();
        //}


    }
}
