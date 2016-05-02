using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Helpers;

namespace Woopin.SGC.Repositories.Common
{
    public class GeneralRepository : IGeneralRepository
    {
        /// <summary>
        /// The hibernate session factory.
        /// </summary>
        private IHibernateSessionFactory hibernateSessionFactory;

        /// <summary>
        /// The get session factory.
        /// </summary>
        /// <returns>
        /// The Repository.IHibernateSessionFactory.
        /// </returns>
        public IHibernateSessionFactory GetSessionFactory()
        {
            return this.hibernateSessionFactory;
        }

        public GeneralRepository(IHibernateSessionFactory hibernateSessionFactory)
        {
            this.hibernateSessionFactory = hibernateSessionFactory;
        }


        public Dashboard GetDashboard()
        {
            Dashboard dashboard = new Dashboard();
            DateTime date = DateTime.Now;
            DateTime mesActualPrimerDia = new DateTime(date.Year, date.Month, 1);
            DateTime mesActualUltimoDia = mesActualPrimerDia.AddMonths(1).AddDays(-1);

            DateTime mesAnterior = DateTime.Now.AddMonths(-1);
            DateTime mesAnteriorPrimerDia = new DateTime(mesAnterior.Year, mesAnterior.Month, 1);
            DateTime mesAnteriorUltimoDia = mesAnteriorPrimerDia.AddMonths(1).AddDays(-1);
            DateTime semanaProxima = DateTime.Now.AddDays(7);

            ComboItem tipoAlias = null;
            ComprobanteVenta cvAlias = null;
            ComprobanteCompra ccAlias = null;

            dashboard.ComprasMes = this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>( () => ccAlias)
                                                                             .Where(() => ccAlias.Fecha >= mesActualPrimerDia && ccAlias.Fecha <= mesActualUltimoDia && ccAlias.Estado != EstadoComprobante.Anulada)
                                                                             .GetFilterBySecurity()
                                                                             .JoinAlias( () => ccAlias.Tipo, () => tipoAlias)
                                                                             .Select(Projections.Sum(
                                                                                Projections.Conditional(
                                                                                    Restrictions.Where(() => tipoAlias.AdditionalData == "-1"),
                                                                                    Projections.SqlFunction(
                                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                                        NHibernateUtil.Decimal,
                                                                                        Projections.Constant(-1),
                                                                                        Projections.Property(() => ccAlias.Subtotal)),
                                                                                    Projections.Property(() => ccAlias.Subtotal))))
                                                                            .SingleOrDefault<decimal>();

            dashboard.VentasMes = this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>(() => cvAlias)
                                                                             .Where(() => cvAlias.Fecha >= mesActualPrimerDia && cvAlias.Fecha <= mesActualUltimoDia && cvAlias.Estado != EstadoComprobante.Anulada)
                                                                             .GetFilterBySecurity()
                                                                             .JoinAlias(() => cvAlias.Tipo, () => tipoAlias)
                                                                             .Select(Projections.Sum(
                                                                                Projections.Conditional(
                                                                                    Restrictions.Where(() => tipoAlias.AdditionalData == "-1"),
                                                                                    Projections.SqlFunction(
                                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                                        NHibernateUtil.Decimal,
                                                                                        Projections.Constant(-1),
                                                                                        Projections.Property(() => cvAlias.Subtotal)),
                                                                                    Projections.Property(() => cvAlias.Subtotal))))
                                                                            .SingleOrDefault<decimal>();

            #region Compras Actual sobre Anterior
            decimal ComprasMesAnt = this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>(() => ccAlias)
                                                                             .Where(() => ccAlias.Fecha >= mesAnteriorPrimerDia && ccAlias.Fecha <= mesAnteriorUltimoDia && ccAlias.Estado != EstadoComprobante.Anulada)
                                                                             .GetFilterBySecurity()
                                                                             .JoinAlias(() => ccAlias.Tipo, () => tipoAlias)
                                                                             .Select(Projections.Sum(
                                                                                Projections.Conditional(
                                                                                    Restrictions.Where(() => tipoAlias.AdditionalData == "-1"),
                                                                                    Projections.SqlFunction(
                                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                                        NHibernateUtil.Decimal,
                                                                                        Projections.Constant(-1),
                                                                                        Projections.Property(() => ccAlias.Subtotal)),
                                                                                    Projections.Property(() => ccAlias.Subtotal))))
                                                                 .SingleOrDefault<decimal>();
            ComprasMesAnt = ComprasMesAnt == 0 ? 1 : ComprasMesAnt;

            dashboard.ComprasMensual = decimal.Round(dashboard.ComprasMes * 100 / ComprasMesAnt,0);
            #endregion

            #region Ventas Actual sobre Anterior
            decimal VentasMesAnt = this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>(() => cvAlias)
                                                                             .Where(() => cvAlias.Fecha >= mesAnteriorPrimerDia && cvAlias.Fecha <= mesAnteriorUltimoDia && cvAlias.Estado != EstadoComprobante.Anulada)
                                                                             .GetFilterBySecurity()
                                                                             .JoinAlias(() => cvAlias.Tipo, () => tipoAlias)
                                                                             .Select(Projections.Sum(
                                                                                Projections.Conditional(
                                                                                    Restrictions.Where(() => tipoAlias.AdditionalData == "-1"),
                                                                                    Projections.SqlFunction(
                                                                                        new VarArgsSQLFunction("(", "*", ")"),
                                                                                        NHibernateUtil.Decimal,
                                                                                        Projections.Constant(-1),
                                                                                        Projections.Property(() => cvAlias.Subtotal)),
                                                                                    Projections.Property(() => cvAlias.Subtotal))))
                                                                 .SingleOrDefault<decimal>();
            VentasMesAnt = VentasMesAnt == 0 ? 1 : VentasMesAnt;

            dashboard.VentasMensual = decimal.Round(dashboard.VentasMes * 100 / VentasMesAnt, 0);
            #endregion

            #region Configuracion: Clientes & Proveedores

            dashboard.Clientes = this.GetSessionFactory().GetSession().QueryOver<Cliente>()
                                                        .GetFilterBySecurity()
                                                        .RowCount();

            dashboard.Proveedores = this.GetSessionFactory().GetSession().QueryOver<Proveedor>()
                                                        .GetFilterBySecurity()
                                                        .RowCount();

            #endregion

            #region Disponibilidades

            dashboard.Disponibilidades += this.GetSessionFactory().GetSession().QueryOver<CuentaBancaria>()
                                                        .GetFilterBySecurity()                                                    
                                                        .Select(Projections.Sum<CuentaBancaria>(x => x.Fondo))
                                                        .SingleOrDefault<decimal>();

            dashboard.Disponibilidades += this.GetSessionFactory().GetSession().QueryOver<Caja>()
                                                        .GetFilterBySecurity()
                                                        .Select(Projections.Sum<Caja>(x => x.Fondos))
                                                        .SingleOrDefault<decimal>();
            #endregion


            dashboard.ValoresADepositar = this.GetSessionFactory().GetSession().QueryOver<Cheque>()
                                                        .Where(x => x.Estado == EstadoCheque.Cartera)
                                                        .GetFilterBySecurity()
                                                        .Select(Projections.Sum<Cheque>(x => x.Importe))
                                                        .SingleOrDefault<decimal>();

            #region Compras Vencidas
            IList<ComprobanteCompra> comprasVencidas = this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                                                                             .Where(x => x.FechaVencimiento < date && x.Estado != EstadoComprobante.Anulada && x.Total > x.TotalPagado).GetFilterBySecurity().List();
            
            dashboard.ComprasVencidas = (decimal)comprasVencidas.Count();
            dashboard.ComprasVencidasMes = (decimal)comprasVencidas.Where(x => x.FechaVencimiento >= mesAnterior && x.FechaVencimiento < date).Count();
            
            dashboard.ComprasPorVencerSemana = this.GetSessionFactory().GetSession().QueryOver<ComprobanteCompra>()
                .Where(x => x.FechaVencimiento >= date && x.FechaVencimiento < semanaProxima && x.Estado != EstadoComprobante.Anulada && x.Total > x.TotalPagado).GetFilterBySecurity().List().Count();

            #endregion 
            
            #region Ventas Vencidas
            IList<ComprobanteVenta> ventasVencidas = this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>()
                                                                             .Where(x => x.FechaVencimiento < date && x.Estado != EstadoComprobante.Anulada && x.Total > x.TotalCobrado)
                                                                             .GetFilterBySecurity()                                                                             
                                                                             .List();
            dashboard.VentasVencidas = (decimal)ventasVencidas.Count();
            dashboard.VentasVencidasMes = (decimal)ventasVencidas.Where(x => x.FechaVencimiento >= mesAnterior && x.FechaVencimiento < date)
                                                                 .Count();

            dashboard.VentasPorVencerSemana = this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>()
                .Where(x => x.FechaVencimiento >= date && x.FechaVencimiento < semanaProxima && x.Estado != EstadoComprobante.Anulada && x.Total > x.TotalCobrado).GetFilterBySecurity().List().Count();

            #endregion    
          
            #region Deudas Bancarias
            dashboard.DeudasBancarias = this.GetSessionFactory().GetSession().QueryOver<PagoTarjeta>()
                                                        .Where(x => x.Estado != EstadoPagoTarjeta.Borrador && x.TotalCancelado < x.Total)
                                                        .GetFilterBySecurity()
                                                        .List().Sum(x => x.Total - x.TotalCancelado);

            #endregion  

            #region Facturacion Electronica
            dashboard.CantFEPendientes = this.GetSessionFactory().GetSession().QueryOver<ComprobanteVenta>()
                                                                             .Where(x => x.Estado == EstadoComprobante.Pendiente_Afip)
                                                                             .GetFilterBySecurity()
                                                                             .RowCount();
            #endregion

            #region Cheques a confirmar Pago
            dashboard.ChequesADebitar = this.GetSessionFactory().GetSession().QueryOver<ChequePropio>()
                                                                             .Where(x => x.Estado == EstadoCheque.Entregado)
                                                                             .GetFilterBySecurity()
                                                                             .Select(Projections.Sum<ChequePropio>(x => x.Importe))
                                                                             .SingleOrDefault<decimal>();
            #endregion


            return dashboard;
        }

        public SessionData CreateSessionData(Usuario user)
        {
            SessionData  s = new SessionData();
            s.CurrentOrganizacion = this.GetSessionFactory().GetSession().QueryOver<Organizacion>()
                                                        .Where(x => x.Id == user.OrganizacionActual.Id)
                                                        .SingleOrDefault();
            s.CurrentUser = user;
            SessionDataFactory.RegisterSessionData(s);
            return s;
        }
        public SessionData CreateSessionData(JobHeader header)
        {
            SessionData s = new SessionData();
            s.CurrentOrganizacion = this.GetSessionFactory().GetSession().QueryOver<Organizacion>()
                                                        .Where(x => x.Id == header.IdOrganizacion)
                                                        .SingleOrDefault();
            s.CurrentUser = this.GetSessionFactory().GetSession().QueryOver<Usuario>()
                                                        .Where(x => x.Id == header.IdUsuario)
                                                        .SingleOrDefault();
            SessionDataFactory.RegisterSessionData(s);
            return s;
        }




    }
}
