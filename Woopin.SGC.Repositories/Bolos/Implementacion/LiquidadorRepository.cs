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
using NHibernate.Transform;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Negocio;
//using Woopin.SGC.Repositories.Compras.Helpers;

namespace Woopin.SGC.Repositories.Bolos
{
    public class LiquidadorRepository : BaseSecuredRepository<Liquidador>, ILiquidadorRepository
    {
        public LiquidadorRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Liquidador> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Liquidador>().Where(c => c.Activo).GetFilterBySecurity().List();
        }

        public IList<Liquidador> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
                                                        .Where((Restrictions.On<Liquidador>(x => x.Trabajador.Nombre).IsLike('%' + req.where + '%')))
                                                        .And(x => x.Activo)
                                                        .GetFilterBySecurity()
                                                        .List();
        }
        public Liquidador GetCompleto(int Id)
        {
            Liquidador Liquidador = this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
                                                        .Where(x => x.Id == Id && x.Activo)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Trabajador).Eager
                                                        .Fetch(x => x.Organizacion).Eager
                                                        .SingleOrDefault();

            if (Liquidador == null) return null;

            return Liquidador;
        }
        

        public IList<Liquidador> GetAllByFilter(PagingRequest req)
        {
            // Este puede tener informacion de paginado para traer solo algunos registros y ordenamiento

            return this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
                                                        .Where((Restrictions.On<Liquidador>(x => x.Trabajador.Nombre).IsLike('%' + req.where + '%')))
                                                        .And(x => x.Activo)
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public int GetProximoNumeroReferencia()
        {
            Liquidador ultimoLiquidador = this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
                                                                                      .GetFilterBySecurity()
                                                                                      .Where(x => x.Activo)
                                                                                      //.OrderBy(x => x.NumeroReferencia).Desc
                                                                                      .Take(1)
                                                                                      .SingleOrDefault();

            return ultimoLiquidador != null ? ultimoLiquidador.NumeroReferencia + 1 : 1;
        }

        public Liquidador GetLiquidadorAnterior()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
                                                                        .GetFilterBySecurity()
                                                                        .Where(x => x.Activo)
                                                                       .OrderBy(x => x.Id).Desc
                                                                       .Take(1)
                                                                       .SingleOrDefault();
        }

        public IList<Liquidador> GetAllByDates(DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
                                                        .GetFilterBySecurity()
                                                        .Where(c => c.FechaCreacion >= start && c.FechaCreacion <= end)
                                                        .OrderBy(x => x.FechaCreacion).Desc
                                                        .List();
        }
        
        public IList<Liquidador> GetLiquidadores(IList<int> Ids)
        {
            IList<Liquidador> Liquidadores = this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
                .Where(c => c.Activo)
                .WhereRestrictionOn(c => c.Id).IsIn(Ids.ToArray())
                //.Fetch(c => c.AdicionalesLiquidador).Eager
                //.Fetch(c => c.Trabajador).Eager
                //.Fetch(c => c.Organizacion).Eager
                .GetFilterBySecurity().List();
            
            return Liquidadores;
        }


        //public IList<Liquidador> GetAllByBolo(int IdBolo, DateTime start, DateTime end, DateTime startvenc, DateTime endvenc, Model.Common.CuentaCorrienteFilter filter)
        public IList<Liquidador> GetAllByBolo(int IdBolo, DateTime start, DateTime end)
        {
            //List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(filter);

            return this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
                                                        //.GetFiltroCuentaCorriente(filter)
                                                        //.GetByPermissions()
                                                        .GetFilterBySecurity()
                                                        .Where(c => c.FechaCreacion >= start && c.FechaCreacion <= end  && (c.Bolo.Id == IdBolo || IdBolo == 0))
                                                            //&& (c.FechaVencimiento >= startvenc && c.FechaVencimiento <= endvenc) && (c.Bolo.Id == IdBolo || IdBolo == 0))
                                                        //.WhereRestrictionOn(c => c.Estado).IsIn(estados)
                                                        .OrderBy(x => x.FechaCreacion).Desc
                                                        .List();
        }



        //public decimal GetMejorRemuneracion(int IdTrabajador)
        //{ 
        //    DateTime today = DateTime.Now;
        //    DateTime mitadAño = new DateTime(today.Year, 6, DateTime.DaysInMonth(today.Year, 6));
        //    Liquidador LiquidadorMejorRemuneracion = null;
        //    if (today.Month > 7)
        //    {
        //        //desde la mitad a Hasta de año
        //        DateTime ultimoDia = new DateTime(today.Year, 12, DateTime.DaysInMonth(today.Year, 12));
        //        LiquidadorMejorRemuneracion = this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
        //                                                                              .GetFilterBySecurity()
        //                                                                              .Where(c => c.Trabajador.Id == IdTrabajador && (c.FechaDesde >= mitadAño && c.FechaDesde <= ultimoDia) && c.Activo)
        //                                                                              //.OrderBy(c => c.TotalRemunerativo).Desc
        //                                                                              .Take(1)
        //                                                                              .SingleOrDefault();

        //    }
        //    else
        //    {
        //        //desde el Desde a mitad de año
        //        DateTime primerDia = new DateTime(today.Year, 1, 1);
        //        LiquidadorMejorRemuneracion = this.GetSessionFactory().GetSession().QueryOver<Liquidador>()
        //                                                                              .GetFilterBySecurity()
        //                                                                              .Where(c => c.Trabajador.Id == IdTrabajador && (c.FechaDesde >= primerDia && c.FechaDesde <= mitadAño) && c.Activo)
        //                                                                              //.OrderBy(c => c.TotalRemunerativo).Desc
        //                                                                              .Take(1)
        //                                                                              .SingleOrDefault();
            
        //    }

        //    if (LiquidadorMejorRemuneracion != null)
        //    {
        //        return LiquidadorMejorRemuneracion.TotalRemunerativo;
        //    }
        //    else {
        //        return 0;
        //    }
            
        //}

        //public decimal[] GetPromedioRemunerativo(int IdTrabajador)
        //{
        //    DateTime today = DateTime.Now;
        //    DateTime meses6 = DateTime.Now;
        //    meses6 = meses6.AddMonths(-6);
        //    DateTime meses12 = DateTime.Now;
        //    meses12 = meses12.AddMonths(-12);
        //    decimal[] promedio = new decimal[2];

        //    //TODO SQL antiguedad, premios, hs extras, dif sindical, dif obra social
        //    int[] adicionales = {6,7,8,9,10,11,12,13,14,15,16,17,18, 1004, 1005, 4008, 4009, 4007, 3007, 3008, 3009, 3010, 3011, 3012};
        //    Liquidador c = null;
        //    AdicionalLiquidador dcc = null;
        //    Adicional rub = null;

        //    Liquidador promedio6 = this.GetSessionFactory().GetSession().QueryOver<Liquidador>(() => c)
        //                                        .Where(() => c.FechaHasta >= meses6
        //                                            && (c.Trabajador.Id == IdTrabajador || IdTrabajador == 0)
        //                                            && (rub.Id.IsIn(adicionales)) && c.Activo)
        //                                        .GetFilterBySecurity()
        //                                        .JoinAlias(() => c.AdicionalesLiquidador, () => dcc)
        //                                        .JoinAlias(() => dcc.Adicional, () => rub)
        //                                        .Select(
        //                                        Projections.Sum(() => dcc.Total).WithAlias(() => c.Total)).TransformUsing(Transformers.AliasToBean<Liquidador>()).SingleOrDefault();

        //    Liquidador promedio12 = this.GetSessionFactory().GetSession().QueryOver<Liquidador>(() => c)
        //                                        .Where(() => c.FechaHasta >= meses12
        //                                            && (c.Trabajador.Id == IdTrabajador || IdTrabajador == 0)
        //                                            && (rub.Id.IsIn(adicionales)) && c.Activo)
        //                                        .GetFilterBySecurity()
        //                                        .JoinAlias(() => c.AdicionalesLiquidador, () => dcc)
        //                                        .JoinAlias(() => dcc.Adicional, () => rub)
        //                                        .Select(
        //                                        Projections.Sum(() => dcc.Total).WithAlias(() => c.Total)).TransformUsing(Transformers.AliasToBean<Liquidador>()).SingleOrDefault();
            
        //    if (promedio6 != null)
        //    {
        //         promedio[0] = promedio6.Total/6;
        //    }
        //    if (promedio12 != null && promedio6.Total != promedio12.Total)
        //    {
        //        promedio[1] = promedio12.Total/12;
        //    }
        //    return promedio;
        //}

    }
}
