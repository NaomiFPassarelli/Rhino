using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Tesoreria.Helpers;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public class PagoTarjetaRepository : BaseSecuredRepository<PagoTarjeta>, IPagoTarjetaRepository
    {
        public PagoTarjetaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<PagoTarjeta> GetAll()
        {
            IList<PagoTarjeta> list = this.GetSessionFactory().GetSession().QueryOver<PagoTarjeta>()
                .Where(c => c.Estado != EstadoPagoTarjeta.Borrador)
                .GetFilterBySecurity()
                .List();

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Cancelaciones = null;
            }
            return list;
        }

        public IList<PagoTarjeta> GetAllByDates(int Id, DateTime start, DateTime end, PagoTarjetaFilter filter)
        {
            IList<PagoTarjeta> list = this.GetSessionFactory().GetSession().QueryOver<PagoTarjeta>()
                                                        .Where(X => X.Fecha >= start && X.Fecha <= end && (X.Tarjeta.Id == Id || Id == 0) 
                                                                            && X.Estado != EstadoPagoTarjeta.Borrador)
                                                        .GetFilterBySecurity()
                                                        .GetFiltroPagosTarjetas(filter)
                                                        .OrderBy(x => x.Id).Desc
                                                        .List();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Cancelaciones = null;
                list[i].Usuario = null;
            }
            return list;
        }

        public PagoTarjeta GetCompleto(int Id)
        {
            PagoTarjeta p = this.GetSessionFactory().GetSession().QueryOver<PagoTarjeta>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Cancelaciones).Eager
                                                        .SingleOrDefault();
            for (int i = 0; i < p.Cancelaciones.Count; i++)
            {
                p.Cancelaciones[i].Pago = null;
                p.Cancelaciones[i].Asiento = new Asiento() { Id = p.Cancelaciones[i].Asiento.Id };
                p.Cancelaciones[i].Usuario = null;
            }

            return p;
        }
    }
}
