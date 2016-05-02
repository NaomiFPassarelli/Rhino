using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Ventas;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Common.App.Logging;

namespace Woopin.SGC.Services
{
    
    public class VentasService : IVentasService
    {

        #region VariablesyConstructor

        private readonly IComprobanteVentaRepository ComprobanteVentaRepository;
        private readonly IDetalleComprobanteVentaRepository DetalleComprobanteVentaRepository;
        private readonly ICobranzaRepository CobranzaRepository;
        private readonly IContabilidadService ContabilidadService;
        private readonly ICobranzaValorItemRepository CobranzaValorItemRepository;
        private readonly IMonedaRepository MonedaRepository;
        private readonly IComboItemRepository comboItemRepository;
        private readonly IClienteRepository ClienteRepository;
        private readonly ITesoreriaService TesoreriaService;
        private readonly IUsuarioRepository UsuarioRepository;
        private readonly IImputacionVentaRepository ImputacionRepository;
        public VentasService(IComprobanteVentaRepository ComprobanteVentaRepository, ITesoreriaService TesoreriaService,
            IDetalleComprobanteVentaRepository DetalleComprobanteVentaRepository, IClienteRepository ClienteRepository,IImputacionVentaRepository ImputacionRepository,
            ICobranzaRepository CobranzaRepository, IContabilidadService ContabilidadService,IComboItemRepository comboItemRepository,
            ICobranzaValorItemRepository CobranzaValorItemRepository, IMonedaRepository MonedaRepository, IUsuarioRepository UsuarioRepository)
        {
            this.ComprobanteVentaRepository = ComprobanteVentaRepository;
            this.DetalleComprobanteVentaRepository = DetalleComprobanteVentaRepository;
            this.CobranzaRepository = CobranzaRepository;
            this.ContabilidadService = ContabilidadService;
            this.CobranzaValorItemRepository = CobranzaValorItemRepository;
            this.MonedaRepository = MonedaRepository;
            this.comboItemRepository = comboItemRepository;
            this.ClienteRepository = ClienteRepository;
            this.TesoreriaService = TesoreriaService;
            this.UsuarioRepository = UsuarioRepository;
            this.ImputacionRepository = ImputacionRepository;
        }

        #endregion

        #region ComprobanteVenta
        
        
        public ComprobanteVenta GetComprobanteVenta(int Id)
        {
            ComprobanteVenta Comprobante = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobante = this.ComprobanteVentaRepository.Get(Id);
            });
            return Comprobante;
        }

        public ComprobanteVenta GetComprobanteVentaCompleto(int Id)
        {
            ComprobanteVenta Comprobante = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobante = this.ComprobanteVentaRepository.GetComprobanteVentaCompleto(Id);
            });
            return Comprobante;
        }

        public IList<ComprobanteVenta> GetAllComprobantesVentasByCliente(int IdCliente, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, Model.Common.CuentaCorrienteFilter filter)
        {
            IList<ComprobanteVenta> Comprobantes = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                DateTime _startvenc = startvenc.HasValue ? startvenc.Value : DateTime.Parse("1970-01-01");
                DateTime _endvenc = endvenc.HasValue ? endvenc.Value : DateTime.Parse("9998-12-31");
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Comprobantes = this.ComprobanteVentaRepository.GetAllByCliente(IdCliente, _start, _end, _startvenc, _endvenc, filter);
            });
            return Comprobantes;
        }

        public IList<ComprobanteVenta> GetAllComprobantesVentas()
        {
            IList<ComprobanteVenta> Comprobantes = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobantes = this.ComprobanteVentaRepository.GetAll();
            });
            return Comprobantes;
        }

        public void AddComprobanteVenta(ComprobanteVenta Comprobante)
        {
            this.ComprobanteVentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddComprobanteVentaNT(Comprobante);
            });
        }

        [Loggable]
        public void AddComprobanteVentaNT(ComprobanteVenta Comprobante)
        {
            ImputacionVenta imputacion = null;

            // Comprobar fecha de Cierre
            this.ContabilidadService.TryControlarIngresoNT(Comprobante.Fecha);


            // Trae la informacion adicional de los objetos relacionados
            Comprobante.CondicionVenta = this.comboItemRepository.Get(Comprobante.CondicionVenta.Id);
            Comprobante.Moneda = this.MonedaRepository.Get(Comprobante.Moneda.Id);
            Comprobante.Tipo = this.comboItemRepository.Get(Comprobante.Tipo.Id);
            Comprobante.Cliente = this.ClienteRepository.Get(Comprobante.Cliente.Id);
            Comprobante = ComprobanteVentaHelper.ReMap(Comprobante);
            Comprobante.Estado = EstadoComprobante.Creada;

            // Nota de credito con imputación
            if (Comprobante.Tipo.AdditionalData == "-1" && Comprobante.Imputacion != null && Comprobante.Imputacion.Count > 0 
                && Comprobante.Imputacion.First().ComprobanteADescontar != null && Comprobante.Imputacion.First().ComprobanteADescontar.Numero != null)
            {
                ComprobanteVenta c = this.ComprobanteVentaRepository.Get(Comprobante.Imputacion.First().ComprobanteADescontar.Id);
                imputacion = new ImputacionVenta();
                if (c == null)
                    throw new BusinessException("La Factura a imputar la Nota de Credito no existe, verificar y volver a intentar");
                if (c.Cliente.Id != Comprobante.Cliente.Id)
                    throw new BusinessException("Esta intentando hacer una Nota de Credito sobre una factura de un Cliente distinto!");
                if (Comprobante.Total > (c.Total - c.TotalCobrado))
                    throw new BusinessException("Cuando Imputa desde la creación del comprobante el importe de la nota de credito debe ser menor o igual a la Factura");

                c.TotalCobrado += Comprobante.Total;
                if (c.Total == c.TotalCobrado)
                {
                    c.Estado = EstadoComprobante.Cobrada;
                }
                this.ComprobanteVentaRepository.Update(c);

                Comprobante.TotalCobrado = Comprobante.Total; // No genera deuda pendiente y se descarto anteriormente que sea mayor el importe de la NC que de la factura
                Comprobante.Estado = EstadoComprobante.Imputado;

                imputacion.ComprobanteADescontar = new ComprobanteVenta();
                imputacion.ComprobanteADescontar.Id = c.Id;
            }             


            // Verifico que ese numero no haya sido tomado.
            Comprobante.Numero = this.ComprobanteVentaRepository.GetProximoComprobante(Comprobante.Letra, Comprobante.Tipo.Id, Comprobante.Talonario.Id);
            Comprobante.Asiento = this.ContabilidadService.NuevoAsientoVentaNT(Comprobante);
            

            if (Comprobante.Tipo.AfipData != null)
            {
                Comprobante.Estado = EstadoComprobante.Pendiente_Afip;
            }

            Comprobante.Usuario = Security.GetCurrentUser();
            Comprobante.Organizacion = Security.GetOrganizacion();

            this.ComprobanteVentaRepository.Add(Comprobante);
            
            // Si imputo, guardo la imputacion.
            if (Comprobante.Tipo.AdditionalData == "-1" && imputacion != null)
            {
                imputacion.Importe = Comprobante.Total;
                imputacion.NotaCredito = new ComprobanteVenta();
                imputacion.NotaCredito.Id = Comprobante.Id;
                this.AddImputacionNT(imputacion);
            }

            Comprobante.Asiento.ComprobanteAsociado = Comprobante.Id;
            this.ContabilidadService.UpdateAsientoNT(Comprobante.Asiento);
        }

        public string GetProximoComprobante(string LetraComprobante, int TipoComprobante, int Talonario)
        {
            string ProximoNumeroComprobante = "";
            this.ComprobanteVentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ProximoNumeroComprobante = this.ComprobanteVentaRepository.GetProximoComprobante(LetraComprobante, TipoComprobante, Talonario);
            });
            return ProximoNumeroComprobante;
        }

        public IList<ComprobanteVenta> GetComprobantesVentasACobrar(int IdCliente)
        {
            IList<ComprobanteVenta> ComprobantesACobrar = new List<ComprobanteVenta>();
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
                {
                    ComprobantesACobrar = this.ComprobanteVentaRepository.GetComprobantesVentasACobrar(IdCliente);
                });
            return ComprobantesACobrar;
        }

        public void UpdateComprobanteVenta(ComprobanteVenta Comprobante)
        {
            this.ComprobanteVentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.UpdateComprobanteVentaNT(Comprobante);
            });
        }

        [Loggable]
        public void SetearCAEComprobanteVenta(int Id, string CAE,string Vencimiento)
        {
            this.ComprobanteVentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ComprobanteVenta c = this.ComprobanteVentaRepository.Get(Id);
                c.Estado = EstadoComprobante.Creada;
                c.CAE = CAE;
                int year = Convert.ToInt32(Vencimiento.Substring(0, 4));
                int month = Convert.ToInt32(Vencimiento.Substring(4, 2));
                int day = Convert.ToInt32(Vencimiento.Substring(6, 2));
                c.VencimientoCAE = new DateTime(year,month,day);
                this.UpdateComprobanteVentaNT(c);
            });
        }

        public void UpdateComprobanteVentaNT(ComprobanteVenta Comprobante)
        {
            this.ComprobanteVentaRepository.Update(Comprobante);
        }

        [Loggable]
        public void AnularComprobante(int IdComprobante)
        {
            
            this.ComprobanteVentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ComprobanteVenta cc = this.ComprobanteVentaRepository.GetComprobanteVentaCompleto(IdComprobante);
                
                this.ContabilidadService.TryControlarIngresoNT(cc.Fecha);

                cc.TryAnular();

                if(cc.Tipo.Id == ComprobanteVentaHelper.NotaCredito)
                {
                    // Nota de credito pre impresa.
                    IList<ImputacionVenta> cds = this.ImputacionRepository.GetAllByComprobante(IdComprobante);
                    if (cds.Count > 0)
                        throw new BusinessException("La Nota de Credito contiene imputaciones hechas en Comprobantes asociados, debe eliminar las imputaciones antes de eliminar la Nota de Credito.");

                }
                
                cc.Estado = EstadoComprobante.Anulada;
                int asientoId = cc.Asiento.Id;
                cc.Asiento = null;
                this.UpdateComprobanteVentaNT(cc);
                this.ContabilidadService.DeleteAsientoNT(asientoId);                
            });

        }

        [Loggable]
        public void EliminarComprobante(int IdComprobante)
        {

            this.ComprobanteVentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ComprobanteVenta cc = this.ComprobanteVentaRepository.GetComprobanteVentaCompleto(IdComprobante);

                this.ContabilidadService.TryControlarIngresoNT(cc.Fecha);

                cc.TryAnular();

                if (cc.Tipo.Id == ComprobanteVentaHelper.NotaCredito)
                {
                    // Nota de credito pre impresa.
                    IList<ImputacionVenta> cds = this.ImputacionRepository.GetAllByComprobante(IdComprobante);
                    if (cds.Count > 0)
                        throw new BusinessException("La Nota de Credito contiene imputaciones hechas en Comprobantes asociados, debe eliminar las imputaciones antes de eliminar la Nota de Credito.");

                }

                int asientoId = cc.Asiento.Id;
                cc.Asiento = null;
                this.ComprobanteVentaRepository.Delete(cc);
                this.ContabilidadService.DeleteAsientoNT(asientoId);
            });

        }
        public void UpdateObservacion(int IdComprobante, string Observacion) 
        {
            this.ComprobanteVentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ComprobanteVenta cv = this.ComprobanteVentaRepository.Get(IdComprobante);
                cv.Observacion = Observacion;
                this.ComprobanteVentaRepository.Update(cv);
            });
        }

        public void UpdateEnvioEmail(int IdComprobante)
        {
            this.ComprobanteVentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ComprobanteVenta cv = this.ComprobanteVentaRepository.Get(IdComprobante);
                cv.EnviadoMail = DateTime.Now;
                this.ComprobanteVentaRepository.Update(cv);
            });
        }
        public IList<ComprobanteVenta> GetAllByClienteFilterNC(int? IdCliente, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Cobrada)
        {
            IList<ComprobanteVenta> Comprobantes = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                int _tipo = Tipo.HasValue ? Tipo.Value : 0;
                int _notipo = NoTipo.HasValue ? NoTipo.Value : 0;
                int _idCliente = IdCliente.HasValue ? IdCliente.Value : 0;
                Comprobantes = this.ComprobanteVentaRepository.GetAllByClienteFilterNC(_idCliente, _tipo, _notipo, Cobrada);
            });
            return Comprobantes;

        }

        #endregion

        #region Cobranza
        public Cobranza GetCobranza(int Id)
        {
            Cobranza Cobranza = null;
            this.CobranzaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Cobranza = this.CobranzaRepository.Get(Id);
            });
            return Cobranza;
        }
        public Cobranza GetCobranzaCompleto(int Id)
        {
            Cobranza Cobranza = null;
            this.CobranzaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Cobranza = this.CobranzaRepository.GetCompleto(Id);
            });
            return Cobranza;
        }
        public IList<Cobranza> GetAllCobranzas()
        {
            IList<Cobranza> Cobranzas = null;
            this.CobranzaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Cobranzas = this.CobranzaRepository.GetAll();
            });
            return Cobranzas;
        }

        [Loggable]
        public void AddCobranza(Cobranza Cobranza)
        {
            this.CobranzaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                // Comprobar fecha de Cierre
                this.ContabilidadService.TryControlarIngresoNT(Cobranza.Fecha);

                decimal TotalRecalculado = 0;
                ComprobanteVenta comprobanteAsociado = null;

                foreach (var comprobante in Cobranza.Comprobantes)
                {
                    comprobanteAsociado = this.ComprobanteVentaRepository.Get(comprobante.ComprobanteVenta.Id);
                    //chequeo que lo que se vaya a cobrar sea menor o igual a lo que faltaba
                    if ((comprobanteAsociado.Total - comprobanteAsociado.TotalCobrado) < comprobante.Importe)
                        throw new BusinessException("El importe a abonar del comprobante es mayor a lo que falta abonar");
                    //Importes correctos entonces recalculo total
                    TotalRecalculado += comprobante.Importe;
                    if (comprobante.Importe <= 0)
                        throw new BusinessException("Todos los importes de los comprobantes a cobrar deben ser mayor a cero");

                    comprobanteAsociado.TotalCobrado += comprobante.Importe;
                    if (comprobanteAsociado.TotalCobrado == comprobanteAsociado.Total)
                        comprobanteAsociado.Estado = EstadoComprobante.Cobrada;

                    this.ComprobanteVentaRepository.Update(comprobanteAsociado);
                }

                Cobranza.FechaCreacion = DateTime.Now;
                Cobranza.Estado = EstadoComprobanteCancelacion.Cobrada;
                Cobranza.NumeroReferencia = this.CobranzaRepository.GetProximoIdCobranza();
                Cobranza.Total = TotalRecalculado;
                string talonario = Cobranza.Numero.Split('-')[0];
                Cobranza.Numero = this.CobranzaRepository.GetProximoRecibo(talonario);
                IList<ValorIngresado> valores = Cobranza.Valores.Select(x => x.Valor).ToList();
                foreach (var Valor in valores)
                {
                    if (Valor.Importe <= 0)
                        throw new BusinessException("Todos los importes de los medios de pagos deben ser mayor a cero");

                    Valor.TipoIngreso = TipoIngreso.Ingreso;
                    Valor.Organizacion = Security.GetOrganizacion();
                    this.TesoreriaService.GetCuentaContableForValor(Valor,TipoIngreso.Ingreso);
                }
                Cobranza.Tipo = this.comboItemRepository.Get(Cobranza.Tipo.Id);
                Cobranza.Cliente = this.ClienteRepository.Get(Cobranza.Cliente.Id);
                Cobranza.Asiento = this.ContabilidadService.NuevoAsientoCobranza(Cobranza);
                string concepto = "Cobranza Nro.:" + Cobranza.Numero + " Ref(" + Cobranza.NumeroReferencia + ") a " + Cobranza.Cliente.RazonSocial;
                Cobranza.Usuario = Security.GetCurrentUser();
                this.CobranzaRepository.Add(Cobranza);
                this.TesoreriaService.RegistrarIngresosNT(valores, TipoIngreso.Ingreso, concepto, Cobranza.Numero);
                Cobranza.Asiento.ComprobanteAsociado = Cobranza.Id;
                this.ContabilidadService.UpdateAsientoNT(Cobranza.Asiento);
            });
        }
        public string GetProximoRecibo(string talonario)
        {
            string ProximoRecibo = "";
            this.CobranzaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ProximoRecibo = this.CobranzaRepository.GetProximoRecibo(talonario);
            });
            return ProximoRecibo;
        }
        public int GetProximoIdCobranza()
        {
            int NextId = 1;
            this.CobranzaRepository.GetSessionFactory().TransactionalInterceptor(() =>
                {
                    NextId = this.CobranzaRepository.GetProximoIdCobranza();
                });
            return NextId;
        }
        public IList<Cobranza> GetAllCobranzaByCliente(int IdCliente, DateTime? start, DateTime? end)
        {
            IList<Cobranza> cobranzas = null;
            this.CobranzaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                cobranzas = this.CobranzaRepository.GetAllByCliente(IdCliente, _start, _end);
            });
            return cobranzas;
        }

        [Loggable]
        public void AnularCobranza(int IdCobranza)
        {
            this.CobranzaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Cobranza c = this.CobranzaRepository.Get(IdCobranza);
                if (c.Estado == EstadoComprobanteCancelacion.Anulada)
                    throw new BusinessException("La orden de Pago ya está cancelada");

                // Comprobar fecha de Cierre
                this.ContabilidadService.TryControlarIngresoNT(c.Fecha);

                ComprobanteVenta comprobanteAsociado = null;
                foreach (var comprobante in c.Comprobantes)
                {
                    comprobanteAsociado = this.ComprobanteVentaRepository.Get(comprobante.ComprobanteVenta.Id);
                    //Importes correctos entonces recalculo total
                    comprobanteAsociado.TotalCobrado -= comprobante.Importe;
                    if (comprobanteAsociado.TotalCobrado < comprobanteAsociado.Total)
                        comprobanteAsociado.Estado = EstadoComprobante.Creada;

                    this.ComprobanteVentaRepository.Update(comprobanteAsociado);
                }
                IList<ValorIngresado> valores = c.Valores.Select(x => x.Valor).ToList();
                this.ContabilidadService.DeleteAsientoNT(c.Asiento.Id);
                c.Estado = EstadoComprobanteCancelacion.Anulada;
                c.Asiento = null;
                this.CobranzaRepository.Update(c);
                string Concepto = "Cobranza Cancelada - N°" + c.Id;
                this.TesoreriaService.CancelarIngresosNT(valores, TipoIngreso.Ingreso, Concepto);
            });
        }

        #endregion

        #region CuentaCorriente
        public VentasCuentaCorriente LoadCtaCorrienteHead(int id, DateTime? start, DateTime? end, Model.Common.CuentaCorrienteFilter filter)
        {
            VentasCuentaCorriente ret = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ret = this.ComprobanteVentaRepository.LoadCtaCorrienteHead(id, _start, _end, filter);
            });
            return ret;
        }

        public List<CuentaCorrienteItem> GetCuentaCorrienteByDates(DateTime? start, DateTime? end, int id, Model.Common.CuentaCorrienteFilter filter)
        {
            List<CuentaCorrienteItem> ret = null;
            this.ComprobanteVentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ret = this.ComprobanteVentaRepository.GetCuentaCorrienteByDates(id, _start, _end, filter);
            });
            return ret;
        }
        #endregion

        #region Imputaciones
        public void AddImputacion(ImputacionVenta Imputacion)
        {
            this.ImputacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddImputacionNT(Imputacion);
            });
        }

        [Loggable]
        public void AddImputacionNT(ImputacionVenta Imputacion)
        {
            Imputacion.Usuario = Security.GetCurrentUser();
            Imputacion.Fecha = DateTime.Now;
            this.ImputacionRepository.Add(Imputacion);
        }

        [Loggable]
        public void AddImputaciones(IList<ImputacionVenta> Imputaciones)
        {
            this.ImputacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ComprobanteVenta NC = this.ComprobanteVentaRepository.Get(Imputaciones.First().NotaCredito.Id);

                // Validacion de Montos.
                NC.TotalCobrado += Imputaciones.Sum(x => x.Importe);
                if (NC.Total < NC.TotalCobrado)
                    throw new BusinessException("El total a cobrar es mayor al Importe de la Nota de Credito");

                if (NC.TotalCobrado == NC.Total)
                    NC.Estado = EstadoComprobante.Imputado;

                this.ComprobanteVentaRepository.Update(NC);


                foreach (ImputacionVenta Imputacion in Imputaciones)
                {
                    this.AddImputacionNT(Imputacion);

                    ComprobanteVenta CD = this.ComprobanteVentaRepository.Get(Imputacion.ComprobanteADescontar.Id);

                    CD.TotalCobrado += Imputacion.Importe;
                    if (CD.Total < CD.TotalCobrado)
                        throw new BusinessException("El total a cobrar es mayor al Importe del Comprobante" + CD.Numero);
                    if (CD.TotalCobrado == CD.Total)
                        CD.Estado = EstadoComprobante.Pagada;

                    this.ComprobanteVentaRepository.Update(CD);
                }
            });
        }
        public ImputacionVenta GetImputacion(int Id)
        {
            ImputacionVenta Imputacion = null;
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Imputacion = this.ImputacionRepository.Get(Id);
            });
            return Imputacion;
        }
        public ImputacionVenta GetImputacionCompleto(int Id)
        {
            ImputacionVenta Imputacion = null;
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Imputacion = this.ImputacionRepository.GetCompleto(Id);
            });
            return Imputacion;
        }
        public IList<ImputacionVenta> GetAllImputaciones()
        {
            IList<ImputacionVenta> Imputaciones = null;
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Imputaciones = this.ImputacionRepository.GetAll();
            });
            return Imputaciones;
        }
        public void UpdateImputacion(ImputacionVenta Imputacion)
        {
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                this.ImputacionRepository.Update(Imputacion);
            });
        }
        public IList<ImputacionVenta> GetAllImputacionesByCliente(int IdCliente, DateTime? start, DateTime? end)
        {
            IList<ImputacionVenta> Imputaciones = null;
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Imputaciones = this.ImputacionRepository.GetAllByCliente(IdCliente, _start, _end);
                foreach (ImputacionVenta Imputacion in Imputaciones)
                {
                    Imputacion.ComprobanteADescontar.Detalle = null;
                    Imputacion.ComprobanteADescontar.Asiento = null;
                    Imputacion.ComprobanteADescontar.Observaciones = null;
                    Imputacion.NotaCredito.Detalle = null;
                    Imputacion.NotaCredito.Asiento = null;
                    Imputacion.NotaCredito.Observaciones = null;
                }
            });
            return Imputaciones;
        }

        [Loggable]
        public void AnularImputacion(int IdImputacion)
        {
            this.ImputacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ImputacionVenta iv = this.ImputacionRepository.Get(IdImputacion);

                ComprobanteVenta NC = this.ComprobanteVentaRepository.Get(iv.NotaCredito.Id);

                NC.TotalCobrado -= iv.Importe;
                NC.Estado = EstadoComprobante.Creada; 

                this.ComprobanteVentaRepository.Update(NC);

                ComprobanteVenta cd = this.ComprobanteVentaRepository.Get(iv.ComprobanteADescontar.Id);

                cd.TotalCobrado -= iv.Importe;
                cd.Estado = EstadoComprobante.Creada; 

                this.ComprobanteVentaRepository.Update(cd);
                
                this.DeleteImputacionNT(iv.Id);
            });
        }

        public void DeleteImputacion(int Id)
        {
            this.ImputacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.DeleteImputacionNT(Id);
            });
        }

        public void DeleteImputacionNT(int Id)
        {
            ImputacionVenta iv = this.ImputacionRepository.Get(Id);
            this.ImputacionRepository.Delete(iv);
        }


        #endregion

    }
}
