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
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Compras.Controllers
{
    public class OrdenesPagosController : BaseController
    {
        private readonly IComprasService ComprasService;
        private readonly ICommonConfigService commonConfigService;
        private readonly ITesoreriaConfigService tesoreriaSerivce;
        private readonly IContabilidadService ContabilidadService;

        public OrdenesPagosController(IComprasService ComprasService, ICommonConfigService commonConfigService, ITesoreriaConfigService tesoreriaSerivce, IContabilidadService ContabilidadService)
        {
            this.ComprasService = ComprasService;
            this.commonConfigService = commonConfigService;
            this.tesoreriaSerivce = tesoreriaSerivce;
            this.ContabilidadService = ContabilidadService;
        }

        //
        // GET: /Compras/OrdenesPagos/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Compras/OrdenesPagos/Nuevo

        public ActionResult Nuevo()
        {
            IList<ComboItem> tipos = this.commonConfigService.GetItemsByCombo(ComboType.TipoOrdenPago);
            ViewBag.Tipo = tipos.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data, Selected = tipos.Count() == 1 }).ToList();
            ViewBag.NumeroRef = this.ComprasService.GetOrdenPagoProximoNumeroReferencia();
            ViewBag.Valores = new SelectCombo() { Items = this.tesoreriaSerivce.GetAllValores().Select(x => new SelectComboItem() { id = x.Id, text = x.Nombre, additionalData = x.TipoValor.Data }).ToList() };
            return View();
        }


        [HttpPost]
        public JsonResult Nuevo(OrdenPago OrdenPago)
        {
            try
            {
                OrdenPago.Estado = EstadoComprobanteCancelacion.Pagada;
                string NumRef = OrdenPago.Numero;
                this.ComprasService.AddOrdenPago(OrdenPago);
                OrdenPago.Asiento = null;
                if (OrdenPago.Id.ToString() != NumRef)
                {
                    return Json(new { Success = true, NumeroRef = OrdenPago.Id, OrdenPago = OrdenPago });
                }
                return Json(new { Success = true, OrdenPago = OrdenPago });
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
            OrdenPago op = this.ComprasService.GetOrdenPagoCompleto(Id);
            if (op == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.EjercicioDesbloqueado = this.ContabilidadService.ControlarIngreso(op.Fecha);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(op);
        }

        [HttpPost]
        public JsonResult CancelarOrdenPago(int IdOrdenPago)
        {
            try
            {
                this.ComprasService.CancelarOrdenPago(IdOrdenPago);
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
        public JsonResult GetAllByProveedor(int IdProveedor, DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.ComprasService.GetAllOrdenPagoByProveedor(IdProveedor,range.Start, range.End).Select(x => new OrdenPago()
                    {
                        Id = x.Id,
                        Proveedor = x.Proveedor,
                        Estado = x.Estado,
                        Fecha = x.Fecha,
                        Numero = x.Numero,
                        Tipo = x.Tipo,
                        Total = x.Total,
                        Detalle = x.Detalle
                    }).ToList(),
                    Success = true
                });
            }
            else
            {
                PagingResponse<OrdenPago> resp = new PagingResponse<OrdenPago>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetAllOrdenPagoByProveedor(IdProveedor, range.Start, range.End).Select(x => new OrdenPago()
                {
                    Id = x.Id,
                    Proveedor = x.Proveedor,
                    Estado = x.Estado,
                    Fecha = x.Fecha,
                    Numero = x.Numero,
                    Tipo = x.Tipo,
                    Total = x.Total,
                    Detalle = x.Detalle
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

    }
}
