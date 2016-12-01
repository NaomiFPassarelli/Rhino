﻿using System;
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
    public class PagoRepository : BaseSecuredRepository<Pago>, IPagoRepository
    {
        public PagoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }
            public IList<Pago> GetAllPagosByAsociado(int IdAsociado)
            {
                return this.GetSessionFactory().GetSession().QueryOver<Pago>().Where(c => c.Asociado.Id == IdAsociado).GetFilterBySecurity().List();
            }

            public int GetProximoNumeroReferencia()
            {
                Pago ultimoPago = this.GetSessionFactory().GetSession().QueryOver<Pago>()
                                                                                          .GetFilterBySecurity()
                                                                                          .OrderBy(x => x.NumeroReferencia).Desc
                                                                                          .Take(1)
                                                                                          .SingleOrDefault();

                return ultimoPago != null ? ultimoPago.NumeroReferencia + 1 : 1;
            }

            public IList<Pago> GetAll(DateTime _start, DateTime _end)
            {
                return this.GetSessionFactory().GetSession().QueryOver<Pago>()
                    .Where(c => c.Activo && c.FechaCreacion >= _start && c.FechaCreacion <= _end)
                    .GetFilterBySecurity().List();
            }

            public IList<Pago> GetPagos(IList<int> Ids)
            {
                return this.GetSessionFactory().GetSession().QueryOver<Pago>()
                    .Where(c => c.Activo)
                    .WhereRestrictionOn(c => c.Id).IsIn(Ids.ToArray())
                    .GetFilterBySecurity().List();
            }
            public IList<Asociado> GetAllPorVencer()
            {
                DateTime mesActual = DateTime.Now;
                DateTime mesAnterior = new DateTime(mesActual.Year, mesActual.Month-1, 1);
                
                Asociado asoc = null;

                IList<Asociado> asociadosActivos = this.GetSessionFactory().GetSession().QueryOver<Asociado>().GetFilterBySecurity().List();
                Pago c = null;
                IList<Asociado> asociadosPagaron = this.GetSessionFactory().GetSession().QueryOver<Pago>(() => c)
                    .JoinAlias(() => c.Asociado, () => asoc)
                    .Where(() => c.Activo && c.FechaPeriodo >= mesAnterior && c.FechaPeriodo <= mesActual && asoc.Activo)
                    .Select(x => c.Asociado)
                    .GetFilterBySecurity()
                    //.TransformUsing(Transformers.AliasToBean<Asociado>())
                    .List<Asociado>();


                //IList<Asociado> asociadosPagaron = this.GetSessionFactory().GetSession().QueryOver<Pago>()
                //    .Where(c => c.Activo && c.FechaPeriodo >= mesAnterior && c.FechaPeriodo <= mesActual)
                //    .JoinAlias(c => c.Asociado, () => asociado)
                //    .Where(() => asociado.AbonoTotalmente == false)
                //    .Select(c => c.Asociado)
                //    .GetFilterBySecurity()
                //    .TransformUsing(Transformers.AliasToBean<Asociado>()).List<Asociado>();

                foreach(Asociado a in asociadosPagaron)
                {
                    asociadosActivos.Remove(a);
                }
                return asociadosActivos;
            }

            public IList<Asociado> GetAllVencidos()
            {
                DateTime mesActual = DateTime.Now;
                //DateTime mesAnterior = mesActual.AddMonths(-1);
                //DateTime mesPenUltimo = mesActual.AddMonths(-2);
                DateTime mesPenUltimo = new DateTime(mesActual.Year, mesActual.Month - 2, 1);

                Asociado asoc = null;

                IList<Asociado> asociadosActivos = this.GetSessionFactory().GetSession().QueryOver<Asociado>().GetFilterBySecurity().List();
                Pago c = null;
                IList<Asociado> asociadosPagaron = this.GetSessionFactory().GetSession().QueryOver<Pago>(() => c)
                    .JoinAlias(() => c.Asociado, () => asoc)
                    .Where(() => c.Activo && c.FechaPeriodo >= mesPenUltimo && c.FechaPeriodo <= mesActual && asoc.Activo)
                    .Select(x => c.Asociado)
                    .GetFilterBySecurity()
                    //.TransformUsing(Transformers.AliasToBean<Asociado>())
                    .List<Asociado>();


                //IList<Asociado> asociadosPagaron = this.GetSessionFactory().GetSession().QueryOver<Pago>()
                //    .Where(c => c.Activo && c.FechaPeriodo >= mesAnterior && c.FechaPeriodo <= mesActual)
                //    .JoinAlias(c => c.Asociado, () => asociado)
                //    .Where(() => asociado.AbonoTotalmente == false)
                //    .Select(c => c.Asociado)
                //    .GetFilterBySecurity()
                //    .TransformUsing(Transformers.AliasToBean<Asociado>()).List<Asociado>();

                foreach (Asociado a in asociadosPagaron)
                {
                    asociadosActivos.Remove(a);
                }
                return asociadosActivos;
            }

        //public IList<Pago> GetAll(DateTime _start, DateTime _end, Model.Common.EstadosGenericoFilter filter)
        //{
        //    //List<EstadosGenericoFilter> estados = ComprobanteHelper.GetEstadosByfilter(filter);
        //    Asociado a = null;
        //    Pago p = null;
        //    switch (filter)
        //    { 
        //        //case Todos:
        //        //    return this.GetSessionFactory().GetSession().QueryOver<Pago>()
        //        //        .Where(c => c.Activo && c.FechaCreacion >= _start && c.FechaCreacion <= _end)
        //        //        .GetFilterBySecurity().List();
        //        //    break;
        //        case Pagados:
        //            return this.GetSessionFactory().GetSession().QueryOver<Pago>()
        //                .Where(c => c.Activo && c.FechaCreacion >= _start && c.FechaCreacion <= _end)
        //                .GetFilterBySecurity().List();
        //            break;
        //        case Vencidos:
        //            this.
        //            this.GetSessionFactory().GetSession().QueryOver<Pago>(() => p)
        //                .JoinAlias(() => p.Asociado, () => a)
        //                .Where(() => a.AbonoTotalmente == false && p.FechaPeriodo )
        //                .WhereNot(() => p.FechaPeriodo )

        //            //return this.GetSessionFactory().GetSession().QueryOver<Pago>()
        //            //    .Where(c => c.Activo && c.FechaCreacion >= _start && c.FechaCreacion <= _end)
        //            //    .GetFilterBySecurity().List();
        //            break;
        //        case PorVencer:
        //            return this.GetSessionFactory().GetSession().QueryOver<Pago>()
        //                .Where(c => c.Activo && c.FechaCreacion >= _start && c.FechaCreacion <= _end)
        //                .GetFilterBySecurity().List();
        //            break;
        //        case Finalizados:
        //            return this.GetSessionFactory().GetSession().QueryOver<Pago>()
        //                .Where(c => c.Activo && c.FechaCreacion >= _start && c.FechaCreacion <= _end)
        //                .GetFilterBySecurity().List();
        //            break;
        //    }
        //}
    }
}
