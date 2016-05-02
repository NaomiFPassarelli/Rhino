using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using NHibernate.Criterion;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public class RetencionRepository : BaseSecuredRepository<Retencion>, IRetencionRepository
    {
        public RetencionRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Retencion> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Retencion>().Where(x => x.Activo).GetFilterBySecurity().OrderBy(x => x.Descripcion).Asc.List();
        }

        public IList<Retencion> GetAllValor()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Retencion>().Where(x => x.Activo && x.EsValor).GetFilterBySecurity().OrderBy(x => x.Descripcion).Asc.List();
        }

        public IList<Retencion> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Retencion>()
                                                        .Where((Restrictions.On<Retencion>(x => x.Descripcion).IsLike('%' + req.where + '%') ||
                                                                Restrictions.On<Retencion>(x => x.Abreviatura).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Retencion> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Retencion>()
                                                        .Where((Restrictions.On<Retencion>(x => x.Descripcion).IsLike('%' + req.where + '%') ||
                                                                Restrictions.On<Retencion>(x => x.Abreviatura).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }
    }
}
