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
    public class GrupoIngresoRepository : BaseRepository<GrupoIngreso>, IGrupoIngresoRepository
    {
        public GrupoIngresoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }
        public IList<GrupoIngreso> GetAllTree(DateTime start, DateTime end)
        {
            //Todo implementar las fechas - hay que joinear con las FV
            IList<GrupoIngreso> GruposIngresos = this.GetSessionFactory().GetSession().QueryOver<GrupoIngreso>()
                                                        .List();

            return GruposIngresos;
        }

        public IList<GrupoIngreso> GetAllGruposIngresosNoHoja(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<GrupoIngreso>()
                                                        .Where(x => x.Articulo == null)
                                                        .And((Restrictions.On<GrupoIngreso>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .List();
        }

        public int GetLastRaiz()
        {
            GrupoIngreso ge = this.GetSessionFactory().GetSession().QueryOver<GrupoIngreso>()
                .Where(x => x.Level == 1).OrderBy(x => x.Raiz).Desc.Take(1).SingleOrDefault();
            if (ge != null)
            {
                return ge.Raiz;
            }
            return 0;
        }

        public IList<GrupoIngreso> GetAllHijos(int Id)
        {
            return this.GetSessionFactory().GetSession().QueryOver<GrupoIngreso>()
                   .Where(x => x.NodoPadre.Id == Id).OrderBy(x => x.Level).Desc.List();
        }

        }

}
