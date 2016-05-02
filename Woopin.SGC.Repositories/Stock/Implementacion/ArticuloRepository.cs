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
    public class ArticuloRepository : BaseSecuredRepository<Articulo>, IArticuloRepository
    {
        public ArticuloRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Articulo> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Articulo>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<Articulo> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Articulo>()
                                                        .Where((Restrictions.On<Articulo>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public Articulo GetConStock(int IdArticulo)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Articulo>()
                                                        .Where(x => x.Id == IdArticulo && x.Tipo == TipoArticulo.Producto && x.Inventario && x.Stock >= 0)
                                                        .GetFilterBySecurity().Take(1).SingleOrDefault();
        }

        public IList<Articulo> GetAllConStockByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Articulo>()
                .Where(x => x.Tipo == TipoArticulo.Producto && x.Inventario && x.Stock >= 0)                                        
                .Where((Restrictions.On<Articulo>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Articulo> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Articulo>()
                                                        .Where((Restrictions.On<Articulo>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

    }
}
