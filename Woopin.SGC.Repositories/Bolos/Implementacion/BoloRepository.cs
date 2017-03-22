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
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.Repositories.Bolos
{
    public class BoloRepository : BaseSecuredRepository<Bolo>, IBoloRepository
    {
        public BoloRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Bolo> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Bolo>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<Bolo> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Bolo>()
                                                        .Where((Restrictions.On<Bolo>(x => x.Nombre).IsLike('%' + req.where + '%')) ||
                                                        (Restrictions.On<Bolo>(x => x.DenominacionPelicula).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        //public IList<Bolo> GetAllByFilter(PagingRequest req)
        //{
        //    // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

        //    return this.GetSessionFactory().GetSession().QueryOver<Bolo>()
        //                                                .Where((Restrictions.On<Bolo>(x => x.Descripcion).IsLike('%' + req.where + '%')))
        //                                                .And(Expression.Eq("Activo", true))
        //                                                .GetFilterBySecurity()
        //                                                .List();
        //}


    }
}
