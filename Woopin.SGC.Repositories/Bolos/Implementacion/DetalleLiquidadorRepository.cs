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
    public class DetalleLiquidadorRepository : BaseSecuredRepository<DetalleLiquidador>, IDetalleLiquidadorRepository
    {
        public DetalleLiquidadorRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        //public override IList<DetalleLiquidador> GetAll()
        //{
        //    return this.GetSessionFactory().GetSession().QueryOver<DetalleLiquidador>().Where(c => c.Activo).GetFilterBySecurity().List();
        //}

        //public IList<DetalleLiquidador> GetAllByFilter(SelectComboRequest req)
        //{
        //    return this.GetSessionFactory().GetSession().QueryOver<DetalleLiquidador>()
        //                                                .Where((Restrictions.On<DetalleLiquidador>(x => x.Descripcion).IsLike('%' + req.where + '%')))
        //                                                .And(Expression.Eq("Activo", true))
        //                                                .GetFilterBySecurity()
        //                                                .List();
        //}

        //public IList<DetalleLiquidador> GetAllByFilter(PagingRequest req)
        //{
        //    // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

        //    return this.GetSessionFactory().GetSession().QueryOver<DetalleLiquidador>()
        //                                                .Where((Restrictions.On<DetalleLiquidador>(x => x.Descripcion).IsLike('%' + req.where + '%')))
        //                                                .And(Expression.Eq("Activo", true))
        //                                                .GetFilterBySecurity()
        //                                                .List();
        //}


    }
}
