using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Common.HtmlModel;
using NHibernate.Criterion;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Repositories.Compras.Helpers;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Compras
{
    public class ProveedorRepository : BaseSecuredRepository<Proveedor>, IProveedorRepository
    {
        public ProveedorRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Proveedor> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Proveedor>()
                                                        .Where(c => c.Activo)
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Proveedor> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Proveedor>()
                                                        .Where((Restrictions.On<Proveedor>(x => x.RazonSocial).IsLike('%' + req.where + '%') ||
                                                                Restrictions.On<Proveedor>(x => x.CUIT).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Proveedor> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Proveedor>()
                                                        .Where((Restrictions.On<Proveedor>(x => x.RazonSocial).IsLike('%' + req.where + '%') ||
                                                                Restrictions.On<Proveedor>(x => x.CUIT).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public bool ExistCUIT(string cuit, int? IdUpdate)
        {
            Proveedor p = null;
            if (IdUpdate != null && IdUpdate > 0)
            {
                p = this.GetSessionFactory().GetSession().QueryOver<Proveedor>()
                    .Where(x => x.CUIT == cuit && x.Activo && IdUpdate != x.Id).GetFilterBySecurity().SingleOrDefault();
            }
            else
            {
                p = this.GetSessionFactory().GetSession().QueryOver<Proveedor>()
                    .Where(x => x.CUIT == cuit && x.Activo).GetFilterBySecurity().SingleOrDefault();
            }
            if(p != null)
                return true;
            return false;
        }
    }
}
