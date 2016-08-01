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
    public class AsociadoRepository : BaseSecuredRepository<Asociado>, IAsociadoRepository
    {
        public AsociadoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Asociado> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Asociado>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<Asociado> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                                                        .Where((Restrictions.On<Asociado>(x => x.Nombre).IsLike('%' + req.where + '%')) ||
                                                        (Restrictions.On<Asociado>(x => x.Apellido).IsLike('%' + req.where + '%')) ||
                                                        (Restrictions.On<Asociado>(x => x.CUIT).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public IList<Asociado> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                                                        .Where((Restrictions.On<Asociado>(x => x.Nombre).IsLike('%' + req.where + '%')))
                                                        .Where((Restrictions.On<Asociado>(x => x.Apellido).IsLike('%' + req.where + '%')))
                                                        .And(Expression.Eq("Activo", true))
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public bool ExistCUIT(string cuit, int? IdUpdate)
        {
            Asociado e = null;
            if (IdUpdate != null && IdUpdate > 0)
            {
                e = this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                    .Where(x => x.CUIT == cuit && x.Activo && IdUpdate != x.Id).GetFilterBySecurity().SingleOrDefault();
            }else{
                e = this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                    .Where(x => x.CUIT == cuit && x.Activo).GetFilterBySecurity().SingleOrDefault();
            }
            
            if (e != null)
                return true;
            return false;
        }

        public Asociado GetCompleto(int IdAsociado)
        {
            Asociado e = null;
            e = this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                    .Where(x => x.Activo && (x.Id == IdAsociado)).GetFilterBySecurity()
                    .SingleOrDefault();
            return e;
        }

        public IList<Asociado> GetAsociadosMes(int Mes, int Año)
        {
            IList<Asociado> As = null;
            As = this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                .Where(x => x.FechaIngreso.Month == Mes && x.Activo && x.FechaIngreso.Year == Año).GetFilterBySecurity().List();
            return As;
        }

    }
}
