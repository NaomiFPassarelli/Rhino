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
    public class ClienteRepository : BaseSecuredRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Cliente> GetAll()
        {
           return this.GetSessionFactory().GetSession().QueryOver<Cliente>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<Cliente> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Cliente>()
                                                        .Where((Restrictions.On<Cliente>(x => x.RazonSocial).IsLike('%' + req.where + '%') ||
                                                                Restrictions.On<Cliente>(x => x.CUIT).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Cliente> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Cliente>()
                                                        .Where((Restrictions.On<Cliente>(x => x.RazonSocial).IsLike('%' + req.where + '%') ||
                                                                Restrictions.On<Cliente>(x => x.CUIT).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo",true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public bool ExistCUIT(string cuit, int? IdUpdate)
        {
            Cliente c = null;
            if (IdUpdate != null && IdUpdate > 0)
            {
                c = this.GetSessionFactory().GetSession().QueryOver<Cliente>()
                    .Where(x => x.CUIT == cuit && x.Activo && IdUpdate != x.Id).GetFilterBySecurity().SingleOrDefault();
            }
            else
            {
                c = this.GetSessionFactory().GetSession().QueryOver<Cliente>()
                    .Where(x => x.CUIT == cuit && x.Activo).GetFilterBySecurity().SingleOrDefault();
            }
            if (c != null)
                return true;
            return false;
        }
    }
}
