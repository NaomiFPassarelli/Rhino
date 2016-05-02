using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Tesoreria.Controllers
{
    public class ValoresController : BaseController
    {
        private readonly ITesoreriaConfigService TesoreriaConfigService;
        private readonly ITesoreriaService TesoreriaService;
        private readonly ICommonConfigService commonConfigService;
        private readonly IContabilidadConfigService ContabilidadConfigService;

        public ValoresController(ITesoreriaConfigService TesoreriaConfigService, ICommonConfigService commonConfigService, ITesoreriaService TesoreriaService, IContabilidadConfigService ContabilidadConfigService)
        {
            this.TesoreriaConfigService = TesoreriaConfigService;
            this.commonConfigService = commonConfigService;
            this.TesoreriaService = TesoreriaService;
            this.ContabilidadConfigService = ContabilidadConfigService;
        }

        #region ABM de Valores
        // GET: /Configuracion/Valores
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nueva()
        {
            ViewBag.Monedas = this.commonConfigService.GetAllMonedas().Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Nombre, Selected = m.Predeterminado }).ToList();
            ViewBag.TipoValores = this.commonConfigService.GetItemsByCombo(ComboType.TipoValor).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nueva(Valor Valor)
        {
            try
            {
                ClearNotValidatedProperties(Valor);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.AddValor(Valor);
                    Valor.TipoValor = this.commonConfigService.GetComboItem(Valor.TipoValor.Id);
                    Valor.Moneda = this.commonConfigService.GetMoneda(Valor.Moneda.Id);
                    return Json(new { Success = true, Valor = Valor });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Valor" });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        [HttpPost]
        public JsonResult Eliminar(List<int> Ids)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.DeleteValores(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar Valor, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            ViewBag.Monedas = this.commonConfigService.GetAllMonedas().Select(m => new SelectListItem() { Value = m.Id.ToString(), Text = m.Nombre }).ToList();
            ViewBag.TipoValores = this.commonConfigService.GetItemsByCombo(ComboType.TipoValor).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Valor Valor = this.TesoreriaConfigService.GetValor(Id);
            return View(Valor);
        }


        [HttpPost]
        public JsonResult Editar(Valor Valor)
        {
            try
            {
                ClearNotValidatedProperties(Valor);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.UpdateValor(Valor);
                    Valor.Moneda = this.commonConfigService.GetMoneda(Valor.Moneda.Id);
                    return Json(new { Success = true, Valor = Valor });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Algunos de los campos no se encuentran correctamente introducidos." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        [HttpPost]
        public JsonResult GetAll(PagingRequest paging )
        {
            if(paging.page == 0)
            {
                return Json(new { Data = this.TesoreriaConfigService.GetAllValores(), Success = true });
            }
            else
            {
                PagingResponse<Valor> resp = new PagingResponse<Valor>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaConfigService.GetAllValores();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double) resp.Records.Count/paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        #endregion

        #region Ingreso de Valores

        public ActionResult NuevaRetencion(int IdValor, string Callback, int? IdCliente, int? IdProveedor)
        {
            IList<Retencion> retenciones = this.ContabilidadConfigService.GetAllRetenciones();
            if (retenciones.Count == 0)
                return RedirectToAction("SinRetenciones", "ConfiguracionErrors", new { Area = "" });

            ViewBag.Retenciones = retenciones;
            ViewBag.RetencionUnica = retenciones.Count == 1 ? retenciones.First().Id : 0;
            ViewBag.Valor = this.TesoreriaConfigService.GetValor(IdValor);
            ViewBag.Callback = Callback;
            ViewBag.IdProveedor = IdProveedor.HasValue ? IdProveedor : 0;
            ViewBag.IdCliente = IdCliente.HasValue ? IdCliente : 0;
            return View();
        }

        [HttpPost]
        public JsonResult NuevaRetencion(ComprobanteRetencion ComprobanteRetencion)
        {
            try
            {
                ComprobanteRetencion.FechaCreacion = DateTime.Now;
                ComprobanteRetencion.Estado = EstadoRetencion.Borrador;
                ComprobanteRetencion.Cliente = ComprobanteRetencion.Cliente.Id > 0 ? ComprobanteRetencion.Cliente : null;
                ComprobanteRetencion.Proveedor = ComprobanteRetencion.Proveedor.Id > 0 ? ComprobanteRetencion.Proveedor : null;
                ClearNotValidatedProperties(ComprobanteRetencion);
                if (ModelState.IsValid)
                {
                    this.TesoreriaService.AddComprobanteRetencion(ComprobanteRetencion);
                    ComprobanteRetencion.Retencion = this.ContabilidadConfigService.GetRetencion(ComprobanteRetencion.Retencion.Id);
                    return Json(new { Success = true, ComprobanteRetencion = ComprobanteRetencion });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Comprobante de Retencion", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }

        public ActionResult NuevoCheque(int IdValor, int IdCliente,string Callback)
        {
            
            IList<Banco> bancos = this.TesoreriaConfigService.GetAllBancos();
            if (bancos.Count == 0)
                return RedirectToAction("SinBancos", "ConfiguracionErrors", new { Area = "" });

            ViewBag.Bancos = bancos;
            ViewBag.BancoUnica = bancos.Count == 1 ? bancos.First().Id : 0;
            
            ViewBag.Valor = this.TesoreriaConfigService.GetValor(IdValor);
            ViewBag.Callback = Callback;
            ViewBag.IdCliente = IdCliente;
            return View();
        }

        [HttpPost]
        public JsonResult NuevoCheque(Cheque Cheque)
        {
            try
            {
                Cheque.FechaCreacion = DateTime.Now;
                Cheque.Estado = EstadoCheque.Borrador;
                ClearNotValidatedProperties(Cheque);
                if (ModelState.IsValid)
                {
                    this.TesoreriaService.AddCheque(Cheque);
                    Cheque.Banco = this.TesoreriaConfigService.GetBanco(Cheque.Banco.Id);
                    return Json(new { Success = true, Cheque = Cheque });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Cheque", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }

        public ActionResult NuevoChequePropio(int IdValor, int IdProveedor, string Callback)
        {
            IList<CuentaBancaria> cuentasBancarias = this.TesoreriaConfigService.GetAllEmiteCheque();
            if (cuentasBancarias.Count == 0)
                return RedirectToAction("SinCuentasBancarias", "ConfiguracionErrors", new { Area = "" });

            ViewBag.CuentasBancarias = cuentasBancarias;
            ViewBag.CuentaBancariaUnica = cuentasBancarias.Count == 1 ? cuentasBancarias.First().Id : 0;
            

            ViewBag.Valor = this.TesoreriaConfigService.GetValor(IdValor);
            ViewBag.Callback = Callback;
            ViewBag.IdProveedor = IdProveedor;
            return View();
        }

        public ActionResult CanjeChequePropio(int Id)
        {
            ChequePropio c = this.TesoreriaService.GetChequePropio(Id);
            ViewBag.CuentasBancarias = this.TesoreriaConfigService.GetAllCuentasBancarias();
            return View(c);
        }

        [HttpPost]
        public JsonResult CanjeChequePropio(int IdAnterior, ChequePropio ChequePropio)
        {
            try
            {
                ChequePropio.FechaCreacion = DateTime.Now;
                ClearNotValidatedProperties(ChequePropio);
                if (ModelState.IsValid)
                {
                    this.TesoreriaService.CanjeCheque(IdAnterior,ChequePropio);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Cheque", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }


        [HttpPost]
        public JsonResult NuevoChequePropio(ChequePropio ChequePropio)
        {
            try
            {
                ChequePropio.FechaCreacion = DateTime.Now;
                ChequePropio.Estado = EstadoCheque.Borrador;
                ClearNotValidatedProperties(ChequePropio);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.ControlChequePropioChequera(ChequePropio.CuentaBancaria.Id, ChequePropio.Numero);
                    this.TesoreriaService.AddChequePropio(ChequePropio);
                    ChequePropio.CuentaBancaria = this.TesoreriaConfigService.GetCuentaBancaria(ChequePropio.CuentaBancaria.Id);
                    return Json(new { Success = true, ChequePropio = ChequePropio });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Cheque", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch (BusinessException bs)
            {
                return Json(new { Success = false, ErrorMessage = bs.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }

        public ActionResult NuevaTransferencia(int IdValor,string Callback, int? IdCliente, int? IdProveedor)
        {
            IList<CuentaBancaria> cuentasBancarias = this.TesoreriaConfigService.GetAllCuentasBancarias();
            if (cuentasBancarias.Count == 0)
                return RedirectToAction("SinCuentasBancarias", "ConfiguracionErrors", new { Area = "" });

            ViewBag.CuentasBancarias = cuentasBancarias;
            ViewBag.CuentaBancariaUnica = cuentasBancarias.Count == 1 ? cuentasBancarias.First().Id : 0;
            
            ViewBag.Valor = this.TesoreriaConfigService.GetValor(IdValor);
            ViewBag.Callback = Callback;
            ViewBag.IdProveedor = IdProveedor.HasValue ? IdProveedor : 0;
            ViewBag.IdCliente = IdCliente.HasValue ? IdCliente : 0;
            return View();
        }

        [HttpPost]
        public JsonResult NuevaTransferencia(Transferencia Transferencia)
        {
            try
            {
                Transferencia.FechaCreacion = DateTime.Now;
                Transferencia.Estado = EstadoTransferencia.Borrador;
                Transferencia.Cliente = Transferencia.Cliente.Id > 0 ? Transferencia.Cliente : null;
                Transferencia.Proveedor = Transferencia.Proveedor.Id > 0 ? Transferencia.Proveedor : null;
                ClearNotValidatedProperties(Transferencia);
                if (ModelState.IsValid)
                {
                    this.TesoreriaService.AddTransferencia(Transferencia);
                    Transferencia.CuentaBancaria = this.TesoreriaConfigService.GetCuentaBancaria(Transferencia.CuentaBancaria.Id);
                    return Json(new { Success = true, Transferencia = Transferencia });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Transferencia", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }

        public ActionResult NuevoEfectivo(int IdValor, string Callback)
        {
            ViewBag.Valor = this.TesoreriaConfigService.GetValor(IdValor);
            ViewBag.Callback = Callback;

            // Devuelve las cajas para el dropdown, 
            // y si tiene una sola setea la variable a preguntar para bloquear el select
            IList<Caja> cajas = this.TesoreriaConfigService.GetAllCajas();
            if (cajas.Count == 0)
                return RedirectToAction("SinCajas", "ConfiguracionErrors", new { Area = "" });

            ViewBag.Cajas = cajas;
            ViewBag.CajaUnica = cajas.Count == 1 ? cajas.First().Id : 0;
            
            return View();
        }

        public ActionResult NuevoPagoTarjeta(int IdValor, int IdProveedor, string Callback)
        {
            IList<SelectListItem> tarjetas = this.TesoreriaConfigService.GetAllTarjetaCreditos().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Numero + " - " + x.CuentaBancaria.Nombre }).ToList();
            if (tarjetas.Count == 0)
                return RedirectToAction("SinTarjetas", "ConfiguracionErrors", new { Area = "" });

            ViewBag.TarjetasCombos = tarjetas;
            ViewBag.Valor = this.TesoreriaConfigService.GetValor(IdValor);
            ViewBag.Callback = Callback;
            ViewBag.IdProveedor = IdProveedor;
            return View();
        }

        [HttpPost]
        public JsonResult NuevoPagoTarjeta(PagoTarjeta PagoTarjeta)
        {
            try
            {
                PagoTarjeta.FechaCreacion = DateTime.Now;
                PagoTarjeta.Estado = EstadoPagoTarjeta.Borrador;
                ClearNotValidatedProperties(PagoTarjeta);
                if (ModelState.IsValid)
                {
                    this.TesoreriaService.AddPagoTarjeta(PagoTarjeta);
                    PagoTarjeta.Tarjeta = this.TesoreriaConfigService.GetTarjetaCredito(PagoTarjeta.Tarjeta.Id);
                    return Json(new { Success = true, PagoTarjeta = PagoTarjeta });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Pago con Tarjeta", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }

        #endregion

        #region Listado de Cheques
        public ActionResult IndexCheque()
        {
            ViewBag.Bancos = this.TesoreriaConfigService.GetAllBancos().Select(x => new SelectComboItem() { text = x.Nombre, id = x.Id });
            return View();
        }

        [HttpPost]
        public JsonResult GetChequeFilter(PagingRequest paging, int IdCliente, int IdBanco, DateTime? start, DateTime? end, FilterCheque filter)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.TesoreriaService.GetChequeFilter(IdCliente,IdBanco, range.Start, range.End, filter)
                        .Select(x => new Cheque()
                        {
                            FechaCreacion = x.FechaCreacion,
                            Id = x.Id,
                            Cliente = x.Cliente,
                            Importe = x.Importe,
                            Banco = x.Banco
                        }),
                    Success = true
                });
            }
            else
            {
                PagingResponse<Cheque> resp = new PagingResponse<Cheque>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetChequeFilter(IdCliente,IdBanco, range.Start, range.End, filter).Select(x => new Cheque()
                {
                    FechaCreacion = x.FechaCreacion,
                    Id = x.Id,
                    Cliente = x.Cliente,
                    Importe = x.Importe,
                    Banco = x.Banco
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult DetalleCheque(int Id, bool? opensDialog)
        {
            Cheque d = this.TesoreriaService.GetCheque(Id);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(d);
        }
        #endregion

        #region Listado de Cheques Propios
        public ActionResult IndexChequePropio()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetChequePropioFilter(PagingRequest paging, int IdProveedor, int IdCuenta, DateTime? start, DateTime? end, FilterCheque filter)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.TesoreriaService.GetChequePropioFilter(IdProveedor, IdCuenta, range.Start, range.End, filter)
                        .Select(x => new ChequePropio()
                        {
                            FechaCreacion = x.FechaCreacion,
                            Id = x.Id,
                            Proveedor = x.Proveedor,
                            Importe = x.Importe,
                            CuentaBancaria = x.CuentaBancaria,
                            Estado = x.Estado,
                            Numero = x.Numero
                        }),
                    Success = true
                });
            }
            else
            {
                PagingResponse<ChequePropio> resp = new PagingResponse<ChequePropio>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetChequePropioFilter(IdProveedor,IdCuenta, range.Start, range.End,filter).Select(x => new ChequePropio()
                {
                    FechaCreacion = x.FechaCreacion,
                    Id = x.Id,
                    Proveedor = x.Proveedor,
                    Importe = x.Importe,
                    CuentaBancaria = x.CuentaBancaria,
                    Estado = x.Estado,
                    Numero = x.Numero
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult DetalleChequePropio(int Id, bool? opensDialog)
        {
            ChequePropio d = this.TesoreriaService.GetChequePropio(Id);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(d);
        }

        public ActionResult ConfirmarPago(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public JsonResult ConfirmarPago(int Id, DateTime FechaPago)
        {
            try
            {
                this.TesoreriaService.ConfirmarPagoChequePropio(Id, FechaPago);
                return Json(new { Success = true });
            }
            catch
            {
                return Json(new { Success = false });
            }
        }
        #endregion

        #region Listado de Transferencias
        public ActionResult IndexTransferencia()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetTransferenciaFilter(PagingRequest paging, int IdCuentaBancaria, int IdProveedor, int IdCliente, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.TesoreriaService.GetTransferenciaFilter(IdCuentaBancaria, IdProveedor, IdCliente, range.Start, range.End)
                        .Select(x => new Transferencia()
                        {
                            Fecha = x.Fecha,
                            Id = x.Id,
                            CuentaBancaria = x.CuentaBancaria,
                            Cliente = x.Cliente,
                            Proveedor = x.Proveedor,
                            Importe = x.Importe
                        }),
                    Success = true
                });
            }
            else
            {
                PagingResponse<Transferencia> resp = new PagingResponse<Transferencia>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetTransferenciaFilter(IdCuentaBancaria, IdProveedor, IdCliente, range.Start, range.End).Select(x => new Transferencia()
                {
                    Fecha = x.Fecha,
                    Id = x.Id,
                    CuentaBancaria = x.CuentaBancaria,
                    Cliente = x.Cliente,
                    Proveedor = x.Proveedor,
                    Importe = x.Importe
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult DetalleTransferencia(int Id, bool? opensDialog)
        {
            Transferencia d = this.TesoreriaService.GetTransferencia(Id);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(d);
        }

        #endregion

        #region Listado Comprobante Retencion
        public ActionResult IndexRetencion()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetRetencionFilter(PagingRequest paging, int TipoRetencion, int IdProveedor, int IdCliente, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.TesoreriaService.GetRetencionFilter(TipoRetencion, IdProveedor, IdCliente, range.Start, range.End),
                    Success = true
                });
            }
            else
            {
                PagingResponse<ComprobanteRetencion> resp = new PagingResponse<ComprobanteRetencion>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetRetencionFilter(TipoRetencion, IdProveedor, IdCliente, range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult DetalleRetencion(int Id, bool? opensDialog)
        {
            ComprobanteRetencion d = this.TesoreriaService.GetComprobanteRetencion(Id);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(d);
        }

        #endregion

    }
}