[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Woopin.SGC.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Woopin.SGC.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Woopin.SGC.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Woopin.SGC.Repositories;
    using Woopin.SGC.Repositories.Contabilidad;
    using Woopin.SGC.Services;
    using Woopin.SGC.Repositories.Common;
    using Woopin.SGC.Repositories.Ventas;
    using Woopin.SGC.Repositories.Compras;
    using Woopin.SGC.Repositories.Tesoreria;
    using Woopin.SGC.Services.Afip;
    using Hangfire;
    using Woopin.SGC.Repositories.Reporting;
    using Woopin.SGC.Repositories.Stock;
    using Woopin.SGC.Repositories.Sueldos;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                JobActivator.Current = new NinjectJobActivator(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Instancia la sesion con la base
            kernel.Bind<IHibernateSessionFactory>().To<HibernateSessionFactory>().InSingletonScope();
            // TODO: Armar que la forma en que busca la sesion sea por request.
            // kernel.Bind<ISession>().ToMethod(arg => new HibernateSessionFactory().SessionInterceptor()).InRequestScope();  
            //Instancia los Repositorios

            // Contabilidad
            kernel.Bind<ICuentaRepository>().To<CuentaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IAsientoRepository>().To<AsientoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IAsientoItemRepository>().To<AsientoItemRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IEjercicioRepository>().To<EjercicioRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IBloqueoContableRepository>().To<BloqueoContableRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IRetencionRepository>().To<RetencionRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));

            // Common
            kernel.Bind<ISucursalRepository>().To<SucursalRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IMonedaRepository>().To<MonedaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ILocalizacionRepository>().To<LocalizacionRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IUsuarioRepository>().To<UsuarioRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ICategoriaIVARepository>().To<CategoriaIVARepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IComboRepository>().To<ComboRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IComboItemRepository>().To<ComboItemRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IGeneralRepository>().To<GeneralRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ILogRepository>().To<LogRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IOrganizacionRepository>().To<OrganizacionRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IUsuarioOrganizacionRepository>().To<UsuarioOrganizacionRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));

            // Ventas
            kernel.Bind<IClienteRepository>().To<ClienteRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IComprobanteVentaRepository>().To<ComprobanteVentaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IGrupoEconomicoRepository>().To<GrupoEconomicoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IDetalleComprobanteVentaRepository>().To<DetalleComprobanteVentaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ICobranzaRepository>().To<CobranzaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ICobranzaValorItemRepository>().To<CobranzaValorItemRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ICobranzaComprobanteItemRepository>().To<CobranzaComprobanteItemRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IImputacionVentaRepository>().To<ImputacionVentaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IListaPreciosRepository>().To<ListaPreciosRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ITalonarioRepository>().To<TalonarioRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));

            // Compras
            kernel.Bind<IProveedorRepository>().To<ProveedorRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IRubroCompraRepository>().To<RubroCompraRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IComprobanteCompraRepository>().To<ComprobanteCompraRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IDetalleComprobanteCompraRepository>().To<DetalleComprobanteCompraRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IOrdenPagoRepository>().To<OrdenPagoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IOrdenPagoValorItemRepository>().To<OrdenPagoValorItemRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IOrdenPagoComprobanteItemRepository>().To<OrdenPagoComprobanteItemRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IOtroEgresoRepository>().To<OtroEgresoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IOtroEgresoPagoRepository>().To<OtroEgresoPagoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IOtroEgresoItemRepository>().To<OtroEgresoItemRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IImputacionCompraRepository>().To<ImputacionCompraRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));

            // Tesoreria
            kernel.Bind<ICuentaBancariaRepository>().To<CuentaBancariaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IBancoRepository>().To<BancoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IMovimientoFondoRepository>().To<MovimientoFondoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IValorRepository>().To<ValorRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ICajaRepository>().To<CajaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IHistorialCajaRepository>().To<HistorialCajaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IHistorialCuentaBancariaRepository>().To<HistorialCuentaBancariaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IChequeRepository>().To<ChequeRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IChequePropioRepository>().To<ChequePropioRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IValorIngresadoRepository>().To<ValorIngresadoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ITransferenciaRepository>().To<TransferenciaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ITarjetaCreditoRepository>().To<TarjetaCreditoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IPagoTarjetaRepository>().To<PagoTarjetaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<ICancelacionTarjetaRepository>().To<CancelacionTarjetaRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IDepositoRepository>().To<DepositoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IDepositoItemRepository>().To<DepositoItemRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IComprobanteRetencionRepository>().To<ComprobanteRetencionRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IChequeraRepository>().To<ChequeraRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            
            // Stock
            kernel.Bind<IArticuloRepository>().To<ArticuloRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IRubroArticuloRepository>().To<RubroArticuloRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IEgresoStockRepository>().To<EgresoStockRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IIngresoStockRepository>().To<IngresoStockRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));

            // Sueldos
            kernel.Bind<IEmpleadoRepository>().To<EmpleadoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IAdicionalRepository>().To<AdicionalRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IAdicionalAdicionalesRepository>().To<AdicionalAdicionalesRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IReciboRepository>().To<ReciboRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));

            //Reporting
            kernel.Bind<IGrupoIngresoRepository>().To<GrupoIngresoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));
            kernel.Bind<IGrupoEgresoRepository>().To<GrupoEgresoRepository>().InSingletonScope().WithConstructorArgument("IHibernateSessionFactory", kernel.GetService(typeof(IHibernateSessionFactory)));

            //Instancia los Servicios
            kernel.Bind<IAfipService>().To<AfipService>().InSingletonScope();
            kernel.Bind<IContabilidadConfigService>().To<ContabilidadConfigService>().InSingletonScope()
                .WithConstructorArgument("ICuentaRepository", kernel.GetService(typeof(ICuentaRepository)));

            kernel.Bind<ICommonConfigService>().To<CommonConfigService>().InSingletonScope()
                .WithConstructorArgument("ISucursalRepository", kernel.GetService(typeof(ISucursalRepository)))
                .WithConstructorArgument("IMonedaRepository", kernel.GetService(typeof(IMonedaRepository)))
                .WithConstructorArgument("ICategoriaIVARepository", kernel.GetService(typeof(ICategoriaIVARepository)))
                .WithConstructorArgument("ILocalizacionRepository", kernel.GetService(typeof(ILocalizacionRepository)))
                .WithConstructorArgument("IComboRepository", kernel.GetService(typeof(IComboRepository)))
                .WithConstructorArgument("IComboItemRepository", kernel.GetService(typeof(IComboItemRepository)));

            kernel.Bind<ISystemService>().To<SystemService>().InSingletonScope()
                .WithConstructorArgument("IGeneralRepository", kernel.GetService(typeof(IGeneralRepository)))
                .WithConstructorArgument("IUsuarioRepository", kernel.GetService(typeof(IUsuarioRepository)));

            kernel.Bind<IVentasConfigService>().To<VentasConfigService>().InSingletonScope()
                .WithConstructorArgument("IClienteRepository", kernel.GetService(typeof(IClienteRepository)))
                .WithConstructorArgument("IGrupoEconomicoRepository", kernel.GetService(typeof(IGrupoEconomicoRepository)));

            kernel.Bind<IContabilidadService>().To<ContabilidadService>().InSingletonScope()
                .WithConstructorArgument("IClienteRepository", kernel.GetService(typeof(IClienteRepository)))
                .WithConstructorArgument("ICuentaRepository", kernel.GetService(typeof(ICuentaRepository)))
                .WithConstructorArgument("IAsientoItemRepository", kernel.GetService(typeof(IAsientoItemRepository)))
                .WithConstructorArgument("IAsientoRepository", kernel.GetService(typeof(IAsientoRepository)));

            kernel.Bind<IVentasService>().To<VentasService>().InSingletonScope()
                .WithConstructorArgument("IDetalleComprobanteVentaRepository", kernel.GetService(typeof(IDetalleComprobanteVentaRepository)))
                .WithConstructorArgument("IComprobanteVentaRepository", kernel.GetService(typeof(IComprobanteVentaRepository)))
                .WithConstructorArgument("ICobranzaRepository", kernel.GetService(typeof(ICobranzaRepository)))
                .WithConstructorArgument("ICobranzaValorItemRepository", kernel.GetService(typeof(ICobranzaValorItemRepository)))
                .WithConstructorArgument("ICobranzaComprobanteItemRepository", kernel.GetService(typeof(ICobranzaComprobanteItemRepository)));

            kernel.Bind<IVentasReportService>().To<VentasReportService>().InSingletonScope()
                .WithConstructorArgument("IComprobanteVentaRepository", kernel.GetService(typeof(IComprobanteVentaRepository)));

            kernel.Bind<IComprasConfigService>().To<ComprasConfigService>().InSingletonScope()
                .WithConstructorArgument("IProveedorRepository", kernel.GetService(typeof(IProveedorRepository)))
                .WithConstructorArgument("IRubroCompraRepository", kernel.GetService(typeof(IRubroCompraRepository)));

            kernel.Bind<IComprasReportService>().To<ComprasReportService>().InSingletonScope()
                .WithConstructorArgument("IComprobanteCompraRepository", kernel.GetService(typeof(IComprobanteCompraRepository)));

            kernel.Bind<IComprasService>().To<ComprasService>().InSingletonScope()
                .WithConstructorArgument("IProveedorRepository", kernel.GetService(typeof(IProveedorRepository)))
                .WithConstructorArgument("IDetalleComprobanteCompraRepository", kernel.GetService(typeof(IDetalleComprobanteCompraRepository)))
                .WithConstructorArgument("IComprobanteCompraRepository", kernel.GetService(typeof(IComprobanteCompraRepository)))
                .WithConstructorArgument("IImputacionCompraRepository", kernel.GetService(typeof(IImputacionCompraRepository)));

            kernel.Bind<ITesoreriaConfigService>().To<TesoreriaConfigService>().InSingletonScope()
                .WithConstructorArgument("IBancoRepository", kernel.GetService(typeof(IBancoRepository)))
                .WithConstructorArgument("ICuentaBancariaRepository", kernel.GetService(typeof(ICuentaBancariaRepository)))
                .WithConstructorArgument("ICajaRepository", kernel.GetService(typeof(ICajaRepository)))
                .WithConstructorArgument("IValorRepository", kernel.GetService(typeof(IValorRepository)))
                .WithConstructorArgument("IChequeraRepository", kernel.GetService(typeof(IChequeraRepository)));

            kernel.Bind<ITesoreriaService>().To<TesoreriaService>().InSingletonScope()
                .WithConstructorArgument("IBancoRepository", kernel.GetService(typeof(IBancoRepository)))
                .WithConstructorArgument("ICuentaBancariaRepository", kernel.GetService(typeof(ICuentaBancariaRepository)))
                .WithConstructorArgument("ICajaRepository", kernel.GetService(typeof(ICajaRepository)))
                .WithConstructorArgument("IMovimientoFondoRepository", kernel.GetService(typeof(IMovimientoFondoRepository)))
                .WithConstructorArgument("IValorRepository", kernel.GetService(typeof(IValorRepository)))
                .WithConstructorArgument("IDepositoRepository", kernel.GetService(typeof(IDepositoRepository)))
                .WithConstructorArgument("IDepositoItemRepository", kernel.GetService(typeof(IDepositoItemRepository)));

            kernel.Bind<IContabilidadReportService>().To<ContabilidadReportService>().InSingletonScope()
                .WithConstructorArgument("IAsientoItemRepository", kernel.GetService(typeof(IAsientoItemRepository)));

            kernel.Bind<ITesoreriaReportService>().To<TesoreriaReportService>().InSingletonScope()
                .WithConstructorArgument("IValorIngresadoRepository", kernel.GetService(typeof(IValorIngresadoRepository)));

            kernel.Bind<IStockConfigService>().To<StockConfigService>().InSingletonScope()
                .WithConstructorArgument("IArticuloRepository", kernel.GetService(typeof(IArticuloRepository)))
                .WithConstructorArgument("IRubroArticuloRepository", kernel.GetService(typeof(IRubroArticuloRepository)))
                .WithConstructorArgument("IIngresoStockRepository", kernel.GetService(typeof(IIngresoStockRepository)))
                .WithConstructorArgument("IListaPreciosRepository", kernel.GetService(typeof(IListaPreciosRepository)));
            kernel.Bind<IStockService>().To<StockService>().InSingletonScope()
                .WithConstructorArgument("IArticuloRepository", kernel.GetService(typeof(IArticuloRepository)))
                .WithConstructorArgument("IEgresoStockRepository", kernel.GetService(typeof(IEgresoStockRepository)))
                .WithConstructorArgument("IIngresoStockRepository", kernel.GetService(typeof(IIngresoStockRepository)));

            kernel.Bind<ISueldosConfigService>().To<SueldosConfigService>().InSingletonScope()
                .WithConstructorArgument("IEmpleadoRepository", kernel.GetService(typeof(IEmpleadoRepository)))
                .WithConstructorArgument("IAdicionalRepository", kernel.GetService(typeof(IAdicionalRepository)))
                .WithConstructorArgument("IAdicionalAdicionalesRepository", kernel.GetService(typeof(IAdicionalAdicionalesRepository)));
            kernel.Bind<ISueldosService>().To<SueldosService>().InSingletonScope()
                .WithConstructorArgument("IReciboRepository", kernel.GetService(typeof(IReciboRepository)));
            
            //Reporting
            kernel.Bind<IReportingService>().To<ReportingService>().InSingletonScope()
                .WithConstructorArgument("IGrupoIngresoRepository", kernel.GetService(typeof(IGrupoEgresoRepository)))
                .WithConstructorArgument("IGrupoEgresoRepository", kernel.GetService(typeof(IGrupoIngresoRepository)));

            // SCHEDULER

            kernel.Bind<Scheduler.Scheduler>().ToSelf().InSingletonScope()
                .WithConstructorArgument("ISystemService", kernel.GetService(typeof(SystemService)));


        }
    }
}
