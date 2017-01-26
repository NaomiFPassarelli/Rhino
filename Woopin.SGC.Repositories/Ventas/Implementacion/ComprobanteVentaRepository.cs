using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Helpers;
using Woopin.SGC.Common.Models;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Stock;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Ventas
{
    public class ComprobanteVentaRepository : BaseSecuredRepository<ComprobanteVenta>, IComprobanteVentaRepository
    {
        public ComprobanteVentaRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<ComprobanteVenta> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>()
                                                        .Where(c => c.Estado != EstadoComprobante.Anulada )
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Fecha).Desc.List();
        }

        public IList<ComprobanteVenta> GetAllByCliente(int IdCliente, DateTime _start, DateTime _end, DateTime startvenc, DateTime endvenc, Model.Common.CuentaCorrienteFilter filter)
        {
            List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(filter);
            
             IList<ComprobanteVenta> cs = this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>()
                                                        .GetFiltroCuentaCorriente(filter)
                                                        .GetByPermissions()
                                                        .Fetch(c => c.Cliente).Eager
                                                        .Fetch(c => c.Cliente.DireccionesEntrega).Eager
                                                        .Where(c => c.Fecha >= _start && c.Fecha <= _end && (c.FechaVencimiento >= startvenc && c.FechaVencimiento <= endvenc) && (c.Cliente.Id == IdCliente || IdCliente == 0))
                                                        .WhereRestrictionOn(c => c.Estado).IsIn(estados)
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Fecha).Desc
                                                        .List();
            
            return cs;
        }
     
        public ComprobanteVenta GetByLetrayNumero(string LetraYNumero)
        {
            ComprobanteVenta comp = new ComprobanteVenta();
            comp.SplitLetraNumero(LetraYNumero);
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>().Where(x => x.Letra == comp.Letra && x.Numero == comp.Numero && (x.Estado != EstadoComprobante.Anulada)).GetFilterBySecurity().SingleOrDefault();
        }

        public string GetProximoComprobante(string Letra, int TipoComprobante, int Talonario)
        {
            string ProximoNumeroComprobante = "";

            ComprobanteVenta ultimaComprobante = this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>()
                                                                                      .GetFiltroByTipoComprobante(TipoComprobante, Letra)
                                                                                      .GetFilterBySecurity()
                                                                                      .Where(x => x.Talonario.Id == Talonario )
                                                                                      .OrderBy(x => x.Numero).Desc
                                                                                      .Take(1).SingleOrDefault();
            // No tiene Comprobante
            if (ultimaComprobante == null) 
            {
                Talonario t = this.GetSessionFactory().GetSession().QueryOver<Talonario>().Where(x => x.Id == Talonario).SingleOrDefault();
                return t.Prefijo + "-" + "00000001";
            }
            else
            {
                string[] talonarioNumero = ultimaComprobante.Numero.Split('-');
                if (int.Parse(talonarioNumero[1]) < 99999999)
                {
                    talonarioNumero[1] = (int.Parse(talonarioNumero[1]) + 1).ToString("00000000");
                }
                else
                {
                    talonarioNumero[0] = (int.Parse(talonarioNumero[0]) + 1).ToString("0000");
                    talonarioNumero[1] = "00000001";
                }
                ProximoNumeroComprobante = talonarioNumero[0] + '-' + talonarioNumero[1];
            }

            return ProximoNumeroComprobante;
        }

        public ComprobanteVenta GetComprobanteVentaCompleto(int Id)
        {
            ComprobanteVenta comprobante =  this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Detalle).Eager
                                                        .Fetch(x => x.Cliente).Eager
                                                        .Fetch(x => x.Cliente.DireccionesEntrega).Eager
                                                        .Fetch(x => x.Talonario).Eager
                                                        .Fetch(x => x.Organizacion).Eager
                                                        .SingleOrDefault();

            if (comprobante == null) return null;

            foreach (var detalle in comprobante.Detalle)
            {
                detalle.Comprobante = null;
            }
            // Si lo hago por fectch en los joins se multiplica.
            NHibernateUtil.Initialize(comprobante.Observaciones);
            
            return comprobante;
        }

        public IList<ComprobanteVenta> GetComprobantesVentasACobrar(int IdCliente)
        {
            ComprobanteVenta c = null;
            ComboItem ci = null;
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>(() => c)
                                                .JoinAlias(() => c.Tipo, () => ci)
                                                .Where(() => (c.Cliente.Id == IdCliente) && (c.Total > c.TotalCobrado) && (c.Estado != EstadoComprobante.Anulada) && ci.AdditionalData != "-1")
                                                .GetFilterBySecurity().OrderBy(() => c.Fecha).Desc.List();
        }

        public VentasCuentaCorriente LoadCtaCorrienteHead(int id, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter)
        {
            List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(filter);
            ComprobanteVenta c = null;
            VentasCuentaCorriente header = null;
            DateTime hoy = DateTime.Now;
            ComboItem tipo = null;

            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>()
                                                        .SelectList(list =>
                                                            list.SelectSubQuery(QueryOver.Of<ComprobanteVenta>(() => c)
                                                                .JoinAlias(() => c.Tipo, () => tipo)    
                                                                .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Cliente.Id == id || id == 0) && c.FechaVencimiento < hoy && (c.Estado != EstadoComprobante.Anulada) && tipo.AdditionalData != "-1")
                                                                .And(() => c.Organizacion.Id == Security.GetOrganizacion().Id)
                                                                .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                                .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Total),
                                                                        Projections.Sum(() => c.TotalCobrado))))
                                                                .Take(1)
                                                                ).WithAlias(() => header.DeudaVencida)
                                                            .SelectSubQuery(QueryOver.Of<ComprobanteVenta>(() => c)
                                                                .JoinAlias(() => c.Tipo, () => tipo)    
                                                                .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Cliente.Id == id || id == 0) && c.FechaVencimiento >= hoy && tipo.AdditionalData != "-1" && (c.Estado != EstadoComprobante.Anulada))
                                                                .And(() => c.Organizacion.Id == Security.GetOrganizacion().Id)
                                                                .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                                .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Total),
                                                                        Projections.Sum(() => c.TotalCobrado))))
                                                                .Take(1)
                                                                ).WithAlias(() => header.DeudaNoVencida)
                                                            .SelectSubQuery(QueryOver.Of<ComprobanteVenta>(() => c)
                                                                .JoinAlias(() => c.Tipo, () => tipo)    
                                                                .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Cliente.Id == id || id == 0) && tipo.AdditionalData != "-1" && (c.Estado != EstadoComprobante.Anulada))
                                                                .And(() => c.Organizacion.Id == Security.GetOrganizacion().Id)
                                                                .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                                .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Total),
                                                                        Projections.Sum(() => c.TotalCobrado))))
                                                                .Take(1)
                                                                ).WithAlias(() => header.Saldo)
                                                        )
                                                        .TransformUsing(Transformers.AliasToBean<VentasCuentaCorriente>())
                                                        .Take(1)
                                                        .SingleOrDefault<VentasCuentaCorriente>();
        }

        public List<CuentaCorrienteItem> GetCuentaCorrienteByDates(int id, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter)
        {
            List<CuentaCorrienteItem> items = new List<CuentaCorrienteItem>();
            List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(filter);
            ComprobanteVenta c = null;
            DateTime hoy = DateTime.Now;
            CuentaCorrienteItem cci = null;
            ComboItem tipo = null;
            Cobranza co = null;
            Cliente clienteAlias = null;

            IList<CuentaCorrienteItem> ventasItems = this.GetSessionFactory().GetSession()
                                                            .QueryOver<ComprobanteVenta>(() => c)
                                                            .JoinAlias(() => c.Tipo, () => tipo)
                                                            .JoinAlias(() => c.Cliente, () => clienteAlias)
                                                            .GetFiltroCuentaCorriente(filter)
                                                            .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Cliente.Id == id || id == 0) && (c.Estado != EstadoComprobante.Anulada))
                                                            .GetFilterBySecurity()
                                                            .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                            .SelectList(i => i
                                                                .Select(() => c.Id).WithAlias(() => cci.NroReferencia)
                                                                .Select(() => c.Fecha).WithAlias(() => cci.Fecha)
                                                                .Select(() => clienteAlias.RazonSocial).WithAlias(() => cci.Empresa)
                                                                .Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "+", ")"),
                                                                        NHibernateUtil.String,
                                                                        Projections.Property(() => c.Letra),
                                                                        Projections.Property(() => c.Numero)))
                                                                        .WithAlias(() => cci.LetraNumero)
                                                                .Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Property(() => c.Total),
                                                                        Projections.Property(() => c.TotalCobrado)))
                                                                    .WithAlias(() => cci.Pendiente)
                                                                .Select(() => tipo.Data).WithAlias(() => cci.TipoComprobante)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => tipo.AdditionalData == "-1"),
                                                                        Projections.Constant(0, NHibernateUtil.Decimal),
                                                                        Projections.Property(() => c.Total))).WithAlias(() => cci.Debe)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => tipo.AdditionalData != "-1"),
                                                                        Projections.Constant(0,NHibernateUtil.Decimal),
                                                                        Projections.Property(() => c.Total))).WithAlias(() => cci.Haber)
                                                                .Select(Projections.Constant("ComprobanteVenta")).WithAlias(() => cci.Entidad)
                                                            )
                                                            .TransformUsing(Transformers.AliasToBean<CuentaCorrienteItem>())
                                                            .List<CuentaCorrienteItem>();
            items.AddRange(ventasItems);

            List<EstadoComprobanteCancelacion> estadosCancelacion = CobranzaHelper.GetEstadosByfilter(filter);


            IList<CuentaCorrienteItem> cobrosItems = this.GetSessionFactory().GetSession()
                                                .QueryOver<Cobranza>(() => co)
                                                .JoinAlias(() => co.Tipo, () => tipo)
                                                .JoinAlias(() => co.Cliente, () => clienteAlias)
                                                .Where(() => co.Fecha >= _start && co.Fecha <= _end && (co.Cliente.Id == id || id == 0) )
                                                .GetFilterBySecurity()
                                                .WhereRestrictionOn(() => co.Estado).IsIn(estadosCancelacion)
                                                .SelectList(i => i
                                                    .Select(() => co.Id).WithAlias(() => cci.NroReferencia)
                                                    .Select(() => co.Fecha).WithAlias(() => cci.Fecha)
                                                    .Select(() => clienteAlias.RazonSocial).WithAlias(() => cci.Empresa)
                                                    .Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "+", ")"),
                                                                        NHibernateUtil.String,
                                                                        Projections.Constant(""),
                                                                        Projections.Property(() => co.Numero)))
                                                                        .WithAlias(() => cci.LetraNumero)
                                                    .Select(() => tipo.Data).WithAlias(() => cci.TipoComprobante)
                                                    .Select(() => co.Total).WithAlias(() => cci.Haber)
                                                    .Select(Projections.Constant("Cobranza")).WithAlias(() => cci.Entidad)
                                                )
                                                .TransformUsing(Transformers.AliasToBean<CuentaCorrienteItem>())
                                                .List<CuentaCorrienteItem>();
            items.AddRange(cobrosItems);

            return items.OrderByDescending(x => x.Fecha).ToList();
        }

        public IList<ReporteVenta> GetVencimientosACobrar()
        {
            Cliente cAlias = null;
            ComprobanteVenta cvAlias = null;
            ComboItem tipoAlias = null;

            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>(() => cvAlias)
                                                .JoinAlias(() => cvAlias.Tipo, () => tipoAlias)
                                                .Where(() => cvAlias.Total > cvAlias.TotalCobrado && cvAlias.Estado != EstadoComprobante.Anulada)
                                                .GetFilterBySecurity()
                                                .JoinAlias(() => cvAlias.Cliente, () => cAlias)
                                                .OrderBy(() => cAlias.RazonSocial).Asc
                                                .ThenBy(() => cvAlias.FechaVencimiento).Asc
                                                .List<ComprobanteVenta>()
                                                .Select(x => new ReporteVenta()
                                                {
                                                    Id = x.Id,
                                                    Cliente = x.Cliente.RazonSocial,
                                                    Comprobante = x.Letra + x.Numero,
                                                    Fecha = x.Fecha,
                                                    SemanaFecha = DateHelper.GetWeek(x.Fecha).ToString("dd/MM/yyyy","al"),
                                                    FechaVencimiento = x.FechaVencimiento,
                                                    SemanaVencimiento = DateHelper.GetWeek(x.FechaVencimiento).ToString("dd/MM/yyyy","al"),
                                                    Importe = x.Total - x.TotalCobrado * ( x.Tipo.AdditionalData != "-1" ? 1 : -1),
                                                    TipoComprobante = x.Tipo.Data,
                                                    FechaEstipuladaCobro = x.Cliente.CondicionVentaEstadistica > 0 ?
                                                        x.Fecha.AddDays( (double)x.Cliente.CondicionVentaEstadistica) : x.FechaVencimiento,
                                                    SemanaEstipuladaCobro = ComprobanteVentaHelper.GetSemanaEstipuladaCobro(x),
                                                    ContactoCobro = x.NombreCobro + " (" + x.MailCobro + ")",
                                                    Observacion = x.Observacion
                                                })
                                                .ToList();
        }

        public IList<ReporteVentasArticulo> GetReporteVentasArticulo(int IdCliente, DateTime start, DateTime end)
        {
            DetalleComprobanteVenta dcvAlias = null;
            ComprobanteVenta cvAlias = null;
            ReporteVentasArticulo rvsAlias = null;
            Articulo aAlias = null;
            ComboItem tipoAlias = null;

            IList<ReporteVentasArticulo> ret = this.GetSessionFactory().GetSession().QueryOver<DetalleComprobanteVenta>(() => dcvAlias)
                                                .JoinAlias(() => dcvAlias.Comprobante, () => cvAlias)
                                                .JoinAlias(() => dcvAlias.Articulo, () => aAlias)
                                                .JoinAlias(() => cvAlias.Tipo, () => tipoAlias)
                                                .Where(() => (cvAlias.Cliente.Id == IdCliente || IdCliente == 0) && cvAlias.Fecha >= start && cvAlias.Fecha <= end && cvAlias.Estado != EstadoComprobante.Anulada)
                                                .And(() => cvAlias.Organizacion.Id == Security.GetOrganizacion().Id)
                                                .SelectList(list =>
                                                    list.SelectGroup(() => dcvAlias.Articulo.Id).WithAlias(() => rvsAlias.IdArticulo)
                                                        .SelectGroup(() => aAlias.Descripcion).WithAlias(() => rvsAlias.Descripcion)
                                                        .Select(Projections.SqlGroupProjection(
                                                                "YEAR(Fecha) As [Year]",
                                                                "YEAR(Fecha)",
                                                                new[] { "YEAR" },
                                                                new IType[] { NHibernateUtil.Int32 }))
                                                            .WithAlias(() => rvsAlias.Year)
                                                        .Select(Projections.SqlGroupProjection(
                                                                "MONTH(Fecha) As [Month]",
                                                                "MONTH(Fecha)",
                                                                new[] { "MONTH" },
                                                                new IType[] { NHibernateUtil.Int32 }))
                                                            .WithAlias(() => rvsAlias.Mes)
                                                        .SelectSum(() => dcvAlias.Cantidad).WithAlias(() => rvsAlias.Cantidad)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => tipoAlias.AdditionalData == "-1"),
                                                                    Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Constant(-1),
                                                                        Projections.Property(() => dcvAlias.Total)),
                                                                    Projections.Property(() => dcvAlias.Total)))).WithAlias(() => rvsAlias.Total)
                                                )
                                                .TransformUsing(Transformers.AliasToBean<ReporteVentasArticulo>())
                                                .List<ReporteVentasArticulo>();

            foreach(var item in ret)
            {
                item.MonthGroup = item.Year.ToString() + " - " + DateHelper.GetMonthName(item.Mes);
                item.MonthCode = int.Parse(item.Year.ToString() + item.Mes.ToString("00"));
            }

            return ret.OrderByDescending(x => x.MonthCode).ToList();             
        }


        // Parametros
        // Tipo : Tipo de comprobante, 0 trae todo.
        // NoTipo: Que tipo no trae, 0 trae todo.
        // Pagada: 2 Pago pendiente, 1 Pago total, 0 trae todo.
        public IList<ComprobanteVenta> GetAllByClienteFilterNC(int IdCliente, int Tipo, int NoTipo, ComprobantesACancelarFilter Cobrada)
        {
            ComboItem ciAlias = null;
            ComprobanteVenta cvAlias = null;


            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>(() => cvAlias)
                                                        .JoinAlias(() => cvAlias.Tipo, () => ciAlias)
                                                        .Where(() =>
                                                                    (ciAlias.AdditionalData == Tipo.ToString() || Tipo == 0) &&
                                                                    (ciAlias.AdditionalData != NoTipo.ToString() || NoTipo == 0) &&
                                                                    (cvAlias.Estado != EstadoComprobante.Anulada) &&
                                                                    (cvAlias.Cliente.Id == IdCliente || IdCliente == 0) &&
                                                                    (Cobrada == ComprobantesACancelarFilter.Todos ||
                                                                    (Cobrada == ComprobantesACancelarFilter.Cancelados && cvAlias.Total == cvAlias.TotalCobrado) ||
                                                                    (Cobrada == ComprobantesACancelarFilter.Pendientes && cvAlias.Total > cvAlias.TotalCobrado)
                                                                    ))
                                                                    .GetFilterBySecurity()
                                                                .OrderBy(x => x.Fecha).Desc
                                                                .List();
        }

        public IList<Cliente> GetAllClientesDeudores()
        {
            Cliente cAlias = null;
            ComprobanteVenta cvAlias = null;
            ComboItem ci = null;
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>(() => cvAlias)
                                                .JoinAlias(() => cvAlias.Tipo, () => ci)
                                                .Where(() => cvAlias.Total > cvAlias.TotalCobrado && (cvAlias.Estado != EstadoComprobante.Anulada) && ci.AdditionalData != "-1")
                                                .GetFilterBySecurity()
                                                .JoinAlias(() => cvAlias.Cliente, () => cAlias)
                                                .OrderBy(() => cAlias.RazonSocial).Asc
                                                .SelectList(list =>
                                                    list.SelectGroup(() => cAlias))
                                                .TransformUsing(Transformers.AliasToBean<Cliente>())
                                                .List<Cliente>();
                                                
        }

        public IList<ComprobanteVenta> GetAllComprobantesPendientes()
        {
            List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(CuentaCorrienteFilter.Pendientes);

            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>()
                                                        .GetFiltroCuentaCorriente(CuentaCorrienteFilter.Pendientes)
                                                        .GetFilterBySecurity()
                                                        .WhereRestrictionOn(c => c.Estado).IsIn(estados)
                                                        .OrderBy(x => x.MailCobro).Desc
                                                        .List();
        }


        #region Reportes

        public IList<ReporteCitiItem> GetCitiVentas(DateTime start, DateTime end)
        {
            DetalleComprobanteVenta dcvAlias = null;
            ComprobanteVenta cvAlias = null;
            ReporteCitiItem rcAlias = null;
            ComboItem tipoAlias = null;
            Cliente cAlias = null;


            return this.GetSessionFactory().GetSession().QueryOver<DetalleComprobanteVenta>(() => dcvAlias)
                                                .JoinAlias(() => dcvAlias.Comprobante, () => cvAlias)
                                                .JoinAlias(() => cvAlias.Cliente, () => cAlias)
                                                .JoinAlias(() => cvAlias.Tipo, () => tipoAlias)
                                                .Where(() => cvAlias.Estado != EstadoComprobante.Anulada && cvAlias.Fecha >= start && cvAlias.Fecha <= end)
                                                .And(() => cvAlias.Organizacion.Id == Security.GetOrganizacion().Id)
                                                .SelectList(list =>
                                                    list.SelectGroup(() => cAlias.RazonSocial).WithAlias(() => rcAlias.RazonSocial)
                                                        .SelectGroup(() => tipoAlias.AfipData).WithAlias(() => rcAlias.CodigoAfipComprobante)
                                                        .SelectGroup(() => tipoAlias.Data).WithAlias(() => rcAlias.TipoComprobante)
                                                        .SelectGroup(() => cvAlias.Numero).WithAlias(() => rcAlias.Comprobante)
                                                        .SelectGroup(() => cvAlias.IVA21).WithAlias(() => rcAlias.IVA21)
                                                        .SelectGroup(() => cvAlias.IVA105).WithAlias(() => rcAlias.IVA105)
                                                        .SelectGroup(() => cvAlias.IVA27).WithAlias(() => rcAlias.IVA27)
                                                        .SelectGroup(() => cvAlias.Fecha).WithAlias(() => rcAlias.Fecha)
                                                        .SelectGroup(() => cvAlias.Letra).WithAlias(() => rcAlias.Letra)
                                                        .SelectGroup(() => tipoAlias.Id).WithAlias(() => rcAlias.IdTipoComprobante)
                                                        .SelectGroup(() => cvAlias.IVA).WithAlias(() => rcAlias.IVA)
                                                        .SelectGroup(() => cAlias.CUIT).WithAlias(() => rcAlias.CUIT)
                                                        .SelectGroup(() => cvAlias.ImporteExento).WithAlias(() => rcAlias.Exento)
                                                        .SelectGroup(() => cvAlias.Total).WithAlias(() => rcAlias.Total)
                                                        .SelectGroup(() => cvAlias.FechaVencimiento).WithAlias(() => rcAlias.FechaVto)
                                                        .SelectGroup(() => cvAlias.ImporteNoGravado).WithAlias(() => rcAlias.NoGravado)
                                                        .Select(Projections.Constant(1, NHibernateUtil.Decimal)).WithAlias(() => rcAlias.Cotizacion)
                                                        .Select(Projections.Constant("PES")).WithAlias(() => rcAlias.CodigoMoneda)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => dcvAlias.TipoIva.Id == 93),
                                                                    Projections.Property(() => dcvAlias.Total),
                                                                    Projections.Constant(0, NHibernateUtil.Decimal)))).WithAlias(() => rcAlias.NetoGravado105)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => dcvAlias.TipoIva.Id == 94),
                                                                    Projections.Property(() => dcvAlias.Total),
                                                                    Projections.Constant(0, NHibernateUtil.Decimal)))).WithAlias(() => rcAlias.NetoGravado21)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => dcvAlias.TipoIva.Id == 95),
                                                                    Projections.Property(() => dcvAlias.Total),
                                                                    Projections.Constant(0, NHibernateUtil.Decimal)))).WithAlias(() => rcAlias.NetoGravado27)
                                                )
                                                .TransformUsing(Transformers.AliasToBean<ReporteCitiItem>())
                                                .List<ReporteCitiItem>();
        }


        public IList<ReporteAcumulado> GetVentasPorClientes(DateTime start, DateTime end)
        {
            DetalleComprobanteVenta dcvAlias = null;
            ComprobanteVenta cvAlias = null;
            ReporteAcumulado rvsAlias = null;
            ComboItem tipoAlias = null;
            Cliente cAlias = null;


            return this.GetSessionFactory().GetSession().QueryOver<DetalleComprobanteVenta>( () => dcvAlias)
                                                .JoinAlias(() => dcvAlias.Comprobante, () => cvAlias)
                                                .JoinAlias(() => cvAlias.Cliente, () => cAlias)
                                                .JoinAlias(() => cvAlias.Tipo, () => tipoAlias)
                                                .Where(() => cvAlias.Estado != EstadoComprobante.Anulada && cvAlias.Fecha >= start && cvAlias.Fecha <= end)
                                                .And(() => cvAlias.Organizacion.Id == Security.GetOrganizacion().Id)
                                                .SelectList(list =>
                                                    list.SelectGroup(() => cAlias.RazonSocial).WithAlias(() => rvsAlias.Cliente)
                                                        .SelectGroup(() => cAlias.Id).WithAlias(() => rvsAlias.Id)
                                                        .SelectSum(() => dcvAlias.Cantidad).WithAlias(() => rvsAlias.Cantidad)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => tipoAlias.AdditionalData == "-1"),
                                                                    Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Constant(-1),
                                                                        Projections.Property(() => dcvAlias.Total)),
                                                                    Projections.Property(() => dcvAlias.Total)))).WithAlias(() => rvsAlias.Subtotal)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => tipoAlias.AdditionalData == "-1"),
                                                                    Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Constant(-1),
                                                                        Projections.Property(() => dcvAlias.TotalConIVA)),
                                                                    Projections.Property(() => dcvAlias.TotalConIVA)))).WithAlias(() => rvsAlias.Total)
                                                )
                                                .TransformUsing(Transformers.AliasToBean<ReporteAcumulado>())
                                                .List<ReporteAcumulado>();
        }

        #endregion
    }
}
