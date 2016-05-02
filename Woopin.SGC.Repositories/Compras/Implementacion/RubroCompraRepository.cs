using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Compras
{
    public class RubroCompraRepository : BaseSecuredRepository<RubroCompra>, IRubroCompraRepository
    {
        public RubroCompraRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<RubroCompra> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<RubroCompra>().Where(c => c.Activo).GetFilterBySecurity().OrderBy(x => x.Descripcion).Asc.List();
        }

        public IList<RubroCompra> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<RubroCompra>()
                                                        .Where((Restrictions.On<RubroCompra>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Descripcion).Asc
                                                        .List();
        }

        public IList<RubroCompra> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<RubroCompra>()
                                                       .Where((Restrictions.On<RubroCompra>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                       .And(Expression.Eq("Activo", true))
                                                       .GetFilterBySecurity()
                                                       .OrderBy(x => x.Descripcion).Asc
                                                       .List();
        }

        public IList<RubroCompra> GetAllSinPerceByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<RubroCompra>()
                                                       .Where((Restrictions.On<RubroCompra>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                       .And(x => !x.PercepcionIIBB && !x.PercepcionIVA)
                                                       .And(Expression.Eq("Activo", true))
                                                       .GetFilterBySecurity()
                                                       .OrderBy(x => x.Descripcion).Asc
                                                       .List();
        }

    }
}
