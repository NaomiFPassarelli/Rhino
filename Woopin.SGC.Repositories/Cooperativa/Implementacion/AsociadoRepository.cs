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
using NHibernate.Transform;

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

        public IList<Asociado> GetAsociados(IList<int> Ids)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                        .Where(c => c.Activo)
                        .WhereRestrictionOn(c => c.Id).IsIn(Ids.ToArray())
                        .GetFilterBySecurity().List();
        }


        public IList<Asociado> GetAsociadosMes(int Mes, int Año)
        {
            DateTime inicio = new DateTime(Año, Mes, 01);
            DateTime fin = new DateTime(Año, Mes, DateTime.DaysInMonth(Año, Mes));
            IList<Asociado> As = null;
            //TODO en realidad es FechaActaIngreso
            As = this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                .Where(x => x.FechaIngreso >= inicio && x.Activo && x.FechaIngreso <= fin).GetFilterBySecurity().List();
            return As;
        }

        public IList<Asociado> GetAsociadosMesEgreso(int Mes, int Año)
        {
            DateTime inicio = new DateTime(Año, Mes, 01);
            DateTime fin = new DateTime(Año, Mes, DateTime.DaysInMonth(Año, Mes));
            IList<Asociado> As = null;
            //TODO en realidad es FechaActaEgreso
            As = this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                .Where(x => x.FechaEgreso >= inicio && x.Activo && x.FechaEgreso <= fin).GetFilterBySecurity().List();
            return As;
        }

        //public Asociado LoadHeader()
        //{
        //    Asociado h = null;

        //    Asociado A = this.GetSessionFactory().GetSession().QueryOver<Asociado>()
        //                                                .GetFilterBySecurity()
        //                                                .Select(
        //                                                Projections.Sum<Asociado>(x => x.CantidadCuotas)
        //                                                .WithAlias(() => h.CantidadCuotas),
        //                                                Projections.Sum<Asociado>(x => x.CantidadPagosAbonadas)
        //                                                .WithAlias(() => h.CantidadPagosAbonadas),
        //                                                Projections.Sum<Asociado>(x => x.ImportePago)
        //                                                .WithAlias(() => h.ImportePago)
        //                                                )
        //                                                .TransformUsing(Transformers.AliasToBean<Asociado>())
        //                                                .Take(1)
        //                                                .SingleOrDefault<Asociado>();
        //    return A;
        //}


        public int GetProximoNumeroReferencia()
        {
            Asociado ultimo = this.GetSessionFactory().GetSession().QueryOver<Asociado>()
                                                                                      .GetFilterBySecurity()
                                                                                      .OrderBy(x => x.NumeroReferencia).Desc
                                                                                      .Take(1)
                                                                                      .SingleOrDefault();

            return ultimo != null ? ultimo.NumeroReferencia + 1 : 1;
        }

    }
}
