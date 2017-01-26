using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Compras.Controllers
{
    public class ComprobantesComprasController : BaseController
    {
        private readonly IComprasConfigService ComprasConfigService;
        private readonly IComprasService ComprasService;
        private readonly ICommonConfigService commonConfigService;
        private readonly IContabilidadConfigService contabilidadConfigService;
        private readonly IContabilidadService contabilidadService;
        public ComprobantesComprasController(IComprasConfigService ComprasConfigService, ICommonConfigService commonConfigService, IContabilidadConfigService contabilidadConfigService ,
                                            IComprasService ComprasService, IContabilidadService contabilidadService)
        {
            this.ComprasConfigService = ComprasConfigService;
            this.commonConfigService = commonConfigService;
            this.contabilidadConfigService  = contabilidadConfigService ;
            this.ComprasService = ComprasService;
            this.contabilidadService = contabilidadService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index(string start, string end, string statusType, string startvenc, string endvenc)
        {
            ViewBag.start = start;
            ViewBag.end = end;
            ViewBag.startvenc = startvenc;
            ViewBag.endvenc = endvenc;
            ViewBag.statusType = statusType;
            return View();
        }

        public ActionResult CuentaCorriente(int? Id)
        {
            ViewBag.IdProveedor = Id.HasValue ? Id.Value : 0;
            return View();
        }

        public ActionResult Acumulados()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.NumeroRef = this.ComprasService.GetProximoNumeroReferencia();
            ViewBag.Tipos = this.commonConfigService.GetItemsByCombo(ComboType.TipoComprobanteCompra).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.IVAs = new SelectCombo() { Items = this.commonConfigService.GetItemsByCombo(ComboType.TipoIva).Select(x => new SelectComboItem() { id = x.Id, text = x.Data, additionalData = x.AdditionalData }).ToList() };
            ViewBag.CondicionesCompra = this.commonConfigService.GetItemsByCombo(ComboType.CondicionCompraVenta).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View();
        }


        [HttpPost]
        public JsonResult Nuevo(ComprobanteCompra ComprobanteCompra)
        {
            try
            {
                int Numero = ComprobanteCompra.NumeroReferencia;
                this.ComprasService.AddComprobanteCompra(ComprobanteCompra);
                // Problema de referencia circular, no lo devuelvo
                ComprobanteCompra.Asiento = null;
                ComprobanteCompra.Detalle = null;
                if (Numero != ComprobanteCompra.NumeroReferencia)
                {
                    return Json(new { Success = true, NumeroRef = ComprobanteCompra.NumeroReferencia, Comprobante = ComprobanteCompra });
                }
                return Json(new { Success = true, Comprobante = ComprobanteCompra });
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch (BusinessException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }

        public ActionResult Detalle(int Id, bool? opensDialog)
        {
            ComprobanteCompra c = this.ComprasService.GetComprobanteCompraCompleto(Id);
            if (c == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(c);
        }

        public ActionResult ComprobantesAPagar(int IdProveedor)
        {
            ViewBag.IdProveedor = IdProveedor;
            return View();
        }

        public ActionResult ComprobantesAPagarNC(int IdProveedor, int? Tipo, int? NoTipo,ComprobantesACancelarFilter Pagada)
        {
            ViewBag.IdProveedor = IdProveedor;
            ViewBag.Tipo = Tipo.HasValue ? Tipo.Value : 0;
            ViewBag.NoTipo = NoTipo.HasValue ? NoTipo.Value : 0;
            ViewBag.Pagada = (int) Pagada;
            return View();
        }

        [HttpPost]
        public JsonResult AnularComprobante(int IdComprobante)
        {
            try
            {
                this.ComprasService.AnularComprobante(IdComprobante);
                return Json(new { Success = true, Data = "Fue anulada con exito" });
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch (BusinessException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
            
        }

        [HttpPost]
        public JsonResult EliminarComprobante(int IdComprobante)
        {
            try
            {
                this.ComprasService.EliminarComprobante(IdComprobante);
                return Json(new { Success = true, Data = "Fue eliminado con exito" });
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch (BusinessException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }

        [HttpPost]
        public JsonResult LoadCtaCorrienteHead(int id, DateTime? start, DateTime? end, CuentaCorrienteFilter filter)
        {
            DateRange range = new DateRange(start, end);
            return Json(new { Data = this.ComprasService.LoadCtaCorrienteHead(id,range.Start, range.End, filter), Success = true });
        }

        [HttpPost]
        public JsonResult AcumuladosHead(int id, int idRubro, DateTime? start, DateTime? end, CuentaCorrienteFilter filter)
        {
            DateRange range = new DateRange(start, end);
            return Json(new { Data = this.ComprasService.AcumuladosHead(id, idRubro, range.Start, range.End, filter), Success = true });
        }

        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.ComprasService.GetAllComprobantesCompras(), Success = true });
            }
            else
            {
                PagingResponse<ComprobanteCompra> resp = new PagingResponse<ComprobanteCompra>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetAllComprobantesCompras().Select(x => new ComprobanteCompra()
                                                                                            {
                                                                                                Id = x.Id,
                                                                                                Proveedor = x.Proveedor,
                                                                                                Estado = x.Estado,
                                                                                                Fecha = x.Fecha,
                                                                                                Numero = x.Numero,
                                                                                                CondicionCompra = x.CondicionCompra,
                                                                                                Letra = x.Letra,
                                                                                                IVA = x.IVA,
                                                                                                Tipo = x.Tipo,
                                                                                                Subtotal = x.Subtotal,
                                                                                                Total = x.Total,
                                                                                                TotalPagado = x.TotalPagado
                                                                                            }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAllByProveedor(int Id, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, CuentaCorrienteFilter filter, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.ComprasService.GetAllComprobantesCompraByProveedor(Id, range.Start, range.End, startvenc, endvenc, filter).Select(x => new ComprobanteCompra()
                    {
                        Id = x.Id,
                        Proveedor = x.Proveedor,
                        Estado = x.Estado,
                        Fecha = x.Fecha,
                        Numero = x.Numero,
                        CondicionCompra = x.CondicionCompra,
                        Letra = x.Letra,
                        IVA = x.IVA,
                        Tipo = x.Tipo,
                        Subtotal = x.Subtotal,
                        Total = x.Total,
                        TotalPagado = x.TotalPagado
                    }).ToList(),
                    Success = true
                });
            }
            else
            {
                PagingResponse<ComprobanteCompra> resp = new PagingResponse<ComprobanteCompra>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetAllComprobantesCompraByProveedor(Id, range.Start, range.End, startvenc, endvenc, filter).Select(x => new ComprobanteCompra()
                {
                    Id = x.Id,
                    Proveedor = x.Proveedor,
                    Estado = x.Estado,
                    Fecha = x.Fecha,
                    Numero = x.Numero,
                    CondicionCompra = x.CondicionCompra,
                    Letra = x.Letra,
                    IVA = x.IVA,
                    Tipo = x.Tipo,
                    Subtotal = x.Subtotal,
                    Total = x.Total,
                    TotalPagado = x.TotalPagado
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAllAcumulados(int Id, int idRubro, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, CuentaCorrienteFilter filter, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            DateRange rangevenc = new DateRange(startvenc, endvenc);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ComprasService.GetAllCCAcumulados(Id, idRubro, range.Start, range.End, filter), Success = true });
            }
            else
            {
                PagingResponse<CuentaCorrienteItem> resp = new PagingResponse<CuentaCorrienteItem>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetAllCCAcumulados(Id, idRubro, range.Start, range.End, filter);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetCuentaCorrienteByDates(int id, DateTime? start, DateTime? end, CuentaCorrienteFilter filter, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ComprasService.GetCuentaCorrienteByDates(range.Start, range.End, id, filter), Success = true });
            }
            else
            {
                PagingResponse<CuentaCorrienteItem> resp = new PagingResponse<CuentaCorrienteItem>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetCuentaCorrienteByDates(range.Start, range.End, id, filter);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAllAPagarByProv(int IdProveedor, PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.ComprasService.GetAllComprobanteCompraAPagarByProv(IdProveedor).Select(x => new ComprobanteCompra()
                    {
                        Id = x.Id,
                        Proveedor = x.Proveedor,
                        Estado = x.Estado,
                        Fecha = x.Fecha,
                        Numero = x.Numero,
                        CondicionCompra = x.CondicionCompra,
                        Letra = x.Letra,
                        IVA = x.IVA,
                        Tipo = x.Tipo,
                        Subtotal = x.Subtotal,
                        Total = x.Total,
                        TotalPagado = x.TotalPagado
                    }).ToList(),
                    Success = true
                });
            }
            else
            {
                PagingResponse<ComprobanteCompra> resp = new PagingResponse<ComprobanteCompra>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetAllComprobanteCompraAPagarByProv(IdProveedor).Select(x => new ComprobanteCompra()
                {
                    Id = x.Id,
                    Proveedor = x.Proveedor,
                    Estado = x.Estado,
                    Fecha = x.Fecha,
                    Numero = x.Numero,
                    CondicionCompra = x.CondicionCompra,
                    Letra = x.Letra,
                    IVA = x.IVA,
                    Tipo = x.Tipo,
                    Subtotal = x.Subtotal,
                    Total = x.Total,
                    TotalPagado = x.TotalPagado
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAllByProvFilterNC(int? IdProveedor, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Pagada, PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.ComprasService.GetAllByProvFilterNC(IdProveedor, Tipo, NoTipo, Pagada),
                    Success = true
                });
            }
            else
            {
                PagingResponse<ComprobanteCompra> resp = new PagingResponse<ComprobanteCompra>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetAllByProvFilterNC(IdProveedor, Tipo, NoTipo, Pagada).Select(x => new ComprobanteCompra()
                {
                    Id = x.Id,
                    Proveedor = x.Proveedor,
                    Estado = x.Estado,
                    Fecha = x.Fecha,
                    Numero = x.Numero,
                    CondicionCompra = x.CondicionCompra,
                    Letra = x.Letra,
                    IVA = x.IVA,
                    Tipo = x.Tipo,
                    Subtotal = x.Subtotal,
                    Total = x.Total,
                    TotalPagado = x.TotalPagado
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }




    }
}
