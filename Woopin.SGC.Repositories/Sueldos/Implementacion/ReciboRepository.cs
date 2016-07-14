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
    public class ReciboRepository : BaseSecuredRepository<Recibo>, IReciboRepository
    {
        public ReciboRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<Recibo> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<Recibo>().GetFilterBySecurity().List();
        }

        public IList<Recibo> GetAllByFilter(SelectComboRequest req)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                        .Where((Restrictions.On<Recibo>(x => x.Empleado.Nombre).IsLike('%' + req.where + '%')))
                                                        .GetFilterBySecurity()
                                                        .List();
        }
        public Recibo GetCompleto(int Id)
        {
            Recibo recibo = this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                        .Where(x => x.Id == Id)
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
                                                        .GetFilterBySecurity()
                                                        .List();
        }

        public int GetProximoNumeroReferencia()
        {
            Recibo ultimoRecibo = this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                                                      .GetFilterBySecurity()
                                                                                      .OrderBy(x => x.Id).Desc
                                                                                      .Take(1)
                                                                                      .SingleOrDefault();

            return ultimoRecibo != null ? ultimoRecibo.Id + 1 : 1;
        }

        public Recibo GetReciboAnterior(int IdEmpleado)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                                        .GetFilterBySecurity()
                                                                       .OrderBy(x => x.Id).Desc
                                                                       .Take(1)
                                                                       .SingleOrDefault();
        }

        public decimal GetMejorRemuneracion(int IdEmpleado)
        { 
            DateTime today = DateTime.Now;
            DateTime mitadAño = new DateTime(today.Year, 6, 30);
            Recibo reciboMejorRemuneracion = null;
            DateTime primerDia = new DateTime(today.Year, 1, 1);
            reciboMejorRemuneracion = this.GetSessionFactory().GetSession().QueryOver<Recibo>()
                                                                                  .GetFilterBySecurity()
                                                                                  .Where(c => c.Empleado.Id == IdEmpleado && (c.FechaInicio >= primerDia && c.FechaInicio <= mitadAño))
                                                                                  .OrderBy(c => c.TotalRemunerativo).Desc
                                                                                  .Take(1)
                                                                                  .SingleOrDefault();

            if (reciboMejorRemuneracion != null)
            {
                return reciboMejorRemuneracion.TotalRemunerativo;
            }
            return 0;
        }

    }
}
