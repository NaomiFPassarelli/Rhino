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

namespace Woopin.SGC.Repositories.Ventas
{
    public class GrupoEconomicoRepository : BaseSecuredRepository<GrupoEconomico>, IGrupoEconomicoRepository
    {
        public GrupoEconomicoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<GrupoEconomico> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<GrupoEconomico>().Where(c => c.Activo).GetFilterBySecurity().List();
        }


        public IList<GrupoEconomico> GetAllByFilter(SelectComboRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<GrupoEconomico>()
                                                        .Where((Restrictions.On<GrupoEconomico>(x => x.Nombre).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }
    }
}
