using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class LocalizacionRepository : BaseRepository<Localizacion>, ILocalizacionRepository
    {
        public LocalizacionRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Localizacion> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Localizacion>()
                                                        .Where(m => m.Activo)
                                                        .OrderBy(x => x.Nombre).Asc
                                                        .List();
        }

        public void SetDefault(int Id)
        {
            var Localizacion = this.GetSessionFactory().GetSession().QueryOver<Localizacion>().Where(m => m.Predeterminado).SingleOrDefault();
            if (Localizacion != null)
            {
                Localizacion.Predeterminado = false;
                this.GetSessionFactory().GetSession().Update(Localizacion);
            }
            var LocalizacionDefault = (Localizacion)this.GetSessionFactory().GetSession().Get(typeof(Localizacion), Id);
            LocalizacionDefault.Predeterminado = true;
            this.GetSessionFactory().GetSession().Update(LocalizacionDefault);
        }

        public Localizacion GetByNombre(string nombre)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Localizacion>()
                                                        .Where(m => m.Nombre == nombre)
                                                        .SingleOrDefault();
        }
    }
}
