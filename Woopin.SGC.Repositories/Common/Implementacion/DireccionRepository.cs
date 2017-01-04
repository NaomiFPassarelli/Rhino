using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class DireccionRepository : BaseRepository<Direccion>, IDireccionRepository
    {
        public DireccionRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Direccion> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Direccion>()
                                                        .OrderBy(x => x.Calle).Asc
                                                        .List();
        }

        public void SetDefault(int Id)
        {
            var d = this.GetSessionFactory().GetSession().QueryOver<Direccion>().Where(m => m.Predeterminado).SingleOrDefault();
            if (d != null)
            {
                d.Predeterminado = false;
                this.GetSessionFactory().GetSession().Update(d);
            }
            var dDefault = (Direccion)this.GetSessionFactory().GetSession().Get(typeof(Direccion), Id);
            dDefault.Predeterminado = true;
            this.GetSessionFactory().GetSession().Update(dDefault);
        }

        public Direccion GetByNombre(string nombreCalle)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Direccion>()
                                                        .Where(m => m.Calle == nombreCalle)
                                                        .SingleOrDefault();
        }

    }
}
