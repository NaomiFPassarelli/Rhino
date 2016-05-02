using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Common.HtmlModel;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Helpers;
using Woopin.SGC.CommonApp.Session;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public class AsientoRepository : BaseSecuredRepository<Asiento>, IAsientoRepository
    {
        public AsientoRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public int GetProximoIdAsiento()
        {
            int ProximoId = 1;
            Asiento ultimoAsiento = this.GetSessionFactory().GetSession().QueryOver<Asiento>()
                                                                        .GetFilterBySecurity()
                                                                       .OrderBy(x => x.Id).Desc
                                                                       .Take(1)
                                                                       .SingleOrDefault();
            if (ultimoAsiento != null)
            {
                ProximoId = ultimoAsiento.Id + 1;
            }
                
            return ProximoId;
        }

        public Asiento GetCompleto(int Id)
        {
            Asiento a = this.GetSessionFactory().GetSession().QueryOver<Asiento>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Items).Eager
                                                        .Fetch(x => x.Usuario).Eager
                                                        .SingleOrDefault();
            a.Organizacion = null;
            a.Ejercicio.Organizacion = null;
            return a;
        }

        public IList<Asiento> GetAsientosFilter(DateTime start, DateTime end)
        {
            return this.GetSessionFactory().GetSession().QueryOver<Asiento>()
                                .Where(x => x.Fecha >= start && x.Fecha <= end)
                                .GetFilterBySecurity()
                                .OrderBy(x => x.Fecha)
                                .Desc.List();
        }
        public IList<LibroMayor> GetAsientosFilterCuenta(int id, DateTime start, DateTime end)
        {
            Asiento aalias = null;
            AsientoItem aialias = null;
            LibroMayor lalias = null;
            ChequePropio cAlias = null;

            IList<Asiento> asientos = new List<Asiento>();
            IList<LibroMayor> libroMayor = new List<LibroMayor>();
            
            libroMayor = this.GetSessionFactory().GetSession()
                    .QueryOver<AsientoItem>(() => aialias)
                    .JoinAlias(() => aialias.Asiento, () => aalias)
                    .Left.JoinAlias(() => aialias.ChequePropio, () => cAlias)
                    .Where(() => aalias.Fecha >= start && aalias.Fecha <= end)
                    .And(() => aialias.Cuenta.Id == id)
                    .And(() => aalias.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
                    .OrderBy(() => aalias.Fecha).Asc
                    .SelectList( i => i
                        .Select(() => aalias.Fecha).WithAlias(() => lalias.Fecha)
                        .Select(() => aalias.FechaCreacion).WithAlias(() => lalias.FechaCreacion)
                        .Select(() => aalias.Id).WithAlias(() => lalias.NumeroReferencia)
                        .Select(() => aalias.Leyenda).WithAlias(() => lalias.Leyenda)
                        .Select(() => aalias.Modulo).WithAlias(() => lalias.Modulo)
                        .Select(() => aialias.Debe).WithAlias(() => lalias.Debe)
                        .Select(() => aialias.Haber).WithAlias(() => lalias.Haber)
                        //.Select(() => aalias.Ejercicio).WithAlias(() => lalias.Ejercicio)
                        .Select(() => aalias.TipoOperacion).WithAlias(() => lalias.TipoOperacion)
                        .Select(() => aalias.ComprobanteAsociado).WithAlias(() => lalias.ComprobanteAsociado)
                        .Select(() => cAlias.Numero).WithAlias(() => lalias.NumeroCheque)
                        )
                    .TransformUsing(Transformers.AliasToBean<LibroMayor>())
                    .List<LibroMayor>();

            return libroMayor;
        }

        public LibroMayorHeader GetAsientosHeaderFilterCuenta(int id, DateTime start, DateTime end) 
        {
            LibroMayorHeader headerAlias = null;
            LibroMayorHeader headerAlias_sa = null;
            Asiento a = null;
            AsientoItem ai = null;

            headerAlias = this.GetSessionFactory().GetSession()
                    .QueryOver<Asiento>(() => a)
                    .JoinAlias(() => a.Items, () => ai)
                    .Where(() => a.Fecha >= start && a.Fecha <= end && ai.Cuenta.Id == id)
                    .GetFilterBySecurity()
                    .SelectList(list => list
                        .SelectGroup(() => ai.Cuenta.Id)
                        .SelectSum(() => ai.Debe)
                        .WithAlias(() => headerAlias.Debe)
                        .SelectSum(() => ai.Haber)
                        .WithAlias(() => headerAlias.Haber)
                        .Select(Projections.SqlFunction(
                                new VarArgsSQLFunction("(", "-", ")"),
                                NHibernateUtil.Decimal,
                                Projections.Sum(() => ai.Debe),
                                Projections.Sum(() => ai.Haber)))
                        .WithAlias(() => headerAlias.Saldo)
                        )
                    .TransformUsing(Transformers.AliasToBean<LibroMayorHeader>())
                    .Take(1)
                    .SingleOrDefault<LibroMayorHeader>();

            headerAlias_sa = this.GetSessionFactory().GetSession()
                    .QueryOver<Asiento>(() => a)
                    .JoinAlias(() => a.Items, () => ai)
                    .Where(() => a.Fecha < start && ai.Cuenta.Id == id)
                    .GetFilterBySecurity()
                    .SelectList(list => list
                        .SelectGroup(() => ai.Cuenta.Id)
                        .Select(Projections.SqlFunction(
                                new VarArgsSQLFunction("(", "-", ")"),
                                NHibernateUtil.Decimal,
                                Projections.Sum(() => ai.Debe),
                                Projections.Sum(() => ai.Haber)))
                        .WithAlias(() => headerAlias_sa.SaldoAnterior)
                        )
                        .TransformUsing(Transformers.AliasToBean<LibroMayorHeader>())
                        .Take(1)
                        .SingleOrDefault<LibroMayorHeader>();
            if (headerAlias != null && headerAlias_sa != null)
            {
                headerAlias.SaldoAnterior = headerAlias_sa.SaldoAnterior;
            }

            return headerAlias; 
        }

    }
}
