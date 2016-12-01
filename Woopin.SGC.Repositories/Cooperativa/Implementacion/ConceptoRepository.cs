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
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.Repositories.Cooperativa
{
    public class ConceptoRepository : BaseSecuredRepository<Concepto>, IConceptoRepository
    {
        public ConceptoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Concepto> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Concepto>().GetFilterBySecurity().List();
        }

        public Concepto Get(int IdConcepto)
        {

            return this.GetSessionFactory().GetSession().QueryOver<Concepto>()
                                                    //.Where(x => x.Id == IdConcepto && (IdSindicato == Convert.ToInt32(x.AdditionalDescription) || IdSindicato == 0))
                                                    .Where(x => x.Id == IdConcepto )
                                                    .GetFilterBySecurity().SingleOrDefault();
        }

        public IList<Concepto> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Concepto>()
                                                        .Where((Restrictions.On<Concepto>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Concepto> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Concepto>()
                                                        .Where((Restrictions.On<Concepto>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

    }
}
