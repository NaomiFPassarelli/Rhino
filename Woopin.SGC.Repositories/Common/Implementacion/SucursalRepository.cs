using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Common
{
    public class SucursalRepository : BaseSecuredRepository<Sucursal>, ISucursalRepository
    {
        public SucursalRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Sucursal> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Sucursal>()
                                                        .Where(m => m.Activo)
                                                        .OrderBy(x => x.Nombre).Asc
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public void SetDefault(int Id)
        {
            var sucursal = this.GetSessionFactory().GetSession().QueryOver<Sucursal>().Where(m => m.Predeterminado).SingleOrDefault();
            if (sucursal != null)
            {
                sucursal.Predeterminado = false;
                this.GetSessionFactory().GetSession().Update(sucursal);
            }
            var sucursalDefault = (Sucursal)this.GetSessionFactory().GetSession().Get(typeof(Sucursal), Id);
            sucursalDefault.Predeterminado = true;
            this.Update(sucursalDefault);
        }
    }
}
