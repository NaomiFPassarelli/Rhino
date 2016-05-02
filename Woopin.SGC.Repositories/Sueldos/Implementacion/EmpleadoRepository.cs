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
    public class EmpleadoRepository : BaseSecuredRepository<Empleado>, IEmpleadoRepository
    {
        public EmpleadoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Empleado> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Empleado>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<Empleado> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Empleado>()
                                                        .Where((Restrictions.On<Empleado>(x => x.Nombre).IsLike('%' + req.where + '%')))
                                                        .Where((Restrictions.On<Empleado>(x => x.Apellido).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Empleado> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Empleado>()
                                                        .Where((Restrictions.On<Empleado>(x => x.Nombre).IsLike('%' + req.where + '%')))
                                                        .Where((Restrictions.On<Empleado>(x => x.Apellido).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public bool ExistCUIT(string cuit, int? IdUpdate)
        {
            Empleado e = null;
            if (IdUpdate != null && IdUpdate > 0)
            {
                e = this.GetSessionFactory().GetSession().QueryOver<Empleado>()
                    .Where(x => x.CUIT == cuit && x.Activo && IdUpdate != x.Id).GetFilterBySecurity().SingleOrDefault();
            }else{
                e = this.GetSessionFactory().GetSession().QueryOver<Empleado>()
                    .Where(x => x.CUIT == cuit && x.Activo).GetFilterBySecurity().SingleOrDefault();
            }
            
            if (e != null)
                return true;
            return false;
        }

    }
}
