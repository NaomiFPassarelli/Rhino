using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class LocalidadRepository : BaseRepository<Localidad>, ILocalidadRepository
    {
        public LocalidadRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Localidad> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Localidad>()
                                                        .Where(m => m.Activo)
                                                        .OrderBy(x => x.Nombre).Asc
                                                        .List();
        }

        public void SetDefault(int Id)
        {
            var Localidad = this.GetSessionFactory().GetSession().QueryOver<Localidad>().Where(m => m.Predeterminado).SingleOrDefault();
            if (Localidad != null)
            {
                Localidad.Predeterminado = false;
                this.GetSessionFactory().GetSession().Update(Localidad);
            }
            var LocalidadDefault = (Localidad)this.GetSessionFactory().GetSession().Get(typeof(Localidad), Id);
            LocalidadDefault.Predeterminado = true;
            this.GetSessionFactory().GetSession().Update(LocalidadDefault);
        }

        public Localidad GetByNombre(string nombre)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Localidad>()
                                                        .Where(m => m.Nombre == nombre)
                                                        .SingleOrDefault();
        }
    }
}
