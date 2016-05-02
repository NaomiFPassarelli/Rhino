using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Helpers;
using Woopin.SGC.Common.LinqHelpers;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Repositories.Compras.Helpers;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Compras
{
    public class ComprobanteCompraRepository : BaseSecuredRepository<ComprobanteCompra>, IComprobanteCompraRepository
    {
        public ComprobanteCompraRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }

        public override IList<ComprobanteCompra> GetAll()
        {
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                                                        .Where(c => c.Estado != EstadoComprobante.Anulada)
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Fecha).Desc.List();
        }

        public IList<ComprobanteCompra> GetAllByProveedor(int IdProveedor, DateTime start, DateTime end, DateTime startvenc, DateTime endvenc, Model.Common.CuentaCorrienteFilter filter)
        {
            List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(filter);
            
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                                                        .GetFiltroCuentaCorriente(filter)         
                                                        .GetByPermissions()
                                                        .GetFilterBySecurity()
                                                        .Where(c => c.Fecha >= start && c.Fecha <= end && (c.FechaVencimiento >= startvenc && c.FechaVencimiento <= endvenc) && (c.Proveedor.Id == IdProveedor || IdProveedor == 0))
                                                        .WhereRestrictionOn(c => c.Estado).IsIn(estados)
                                                        .OrderBy(x => x.Fecha).Desc
                                                        .List();
        }
        public IList<ComprobanteCompra> GetAllAPagarByProv(int IdProveedor)
        {
            ComboItem ci = null;
            ComprobanteCompra c = null;
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>(() => c)
                                                        .JoinAlias(() => c.Tipo, () => ci)
                                                        .Where(() => c.Total > c.TotalPagado && (c.Proveedor.Id == IdProveedor || IdProveedor == 0)
                                                            && (c.Estado != EstadoComprobante.Anulada) && ci.AdditionalData != "-1")
                                                        .GetFilterBySecurity()
                                                        .OrderBy(x => x.Fecha).Desc
                                                        .List();
        }
        
        public int GetProximoNumeroReferencia()
        {
            ComprobanteCompra ultimaComprobante = this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                                                                                      .GetFilterBySecurity()
                                                                                      .OrderBy(x => x.Id).Desc
                                                                                      .Take(1)
                                                                                      .SingleOrDefault();

            return ultimaComprobante != null ? ultimaComprobante.Id + 1 : EntidadHelper.PrimerNumeroReferencia;
        }
        public ComprobanteCompra GetCompleto(int Id)
        {
            ComprobanteCompra comprobante = this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                                                        .Where(x => x.Id == Id)
                                                        .GetFilterBySecurity()
                                                        .Fetch(x => x.Detalle).Eager
                                                        .Fetch(x => x.Proveedor).Eager
                                                        .Fetch(x => x.Organizacion).Eager
                                                        .SingleOrDefault();
            
            if (comprobante == null) return null;

            foreach (var detalle in comprobante.Detalle)
            {
                detalle.Comprobante = null;
            }
            
            return comprobante;
        }
        public ComprasCuentaCorriente LoadCtaCorrienteHead(int id, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter)
        {
            List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(filter);
            ComprobanteCompra c = null;
            ComprasCuentaCorriente header = null;
            DateTime hoy = DateTime.Now;

            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                                                        .SelectList( list =>
                                                            list.SelectSubQuery(QueryOver.Of<ComprobanteCompra>(() => c)
                                                                .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Proveedor.Id == id || id == 0) && c.FechaVencimiento < hoy && (c.Estado != EstadoComprobante.Anulada))
                                                                .And(() => c.Organizacion.Id == Security.GetOrganizacion().Id)
                                                                .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                                .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Total),
                                                                        Projections.Sum(() => c.TotalPagado))))
                                                                .Take(1)
                                                                ).WithAlias(() => header.DeudaVencida)
                                                            .SelectSubQuery(QueryOver.Of<ComprobanteCompra>(() => c)
                                                                .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Proveedor.Id == id || id == 0) && c.FechaVencimiento >= hoy)
                                                                .And(() => c.Organizacion.Id == Security.GetOrganizacion().Id)
                                                                .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                                .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Total),
                                                                        Projections.Sum(() => c.TotalPagado))))
                                                                .Take(1)
                                                                ).WithAlias(() => header.DeudaNoVencida)
                                                            .SelectSubQuery(QueryOver.Of<ComprobanteCompra>(() => c)
                                                                .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Proveedor.Id == id || id == 0))
                                                                .And(() => c.Organizacion.Id == Security.GetOrganizacion().Id)
                                                                .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                                .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Total),
                                                                        Projections.Sum(() => c.TotalPagado))))
                                                                .Take(1)
                                                                ).WithAlias(() => header.Saldo)
                                                        )
                                                        .TransformUsing(Transformers.AliasToBean<ComprasCuentaCorriente>())
                                                        .Take(1)
                                                        .SingleOrDefault<ComprasCuentaCorriente>();
        }
        public List<CuentaCorrienteItem> GetCuentaCorrienteByDates(int id, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter)
        {
            List<CuentaCorrienteItem> items = new List<CuentaCorrienteItem>();
            List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(filter);
            List<EstadoComprobanteCancelacion> estadosCancelacion = OrdenPagoHelper.GetEstadosByfilter(filter);
            ComprobanteCompra c = null;
            DateTime hoy = DateTime.Now;
            CuentaCorrienteItem cci = null;
            ComboItem tipo = null;
            OrdenPago o = null;
            Proveedor pAlias = null;

            IList<CuentaCorrienteItem> comprasItems =  this.GetSessionFactory().GetSession()
                                                            .QueryOver<ComprobanteCompra>( () => c)
                                                            .JoinAlias(() => c.Tipo, () => tipo)
                                                            .JoinAlias(() => c.Proveedor, () => pAlias)
                                                            .GetFiltroCuentaCorriente(filter)
                                                            .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Proveedor.Id == id || id == 0) && (c.Estado != EstadoComprobante.Anulada))
                                                            .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                            .GetFilterBySecurity()
                                                            .SelectList(i => i
                                                                .Select(() => c.Id).WithAlias(() => cci.NroReferencia)
                                                                .Select(() => pAlias.RazonSocial).WithAlias(() => cci.Empresa)
                                                                .Select(() => c.Fecha).WithAlias(() => cci.Fecha)
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
                                                                        Projections.Property(() => c.TotalPagado)))
                                                                    .WithAlias(() => cci.Pendiente)
                                                                .Select(() => tipo.Data).WithAlias(() => cci.TipoComprobante)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => tipo.AdditionalData != "-1"),
                                                                        Projections.Constant(0, NHibernateUtil.Decimal),
                                                                        Projections.Property(() => c.Total))).WithAlias(() => cci.Debe)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => tipo.AdditionalData == "-1"),
                                                                        Projections.Constant(0, NHibernateUtil.Decimal),
                                                                        Projections.Property(() => c.Total))).WithAlias(() => cci.Haber)
                                                                .Select(Projections.Constant("ComprobanteCompra")).WithAlias(() => cci.Entidad)
                                                            )
                                                            .TransformUsing(Transformers.AliasToBean<CuentaCorrienteItem>())
                                                            .List<CuentaCorrienteItem>();
            items.AddRange(comprasItems);
            IList<CuentaCorrienteItem> pagosItems = this.GetSessionFactory().GetSession()
                                                .QueryOver<OrdenPago>(() => o).JoinAlias(() => o.Tipo, () => tipo)
                                                .GetFilterBySecurity()
                                                .JoinAlias(() => o.Proveedor, () => pAlias)
                                                .Where(() => o.Fecha >= _start && o.Fecha <= _end && (o.Proveedor.Id == id || id == 0) && o.Estado != EstadoComprobanteCancelacion.Anulada)
                                                .WhereRestrictionOn(() => o.Estado).IsIn(estadosCancelacion)
                                                .SelectList(i => i
                                                    .Select(() => o.Id).WithAlias(() => cci.NroReferencia)
                                                    .Select(() => pAlias.RazonSocial).WithAlias(() => cci.Empresa)
                                                    .Select(() => o.Fecha).WithAlias(() => cci.Fecha)
                                                    .Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "+", ")"),
                                                                        NHibernateUtil.String,
                                                                        Projections.Constant(""),
                                                                        Projections.Property(() => o.Id)))
                                                                        .WithAlias(() => cci.LetraNumero)
                                                    .Select(() => tipo.Data).WithAlias(() => cci.TipoComprobante)
                                                    .Select(() => o.Total).WithAlias(() => cci.Debe)
                                                    .Select(Projections.Constant("OrdenPago")).WithAlias(() => cci.Entidad)
                                                )
                                                .TransformUsing(Transformers.AliasToBean<CuentaCorrienteItem>())
                                                .List<CuentaCorrienteItem>();
            items.AddRange(pagosItems);
            return items.OrderByDescending(x => x.Fecha).ToList();
        }

        public IList<ReporteComprasRubros> GetReporteRubros(int IdProveedor, DateTime start, DateTime end)
        {
            DetalleComprobanteCompra dccAlias = null;
            ComprobanteCompra ccAlias = null;
            ReporteComprasRubros rcbAlias = null;
            RubroCompra rcAlias = null;
            ComboItem tipoAlias = null;

            IList<ReporteComprasRubros> ret = this.GetSessionFactory().GetSession().QueryOver<DetalleComprobanteCompra>(() => dccAlias)
                                                .JoinAlias(() => dccAlias.Comprobante, () => ccAlias)
                                                .JoinAlias(() => dccAlias.RubroCompra, () => rcAlias)
                                                .JoinAlias(() => ccAlias.Tipo, () => tipoAlias)
                                                .Where(() => (ccAlias.Proveedor.Id == IdProveedor || IdProveedor == 0) && ccAlias.Fecha >= start && ccAlias.Fecha <= end && (ccAlias.Estado != EstadoComprobante.Anulada))
                                                .And(() => ccAlias.Organizacion.Id == Security.GetOrganizacion().Id)
                                                .SelectList(list =>
                                                    list.SelectGroup(() => dccAlias.RubroCompra.Id).WithAlias(() => rcbAlias.IdRubro)
                                                        .SelectGroup(() => rcAlias.Descripcion).WithAlias(() => rcbAlias.Descripcion)
                                                        .Select(Projections.SqlGroupProjection(
                                                                "YEAR(Fecha) As [Year]",
                                                                "YEAR(Fecha)",
                                                                new[] { "YEAR" },
                                                                new IType[] { NHibernateUtil.Int32 }))
                                                            .WithAlias(() => rcbAlias.Year)
                                                        .Select(Projections.SqlGroupProjection(
                                                                "MONTH(Fecha) As [Month]",
                                                                "MONTH(Fecha)",
                                                                new[] { "MONTH" },
                                                                new IType[] { NHibernateUtil.Int32 }))
                                                            .WithAlias(() => rcbAlias.Mes)
                                                        .SelectSum(() => dccAlias.Total).WithAlias(() => rcbAlias.Total)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => tipoAlias.AdditionalData == "-1"),
                                                                    Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Constant(-1),
                                                                        Projections.Property(() => dccAlias.Total)),
                                                                    Projections.Property(() => dccAlias.Total)))).WithAlias(() => rcbAlias.Total)
                                                )
                                                .TransformUsing(Transformers.AliasToBean<ReporteComprasRubros>())
                                                .List<ReporteComprasRubros>();

            foreach (var item in ret)
            {
                item.MonthGroup = item.Year.ToString() + " - " + DateHelper.GetMonthName(item.Mes);
                item.MonthCode = int.Parse(item.Year.ToString() + item.Mes.ToString("00"));
            }

            return ret.OrderByDescending(x => x.MonthCode).ToList();
        }


        public IList<ReporteCompra> GetVencimientosAPagar()
        {
            Proveedor pAlias = null;
            ComprobanteCompra ccAlias = null;
            ComboItem tipoAlias = null;

            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>(() => ccAlias)
                                                .JoinAlias(() => ccAlias.Tipo, () => tipoAlias)
                                                .Where(() => ccAlias.Total > ccAlias.TotalPagado && (ccAlias.Estado != EstadoComprobante.Anulada))
                                                .GetFilterBySecurity()
                                                .JoinAlias(() => ccAlias.Proveedor, () => pAlias)
                                                .OrderBy(() => pAlias.RazonSocial).Asc
                                                .List<ComprobanteCompra>()
                                                .Select(x => new ReporteCompra()
                                                {
                                                    Id = x.Id,
                                                    Proveedor = x.Proveedor.RazonSocial,
                                                    Comprobante = x.Letra + x.Numero,
                                                    Fecha = x.Fecha,
                                                    SemanaFecha = DateHelper.GetWeek(x.Fecha).ToString("dd/MM/yyyy", "al"),
                                                    FechaVencimiento = x.FechaVencimiento,
                                                    SemanaVencimiento = DateHelper.GetWeek(x.FechaVencimiento).ToString("dd/MM/yyyy", "al"),
                                                    Importe = (x.Total - x.TotalPagado) * (x.Tipo.AdditionalData != "-1" ? 1 : -1),
                                                    TipoComprobante = x.Tipo.Data
                                                })
                                                .ToList();
        }

        public ComprasCuentaCorriente AcumuladosHead(int id, int IdRubro, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter)
        {
            List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(filter);
            ComprobanteCompra c = null;
            ComprasCuentaCorriente header = null;
            DateTime hoy = DateTime.Now;
            DetalleComprobanteCompra dcc = null;
            RubroCompra rub = null;

            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                                                        .SelectList(list =>
                                                            list.SelectSubQuery(QueryOver.Of<ComprobanteCompra>(() => c)
                                                            .JoinAlias(() => c.Detalle, () => dcc)
                                                            .JoinAlias(() => dcc.RubroCompra, () => rub)
                                                            .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Proveedor.Id == id || id == 0) && (rub.Id == IdRubro || IdRubro == 0) && c.FechaVencimiento < hoy && (c.Estado != EstadoComprobante.Anulada))
                                                                .And(() => c.Organizacion.Id == Security.GetOrganizacion().Id)
                                                                .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                                .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Total),
                                                                        Projections.Sum(() => c.TotalPagado))))
                                                                .Take(1)
                                                                ).WithAlias(() => header.DeudaVencida)
                                                            .SelectSubQuery(QueryOver.Of<ComprobanteCompra>(() => c)
                                                            .JoinAlias(() => c.Detalle, () => dcc)
                                                            .JoinAlias(() => dcc.RubroCompra, () => rub)
                                                                .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Proveedor.Id == id || id == 0) && (rub.Id == IdRubro || IdRubro == 0) && c.FechaVencimiento >= hoy && (c.Estado != EstadoComprobante.Anulada))
                                                                .And(() => c.Organizacion.Id == Security.GetOrganizacion().Id)
                                                                .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                                .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Total),
                                                                        Projections.Sum(() => c.TotalPagado))))
                                                                .Take(1)
                                                                ).WithAlias(() => header.DeudaNoVencida)
                                                            .SelectSubQuery(QueryOver.Of<ComprobanteCompra>(() => c)
                                                            .JoinAlias(() => c.Detalle, () => dcc)
                                                            .JoinAlias(() => dcc.RubroCompra, () => rub)
                                                                .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Proveedor.Id == id || id == 0) && (rub.Id == IdRubro || IdRubro == 0) && (c.Estado != EstadoComprobante.Anulada))
                                                                .And(() => c.Organizacion.Id == Security.GetOrganizacion().Id)
                                                                .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                                .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Total),
                                                                        Projections.Sum(() => c.TotalPagado))))
                                                                .Take(1)
                                                                ).WithAlias(() => header.Saldo)
                                                        )
                                                        .TransformUsing(Transformers.AliasToBean<ComprasCuentaCorriente>())
                                                        .Take(1)
                                                        .SingleOrDefault<ComprasCuentaCorriente>();
        }
        

        public IList<CuentaCorrienteItem> GetAllAcumulados(int IdProveedor, int IdRubro, DateTime _start, DateTime _end, Model.Common.CuentaCorrienteFilter filter)
        {
            List<CuentaCorrienteItem> items = new List<CuentaCorrienteItem>();
            List<EstadoComprobante> estados = ComprobanteHelper.GetEstadosByfilter(filter);
            ComprobanteCompra c = null;
            DateTime hoy = DateTime.Now;
            CuentaCorrienteItem cci = null;
            ComboItem tipo = null;
            Proveedor proveedorAlias = null;
            DetalleComprobanteCompra dcc = null;
            RubroCompra rub = null;

            IList<CuentaCorrienteItem> comprasItems = this.GetSessionFactory().GetSession()
                                                            .QueryOver<ComprobanteCompra>(() => c)
                                                            .JoinAlias(() => c.Tipo, () => tipo)
                                                            .JoinAlias(() => c.Proveedor, () => proveedorAlias)
                                                            .JoinAlias(() => c.Detalle, () => dcc)
                                                            .JoinAlias(() => dcc.RubroCompra, () => rub)
                                                            .GetFiltroCuentaCorriente(filter)
                                                            .Where(() => c.Fecha >= _start && c.Fecha <= _end && (c.Proveedor.Id == IdProveedor || IdProveedor == 0) && (rub.Id == IdRubro || IdRubro == 0) && (c.Estado != EstadoComprobante.Anulada))
                                                            .GetFilterBySecurity()
                                                            .WhereRestrictionOn(() => c.Estado).IsIn(estados)
                                                            .SelectList(i => i
                                                                .Select(() => c.Id).WithAlias(() => cci.NroReferencia)
                                                                .Select(() => c.Fecha).WithAlias(() => cci.Fecha)
                                                                .Select(() => proveedorAlias.RazonSocial).WithAlias(() => cci.Empresa)
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
                                                                        Projections.Property(() => c.TotalPagado)))
                                                                    .WithAlias(() => cci.Pendiente)
                                                                .Select(() => tipo.Data).WithAlias(() => cci.TipoComprobante)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => tipo.AdditionalData == "-1"),
                                                                        Projections.Constant(0, NHibernateUtil.Decimal),
                                                                        Projections.Property(() => c.Total))).WithAlias(() => cci.Debe)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => tipo.AdditionalData != "-1"),
                                                                        Projections.Constant(0, NHibernateUtil.Decimal),
                                                                        Projections.Property(() => c.Total))).WithAlias(() => cci.Haber)
                                                                .Select(Projections.Constant("ComprobanteCompra")).WithAlias(() => cci.Entidad)
                                                            )
                                                            .TransformUsing(Transformers.AliasToBean<CuentaCorrienteItem>())
                                                            .List<CuentaCorrienteItem>();
            items.AddRange(comprasItems);

            List<EstadoComprobanteCancelacion> estadosCancelacion = CobranzaHelper.GetEstadosByfilter(filter);

            return items.OrderByDescending(x => x.Fecha).ToList();


        }
        

        public ComprobanteCompra GetByLetrayNumero(string LetraYNumero)
        {
            ComprobanteCompra comp = new ComprobanteCompra();
            comp.SplitLetraNumero(LetraYNumero);
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                                                        .Where(x => x.Letra == comp.Letra && x.Numero == comp.Numero 
                                                            && (x.Estado != EstadoComprobante.Anulada))
                                                        .GetFilterBySecurity()
                                                        .SingleOrDefault();
        }


        // Parametros
        // Tipo : Tipo de comprobante, 0 trae todo.
        // NoTipo: Que tipo no trae, 0 trae todo.
        // Pagada: 2 Pago pendiente, 1 Pago total, 0 trae todo.
        public IList<ComprobanteCompra> GetAllByProvFilterNC(int IdProveedor, int Tipo, int NoTipo, ComprobantesACancelarFilter Pagada)
        {
            IList<ComprobanteCompra> comprobantes = new List<ComprobanteCompra>();
            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>().Where( x =>
                                                                    (x.Tipo.Id == Tipo || Tipo == 0) && 
                                                                    (x.Tipo.Id != NoTipo || NoTipo == 0) &&
                                                                    (x.Estado != EstadoComprobante.Anulada) &&
                                                                    (x.Proveedor.Id == IdProveedor || IdProveedor == 0) &&
                                                                    (Pagada == ComprobantesACancelarFilter.Todos ||
                                                                    (Pagada == ComprobantesACancelarFilter.Cancelados && x.Total == x.TotalPagado) ||
                                                                    (Pagada == ComprobantesACancelarFilter.Pendientes && x.Total > x.TotalPagado)
                                                                    ))
                                                                .GetFilterBySecurity()
                                                                .OrderBy(x => x.Fecha).Desc
                                                                .List();
        }

        public ComprobanteCompra GetComprobanteByInfo(int IdProveedor, string Letra, string Numero, int tipoComprobante)
        {
            ComprobanteCompra c = this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                                                                        .Where(x => x.Letra == Letra && x.Numero == Numero && 
                                                                                (x.Tipo.Id == tipoComprobante || tipoComprobante == 0)
                                                                                && x.Proveedor.Id == IdProveedor)
                                                                        .GetFilterBySecurity()
                                                                        .SingleOrDefault();
            return c;
        }

        public IList<ReporteCitiItem> GetCitiCompras(DateTime start, DateTime end)
        {
            DetalleComprobanteCompra dccAlias = null;
            ComprobanteCompra ccAlias = null;
            ReporteCitiItem rcAlias = null;
            ComboItem tipoAlias = null;
            Proveedor pAlias = null;


            return this.GetSessionFactory().GetSession().QueryOver<DetalleComprobanteCompra>(() => dccAlias)
                                                .JoinAlias(() => dccAlias.Comprobante, () => ccAlias)
                                                .JoinAlias(() => ccAlias.Proveedor, () => pAlias)
                                                .JoinAlias(() => ccAlias.Tipo, () => tipoAlias)
                                                .Where(() => ccAlias.Estado != EstadoComprobante.Anulada && ccAlias.Fecha >= start && ccAlias.Fecha <= end)
                                                .And(() => ccAlias.Organizacion.Id == Security.GetOrganizacion().Id)
                                                .SelectList(list =>
                                                    list.SelectGroup(() => pAlias.RazonSocial).WithAlias(() => rcAlias.RazonSocial)
                                                        .SelectGroup(() => tipoAlias.AfipData).WithAlias(() => rcAlias.CodigoAfipComprobante)
                                                        .SelectGroup(() => tipoAlias.Data).WithAlias(() => rcAlias.TipoComprobante)
                                                        .SelectGroup(() => ccAlias.Numero).WithAlias(() => rcAlias.Comprobante)
                                                        .SelectGroup(() => ccAlias.IVA21).WithAlias(() => rcAlias.IVA21)
                                                        .SelectGroup(() => ccAlias.IVA105).WithAlias(() => rcAlias.IVA105)
                                                        .SelectGroup(() => ccAlias.IVA27).WithAlias(() => rcAlias.IVA27)
                                                        .SelectGroup(() => ccAlias.Fecha).WithAlias(() => rcAlias.Fecha)
                                                        .SelectGroup(() => ccAlias.Letra).WithAlias(() => rcAlias.Letra)
                                                        .SelectGroup(() => tipoAlias.Id).WithAlias(() => rcAlias.IdTipoComprobante)
                                                        .SelectGroup(() => ccAlias.IVA).WithAlias(() => rcAlias.IVA)
                                                        .SelectGroup(() => pAlias.CUIT).WithAlias(() => rcAlias.CUIT)
                                                        .SelectGroup(() => ccAlias.ImporteExento).WithAlias(() => rcAlias.Exento)
                                                        .SelectGroup(() => ccAlias.Total).WithAlias(() => rcAlias.Total)
                                                        .SelectGroup(() => ccAlias.FechaVencimiento).WithAlias(() => rcAlias.FechaVto)
                                                        .SelectGroup(() => ccAlias.ImporteNoGravado).WithAlias(() => rcAlias.NoGravado)
                                                        .SelectGroup(() => ccAlias.PercepcionesIIBB).WithAlias(() => rcAlias.PercepcionesIIBB)
                                                        .SelectGroup(() => ccAlias.PercepcionesIVA).WithAlias(() => rcAlias.PercepcionesIVA)
                                                        .Select(Projections.Constant(1, NHibernateUtil.Decimal)).WithAlias(() => rcAlias.Cotizacion)
                                                        .Select(Projections.Constant("PES")).WithAlias(() => rcAlias.CodigoMoneda)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => dccAlias.TipoIva.Id == 93),
                                                                    Projections.Property(() => dccAlias.Total),
                                                                    Projections.Constant(0, NHibernateUtil.Decimal)))).WithAlias(() => rcAlias.NetoGravado105)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => dccAlias.TipoIva.Id == 94),
                                                                    Projections.Property(() => dccAlias.Total),
                                                                    Projections.Constant(0, NHibernateUtil.Decimal)))).WithAlias(() => rcAlias.NetoGravado21)
                                                        .Select(Projections.Sum(
                                                                Projections.Conditional(
                                                                    Restrictions.Where(() => dccAlias.TipoIva.Id == 95),
                                                                    Projections.Property(() => dccAlias.Total),
                                                                    Projections.Constant(0, NHibernateUtil.Decimal)))).WithAlias(() => rcAlias.NetoGravado27)
                                                )
                                                .TransformUsing(Transformers.AliasToBean<ReporteCitiItem>())
                                                .List<ReporteCitiItem>();
        }


    }
}
