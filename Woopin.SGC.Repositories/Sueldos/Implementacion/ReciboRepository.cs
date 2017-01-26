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
using NHibernate.Transform;
using Woopin.SGC.CommonApp.Security;

namespace Woopin.SGC.Repositories.Sueldos
{
    public class ReciboRepository : BaseSecuredRepository<Recibo>, IReciboRepository
    {
        public ReciboRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Recibo> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Recibo>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<Recibo> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                        .Where((Restrictions.On<Recibo>(x => x.Empleado.Nombre).IsLike('%' + req.where + '%')))
                                                        .And(x => x.Activo)
                                                        .GetFilterBySecurity()
                                                        .List();
        }
        public Recibo GetCompleto(int Id)
        {
            Recibo recibo = this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                        .Where(x => x.Id == Id && x.Activo)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.AdicionalesRecibo).Eager
                                                        .Fetch(x => x.Empleado).Eager
                                                        .Fetch(x => x.Organizacion).Eager
                                                        .SingleOrDefault();

            if (recibo == null) return null;

            foreach (var adicional in recibo.AdicionalesRecibo)
            {
                adicional.Recibo = null;
            }

            return recibo;
        }
        

        public IList<Recibo> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                        .Where((Restrictions.On<Recibo>(x => x.Empleado.Nombre).IsLike('%' + req.where + '%')))
                                                        .And(x => x.Activo)
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public int GetProximoNumeroReferencia()
        {
            Recibo ultimoRecibo = this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                                                      .GetFilterBySecurity()
                                                                                      .Where(x => x.Activo)
                                                                                      .OrderBy(x => x.NumeroReferencia).Desc
                                                                                      .Take(1)
                                                                                      .SingleOrDefault();

            return ultimoRecibo != null ? ultimoRecibo.NumeroReferencia + 1 : 1;
        }

        public Recibo GetReciboAnterior(int IdEmpleado)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                                        .GetFilterBySecurity()
                                                                        .Where(x => x.Activo)
                                                                       .OrderBy(x => x.Id).Desc
                                                                       .Take(1)
                                                                       .SingleOrDefault();
        }

        public IList<Recibo> GetAllByDates(DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                        .GetFilterBySecurity()
                                                        .Where(c => c.FechaCreacion >= start && c.FechaCreacion <= end)
                                                        .OrderBy(x => x.FechaCreacion).Desc
                                                        .List();
        }
        
        public IList<Recibo> GetRecibos(IList<int> Ids)
        {
            IList<Recibo> Recibos = this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                .Where(c => c.Activo)
                .WhereRestrictionOn(c => c.Id).IsIn(Ids.ToArray())
                //.Fetch(c => c.AdicionalesRecibo).Eager
                //.Fetch(c => c.Empleado).Eager
                //.Fetch(c => c.Organizacion).Eager
                .GetFilterBySecurity().List();
            


            foreach (Recibo recibo in Recibos)
            {
                foreach (var adicional in recibo.AdicionalesRecibo)
                {
                    adicional.Recibo = null;
                }
            }
            return Recibos;
        }
            

        public decimal GetMejorRemuneracion(int IdEmpleado)
        { 
            DateTime today = DateTime.Now;
            DateTime mitadAño = new DateTime(today.Year, 6, DateTime.DaysInMonth(today.Year, 6));
            Recibo reciboMejorRemuneracion = null;
            if (today.Month > 7)
            {
                //desde la mitad a fin de año
                DateTime ultimoDia = new DateTime(today.Year, 12, DateTime.DaysInMonth(today.Year, 12));
                reciboMejorRemuneracion = this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                                                      .GetFilterBySecurity()
                                                                                      .Where(c => c.Empleado.Id == IdEmpleado && (c.FechaInicio >= mitadAño && c.FechaInicio <= ultimoDia) && c.Activo)
                                                                                      .OrderBy(c => c.TotalRemunerativo).Desc
                                                                                      .Take(1)
                                                                                      .SingleOrDefault();

            }
            else
            {
                //desde el inicio a mitad de año
                DateTime primerDia = new DateTime(today.Year, 1, 1);
                reciboMejorRemuneracion = this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                                                      .GetFilterBySecurity()
                                                                                      .Where(c => c.Empleado.Id == IdEmpleado && (c.FechaInicio >= primerDia && c.FechaInicio <= mitadAño) && c.Activo)
                                                                                      .OrderBy(c => c.TotalRemunerativo).Desc
                                                                                      .Take(1)
                                                                                      .SingleOrDefault();
            
            }

            if (reciboMejorRemuneracion != null)
            {
                return reciboMejorRemuneracion.TotalRemunerativo;
            }
            else {
                return 0;
            }
            
        }

        public decimal[] GetPromedioRemunerativo(int IdEmpleado)
        {
            DateTime today = DateTime.Now;
            DateTime meses6 = DateTime.Now;
            meses6 = meses6.AddMonths(-6);
            DateTime meses12 = DateTime.Now;
            meses12 = meses12.AddMonths(-12);
            decimal[] promedio = new decimal[2];

            //TODO SQL antiguedad, premios, hs extras, dif sindical, dif obra social
            int[] adicionales = {6,7,8,9,10,11,12,13,14,15,16,17,18, 1004, 1005, 4008, 4009, 4007, 3007, 3008, 3009, 3010, 3011, 3012};
            Recibo c = null;
            AdicionalRecibo dcc = null;
            Adicional rub = null;

            Recibo promedio6 = this.GetSessionFactory().GetSession().QueryOver<Recibo>(() => c)
                                                .Where(() => c.FechaFin >= meses6
                                                    && (c.Empleado.Id == IdEmpleado || IdEmpleado == 0)
                                                    && (rub.Id.IsIn(adicionales)) && c.Activo)
                                                .GetFilterBySecurity()
                                                .JoinAlias(() => c.AdicionalesRecibo, () => dcc)
                                                .JoinAlias(() => dcc.Adicional, () => rub)
                                                .Select(
                                                Projections.Sum(() => dcc.Total).WithAlias(() => c.Total)).TransformUsing(Transformers.AliasToBean<Recibo>()).SingleOrDefault();

            Recibo promedio12 = this.GetSessionFactory().GetSession().QueryOver<Recibo>(() => c)
                                                .Where(() => c.FechaFin >= meses12
                                                    && (c.Empleado.Id == IdEmpleado || IdEmpleado == 0)
                                                    && (rub.Id.IsIn(adicionales)) && c.Activo)
                                                .GetFilterBySecurity()
                                                .JoinAlias(() => c.AdicionalesRecibo, () => dcc)
                                                .JoinAlias(() => dcc.Adicional, () => rub)
                                                .Select(
                                                Projections.Sum(() => dcc.Total).WithAlias(() => c.Total)).TransformUsing(Transformers.AliasToBean<Recibo>()).SingleOrDefault();
            
            if (promedio6 != null)
            {
                 promedio[0] = promedio6.Total/6;
            }
            if (promedio12 != null && promedio6.Total != promedio12.Total)
            {
                promedio[1] = promedio12.Total/12;
            }
            return promedio;
        }

    }
}
