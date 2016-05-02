using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Contabilidad;
using Woopin.SGC.Repositories.Tesoreria;
using PostSharp.Patterns.Diagnostics;
using Woopin.SGC.Common.App.Logging;

namespace Woopin.SGC.Services
{
    public class TesoreriaService : ITesoreriaService
    {
        #region Variables y Constructor

        private readonly ICuentaBancariaRepository CuentaBancariaRepository;
        private readonly ICuentaRepository CuentaRepository;
        private readonly IMovimientoFondoRepository MovimientoFondoRepository;
        private readonly IComboItemRepository ComboItemRepository;
        private readonly ICajaRepository CajaRepository;
        private readonly IValorRepository ValorRepository;
        private readonly IValorIngresadoRepository ValorIngresadoRepository;
        private readonly IHistorialCajaRepository HistorialCajaRepository;
        private readonly IHistorialCuentaBancariaRepository HistorialCuentaBancariaRepository;
        private readonly IContabilidadService ContabilidadService;
        private readonly IChequeRepository ChequeRepository;
        private readonly IChequePropioRepository ChequePropioRepository;
        private readonly ITransferenciaRepository TransferenciaRepository;
        private readonly IPagoTarjetaRepository PagoTarjetaRepository;
        private readonly ITarjetaCreditoRepository TarjetaCreditoRepository;
        private readonly ICancelacionTarjetaRepository CancelacionTarjetaRepository;
        private readonly AsientosHelper AsientoHelper;
        private readonly IDepositoRepository DepositoRepository;
        private readonly IRetencionRepository RetencionRepository;
        private readonly IComprobanteRetencionRepository ComprobanteRetencionRepository;
        private readonly IUsuarioRepository UsuarioRepository;
        private readonly IChequeraRepository ChequeraRepository;
        public TesoreriaService(ICuentaBancariaRepository CuentaBancariaRepository, ITarjetaCreditoRepository TarjetaCreditoRepository, ICancelacionTarjetaRepository CancelacionTarjetaRepository, IValorIngresadoRepository ValorIngresadoRepository,
                            IMovimientoFondoRepository MovimientoFondoRepository, IComboItemRepository ComboItemRepository, IChequeRepository ChequeRepository, ITransferenciaRepository TransferenciaRepository,
                            ICuentaRepository CuentaRepository, ICajaRepository CajaRepository, IHistorialCuentaBancariaRepository HistorialCuentaBancariaRepository, IPagoTarjetaRepository PagoTarjetaRepository,
                            IValorRepository ValorRepository, IHistorialCajaRepository HistorialCajaRepository, IContabilidadService ContabilidadService, IChequePropioRepository ChequePropioRepository,
                            IDepositoRepository DepositoRepository, IRetencionRepository RetencionRepository, IComprobanteRetencionRepository ComprobanteRetencionRepository, IUsuarioRepository UsuarioRepository, IChequeraRepository ChequeraRepository)
        {
            this.CuentaBancariaRepository = CuentaBancariaRepository;
            this.MovimientoFondoRepository = MovimientoFondoRepository;
            this.ComboItemRepository = ComboItemRepository;
            this.CuentaRepository = CuentaRepository;
            this.CajaRepository = CajaRepository;
            this.ValorRepository = ValorRepository;
            this.HistorialCajaRepository = HistorialCajaRepository;
            this.HistorialCuentaBancariaRepository = HistorialCuentaBancariaRepository;
            this.ContabilidadService = ContabilidadService;
            this.ChequeRepository = ChequeRepository;
            this.ChequePropioRepository = ChequePropioRepository;
            this.AsientoHelper = new AsientosHelper();
            this.TransferenciaRepository = TransferenciaRepository;
            this.PagoTarjetaRepository = PagoTarjetaRepository;
            this.TarjetaCreditoRepository = TarjetaCreditoRepository;
            this.CancelacionTarjetaRepository = CancelacionTarjetaRepository;
            this.DepositoRepository = DepositoRepository;
            this.RetencionRepository = RetencionRepository;
            this.ComprobanteRetencionRepository = ComprobanteRetencionRepository;
            this.UsuarioRepository = UsuarioRepository;
            this.ValorIngresadoRepository = ValorIngresadoRepository;
            this.ChequeraRepository = ChequeraRepository;
        }

        #endregion

        #region MovimientoFondo

        [Loggable]
        public void AddMovimientoFondo(MovimientoFondo MovimientoFondo)
        {
            this.CuentaBancariaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                // Controlo ejercicio contable
                this.ContabilidadService.TryControlarIngresoNT(MovimientoFondo.Fecha);

                switch (MovimientoFondo.Movimiento.Id)
                {
                    case ValoresHelper.Transferencia:
                        MovimientoFondo.CuentaBancaria = this.RegistrarMovCuentaNT(MovimientoFondo.CuentaBancaria.Id, MovimientoFondo.Importe * -1, MovimientoFondo.Concepto);
                        MovimientoFondo.CuentaDestino = this.RegistrarMovCuentaNT(MovimientoFondo.CuentaDestino.Id, MovimientoFondo.Importe, MovimientoFondo.Concepto);
                        break;
                    case ValoresHelper.Extraccion:
                        MovimientoFondo.CuentaBancaria = this.RegistrarMovCuentaNT(MovimientoFondo.CuentaBancaria.Id, MovimientoFondo.Importe * -1, MovimientoFondo.Concepto);
                        MovimientoFondo.Caja = this.RegistrarMovCajaNT(MovimientoFondo.Caja.Id, MovimientoFondo.Importe, MovimientoFondo.Concepto);
                        break;
                    case ValoresHelper.Deposito:
                        MovimientoFondo.CuentaBancaria = this.RegistrarMovCuentaNT(MovimientoFondo.CuentaBancaria.Id, MovimientoFondo.Importe, MovimientoFondo.Concepto);
                        MovimientoFondo.Caja = this.RegistrarMovCajaNT(MovimientoFondo.Caja.Id, MovimientoFondo.Importe * -1, MovimientoFondo.Concepto);
                        break;

                }

                MovimientoFondo.Asiento = this.ContabilidadService.NuevoAsientoMovimientoFondos(MovimientoFondo);
                MovimientoFondo.Usuario = Security.GetCurrentUser();
                this.MovimientoFondoRepository.Add(MovimientoFondo);
                MovimientoFondo.Asiento.ComprobanteAsociado = MovimientoFondo.Id;
                this.ContabilidadService.UpdateAsientoNT(MovimientoFondo.Asiento);
            });


        }
        public MovimientoFondo GetMovimientoFondo(int Id)
        {
            MovimientoFondo MovimientoFondo = null;
            this.MovimientoFondoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                MovimientoFondo = this.MovimientoFondoRepository.Get(Id);
            });
            return MovimientoFondo;
        }
        public void UpdateMovimientoFondo(MovimientoFondo MovimientoFondo)
        {
            this.MovimientoFondoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.MovimientoFondoRepository.Update(MovimientoFondo);
            });
        }
        public IList<MovimientoFondo> GetAllMovimientosFondos()
        {
            IList<MovimientoFondo> MovimientosFondos = null;
            this.MovimientoFondoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                MovimientosFondos = this.MovimientoFondoRepository.GetAll();
            });
            return MovimientosFondos;
        }

        public IList<MovimientoFondo> GetAllMovimientosFondosByDates(DateTime? start, DateTime? end)
        {
            IList<MovimientoFondo> MovimientosFondos = null;
            this.MovimientoFondoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                MovimientosFondos = this.MovimientoFondoRepository.GetAllByDates(_start, _end);
            });
            return MovimientosFondos;
        }

        #endregion

        #region Historiales Caja y Banco
        public IList<HistorialCaja> GetAllHistorialCajaByDates(int IdCaja, DateTime? start, DateTime? end)
        {
            IList<HistorialCaja> Historial = null;
            this.MovimientoFondoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Historial = this.HistorialCajaRepository.GetAllByDates(IdCaja,_start, _end);
            });
            return Historial;
        }

        public IList<HistorialCuentaBancaria> GetAllHistorialCuentasByDates(int Id, DateTime? start, DateTime? end)
        {
            IList<HistorialCuentaBancaria> Historial = null;
            this.MovimientoFondoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Historial = this.HistorialCuentaBancariaRepository.GetAllByDates(Id, _start, _end);
            });
            return Historial;
        }
        #endregion

        #region Cheque
        public Cheque GetCheque(int Id)
        {
            Cheque Cheque = null;
            this.ChequeRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Cheque = this.ChequeRepository.Get(Id);
            });
            return Cheque;
        }

        public IList<Cheque> GetAllCheques()
        {
            IList<Cheque> Cheques = null;
            this.ChequeRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Cheques = this.ChequeRepository.GetAll();
            });
            return Cheques;
        }

        public IList<Cheque> GetAllChequesEnCartera()
        {
            IList<Cheque> Cheques = null;
            this.ChequeRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Cheques = this.ChequeRepository.GetAllChequesEnCartera();
            });
            return Cheques;
        }

        [Loggable]
        public void AddCheque(Cheque Cheque)
        {
            this.ChequeRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Cheque.Usuario = Security.GetCurrentUser();
                this.ChequeRepository.Add(Cheque);
            });
        }
        public void UpdateCheque(Cheque Cheque)
        {
            this.ChequeRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ChequeRepository.Update(Cheque);
            });
        }

        public IList<Cheque> GetChequeFilter(int IdCliente, int IdBanco, DateTime? start, DateTime? end, FilterCheque filter)
        {
            IList<Cheque> Cheques = null;
            this.ChequeRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Cheques = this.ChequeRepository.GetChequeFilter(IdCliente, _start, _end, filter, IdBanco);
            });
            return Cheques;
        }

        #endregion

        #region ChequePropio
        public ChequePropio GetChequePropio(int Id)
        {
            ChequePropio ChequePropio = null;
            this.ChequePropioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ChequePropio = this.ChequePropioRepository.Get(Id);
            });
            return ChequePropio;
        }

        public IList<ChequePropio> GetAllChequesPropios()
        {
            IList<ChequePropio> Cheques = null;
            this.ChequePropioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Cheques = this.ChequePropioRepository.GetAll();
            });
            return Cheques;
        }

        [Loggable]
        public void AddChequePropio(ChequePropio Cheque)
        {
            this.ChequePropioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ChequePropio cp = this.ChequePropioRepository.GetByFilter(Cheque.CuentaBancaria.Id, Cheque.Numero);
                if (cp != null)
                    throw new BusinessException("Este Numero de Cheque ya fue utilizado o esta anulado");

                Cheque.Usuario = Security.GetCurrentUser();

                this.ChequePropioRepository.Add(Cheque);
            });
        }
        public void UpdateChequePropio(ChequePropio Cheque)
        {
            this.ChequePropioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ChequePropioRepository.Update(Cheque);
            });
        }

        public IList<ChequePropio> GetChequePropioFilter(int IdProveedor, int IdCuenta, DateTime? start, DateTime? end, FilterCheque filter)
        {
            IList<ChequePropio> ChequesPropios = null;
            this.ChequePropioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ChequesPropios = this.ChequePropioRepository.GetAllByFilter(IdProveedor, IdCuenta, _start, _end, filter);
            });
            return ChequesPropios;
        }

        [Loggable]
        public void CanjeCheque(int IdAnterior, ChequePropio NuevoCheque)
        {
            this.ChequePropioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ChequePropio chequeAnterior = this.ChequePropioRepository.Get(IdAnterior);
                NuevoCheque.CuentaBancaria = this.CuentaBancariaRepository.Get(NuevoCheque.CuentaBancaria.Id);



                // Cancelo el cheque anterior.
                chequeAnterior.Estado = EstadoCheque.Anulado;
                this.ChequePropioRepository.Update(chequeAnterior);

                // Creo el nuevo cheque.
                ChequePropio newCheque = new ChequePropio()
                {
                    Estado = EstadoCheque.Entregado,
                    Fecha = NuevoCheque.Fecha,
                    FechaCreacion = DateTime.Now,
                    Importe = chequeAnterior.Importe,
                    CuentaBancaria = NuevoCheque.CuentaBancaria,
                    Proveedor = chequeAnterior.Proveedor,
                    Usuario = Security.GetCurrentUser(),
                    Numero = NuevoCheque.Numero
                };
                this.ChequePropioRepository.Add(newCheque);

                // Controlo si hay que realizar un asiento para mover banco.
                if (chequeAnterior.CuentaBancaria.Id != NuevoCheque.CuentaBancaria.Id)
                {
                    this.ContabilidadService.TryControlarIngresoNT(newCheque.Fecha);
                    this.ContabilidadService.NuevoAsientoCanjeCheques(newCheque, chequeAnterior);
                }



                // Actualizo el valor ingresado para que vea reflejada la nueva información.
                ValorIngresado vi = this.ValorIngresadoRepository.GetByTipo(chequeAnterior.Id, TipoValor.ChequePropio);
                vi.NumeroOperacion = NuevoCheque.Numero.ToString();

                vi.Descripcion = NuevoCheque.CuentaBancaria.Banco.Nombre + "-" + NuevoCheque.CuentaBancaria.Nombre;
                vi.Fecha = NuevoCheque.Fecha;
                vi.NumeroReferencia = newCheque.Id;
                this.ValorIngresadoRepository.Update(vi);

            });
        }

        public void DeleteChequesPropios(List<int> Ids)
        {
            this.ChequePropioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    ChequePropio ChequePropio = this.ChequePropioRepository.Get(Id);
                    this.ChequePropioRepository.Delete(ChequePropio);
                }
            });
        }

        [Loggable]
        public void AnularChequePropio(int IdCuentaBancaria, int Numero)
        {
            this.ChequePropioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ChequePropio chequeExistente = this.ChequePropioRepository.GetByFilter(IdCuentaBancaria, Numero);

                if (chequeExistente != null)
                    throw new BusinessException("El cheque ya fue usado");

                ChequePropio cp = new ChequePropio()
                {
                    CuentaBancaria = new CuentaBancaria() { Id = IdCuentaBancaria },
                    Numero = Numero,
                    Usuario = Security.GetCurrentUser(),
                    Estado = EstadoCheque.Anulado,
                    Fecha = DateTime.Now,
                    FechaCreacion = DateTime.Now,
                    Importe = 0,
                    Proveedor = null
                };

                this.ChequePropioRepository.Add(cp);

            });

        }

        [Loggable]
        public void ConfirmarPagoChequePropio(int IdChequePropio, DateTime fechaPago)
        {
            this.ChequePropioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ChequePropio c = this.ChequePropioRepository.Get(IdChequePropio);
                c.FechaPago = fechaPago;
                c.Estado = EstadoCheque.Pagado;
                this.ChequePropioRepository.Update(c);
                string concepto = "Debito del proveedor" + c.Proveedor.RazonSocial + " por cheque " + c.Numero;
                this.RegistrarMovCuentaNT(c.CuentaBancaria.Id, c.Importe * (int)TipoIngreso.Egreso, concepto);
            });
        }

        #endregion

        #region Transferencia
        public Transferencia GetTransferencia(int Id)
        {
            Transferencia transferencia = null;
            this.TransferenciaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                transferencia = this.TransferenciaRepository.Get(Id);
            });
            return transferencia;
        }

        public IList<Transferencia> GetAllTransferencias()
        {
            IList<Transferencia> transferencias = null;
            this.TransferenciaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                transferencias = this.TransferenciaRepository.GetAll();
            });
            return transferencias;
        }

        [Loggable]
        public void AddTransferencia(Transferencia transferencia)
        {
            this.TransferenciaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                transferencia.Usuario = Security.GetCurrentUser();
                this.TransferenciaRepository.Add(transferencia);
            });
        }
        public void UpdateTransferencia(Transferencia transferencia)
        {
            this.TransferenciaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.TransferenciaRepository.Update(transferencia);
            });
        }
        public IList<Transferencia> GetTransferenciaFilter(int IdCuentaBancaria, int IdProveedor, int IdCliente, DateTime? start, DateTime? end)
        {
            IList<Transferencia> Transferencias = null;
            this.TransferenciaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Transferencias = this.TransferenciaRepository.GetTransferenciaFilter(IdCuentaBancaria, IdProveedor, IdCliente, _start, _end);
            });
            return Transferencias;
        }


        #endregion

        #region Pago con Tarjeta
        public PagoTarjeta GetPagoTarjeta(int Id)
        {
            PagoTarjeta PagoTarjeta = null;
            this.PagoTarjetaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                PagoTarjeta = this.PagoTarjetaRepository.Get(Id);
            });
            return PagoTarjeta;
        }
        public PagoTarjeta GetPagoTarjetaCompleta(int Id)
        {
            PagoTarjeta PagoTarjeta = null;
            this.PagoTarjetaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                PagoTarjeta = this.PagoTarjetaRepository.GetCompleto(Id);
            });
            return PagoTarjeta;
        }
        public IList<PagoTarjeta> GetAllPagoTarjetas()
        {
            IList<PagoTarjeta> pagos = null;
            this.PagoTarjetaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                pagos = this.PagoTarjetaRepository.GetAll();
            });
            return pagos;
        }
        public void AddPagoTarjeta(PagoTarjeta PagoTarjeta)
        {
            this.PagoTarjetaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                PagoTarjeta.Usuario = Security.GetCurrentUser();
                this.PagoTarjetaRepository.Add(PagoTarjeta);
            });
        }
        public void UpdatePagoTarjeta(PagoTarjeta PagoTarjeta)
        {
            this.PagoTarjetaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.PagoTarjetaRepository.Update(PagoTarjeta);
            });
        }
        public IList<PagoTarjeta> GetAllPagoTarjetasByDates(int Id, DateTime? start, DateTime? end, PagoTarjetaFilter filter)
        {
            IList<PagoTarjeta> Pagos = null;
            this.MovimientoFondoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Pagos = this.PagoTarjetaRepository.GetAllByDates(Id, _start, _end, filter);
            });
            return Pagos;
        }
        #endregion

        #region Cancelacion de Tarjeta
        public CancelacionTarjeta GetCancelacionTarjeta(int Id)
        {
            CancelacionTarjeta CancelacionTarjeta = null;
            this.CancelacionTarjetaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                CancelacionTarjeta = this.CancelacionTarjetaRepository.Get(Id);
            });
            return CancelacionTarjeta;
        }
        public IList<CancelacionTarjeta> GetAllCancelacionTarjetas()
        {
            IList<CancelacionTarjeta> cancelaciones = null;
            this.CancelacionTarjetaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                cancelaciones = this.CancelacionTarjetaRepository.GetAll();
            });
            return cancelaciones;
        }
        public void AddCancelacionTarjeta(CancelacionTarjeta CancelacionTarjeta)
        {
            this.CancelacionTarjetaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {

                // Comprobar fecha de Cierre
                this.ContabilidadService.TryControlarIngresoNT(CancelacionTarjeta.Fecha);

                string Concepto = "Cancelación de Tarjeta - Debito Automatico";
                CancelacionTarjeta.Pago = this.PagoTarjetaRepository.Get(CancelacionTarjeta.Pago.Id);

                if (CancelacionTarjeta.Pago.Estado == EstadoPagoTarjeta.Anulada)
                    throw new ValidationException("El Pago está anulado");


                CancelacionTarjeta.Pago.TotalCancelado += CancelacionTarjeta.Importe;
                if (CancelacionTarjeta.Pago.CuotasCanceladas + CancelacionTarjeta.Cuotas > CancelacionTarjeta.Pago.Cuotas ||
                            CancelacionTarjeta.Pago.TotalCancelado >= CancelacionTarjeta.Pago.Total)
                {
                    CancelacionTarjeta.Pago.CuotasCanceladas = CancelacionTarjeta.Pago.Cuotas;
                    CancelacionTarjeta.Pago.Estado = EstadoPagoTarjeta.Cancelada;
                }
                else
                {
                    CancelacionTarjeta.Pago.CuotasCanceladas = CancelacionTarjeta.Pago.CuotasCanceladas + CancelacionTarjeta.Cuotas;
                    CancelacionTarjeta.Pago.Estado = EstadoPagoTarjeta.Cancelando;
                }

                if (CancelacionTarjeta.Pago.CuotasCanceladas == CancelacionTarjeta.Pago.Cuotas &&
                    CancelacionTarjeta.Pago.TotalCancelado < CancelacionTarjeta.Pago.Total)
                    throw new ValidationException("Esta cancelando todas las cuotas, y tiene saldo pendiente.");

                this.PagoTarjetaRepository.Update(CancelacionTarjeta.Pago);

                CancelacionTarjeta.FechaCreacion = DateTime.Now;

                Transferencia t = new Transferencia()
                {
                    Estado = EstadoTransferencia.Creado,
                    Fecha = CancelacionTarjeta.Fecha,
                    FechaCreacion = DateTime.Now,
                    Importe = CancelacionTarjeta.Importe,
                    CuentaBancaria = CancelacionTarjeta.Pago.Tarjeta.CuentaBancaria
                };
                this.TransferenciaRepository.Add(t);

                CancelacionTarjeta.Valor = new ValorIngresado()
                {
                    TipoIngreso = TipoIngreso.Egreso,
                    Descripcion = Concepto,
                    Fecha = CancelacionTarjeta.Fecha,
                    FechaCreacion = DateTime.Now,
                    IdEntidadExt = CancelacionTarjeta.Pago.Id,
                    Importe = CancelacionTarjeta.Importe,
                    NumeroReferencia = t.Id,
                    CuentaContable = CancelacionTarjeta.Pago.Tarjeta.CuentaBancaria.CuentaContable,
                    Valor = new Valor() { Id = 6 } // TODO! : Sacar el magic number 6. Establecer alguna equivalencia.
                };

                CancelacionTarjeta.Pago.Tarjeta.CuentaBancaria = this.RegistrarMovCuentaNT(CancelacionTarjeta.Pago.Tarjeta.CuentaBancaria.Id, CancelacionTarjeta.Importe * (int)TipoIngreso.Egreso, Concepto);
                CancelacionTarjeta.Asiento = this.ContabilidadService.NuevoAsientoCancelacionTarjeta(CancelacionTarjeta);

                CancelacionTarjeta.Usuario = Security.GetCurrentUser();
                this.CancelacionTarjetaRepository.Add(CancelacionTarjeta);

                CancelacionTarjeta.Asiento.ComprobanteAsociado = CancelacionTarjeta.Id;
                this.ContabilidadService.UpdateAsientoNT(CancelacionTarjeta.Asiento);
            });
        }
        public void UpdateCancelacionTarjeta(CancelacionTarjeta CancelacionTarjeta)
        {
            this.CancelacionTarjetaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.CancelacionTarjetaRepository.Update(CancelacionTarjeta);
            });
        }

        #endregion

        #region ValoresIngresados
        public void RegistrarIngresosNT(IList<ValorIngresado> Valores, TipoIngreso tipo, string Concepto, string NumeroRetencion)
        {
            foreach (var Valor in Valores)
            {
                switch (Valor.Valor.TipoValor.Data)
                {
                    case TipoValor.Cheque:
                        Cheque c = this.ChequeRepository.Get(Valor.NumeroReferencia);
                        c.Estado = EstadoCheque.Cartera;
                        this.ChequeRepository.Update(c);
                        break;

                    case TipoValor.ChequePropio:
                        ChequePropio cp = this.ChequePropioRepository.Get(Valor.NumeroReferencia);
                        cp.Estado = EstadoCheque.Entregado;
                        this.ChequePropioRepository.Update(cp);
                        break;

                    case TipoValor.Efectivo:
                        this.RegistrarMovCajaNT(Valor.IdEntidadExt, Valor.Importe * (int)tipo, Concepto);
                        break;

                    case TipoValor.Transferencia:
                        Transferencia t = this.TransferenciaRepository.Get(Valor.NumeroReferencia);
                        t.Estado = EstadoTransferencia.Creado;
                        this.TransferenciaRepository.Update(t);
                        this.RegistrarMovCuentaNT(t.CuentaBancaria.Id, Valor.Importe * (int)tipo, Concepto);
                        break;

                    case TipoValor.TarjetaCredito:
                        PagoTarjeta pagoTarjeta = this.PagoTarjetaRepository.Get(Valor.NumeroReferencia);
                        pagoTarjeta.Estado = EstadoPagoTarjeta.Emitida;
                        pagoTarjeta.Detalle = Concepto;
                        this.PagoTarjetaRepository.Update(pagoTarjeta);
                        break;

                    case TipoValor.Retencion:
                        ComprobanteRetencion comprobanteRetencion = this.ComprobanteRetencionRepository.Get(Valor.NumeroReferencia);
                        comprobanteRetencion.Estado = EstadoRetencion.Recibido;
                        comprobanteRetencion.NumeroRetencion = NumeroRetencion.ToString();
                        this.ComprobanteRetencionRepository.Update(comprobanteRetencion);
                        break;

                    default:
                        throw new BusinessException("El Tipo de Valor Ingresado no existe.");
                }
            }
        }
        public void GetCuentaContableForValor(ValorIngresado valor, TipoIngreso tipo)
        {
            valor.Valor = this.ValorRepository.Get(valor.Valor.Id);
            string tipoValor = valor.Valor.TipoValor.Data;

            switch (tipoValor)
            {
                case TipoValor.Cheque:
                    valor.CuentaContable = new Cuenta { Codigo = this.AsientoHelper.ValoresADepositar };
                    break;

                case TipoValor.ChequePropio:
                    CuentaBancaria cuenta = this.CuentaBancariaRepository.Get(valor.IdEntidadExt);
                    valor.CuentaContable = new Cuenta { Id = cuenta.CuentaContable.Id };
                    break;

                case TipoValor.Efectivo:
                    Caja caja = this.CajaRepository.Get(valor.IdEntidadExt);
                    valor.CuentaContable = new Cuenta { Id = caja.CuentaContable.Id };
                    break;

                case TipoValor.Transferencia:
                    CuentaBancaria cuentaTransferencia = this.CuentaBancariaRepository.Get(valor.IdEntidadExt);
                    valor.CuentaContable = new Cuenta { Id = cuentaTransferencia.CuentaContable.Id };
                    break;

                case TipoValor.TarjetaCredito:
                    TarjetaCredito tarjeta = this.TarjetaCreditoRepository.Get(valor.IdEntidadExt);
                    valor.CuentaContable = new Cuenta { Id = tarjeta.CuentaContable.Id };
                    break;

                case TipoValor.Retencion:
                    Retencion retencion = this.RetencionRepository.Get(valor.IdEntidadExt);
                    if (tipo == TipoIngreso.Egreso)
                    {
                        valor.CuentaContable = new Cuenta { Id = retencion.CuentaContable.Id };
                    }
                    else
                    {
                        valor.CuentaContable = new Cuenta { Id = retencion.CuentaADepositar.Id };
                    }
                    break;

                default:
                    throw new BusinessException("El Tipo de Valor Ingresado no existe.");
            }
        }
        public void CancelarIngresosNT(IList<ValorIngresado> Valores, TipoIngreso tipo, string Concepto)
        {
            foreach (var Valor in Valores)
            {
                switch (Valor.Valor.TipoValor.Data)
                {
                    case TipoValor.Cheque:
                        Cheque c = this.ChequeRepository.Get(Valor.NumeroReferencia);
                        if (c.Estado == EstadoCheque.Depositado)
                            throw new BusinessException("Uno de los cheques de terceros se encuentra depositado");
                        c.Estado = EstadoCheque.Anulado;
                        this.ChequeRepository.Update(c);
                        break;

                    case TipoValor.ChequePropio:
                        ChequePropio cp = this.ChequePropioRepository.Get(Valor.NumeroReferencia);
                        if (cp.Estado == EstadoCheque.Pagado)
                        {
                            this.RegistrarMovCuentaNT(cp.CuentaBancaria.Id, Valor.Importe * (int)tipo * -1, Concepto);
                        }
                        cp.Estado = EstadoCheque.Borrador;
                        this.ChequePropioRepository.Update(cp);
                        break;

                    case TipoValor.Efectivo:
                        this.RegistrarMovCajaNT(Valor.IdEntidadExt, Valor.Importe * (int)tipo * -1, Concepto);
                        break;

                    case TipoValor.Transferencia:
                        Transferencia t = this.TransferenciaRepository.Get(Valor.NumeroReferencia);
                        t.Estado = EstadoTransferencia.Anulado;
                        this.TransferenciaRepository.Update(t);
                        this.RegistrarMovCuentaNT(t.CuentaBancaria.Id, Valor.Importe * (int)tipo * -1, Concepto);
                        break;

                    case TipoValor.TarjetaCredito:
                        PagoTarjeta pagoTarjeta = this.PagoTarjetaRepository.Get(Valor.NumeroReferencia);
                        if (pagoTarjeta.Cancelaciones.Count > 0)
                            throw new BusinessException("Uno de los pagos con tarjeta posee una cancelación.");
                        pagoTarjeta.Estado = EstadoPagoTarjeta.Anulada;
                        this.PagoTarjetaRepository.Update(pagoTarjeta);
                        break;

                    case TipoValor.Retencion:
                        ComprobanteRetencion comprobanteRetencion = this.ComprobanteRetencionRepository.Get(Valor.NumeroReferencia);
                        comprobanteRetencion.Estado = EstadoRetencion.Anulada;
                        this.ComprobanteRetencionRepository.Update(comprobanteRetencion);
                        break;

                    default:
                        throw new BusinessException("El Tipo de Valor Ingresado no existe.");
                }
            }
        }

        #endregion

        #region Registrar Movimientos
        public Caja RegistrarMovCajaNT(int IdCaja, decimal Monto, string Concepto)
        {
            Caja caja = this.CajaRepository.Get(IdCaja);
            caja.Fondos += Monto;
            this.CajaRepository.Update(caja);
            this.HistorialCajaRepository.Add(new HistorialCaja(Concepto, Monto, caja.Fondos, caja));
            return caja;
        }
        public CuentaBancaria RegistrarMovCuentaNT(int IdCuenta, decimal Monto, string Concepto)
        {
            CuentaBancaria cuenta = this.CuentaBancariaRepository.Get(IdCuenta);
            cuenta.Fondo += Monto;
            this.CuentaBancariaRepository.Update(cuenta);
            this.HistorialCuentaBancariaRepository.Add(new HistorialCuentaBancaria(Concepto, Monto, cuenta.Fondo, cuenta));
            return cuenta;
        }
        #endregion

        #region Deposito
        public Deposito GetDeposito(int Id)
        {
            Deposito Deposito = null;
            this.DepositoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Deposito = this.DepositoRepository.Get(Id);
            });
            return Deposito;
        }

        public IList<Deposito> GetDepositoFilter(int idCuentaBancaria, DateTime? start, DateTime? end)
        {
            IList<Deposito> Depositos = null;
            this.DepositoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Depositos = this.DepositoRepository.GetDepositoFilter(idCuentaBancaria, _start, _end);
            });
            return Depositos;
        }

        [Loggable]
        public void AddDeposito(Deposito Deposito)
        {
            this.DepositoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                decimal Importe = 0;
                foreach (var itemdeposito in Deposito.Cheques)
                {
                    itemdeposito.Cheque = this.ChequeRepository.Get(itemdeposito.Cheque.Id);
                    Importe += itemdeposito.Cheque.Importe;

                    if (itemdeposito.Cheque.FechaVencimiento < DateTime.Now)
                    {
                        throw new BusinessException("El cheque " + itemdeposito.Cheque.Numero + " se encuentra vencido, no es posible seguir con la operación");
                    }

                    itemdeposito.Cheque.Estado = EstadoCheque.Depositado;
                    itemdeposito.Cheque.FechaEfectivizado = Deposito.FechaAcreditacion;
                    this.ChequeRepository.Update(itemdeposito.Cheque);
                }
                string Concepto = Deposito.Concepto != null ? Deposito.Concepto : "Deposito";
                this.RegistrarMovCuentaNT(Deposito.Cuenta.Id, Importe, Concepto);
                Deposito.Cuenta = this.CuentaBancariaRepository.Get(Deposito.Cuenta.Id);
                Deposito.Asiento = this.ContabilidadService.NuevoAsientoDeposito(Deposito);
                Deposito.Usuario = Security.GetCurrentUser();
                Deposito.Total = Importe;
                this.DepositoRepository.Add(Deposito);
                Deposito.Asiento.ComprobanteAsociado = Deposito.Id;
                this.ContabilidadService.UpdateAsientoNT(Deposito.Asiento);
            });
        }
        public void UpdateDeposito(Deposito Deposito)
        {
            this.DepositoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.DepositoRepository.Update(Deposito);
            });
        }

        public int GetProximoNumeroReferenciaDeposito()
        {
            int proximoDeposito = 0;
            this.DepositoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                proximoDeposito = this.DepositoRepository.GetProximoNumeroReferencia();
            });
            return proximoDeposito;
        }

        public Deposito GetDepositoCompleto(int Id)
        {
            Deposito Deposito = null;
            this.DepositoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Deposito = this.DepositoRepository.GetDepositoCompleto(Id);
            });
            return Deposito;
        }

        #endregion

        #region Comprobante de Retenciones
        public ComprobanteRetencion GetComprobanteRetencion(int Id)
        {
            ComprobanteRetencion ComprobanteRetencion = null;
            this.ComprobanteRetencionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ComprobanteRetencion = this.ComprobanteRetencionRepository.Get(Id);
            });
            return ComprobanteRetencion;
        }
        public IList<ComprobanteRetencion> GetAllComprobanteRetenciones()
        {
            IList<ComprobanteRetencion> ComprobanteRetenciones = null;
            this.ComprobanteRetencionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ComprobanteRetenciones = this.ComprobanteRetencionRepository.GetAll();
            });
            return ComprobanteRetenciones;
        }

        [Loggable]
        public void AddComprobanteRetencion(ComprobanteRetencion ComprobanteRetencion)
        {
            this.ComprobanteRetencionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ComprobanteRetencion.Usuario = Security.GetCurrentUser();
                this.ComprobanteRetencionRepository.Add(ComprobanteRetencion);
            });
        }
        public void UpdateComprobanteRetencion(ComprobanteRetencion ComprobanteRetencion)
        {
            this.ComprobanteRetencionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ComprobanteRetencionRepository.Update(ComprobanteRetencion);
            });
        }
        public IList<ComprobanteRetencion> GetRetencionFilter(int TipoRetencion, int IdProveedor, int IdCliente, DateTime? start, DateTime? end)
        {
            IList<ComprobanteRetencion> ComprobantesRetenciones = null;
            this.ComprobanteRetencionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ComprobantesRetenciones = this.ComprobanteRetencionRepository.GetRetencionFilter(TipoRetencion, IdProveedor, IdCliente, _start, _end);
            });
            return ComprobantesRetenciones;
        }

        public IList<ComprobanteRetencionReporte> GetRetencionFilterReporte(int TipoRetencion, int IdProveedor, int IdCliente, DateTime? start, DateTime? end)
        {
            IList<ComprobanteRetencionReporte> ComprobantesRetencionesReportes = null;
            this.ComprobanteRetencionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ComprobantesRetencionesReportes = this.ComprobanteRetencionRepository.GetRetencionFilterReporte(TipoRetencion, IdProveedor, IdCliente, _start, _end);
            });
            return ComprobantesRetencionesReportes;
        }

        #endregion
    }
}
