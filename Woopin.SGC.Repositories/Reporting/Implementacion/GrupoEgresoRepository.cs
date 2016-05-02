using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Compras;
using NHibernate.Transform;
using NHibernate.Criterion;
using NHibernate;
using NHibernate.Dialect.Function;
using Woopin.SGC.Model.Reporting;

namespace Woopin.SGC.Repositories.Reporting
{
    public class GrupoEgresoRepository : BaseRepository<GrupoEgreso>, IGrupoEgresoRepository
    {
        public GrupoEgresoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
    
        }
            public IList<GrupoEgreso> GetAllTree(DateTime start, DateTime end)
            {
                //Todo implementar las fechas - hay que joinear con las FV
                IList<GrupoEgreso> GruposEgresos = this.GetSessionFactory().GetSession().QueryOver<GrupoEgreso>()
                                                            .List();

                return GruposEgresos;
            }

            public IList<GrupoEgreso> GetAllGruposEgresosNoHoja(SelectComboRequest req)
            {
                return this.GetSessionFactory().GetSession().QueryOver<GrupoEgreso>()
                                                            .Where(x => x.Rubro == null)
                                                            .And((Restrictions.On<GrupoEgreso>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                            .List();
            }

            public int GetLastRaiz()
            {
                GrupoEgreso ge = this.GetSessionFactory().GetSession().QueryOver<GrupoEgreso>()
                    .Where(x => x.Level == 1).OrderBy(x => x.Raiz).Desc.Take(1).SingleOrDefault();
                if(ge != null)
                {
                    return ge.Raiz;
                }
                return 0;
            }

            public IList<GrupoEgreso> GetAllHijos(int Id)
            {
                 return this.GetSessionFactory().GetSession().QueryOver<GrupoEgreso>()
                        .Where(x => x.NodoPadre.Id == Id).OrderBy(x => x.Level).Desc.List();
            }

    }
}
