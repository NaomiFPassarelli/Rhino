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
    public class AdicionalRepository : BaseSecuredRepository<Adicional>, IAdicionalRepository
    {
        public AdicionalRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Adicional> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Adicional>().GetFilterBySecurity().List();
        }

        public Adicional Get(int IdAdicional, int IdSindicato)
        {

            return this.GetSessionFactory().GetSession().QueryOver<Adicional>()
                                                    //.Where(x => x.Id == IdAdicional && (IdSindicato == Convert.ToInt32(x.AdditionalDescription) || IdSindicato == 0))
                                                    .Where(x => x.Id == IdAdicional && (IdSindicato == 0 || x.AdditionalDescription == null || IdSindicato.ToString() == x.AdditionalDescription))
                                                    .GetFilterBySecurity().SingleOrDefault();
        }

        public IList<Adicional> GetAllByFilter(SelectComboRequest req, int IdSindicato)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Adicional>()
                                                        .Where((Restrictions.On<Adicional>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .And(x => IdSindicato == 0 || x.AdditionalDescription == null || IdSindicato.ToString() == x.AdditionalDescription)
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Adicional> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Adicional>()
                                                        .Where((Restrictions.On<Adicional>(x => x.Descripcion).IsLike('%' + req.where + '%')))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

    }
}
