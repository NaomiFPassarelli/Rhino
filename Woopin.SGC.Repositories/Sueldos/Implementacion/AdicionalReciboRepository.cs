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
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.Repositories.Sueldos
{
    public class AdicionalReciboRepository : BaseSecuredRepository<AdicionalRecibo>, IAdicionalReciboRepository
    {
        public AdicionalReciboRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<AdicionalRecibo> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<AdicionalRecibo>().GetFilterBySecurity().List();
        }

        public IList<AdicionalRecibo> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<AdicionalRecibo>()
                                                        //.Where((Restrictions.On<AdicionalRecibo>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<AdicionalRecibo> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<AdicionalRecibo>()
                                                        //.Where((Restrictions.On<AdicionalRecibo>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

    }
}
