using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Compras;
using NHibernate.Transform;
using NHibernate.Criterion;
using Woopin.SGC.Model.Common;
using NHibernate;
using NHibernate.Dialect.Function;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Contabilidad.Helper;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.CommonApp.Session;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public class AsientoItemRepository : BaseRepository<AsientoItem>, IAsientoItemRepository
    {
        public AsientoItemRepository(IHibernateSessionFactory hibernateSessionFactory)
            : base(hibernateSessionFactory)
        {
        }


        public IList<LibroIVA> GetLibroIVACompras(DateTime start, DateTime end)
        {
            LibroIVA libro = null;
            ComboItem tipo = null;
            Proveedor p = null;
            ComprobanteCompra c = null;

            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>(() => c)
                                                        .Where(() => c.FechaContable >= start && c.FechaContable <= end && c.Estado != EstadoComprobante.Anulada)
                                                        .And(() => c.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
                                                        .JoinAlias( () => c.Proveedor, () => p)
                                                        .JoinAlias(() => c.Tipo, () => tipo)
                                                        .SelectList(list =>
                                                            list.Select(() => c.FechaContable).WithAlias(() => libro.Fecha)
                                                                .Select(() => p.CUIT).WithAlias(() => libro.CUIT)
                                                                .Select(() => p.RazonSocial).WithAlias(() => libro.RazonSocial)
                                                                .Select(() => tipo.Data).WithAlias(() => libro.Comprobante)
                                                                .Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "+", ")"),
                                                                        NHibernateUtil.String,
                                                                        Projections.Property(() => c.Letra),
                                                                        Projections.Property(() => c.Numero))).WithAlias(() => libro.LetraNumero)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => c.Tipo.Id != ComprobanteCompraHelper.NotaCredito),
                                                                        Projections.Property(() => c.IVA21),
                                                                        Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Property(() => c.IVA21),
                                                                        Projections.Constant(-1))
                                                                        )).WithAlias(() => libro.ImporteIVA21)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => c.Tipo.Id != ComprobanteCompraHelper.NotaCredito),
                                                                        Projections.Property(() => c.IVA105),
                                                                        Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Property(() => c.IVA105),
                                                                        Projections.Constant(-1))
                                                                        )).WithAlias(() => libro.ImporteIVA105)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => c.Tipo.Id != ComprobanteCompraHelper.NotaCredito),
                                                                        Projections.Property(() => c.IVA27),
                                                                        Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Property(() => c.IVA27),
                                                                        Projections.Constant(-1))
                                                                        )).WithAlias(() => libro.ImporteIVA27)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => c.Tipo.Id != ComprobanteCompraHelper.NotaCredito),
                                                                        Projections.Property(() => c.IVA),
                                                                        Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Property(() => c.IVA),
                                                                        Projections.Constant(-1))
                                                                        )).WithAlias(() => libro.IVA)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => c.Tipo.Id != ComprobanteCompraHelper.NotaCredito),
                                                                        Projections.Property(() => c.ImporteExento),
                                                                        Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Property(() => c.ImporteExento),
                                                                        Projections.Constant(-1))
                                                                        )).WithAlias(() => libro.ImporteExento)
                                                                .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => c.Tipo.Id != ComprobanteCompraHelper.NotaCredito),
                                                                        Projections.Property(() => c.Total),
                                                                        Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Property(() => c.Total),
                                                                        Projections.Constant(-1))
                                                                        )).WithAlias(() => libro.Total)
                                                                 .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => c.Tipo.Id != ComprobanteCompraHelper.NotaCredito),
                                                                        Projections.Property(() => c.PercepcionesIIBB),
                                                                        Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Property(() => c.PercepcionesIIBB),
                                                                        Projections.Constant(-1))
                                                                        )).WithAlias(() => libro.PercepcionIIBB)
                                                                 .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => c.Tipo.Id != ComprobanteCompraHelper.NotaCredito),
                                                                        Projections.Property(() => c.PercepcionesIVA),
                                                                        Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Property(() => c.PercepcionesIVA),
                                                                        Projections.Constant(-1))
                                                                        )).WithAlias(() => libro.PercepcionIVA)
                                                                 .Select(Projections.Conditional(
                                                                        Restrictions.Where(() => c.Tipo.Id != ComprobanteCompraHelper.NotaCredito),
                                                                        Projections.SqlFunction(
                                                                            new VarArgsSQLFunction("(", "-", ")"),
                                                                            NHibernateUtil.Decimal,
                                                                            Projections.Property(() => c.Subtotal),
                                                                            Projections.Property(() => c.ImporteExento)),
                                                                        Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.SqlFunction(
                                                                            new VarArgsSQLFunction("(", "-", ")"),
                                                                            NHibernateUtil.Decimal,
                                                                            Projections.Property(() => c.Subtotal),
                                                                            Projections.Property(() => c.ImporteExento)),
                                                                        Projections.Constant(-1))
                                                                        )).WithAlias(() => libro.ImporteGravado)
                                                        )
                                                        .OrderBy(() => c.Fecha).Asc
                                                        .TransformUsing(Transformers.AliasToBean<LibroIVA>())
                                                        .List<LibroIVA>();
        }

        public IList<LibroIVA> GetLibroIVAVentas(DateTime start, DateTime end)
        {
            LibroIVA libro = null;
            ComboItem tipo = null;
            Cliente cl = null;
            ComprobanteVenta c = null;

            return this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>(() => c)
                                                        .JoinAlias(() => c.Cliente, () => cl)
                                                        .JoinAlias(() => c.Tipo, () => tipo)
                                                        .Where(() => c.Fecha >= start && c.Fecha <= end)
                                                        .And(() => c.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
                                                          .SelectList(list =>
                                                            list.Select(() => c.Fecha).WithAlias(() => libro.Fecha)
                                                                .Select(Projections.Conditional(
                                                                            Restrictions.Where(() => c.Estado != EstadoComprobante.Anulada),
                                                                            Projections.Property(() => cl.CUIT),
                                                                            Projections.Constant("", NHibernateUtil.String)))
                                                                     .WithAlias(() => libro.CUIT)
                                                                .Select(Projections.Conditional(
                                                                            Restrictions.Where(() => c.Estado != EstadoComprobante.Anulada),
                                                                            Projections.Property(() => cl.RazonSocial),
                                                                            Projections.Constant("Anulada", NHibernateUtil.String)))
                                                                     .WithAlias(() => libro.RazonSocial)
                                                                .Select(() => tipo.Data).WithAlias(() => libro.Comprobante)
                                                                .Select(Projections.SqlFunction(
                                                                                new VarArgsSQLFunction("(", "+", ")"),
                                                                                NHibernateUtil.String,
                                                                                Projections.Property(() => c.Letra),
                                                                                Projections.Property(() => c.Numero)))
                                                                     .WithAlias(() => libro.LetraNumero)
                                                                .Select(Projections.Conditional(
                                                                            Restrictions.Where(() => c.Estado != EstadoComprobante.Anulada),
                                                                                Projections.Conditional(
                                                                                    Restrictions.Where(() => tipo.AdditionalData != "-1"),
                                                                                        Projections.Property(() => c.IVA105),
                                                                                        Projections.SqlFunction(
                                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                                        NHibernateUtil.Decimal,
                                                                                        Projections.Property(() => c.IVA105),
                                                                                        Projections.Constant(-1))
                                                                                ),
                                                                                Projections.Constant(0, NHibernateUtil.Decimal))
                                                                            ).WithAlias(() => libro.ImporteIVA105)
                                                                .Select(Projections.Conditional(
                                                                            Restrictions.Where(() => c.Estado != EstadoComprobante.Anulada),
                                                                                Projections.Conditional(
                                                                                    Restrictions.Where(() => tipo.AdditionalData != "-1"),
                                                                                        Projections.Property(() => c.IVA27),
                                                                                        Projections.SqlFunction(
                                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                                        NHibernateUtil.Decimal,
                                                                                        Projections.Property(() => c.IVA27),
                                                                                        Projections.Constant(-1))
                                                                                ),
                                                                                Projections.Constant(0, NHibernateUtil.Decimal))
                                                                            ).WithAlias(() => libro.ImporteIVA27)
                                                                 .Select(Projections.Conditional(
                                                                            Restrictions.Where(() => c.Estado != EstadoComprobante.Anulada),
                                                                                Projections.Conditional(
                                                                                    Restrictions.Where(() => tipo.AdditionalData != "-1"),
                                                                                        Projections.Property(() => c.IVA21),
                                                                                        Projections.SqlFunction(
                                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                                        NHibernateUtil.Decimal,
                                                                                        Projections.Property(() => c.IVA21),
                                                                                        Projections.Constant(-1))
                                                                                ),
                                                                                Projections.Constant(0, NHibernateUtil.Decimal))
                                                                            ).WithAlias(() => libro.ImporteIVA21)
                                                                .Select(Projections.Conditional(
                                                                            Restrictions.Where(() => c.Estado != EstadoComprobante.Anulada),
                                                                                Projections.Conditional(
                                                                                    Restrictions.Where(() => tipo.AdditionalData != "-1"),
                                                                                        Projections.Property(() => c.ImporteExento),
                                                                                        Projections.SqlFunction(
                                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                                        NHibernateUtil.Decimal,
                                                                                        Projections.Property(() => c.ImporteExento),
                                                                                        Projections.Constant(-1))
                                                                                    ),
                                                                                Projections.Constant(0, NHibernateUtil.Decimal)))
                                                                      .WithAlias(() => libro.ImporteExento)
                                                                .Select(Projections.Conditional(
                                                                            Restrictions.Where(() => c.Estado != EstadoComprobante.Anulada),
                                                                                Projections.Conditional(
                                                                                    Restrictions.Where(() => tipo.AdditionalData != "-1"),
                                                                                    Projections.Property(() => c.Total),
                                                                                    Projections.SqlFunction(
                                                                                    new VarArgsSQLFunction("(", "*", ")"),
                                                                                    NHibernateUtil.Decimal,
                                                                                    Projections.Property(() => c.Total),
                                                                                    Projections.Constant(-1))),
                                                                                Projections.Constant(0, NHibernateUtil.Decimal)))
                                                                      .WithAlias(() => libro.Total)
                                                                 .Select(Projections.Conditional(
                                                                            Restrictions.Where(() => c.Estado != EstadoComprobante.Anulada),
                                                                            Projections.Conditional(
                                                                                Restrictions.Where(() => tipo.AdditionalData != "-1"),
                                                                                Projections.SqlFunction(
                                                                                    new VarArgsSQLFunction("(", "-", ")"),
                                                                                    NHibernateUtil.Decimal,
                                                                                    Projections.Property(() => c.Subtotal),
                                                                                    Projections.Property(() => c.ImporteExento)),
                                                                                Projections.SqlFunction(
                                                                                new VarArgsSQLFunction("(", "*", ")"),
                                                                                NHibernateUtil.Decimal,
                                                                                Projections.SqlFunction(
                                                                                    new VarArgsSQLFunction("(", "-", ")"),
                                                                                    NHibernateUtil.Decimal,
                                                                                    Projections.Property(() => c.Subtotal),
                                                                                    Projections.Property(() => c.ImporteExento)),
                                                                                Projections.Constant(-1))),
                                                                            Projections.Constant(0, NHibernateUtil.Decimal)))
                                                                     .WithAlias(() => libro.ImporteGravado)
                                                        )
                                                        .OrderBy(() => c.Fecha).Asc
                                                        .TransformUsing(Transformers.AliasToBean<LibroIVA>())
                                                        .List<LibroIVA>();
        }

        public IList<SumaSaldo> GetAllSumasYSaldos(DateTime start, DateTime end)
        {
            SumaSaldo sumasaldo = null;
            AsientoItem c = null;
            Cuenta cl = null;
            Asiento a = null;
            return this.GetSessionFactory().GetSession().QueryOver<AsientoItem>(() => c)
                                                        .Where(() => a.Fecha >= start && a.Fecha <= end)
                                                        .And(() => a.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
                                                        .JoinAlias(() => c.Cuenta, () => cl)
                                                        .JoinAlias(() => c.Asiento, () => a)
                                                        .SelectList(list =>
                                                            list
                                                                .SelectGroup(() => cl.Id).WithAlias(() => sumasaldo.CuentaId)
                                                                .SelectGroup(() => cl.Codigo).WithAlias(() => sumasaldo.Codigo)
                                                                .SelectGroup(() => cl.Nombre).WithAlias(() => sumasaldo.NombreCuenta)
                                                                .SelectSum(() => c.Debe).WithAlias(() => sumasaldo.Debe)
                                                                .SelectSum(() => c.Haber).WithAlias(() => sumasaldo.Haber)
                                                                .Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => c.Debe),
                                                                        Projections.Sum(() => c.Haber)))
                                                                .WithAlias(() => sumasaldo.Saldo) //saldo relativo a este periodo de tiempo
                                                                .SelectSubQuery(QueryOver.Of<AsientoItem>(() => c)
                                                                    .JoinAlias(() => c.Asiento, () => a)
                                                                    .Where(() => a.Fecha < start && c.Cuenta.Id == cl.Id)
                                                                    .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                            new VarArgsSQLFunction("(", "-", ")"),
                                                                            NHibernateUtil.Decimal,
                                                                            Projections.Sum(() => c.Debe),
                                                                            Projections.Sum(() => c.Haber))))
                                                                    .Take(1))
                                                                    .WithAlias(() => sumasaldo.SaldoAnterior)
                                                            )
                                                        .OrderBy(() => cl.Codigo).Asc
                                                        .TransformUsing(Transformers.AliasToBean<SumaSaldo>())
                                                        .List<SumaSaldo>();
        }

        public IList<SumaSaldo> GetAllSumasYSaldosTree(DateTime start, DateTime end)
        {
            SumaSaldo sumasaldo = null;
            AsientoItem ai = null;
            Cuenta c = null;
            Asiento a = null;

            return this.GetSessionFactory().GetSession().QueryOver<AsientoItem>(() => ai)
                                                        .Where(() => (a.Fecha >= start && a.Fecha <= end) || ai.Asiento.Id == null || ai.Id == null)
                                                        .And(() => c.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
                                                        .Right.JoinAlias(() => ai.Cuenta, () => c)
                                                        .Left.JoinAlias(() => ai.Asiento, () => a)
                                                        .SelectList(list =>
                                                            list
                                                                .SelectGroup(() => c.Id).WithAlias(() => sumasaldo.CuentaId)
                                                                .SelectGroup(() => c.Codigo).WithAlias(() => sumasaldo.Codigo)
                                                                .SelectGroup(() => c.Nombre).WithAlias(() => sumasaldo.NombreCuenta)
                                                                .SelectGroup(() => c.Rubro).WithAlias(() => sumasaldo.Rubro)
                                                                .SelectGroup(() => c.SubRubro).WithAlias(() => sumasaldo.SubRubro)
                                                                .SelectGroup(() => c.Corriente).WithAlias(() => sumasaldo.Corriente)
                                                                .SelectGroup(() => c.Numero).WithAlias(() => sumasaldo.Numero)
                                                                .SelectSum(() => ai.Debe).WithAlias(() => sumasaldo.Debe)
                                                                .SelectSum(() => ai.Haber).WithAlias(() => sumasaldo.Haber)
                                                                .Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => ai.Debe),
                                                                        Projections.Sum(() => ai.Haber)))
                                                                .WithAlias(() => sumasaldo.Saldo) //saldo relativo a este periodo de tiempo
                                                                .SelectSubQuery(QueryOver.Of<AsientoItem>(() => ai)
                                                                    .JoinAlias(() => ai.Asiento, () => a)
                                                                    .Where(() => a.Fecha < start && ai.Cuenta.Id == c.Id)
                                                                    .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                            new VarArgsSQLFunction("(", "-", ")"),
                                                                            NHibernateUtil.Decimal,
                                                                            Projections.Sum(() => ai.Debe),
                                                                            Projections.Sum(() => ai.Haber))))
                                                                    .Take(1))
                                                                    .WithAlias(() => sumasaldo.SaldoAnterior)
                                                        )
                                                        .OrderBy(() => c.Codigo).Asc
                                                        .TransformUsing(Transformers.AliasToBean<SumaSaldo>())
                                                        .List<SumaSaldo>();
        }

        public IList<MayorItem> GetAllMayorProveedores(DateTime start, DateTime end)
        {
            Asiento aAlias = null;
            Cuenta cAlias = null;
            AsientoItem aiAlias = null;
            MayorItem mayor = null;

            return this.GetSessionFactory().GetSession().QueryOver<AsientoItem>(() => aiAlias)
                                                        .JoinAlias(() => aiAlias.Cuenta, () => cAlias)
                                                        .JoinAlias(() => aiAlias.Asiento, () => aAlias)
                                                        .Where(() => cAlias.Rubro == 2 && cAlias.Corriente == 1 && cAlias.SubRubro == 1 && aAlias.Fecha >= start && aAlias.Fecha <= end)
                                                        .And(() => aAlias.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
                                                        .SelectList(list =>
                                                            list.SelectGroup(() => cAlias.Codigo).WithAlias(() => mayor.Codigo)
                                                                .SelectGroup(() => cAlias.Id).WithAlias(() => mayor.Id)
                                                                .SelectGroup(() => cAlias.Nombre).WithAlias(() => mayor.NombreCuenta)
                                                                .Select(Projections.Sum(() => aiAlias.Debe)).WithAlias(() => mayor.Debe)
                                                                .Select(Projections.Sum(() => aiAlias.Haber)).WithAlias(() => mayor.Haber)
                                                                .Select(Projections.SqlFunction(
                                                                            new VarArgsSQLFunction("(", "-", ")"),
                                                                            NHibernateUtil.Decimal,
                                                                            Projections.Sum(() => aiAlias.Debe),
                                                                            Projections.Sum(() => aiAlias.Haber))
                                                                    ).WithAlias(() => mayor.Saldo)
                                                                .SelectSubQuery(QueryOver.Of<AsientoItem>(() => aiAlias)
                                                                    .JoinAlias(() => aiAlias.Asiento, () => aAlias)
                                                                    .Where(() => aAlias.Fecha < start && aiAlias.Cuenta.Id == cAlias.Id)
                                                                    .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                            new VarArgsSQLFunction("(", "-", ")"),
                                                                            NHibernateUtil.Decimal,
                                                                            Projections.Sum(() => aiAlias.Debe),
                                                                            Projections.Sum(() => aiAlias.Haber))))
                                                                    .Take(1)
                                                                    ).WithAlias(() => mayor.SaldoActual)
                                                        )
                                                        .OrderBy(() => cAlias.Nombre).Asc
                                                        .TransformUsing(Transformers.AliasToBean<MayorItem>())
                                                        .List<MayorItem>();
        }

        public IList<MayorItem> GetAllMayorClientes(DateTime start, DateTime end)
        {
            Asiento aAlias = null;
            Cuenta cAlias = null;
            AsientoItem aiAlias = null;
            MayorItem mayor = null;

            return this.GetSessionFactory().GetSession().QueryOver<AsientoItem>(() => aiAlias)
                                                        .JoinAlias(() => aiAlias.Cuenta, () => cAlias)
                                                        .JoinAlias(() => aiAlias.Asiento, () => aAlias)
                                                        .Where(() => cAlias.Rubro == 1 && cAlias.Corriente == 1 && cAlias.SubRubro == 2 && aAlias.Fecha >= start && aAlias.Fecha <= end)
                                                        .And(() => aAlias.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
                                                        .SelectList(list =>
                                                            list.SelectGroup(() => cAlias.Codigo).WithAlias(() => mayor.Codigo)
                                                                .SelectGroup(() => cAlias.Id).WithAlias(() => mayor.Id)
                                                                .SelectGroup(() => cAlias.Nombre).WithAlias(() => mayor.NombreCuenta)
                                                                .Select(Projections.Sum(() => aiAlias.Debe)).WithAlias(() => mayor.Debe)
                                                                .Select(Projections.Sum(() => aiAlias.Haber)).WithAlias(() => mayor.Haber)
                                                                .Select(Projections.SqlFunction(
                                                                            new VarArgsSQLFunction("(", "-", ")"),
                                                                            NHibernateUtil.Decimal,
                                                                            Projections.Sum(() => aiAlias.Debe),
                                                                            Projections.Sum(() => aiAlias.Haber))
                                                                    ).WithAlias(() => mayor.Saldo)
                                                                .SelectSubQuery(QueryOver.Of<AsientoItem>(() => aiAlias)
                                                                    .JoinAlias(() => aiAlias.Asiento, () => aAlias)
                                                                    .Where(() => aAlias.Fecha < start && aiAlias.Cuenta.Id == cAlias.Id)
                                                                    .SelectList(inner => inner.Select(Projections.SqlFunction(
                                                                            new VarArgsSQLFunction("(", "-", ")"),
                                                                            NHibernateUtil.Decimal,
                                                                            Projections.Sum(() => aiAlias.Debe),
                                                                            Projections.Sum(() => aiAlias.Haber))))
                                                                    .Take(1)
                                                                    ).WithAlias(() => mayor.SaldoActual)
                                                        )
                                                        .OrderBy(() => cAlias.Nombre).Asc
                                                        .TransformUsing(Transformers.AliasToBean<MayorItem>())
                                                        .List<MayorItem>();
        }

        public IList<SumaSaldo> GetBalance(DateTime start, DateTime end)
        {
            SumaSaldo sumasaldo = null;
            AsientoItem ai = null;
            Cuenta c = null;
            Asiento a = null;

            IList<SumaSaldo> retList = this.GetSessionFactory().GetSession().QueryOver<AsientoItem>(() => ai)
                                                        .Where(() => (a.Fecha >= start && a.Fecha <= end) || ai.Asiento.Id == null || ai.Id == null)
                                                        .And(() => c.Organizacion.Id == SessionDataManager.GetOrganizacion().Id)
                                                        .Right.JoinAlias(() => ai.Cuenta, () => c)
                                                        .Left.JoinAlias(() => ai.Asiento, () => a)
                                                        .SelectList(list =>
                                                            list
                                                                .SelectGroup(() => c.Id).WithAlias(() => sumasaldo.CuentaId)
                                                                .SelectGroup(() => c.Codigo).WithAlias(() => sumasaldo.Codigo)
                                                                .SelectGroup(() => c.Nombre).WithAlias(() => sumasaldo.NombreCuenta)
                                                                .SelectGroup(() => c.Rubro).WithAlias(() => sumasaldo.Rubro)
                                                                .SelectGroup(() => c.SubRubro).WithAlias(() => sumasaldo.SubRubro)
                                                                .SelectGroup(() => c.Corriente).WithAlias(() => sumasaldo.Corriente)
                                                                .SelectGroup(() => c.Numero).WithAlias(() => sumasaldo.Numero)
                                                                .SelectSum(() => ai.Debe).WithAlias(() => sumasaldo.Debe)
                                                                .SelectSum(() => ai.Haber).WithAlias(() => sumasaldo.Haber)
                                                                .Select(Projections.SqlFunction(
                                                                        new VarArgsSQLFunction("(", "-", ")"),
                                                                        NHibernateUtil.Decimal,
                                                                        Projections.Sum(() => ai.Debe),
                                                                        Projections.Sum(() => ai.Haber)))
                                                                .WithAlias(() => sumasaldo.Saldo) //saldo relativo a este periodo de tiempo
                                                        )
                                                        .OrderBy(() => c.Codigo).Asc
                                                        .TransformUsing(Transformers.AliasToBean<SumaSaldo>())
                                                        .List<SumaSaldo>();
            foreach (var cuenta in retList)
            {
                if (cuenta.Numero == 0)
                {
                    cuenta.Debe = retList.Where(cInnerAlias => cuenta.Rubro == cInnerAlias.Rubro
                                                 && ((cuenta.Corriente > 0 && cuenta.Corriente == cInnerAlias.Corriente) || cuenta.Corriente == 0)
                                                 && ((cuenta.SubRubro > 0 && cuenta.SubRubro == cInnerAlias.SubRubro) || cuenta.SubRubro == 0)
                                                 && (cInnerAlias.Numero > 0)).Sum(x => x.Debe);
                    cuenta.Haber = retList.Where(cInnerAlias => cuenta.Rubro == cInnerAlias.Rubro
                                                 && ((cuenta.Corriente > 0 && cuenta.Corriente == cInnerAlias.Corriente) || cuenta.Corriente == 0)
                                                 && ((cuenta.SubRubro > 0 && cuenta.SubRubro == cInnerAlias.SubRubro) || cuenta.SubRubro == 0)
                                                 && (cInnerAlias.Numero > 0)).Sum(x => x.Haber);

                    cuenta.Saldo = retList.Where(cInnerAlias => cuenta.Rubro == cInnerAlias.Rubro
                                                 && ((cuenta.Corriente > 0 && cuenta.Corriente == cInnerAlias.Corriente) || cuenta.Corriente == 0)
                                                 && ((cuenta.SubRubro > 0 && cuenta.SubRubro == cInnerAlias.SubRubro) || cuenta.SubRubro == 0)
                                                 && (cInnerAlias.Numero > 0)).Sum(x => x.Saldo);
                }
            }


            return retList;
        }

    }
}
