using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Compras;
using Woopin.SGC.Repositories.Contabilidad;
using System.Security;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Common.App.Logging;

namespace Woopin.SGC.Services
{
    public class ComprasService : IComprasService
    {
        #region VariablesyConstructor

        private readonly IProveedorRepository ProveedorRepository;
        private readonly IComprobanteCompraRepository ComprobanteCompraRepository;
        private readonly ICuentaRepository CuentaRepository;
        private readonly IContabilidadService ContabilidadService;
        private readonly IComboItemRepository comboItemRepository;
        private readonly IOrdenPagoRepository OrdenPagoRepository;
        private readonly ITesoreriaService TesoreriaService;
        private readonly IOtroEgresoRepository OtroEgresoRepository;
        private readonly IUsuarioRepository UsuarioRepository;
        private readonly IImputacionCompraRepository ImputacionRepository;
        private readonly IRubroCompraRepository rubroCompraRepository;
        public ComprasService(IProveedorRepository ProveedorRepository, IRubroCompraRepository RubroCompraRepository, IComprobanteCompraRepository ComprobanteCompraRepository,
            ICuentaRepository CuentaRepository, IContabilidadService ContabilidadService, IComboItemRepository comboItemRepository, IOrdenPagoRepository OrdenPagoRepository,IRubroCompraRepository rubroCompraRepository,
            ITesoreriaService TesoreriaService, IOtroEgresoRepository OtroEgresoRepository, IUsuarioRepository UsuarioRepository, IImputacionCompraRepository ImputacionRepository)
        {
            this.ProveedorRepository = ProveedorRepository;
            this.ComprobanteCompraRepository = ComprobanteCompraRepository;
            this.CuentaRepository = CuentaRepository;
            this.ContabilidadService = ContabilidadService;
            this.comboItemRepository = comboItemRepository;
            this.OrdenPagoRepository = OrdenPagoRepository;
            this.TesoreriaService = TesoreriaService;
            this.OtroEgresoRepository = OtroEgresoRepository;
            this.UsuarioRepository = UsuarioRepository;
            this.ImputacionRepository = ImputacionRepository;
            this.rubroCompraRepository = rubroCompraRepository;
        }

        #endregion

        #region ComprobanteCompra
        public ComprobanteCompra GetComprobanteCompra(int Id)
        {
            ComprobanteCompra Comprobante = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobante = this.ComprobanteCompraRepository.Get(Id);
            });
            return Comprobante;
        }

        public ComprobanteCompra GetComprobanteCompraCompleto(int Id)
        {
            ComprobanteCompra Comprobante = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobante = this.ComprobanteCompraRepository.GetCompleto(Id);
            });
            return Comprobante;
        }

        public IList<CuentaCorrienteItem> GetAllCCAcumulados(int IdProveedor, int IdRubro, DateTime? start, DateTime? end, Model.Common.CuentaCorrienteFilter filter)
        {
            IList<CuentaCorrienteItem> Comprobantes = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Comprobantes = this.ComprobanteCompraRepository.GetAllAcumulados(IdProveedor, IdRubro, _start, _end, filter);
            });
            return Comprobantes;
        }


        public IList<ComprobanteCompra> GetAllComprobantesCompras()
        {
            IList<ComprobanteCompra> Comprobantes = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobantes = this.ComprobanteCompraRepository.GetAll();
            });
            return Comprobantes;
        }

        public IList<ComprobanteCompra> GetAllComprobantesCompraByProveedor(int IdProveedor, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, Model.Common.CuentaCorrienteFilter filter)
        {
            IList<ComprobanteCompra> Comprobantes = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                DateTime _startvenc = startvenc.HasValue ? startvenc.Value : DateTime.Parse("1970-01-01");
                DateTime _endvenc = endvenc.HasValue ? endvenc.Value : DateTime.Parse("9998-12-31");
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Comprobantes = this.ComprobanteCompraRepository.GetAllByProveedor(IdProveedor, _start, _end, _startvenc, _endvenc, filter);
            });
            return Comprobantes;
        }
        public IList<ComprobanteCompra> GetAllComprobanteCompraAPagarByProv(int IdProveedor)
        {
            IList<ComprobanteCompra> Comprobantes = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Comprobantes = this.ComprobanteCompraRepository.GetAllAPagarByProv(IdProveedor);
            });
            return Comprobantes;
        }

        [Loggable]
        public void AddComprobanteCompra(ComprobanteCompra Comprobante)
        {
            this.ComprobanteCompraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ImputacionCompra imputacion = null;
                Comprobante.Estado = EstadoComprobante.Creada;

                // Verifico que ese numero no haya sido tomado.
                // en compra la verificacion de numero es asi, ya que no se puede determinar cual es el prox que uso el proveedor
                // solo podemos decir si nosotros lo usamos o no
                if ((this.ComprobanteCompraRepository.GetComprobanteByInfo(Comprobante.Proveedor.Id, Comprobante.Letra, Comprobante.Numero, Comprobante.Tipo.Id) != null))
                    throw new BusinessException("Ya fue utilizado ese numero de comprobante de ese proveedor"); 

                // Comprobar fecha de Cierre
                this.ContabilidadService.TryControlarIngresoNT(Comprobante.FechaContable);

                // Trae la informacion adicional de los objetos relacionados
                Comprobante.CondicionCompra = this.comboItemRepository.Get(Comprobante.CondicionCompra.Id);
                Comprobante.Tipo = this.comboItemRepository.Get(Comprobante.Tipo.Id);
                Comprobante.Proveedor = this.ProveedorRepository.Get(Comprobante.Proveedor.Id);
                Comprobante = ComprobanteCompraHelper.ReMap(Comprobante);
                
                // Nota de credito con imputación
                if (Comprobante.Tipo.Id == ComprobanteCompraHelper.NotaCredito && Comprobante.Imputacion != null && Comprobante.Imputacion.Count > 0 && Comprobante.Imputacion.First().ComprobanteADescontar.Numero != null)
                {
                    ComprobanteCompra c = this.ComprobanteCompraRepository.Get(Comprobante.Imputacion.First().ComprobanteADescontar.Id);
                    imputacion = new ImputacionCompra();
                    if (c == null)
                        throw new BusinessException("La Factura a imputar la Nota de Credito no existe, verificar y volver a intentar");
                    if (c.Proveedor.Id != Comprobante.Proveedor.Id)
                        throw new BusinessException("Esta intentando hacer una Nota de Credito sobre una factura de un Proveedor distinto!");                 
                    if (Comprobante.Total > (c.Total - c.TotalPagado))
                        throw new BusinessException("Cuando Imputa desde la creación del comprobante el importe de la nota de credito debe ser menor o igual a la Factura");
                    
                    c.TotalPagado += Comprobante.Total;
                    if (c.Total == c.TotalPagado)
                    {
                        c.Estado = EstadoComprobante.Pagada;
                    }
                    this.ComprobanteCompraRepository.Update(c);

                    Comprobante.TotalPagado = Comprobante.Total; // No genera deuda pendiente y se descarto anteriormente que sea mayor el importe de la NC que de la factura
                    Comprobante.Estado = EstadoComprobante.Imputado;
                    
                    imputacion.ComprobanteADescontar = new ComprobanteCompra();
                    imputacion.ComprobanteADescontar.Id = c.Id;
                }              
                
                Comprobante.Asiento = this.ContabilidadService.NuevoAsientoCompraNT(Comprobante);
                Comprobante.Usuario = Security.GetCurrentUser();
                Comprobante.Imputacion = null;
                
                this.ComprobanteCompraRepository.Add(Comprobante);

                // Si imputo, guardo la imputacion.
                if (Comprobante.Tipo.Id == ComprobanteCompraHelper.NotaCredito && imputacion != null)
                {
                    imputacion.Importe = Comprobante.Total;
                    imputacion.NotaCredito = new ComprobanteCompra();
                    imputacion.NotaCredito.Id = Comprobante.Id;
                    this.AddImputacionNT(imputacion);
                }

                Comprobante.Asiento.ComprobanteAsociado = Comprobante.Id;
                this.ContabilidadService.UpdateAsientoNT(Comprobante.Asiento);
            });
        }
        public int GetProximoNumeroReferencia()
        {
            int ProximoNumeroReferencia = 1;
            this.ComprobanteCompraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ProximoNumeroReferencia = this.ComprobanteCompraRepository.GetProximoNumeroReferencia();
            });
            return ProximoNumeroReferencia;
        }

        public ComprasCuentaCorriente LoadCtaCorrienteHead(int id, DateTime? start, DateTime? end, Model.Common.CuentaCorrienteFilter filter)
        {
            ComprasCuentaCorriente ret = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ret = this.ComprobanteCompraRepository.LoadCtaCorrienteHead(id, _start, _end,filter);
            });
            return ret;
        }
        public ComprasCuentaCorriente AcumuladosHead(int id, int idRubro, DateTime? start, DateTime? end, Model.Common.CuentaCorrienteFilter filter)
        {
            ComprasCuentaCorriente ret = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ret = this.ComprobanteCompraRepository.AcumuladosHead(id, idRubro, _start, _end, filter);
            });
            return ret;
        }
        public IList<CuentaCorrienteItem> GetCuentaCorrienteByDates(DateTime? start, DateTime? end, int id, Model.Common.CuentaCorrienteFilter filter)
        {
            IList<CuentaCorrienteItem> ret = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                ret = this.ComprobanteCompraRepository.GetCuentaCorrienteByDates(id, _start, _end, filter);
            });
            return ret;
        }

        public void UpdateComprobanteCompra(ComprobanteCompra Comprobante)
        {
            this.ComprobanteCompraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.UpdateComprobanteCompraNT(Comprobante);
            });
        }

        public void UpdateComprobanteCompraNT(ComprobanteCompra Comprobante)
        {
            this.ComprobanteCompraRepository.Update(Comprobante);
        }

        [Loggable]
        public void AnularComprobante(int IdComprobante)
        {
            ComprobanteCompra cc = this.GetComprobanteCompra(IdComprobante);
            this.ComprobanteCompraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                //Controlar fecha contable
                this.ContabilidadService.TryControlarIngresoNT(cc.FechaContable);

                cc.TryAnular();

                if (cc.Tipo.Id == ComprobanteCompraHelper.NotaCredito)
                {
                    // Nota de credito pre impresa.
                    IList<ImputacionCompra> cds = this.ImputacionRepository.GetAllByComprobante(IdComprobante);
                    if (cds.Count > 0)
                        throw new BusinessException("La Nota de Credito contiene imputaciones hechas en Comprobantes asociados, debe eliminar las imputaciones antes de eliminar la Nota de Credito.");

                }

                cc.Estado = EstadoComprobante.Anulada;
                int asientoId = cc.Asiento.Id;
                cc.Asiento = null;
                this.UpdateComprobanteCompraNT(cc);
                this.ContabilidadService.DeleteAsientoNT(asientoId);
            });
            
        }

        [Loggable]
        public void EliminarComprobante(int IdComprobante)
        {
            ComprobanteCompra cc = this.GetComprobanteCompra(IdComprobante);
            this.ComprobanteCompraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                //Controlar fecha contable
                this.ContabilidadService.TryControlarIngresoNT(cc.FechaContable);

                cc.TryAnular();

                if (cc.Tipo.Id == ComprobanteCompraHelper.NotaCredito)
                {
                    // Nota de credito pre impresa.
                    IList<ImputacionCompra> cds = this.ImputacionRepository.GetAllByComprobante(IdComprobante);
                    if (cds.Count > 0)
                        throw new BusinessException("La Nota de Credito contiene imputaciones hechas en Comprobantes asociados, debe eliminar las imputaciones antes de eliminar la Nota de Credito.");

                }

                int asientoId = cc.Asiento.Id;
                cc.Asiento = null;
                this.ComprobanteCompraRepository.Delete(cc);
                this.ContabilidadService.DeleteAsientoNT(asientoId);
            });

        }

        public IList<ComprobanteCompra> GetAllByProvFilterNC(int? IdProveedor, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Pagada)
        {
            IList<ComprobanteCompra> Comprobantes = null;
            this.ComprobanteCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                int _tipo = Tipo.HasValue ? Tipo.Value :0;
                int _notipo = NoTipo.HasValue ? NoTipo.Value : 0;
                int _idProveedor = IdProveedor.HasValue ? IdProveedor.Value : 0;
                Comprobantes = this.ComprobanteCompraRepository.GetAllByProvFilterNC(_idProveedor, _tipo, _notipo, Pagada);
            });
            return Comprobantes;

        }

        #endregion

        #region Orden de Pago
        public OrdenPago GetOrdenPago(int Id)
        {
            OrdenPago OrdenPago = null;
            this.OrdenPagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                OrdenPago = this.OrdenPagoRepository.Get(Id);
            });
            return OrdenPago;
        }
        public OrdenPago GetOrdenPagoCompleto(int Id)
        {
            OrdenPago OrdenPago = null;
            this.OrdenPagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                OrdenPago = this.OrdenPagoRepository.GetCompleto(Id);
            });
            return OrdenPago;
        }
        public IList<OrdenPago> GetAllOrdenPagos()
        {
            IList<OrdenPago> OrdenPagos = null;
            this.OrdenPagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                OrdenPagos = this.OrdenPagoRepository.GetAll();
            });
            return OrdenPagos;
        }
        public IList<OrdenPago> GetAllOrdenPagoByProveedor(int IdProveedor, DateTime? start, DateTime? end)
        {
            IList<OrdenPago> OrdenPagos = null;
            this.OrdenPagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                OrdenPagos = this.OrdenPagoRepository.GetAllByProveedor(IdProveedor, _start, _end);
            });
            return OrdenPagos;
        }
        public IList<OrdenPago> GetAllOrdenPagoByDates(DateTime? start, DateTime? end)
        {
            IList<OrdenPago> Comprobantes = null;
            this.OrdenPagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Comprobantes = this.OrdenPagoRepository.GetAllByProveedor(0,_start, _end);
            });
            return Comprobantes;
        }

        [Loggable]
        public void AddOrdenPago(OrdenPago OrdenPago)
        {
            this.OrdenPagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                // Comprobar fecha de Cierre
                this.ContabilidadService.TryControlarIngresoNT(OrdenPago.Fecha);

                decimal TotalRecalculado = 0;
                ComprobanteCompra comprobanteAsociado = null;
                foreach (var comprobante in OrdenPago.Comprobantes)
                {
                    comprobanteAsociado = this.ComprobanteCompraRepository.Get(comprobante.ComprobanteCompra.Id);
                    //chequeo que lo que se vaya a cobrar sea menor o igual a lo que faltaba
                    if ((comprobanteAsociado.Total - comprobanteAsociado.TotalPagado) < comprobante.Importe)
                        throw new BusinessException("El importe a abonar del comprobante es mayor a lo que falta abonar");

                    //Importes correctos entonces recalculo total
                    TotalRecalculado += comprobante.Importe;
                    if (comprobante.Importe <= 0)
                        throw new BusinessException("Todos los importes de los comprobantes a pagar deben ser mayor a cero");

                    comprobanteAsociado.TotalPagado += comprobante.Importe;
                    if (comprobanteAsociado.TotalPagado == comprobanteAsociado.Total)
                        comprobanteAsociado.Estado = EstadoComprobante.Pagada;

                    this.ComprobanteCompraRepository.Update(comprobanteAsociado);
                }

                OrdenPago.Total = TotalRecalculado;
                IList<ValorIngresado> valores = OrdenPago.Pagos.Select(x => x.Valor).ToList();
                foreach(var Valor in valores)
                {
                    if (Valor.Importe <= 0)
                        throw new BusinessException("Todos los importes de los medios de pagos deben ser mayor a cero");

                    Valor.TipoIngreso = TipoIngreso.Egreso;
                    this.TesoreriaService.GetCuentaContableForValor(Valor,TipoIngreso.Egreso);
                    Valor.Organizacion = Security.GetOrganizacion();
                }
                OrdenPago.Tipo = this.comboItemRepository.Get(OrdenPago.Tipo.Id);
                OrdenPago.Proveedor = this.ProveedorRepository.Get(OrdenPago.Proveedor.Id);
                OrdenPago.Asiento = this.ContabilidadService.NuevoAsientoOP(OrdenPago);
                OrdenPago.Usuario = Security.GetCurrentUser();
                this.OrdenPagoRepository.Add(OrdenPago);
                this.TesoreriaService.RegistrarIngresosNT(valores, TipoIngreso.Egreso, "Orden de Pago Nro.:" + OrdenPago.Id + " a " + OrdenPago.Proveedor.RazonSocial, OrdenPago.Id.ToString());
                OrdenPago.Numero = OrdenPago.Id.ToString();
                OrdenPago.Asiento.ComprobanteAsociado = OrdenPago.Id;
                this.ContabilidadService.UpdateAsientoNT(OrdenPago.Asiento);
            });
        }

        [Loggable]
        public void CancelarOrdenPago(int IdOrdenPago)
        {
            this.OrdenPagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                OrdenPago op = this.OrdenPagoRepository.Get(IdOrdenPago);
                if(op.Estado == EstadoComprobanteCancelacion.Anulada)
                    throw new BusinessException("La orden de Pago ya está cancelada");

                
                // Comprobar fecha de Cierre
                this.ContabilidadService.TryControlarIngresoNT(op.Fecha);

                ComprobanteCompra comprobanteAsociado = null;
                foreach (var comprobante in op.Comprobantes)
                {
                    comprobanteAsociado = this.ComprobanteCompraRepository.Get(comprobante.ComprobanteCompra.Id);
                    //Importes correctos entonces recalculo total
                    comprobanteAsociado.TotalPagado -= comprobante.Importe;
                    if (comprobanteAsociado.TotalPagado < comprobanteAsociado.Total)
                        comprobanteAsociado.Estado = EstadoComprobante.Creada;

                    this.ComprobanteCompraRepository.Update(comprobanteAsociado);
                }
                IList<ValorIngresado> valores = op.Pagos.Select(x => x.Valor).ToList();
                this.ContabilidadService.DeleteAsientoNT(op.Asiento.Id);
                op.Estado = EstadoComprobanteCancelacion.Anulada;
                op.Asiento = null;
                this.OrdenPagoRepository.Update(op);
                string Concepto = "Orden de Pago Cancelada - N°" + op.Id;
                this.TesoreriaService.CancelarIngresosNT(valores, TipoIngreso.Egreso, Concepto);
            });
        }
        public string GetProximoOrdenPago()
        {
            string ProximoNumeroComprobante = "";
            this.OrdenPagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ProximoNumeroComprobante = this.OrdenPagoRepository.GetProximoComprobante();
            });
            return ProximoNumeroComprobante;
        }

        public int GetOrdenPagoProximoNumeroReferencia()
        {
            int ProximoNumeroReferencia = 1;
            this.OrdenPagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ProximoNumeroReferencia = this.OrdenPagoRepository.GetProximoNumeroReferencia();
            });
            return ProximoNumeroReferencia;
        }


        #endregion

        #region Otro Egreso
        public OtroEgreso GetOtroEgreso(int Id)
        {
            OtroEgreso OtroEgreso = null;
            this.OtroEgresoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                OtroEgreso = this.OtroEgresoRepository.Get(Id);
            });
            return OtroEgreso;
        }
        public OtroEgreso GetOtroEgresoCompleto(int Id)
        {
            OtroEgreso OtroEgreso = null;
            this.OtroEgresoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                OtroEgreso = this.OtroEgresoRepository.GetCompleto(Id);
            });
            return OtroEgreso;
        }
        public IList<OtroEgreso> GetAllOtroEgresos()
        {
            IList<OtroEgreso> OtroEgresos = null;
            this.OtroEgresoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                OtroEgresos = this.OtroEgresoRepository.GetAll();
            });
            return OtroEgresos;
        }
        public IList<OtroEgreso> GetAllOtroEgresoByProveedor(int IdProveedor, DateTime? start, DateTime? end)
        {
            IList<OtroEgreso> OtroEgresos = null;
            this.OtroEgresoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                OtroEgresos = this.OtroEgresoRepository.GetAllByProveedor(IdProveedor, _start, _end);
                 
            });
            return OtroEgresos;
        }

        [Loggable]
        public void AddOtroEgreso(OtroEgreso OtroEgreso)
        {
            this.OtroEgresoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                // Comprobar fecha de Cierre
                this.ContabilidadService.TryControlarIngresoNT(OtroEgreso.FechaContable);

                decimal TotalRecalculado = 0;
                foreach(var Item in OtroEgreso.Detalle)
                {
                    TotalRecalculado += Item.Total;
                    if (Item.Total <= 0)
                        throw new BusinessException("Todos los importes de los comprobantes a pagar deben ser mayor a cero");

                }
                OtroEgreso.FechaCreacion = DateTime.Now;
                OtroEgreso.Total = TotalRecalculado;
                OtroEgreso.Estado = EstadoComprobante.Creada;
                
                IList<ValorIngresado> valores = OtroEgreso.Pagos.Select(x => x.Valor).ToList();
                foreach (var Valor in valores)
                {
                    if(Valor.Importe <= 0)
                        throw new BusinessException("Todos los importes de los medios de pagos deben ser mayor a cero");

                    Valor.TipoIngreso = TipoIngreso.Egreso;
                    Valor.Organizacion = Security.GetOrganizacion();
                    this.TesoreriaService.GetCuentaContableForValor(Valor,TipoIngreso.Egreso);
                }
                OtroEgreso.Proveedor = this.ProveedorRepository.Get(OtroEgreso.Proveedor.Id);
                OtroEgreso.Asiento = this.ContabilidadService.NuevoAsientoOE(OtroEgreso);
                OtroEgreso.Usuario = Security.GetCurrentUser();
                this.OtroEgresoRepository.Add(OtroEgreso);
                this.TesoreriaService.RegistrarIngresosNT(valores, TipoIngreso.Egreso, "Otro Egreso Nro.:" + OtroEgreso.Id + " a " + OtroEgreso.Proveedor.RazonSocial, OtroEgreso.Id.ToString());
                OtroEgreso.NumeroReferencia = OtroEgreso.Id;
                OtroEgreso.Asiento.ComprobanteAsociado = OtroEgreso.Id;
                this.ContabilidadService.UpdateAsientoNT(OtroEgreso.Asiento);
            });
        }
        public int GetOtroEgresoProximoNumeroReferencia()
        {
            int ProximoNumeroReferencia = 1;
            this.OtroEgresoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ProximoNumeroReferencia = this.OtroEgresoRepository.GetProximoNumeroReferencia();
            });
            return ProximoNumeroReferencia;
        }

        [Loggable]
        public void AnularOE(int IdOE)
        {
            this.OtroEgresoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                OtroEgreso oe = this.OtroEgresoRepository.Get(IdOE);
                if (oe.Estado == EstadoComprobante.Anulada)
                    throw new BusinessException("La orden de Pago ya está anulada");

                //TODO ver si hay que revisar los pagos o algo asi de si estan o no pagados

                // Comprobar fecha de Cierre
                this.ContabilidadService.TryControlarIngresoNT(oe.Fecha);

                IList<ValorIngresado> valores = oe.Pagos.Select(x => x.Valor).ToList();
                this.ContabilidadService.DeleteAsientoNT(oe.Asiento.Id);
                oe.Estado = EstadoComprobante.Anulada;
                oe.Asiento = null;
                this.OtroEgresoRepository.Update(oe);
                string Concepto = "Otro Egreso Cancelado - N°" + oe.Id;
                this.TesoreriaService.CancelarIngresosNT(valores, TipoIngreso.Egreso, Concepto);
            });
        }


        #endregion

        #region Imputaciones

        [Loggable]
        public void AddImputacion(ImputacionCompra Imputacion)
        {
            this.ImputacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddImputacionNT(Imputacion);
            });
        }

        [Loggable]
        public void AddImputacionNT(ImputacionCompra Imputacion)
        {
            Imputacion.Usuario = Security.GetCurrentUser();
            Imputacion.Fecha = DateTime.Now;
            this.ImputacionRepository.Add(Imputacion);
        }

        [Loggable]
        public void AddImputaciones(IList<ImputacionCompra> Imputaciones)
        {
            this.ImputacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ComprobanteCompra NC = this.ComprobanteCompraRepository.Get(Imputaciones.First().NotaCredito.Id);

                NC.TotalPagado += Imputaciones.Sum(x => x.Importe);
                if (NC.Total < NC.TotalPagado)
                    throw new BusinessException("El total a pagar es mayor al Importe de la Nota de Credito");

                if (NC.TotalPagado == NC.Total)
                    NC.Estado = EstadoComprobante.Imputado;

                this.ComprobanteCompraRepository.Update(NC);

                
                foreach (ImputacionCompra Imputacion in Imputaciones)
                {
                    this.AddImputacionNT(Imputacion);

                    ComprobanteCompra CD = this.ComprobanteCompraRepository.Get(Imputacion.ComprobanteADescontar.Id);
                    CD.TotalPagado += Imputacion.Importe;
                    if (CD.Total < CD.TotalPagado)
                        throw new BusinessException("El total a pagar es mayor al Importe del Comprobante" + CD.Numero);
                    if (CD.TotalPagado == CD.Total)
                        CD.Estado = EstadoComprobante.Pagada;

                    this.ComprobanteCompraRepository.Update(CD);
                }
            });
        }
        
        public ImputacionCompra GetImputacion(int Id)
        {
            ImputacionCompra Imputacion = null;
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Imputacion = this.ImputacionRepository.Get(Id);
            });
            return Imputacion;
        }

        public ImputacionCompra GetImputacionCompleto(int Id)
        {
            ImputacionCompra Imputacion = null;
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Imputacion = this.ImputacionRepository.GetCompleto(Id);
            });
            return Imputacion;
        }
        public IList<ImputacionCompra> GetAllImputaciones()
        {
            IList<ImputacionCompra> Imputaciones = null;
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Imputaciones = this.ImputacionRepository.GetAll();
            });
            return Imputaciones;
        }

        [Loggable]
        public void UpdateImputacion(ImputacionCompra Imputacion)
        {
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                this.ImputacionRepository.Update(Imputacion);
            });
        }

        public IList<ImputacionCompra> GetAllImputacionesByProveedor(int IdProveedor, DateTime? start, DateTime? end)
        {
            IList<ImputacionCompra> Imputaciones = null;
            this.ImputacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Imputaciones = this.ImputacionRepository.GetAllByProveedor(IdProveedor, _start, _end);
                foreach (ImputacionCompra Imputacion in Imputaciones)
                {
                    Imputacion.ComprobanteADescontar.Detalle = null;
                    Imputacion.ComprobanteADescontar.Asiento = null;
                    Imputacion.NotaCredito.Detalle = null;
                    Imputacion.NotaCredito.Asiento = null;
                }
            });
            return Imputaciones;
        }

        [Loggable]

        public void AnularImputacion(int IdImputacion)
        {
            this.ImputacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ImputacionCompra ic = this.ImputacionRepository.Get(IdImputacion);

                ComprobanteCompra NC = this.ComprobanteCompraRepository.Get(ic.NotaCredito.Id);

                NC.TotalPagado -= ic.Importe;
                if (DateTime.Compare(NC.FechaVencimiento, DateTime.Now) < 0) //fecha vencimiento ya paso
                {
                    NC.Estado = EstadoComprobante.Vencida;
                }
                else
                {//fecha vencimiento es hoy o falta
                    NC.Estado = EstadoComprobante.Creada;
                }

                this.ComprobanteCompraRepository.Update(NC);

                ComprobanteCompra cd = this.ComprobanteCompraRepository.Get(ic.ComprobanteADescontar.Id);

                cd.TotalPagado -= ic.Importe;
                if (DateTime.Compare(cd.FechaVencimiento, DateTime.Now) < 0)//fecha vencimiento ya paso
                {
                    cd.Estado = EstadoComprobante.Vencida;
                }
                else
                {//fecha vencimiento es hoy o falta
                    cd.Estado = EstadoComprobante.Creada;
                }
                this.ComprobanteCompraRepository.Update(cd);

                this.DeleteImputacionNT(ic.Id);
            });
        }

        [Loggable]
        public void DeleteImputacion(int Id)
        {
            this.ImputacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.DeleteImputacionNT(Id);
            });
        }

        [Loggable]
        public void DeleteImputacionNT(int Id)
        {
            ImputacionCompra ic = this.ImputacionRepository.Get(Id);
            this.ImputacionRepository.Delete(ic);
        }
        
        #endregion


    }
}
