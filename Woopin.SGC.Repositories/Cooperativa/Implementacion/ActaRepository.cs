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
    public class ActaRepository : BaseSecuredRepository<Acta>, IActaRepository
    {
        public ActaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public Acta GetActaByFecha(DateTime endOfMonth)
        {

            return this.GetSessionFactory().GetSession().QueryOver<Acta>()
                .Where(c => c.FechaActa == endOfMonth)
                .GetFilterBySecurity()
                .SingleOrDefault();
        }


        public Acta GetActaCompleta(int ActaId)
        {
            //Acta a = this.GetSessionFactory().GetSession().QueryOver<Acta>()
            //    .Where(c => c.Id == ActaId)
            //    .Fetch(c => c.AsociadosIngreso).Eager
            //    .Fetch(c => c.AsociadosEgreso).Eager
            //    .Fetch(c => c.OtrosPuntos).Lazy
            //    .Fetch(c => c.Organizacion).Eager
            //    .GetFilterBySecurity().SingleOrDefault();

            //return a;

            Acta acta = this.GetSessionFactory().GetSession().QueryOver<Acta>()
                                                        .Where(x => x.Id == ActaId)
                                                        .GetFilterBySecurity()
                                                        //.Fetch(x => x.AsociadosIngreso).Eager
                                                        //.Fetch(x => x.AsociadosEgreso).Eager
                                                        //.Fetch(x => x.Organizacion).Eager
                                                        .SingleOrDefault();

            if (acta == null) return null;

            foreach (var ai in acta.AsociadosIngreso)
            {
                ai.ActaAlta = null;
            }

            foreach (var ae in acta.AsociadosEgreso)
            {
                ae.ActaBaja = null;
            }

            foreach (var op in acta.OtrosPuntos)
            {
                op.Acta = null;
            }

            return acta;

        }
        public IList<Acta> GetActas(IList<int> Ids)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Acta>()
                .WhereRestrictionOn(c => c.Id).IsIn(Ids.ToArray())
                .Fetch(c => c.Organizacion).Eager
                .GetFilterBySecurity().List();
        }

        public IList<Acta> GetAllActasCompletas()
        {
            //de esta forma no porque duplica/triplica
            //IList<Acta> A = this.GetSessionFactory().GetSession().QueryOver<Acta>()
            //       .Fetch(c => c.AsociadosIngreso).Eager
            //       .Fetch(c => c.AsociadosEgreso).Eager
            //    //.Fetch(c => c.OtrosPuntos).Eager
            //       .GetFilterBySecurity().List();


            IList<Acta> A = this.GetSessionFactory().GetSession().QueryOver<Acta>()
                                                        .GetFilterBySecurity().List();


            //foreach(Acta Acta in A)
            //{
            //    Acta.OtrosPuntos = null;
            //}
            return A;
        }

        public bool existActaNumero(int ActaNumero, int? IdUpdate)
        {
            Acta p = null;
            if (IdUpdate != null && IdUpdate > 0)
            {
                p = this.GetSessionFactory().GetSession().QueryOver<Acta>()
                    .Where(x => x.NumeroActa == ActaNumero && IdUpdate != x.Id).GetFilterBySecurity().SingleOrDefault();
            }
            else
            {
                p = this.GetSessionFactory().GetSession().QueryOver<Acta>()
                    .Where(x => x.NumeroActa == ActaNumero).GetFilterBySecurity().SingleOrDefault();
            }
            if (p != null)
                return true;
            return false;
        }

        
            
    }
}