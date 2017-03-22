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
    public class TrabajadorRepository : BaseSecuredRepository<Trabajador>, ITrabajadorRepository
    {
        public TrabajadorRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Trabajador> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Trabajador>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<Trabajador> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Trabajador>()
                                                        .Where((Restrictions.On<Trabajador>(x => x.Nombre).IsLike('%' + req.where + '%')) ||
                                                        (Restrictions.On<Trabajador>(x => x.Apellido).IsLike('%' + req.where + '%')) ||
                                                        (Restrictions.On<Trabajador>(x => x.CUIT).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Trabajador> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Trabajador>()
                                                        .Where((Restrictions.On<Trabajador>(x => x.Nombre).IsLike('%' + req.where + '%')))
                                                        .Where((Restrictions.On<Trabajador>(x => x.Apellido).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public bool ExistCUIT(string cuit, int? IdUpdate)
        {
            Trabajador e = null;
            if (IdUpdate != null && IdUpdate > 0)
            {
                e = this.GetSessionFactory().GetSession().QueryOver<Trabajador>()
                    .Where(x => x.CUIT == cuit && x.Activo && IdUpdate != x.Id).GetFilterBySecurity().SingleOrDefault();
            }else{
                e = this.GetSessionFactory().GetSession().QueryOver<Trabajador>()
                    .Where(x => x.CUIT == cuit && x.Activo).GetFilterBySecurity().SingleOrDefault();
            }
            
            if (e != null)
                return true;
            return false;
        }

        public Trabajador GetCompleto(int IdTrabajador)
        {
            Trabajador e = null;
            e = this.GetSessionFactory().GetSession().QueryOver<Trabajador>()
                    .Where(x => x.Activo && (x.Id == IdTrabajador)).GetFilterBySecurity()
                    .Fetch(x => x.Sindicato).Eager
                    .SingleOrDefault();
            return e;
        }

        public int GetProximoNumeroReferencia()
        {
            Trabajador ultimo = this.GetSessionFactory().GetSession().QueryOver<Trabajador>()
                                                                                      .GetFilterBySecurity()
                                                                                      .OrderBy(x => x.NumeroReferencia).Desc
                                                                                      .Take(1)
                                                                                      .SingleOrDefault();

            return ultimo != null ? ultimo.NumeroReferencia + 1 : 1;
        }
        

    }
}
