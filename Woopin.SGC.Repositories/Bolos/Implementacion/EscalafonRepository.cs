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
    public class EscalafonRepository : BaseSecuredRepository<Escalafon>, IEscalafonRepository
    {
        public EscalafonRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Escalafon> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Escalafon>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<Escalafon> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Escalafon>()
                                                        .Where((Restrictions.On<Escalafon>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Escalafon> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Escalafon>()
                                                        .Where((Restrictions.On<Escalafon>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }


    }
}
