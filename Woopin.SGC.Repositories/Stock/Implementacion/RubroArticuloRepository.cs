using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Common.HtmlModel;
using NHibernate.Criterion;
using Woopin.SGC.Repositories.Helpers;
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.Repositories.Stock
{
    public class RubroArticuloRepository : BaseSecuredRepository<RubroArticulo>, IRubroArticuloRepository
    {
        public RubroArticuloRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<RubroArticulo> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<RubroArticulo>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<RubroArticulo> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<RubroArticulo>()
                                                        .Where((Restrictions.On<RubroArticulo>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<RubroArticulo> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<RubroArticulo>()
                                                        .Where((Restrictions.On<RubroArticulo>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

    }
}
