using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public class CategoriaIVARepository : BaseRepository<CategoriaIVA>, ICategoriaIVARepository
    {
        public CategoriaIVARepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<CategoriaIVA> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<CategoriaIVA>()
                                                        .Where(m => m.Activo)
                                                        .OrderBy(x => x.Nombre).Asc
                                                        .List();
        }

        public void SetDefault(int Id)
        {
            var categoria = this.GetSessionFactory().GetSession().QueryOver<CategoriaIVA>().Where(m => m.Predeterminado).SingleOrDefault();
            if (categoria != null)
            {
                categoria.Predeterminado = false;
                this.GetSessionFactory().GetSession().Update(categoria);
            }
            var categoriaDefault = (CategoriaIVA)this.GetSessionFactory().GetSession().Get(typeof(CategoriaIVA), Id);
            categoriaDefault.Predeterminado = true;
            this.GetSessionFactory().GetSession().Update(categoriaDefault);
        }

        public CategoriaIVA GetByNombre(string nombre)
        {
            return this.GetSessionFactory().GetSession().QueryOver<CategoriaIVA>()
                                                        .Where(m => m.Nombre == nombre)
                                                        .SingleOrDefault();
        }
    }
}
