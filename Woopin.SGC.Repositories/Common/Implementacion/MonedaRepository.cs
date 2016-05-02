using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class MonedaRepository : BaseRepository<Moneda>, IMonedaRepository
    {
        public MonedaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }
        public override IList<Moneda> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Moneda>()
                                                        .Where(m => m.Activo)
                                                        .OrderBy(x => x.Nombre).Asc
                                                        .List();
        }

        public void SetDefaultMoneda(int Id)
        {
            var Moneda = this.GetSessionFactory().GetSession().QueryOver<Moneda>().Where(m => m.Predeterminado).SingleOrDefault();
            if (Moneda != null)
            {
                Moneda.Predeterminado = false;
                this.GetSessionFactory().GetSession().Update(Moneda);
            }
            var MonedaDefault = (Moneda)this.GetSessionFactory().GetSession().Get(typeof(Moneda), Id);
            MonedaDefault.Predeterminado = true;
            this.GetSessionFactory().GetSession().Update(MonedaDefault);
        }
    }
}
