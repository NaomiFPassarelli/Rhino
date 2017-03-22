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
    public class TrabajadorBoloEscalafonRepository : BaseSecuredRepository<TrabajadorBoloEscalafon>, ITrabajadorBoloEscalafonRepository
    {
        public TrabajadorBoloEscalafonRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<TrabajadorBoloEscalafon> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<TrabajadorBoloEscalafon>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        //public IList<TrabajadorBoloEscalafon> GetAllByFilter(SelectComboRequest req)
        //{
        //    return this.GetSessionFactory().GetSession().QueryOver<TrabajadorBoloEscalafon>()
        //                                                .Where((Restrictions.On<TrabajadorBoloEscalafon>(x => x.Descripcion).IsLike('%' + req.where + '%')))
        //                                                .And(Expression.Eq("Activo", true))
        //                                                .GetFilterBySecurity()
        //                                                .List();
        //}

        //public IList<TrabajadorBoloEscalafon> GetAllByFilter(PagingRequest req)
        //{
        //    // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

        //    return this.GetSessionFactory().GetSession().QueryOver<TrabajadorBoloEscalafon>()
        //                                                .Where((Restrictions.On<TrabajadorBoloEscalafon>(x => x.Descripcion).IsLike('%' + req.where + '%')))
        //                                                .And(Expression.Eq("Activo", true))
        //                                                .GetFilterBySecurity()
        //                                                .List();
        //}


    }
}
