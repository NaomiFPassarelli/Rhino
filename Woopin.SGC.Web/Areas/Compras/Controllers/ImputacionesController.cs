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
    public class ImputacionesController : BaseController
    {
        private readonly IComprasConfigService ComprasConfigService;
        private readonly IContabilidadConfigService contabilidadService;
        private readonly IComprasService ComprasService;
        public ImputacionesController(IComprasConfigService ComprasConfigService, IContabilidadConfigService contabilidadService, IComprasService ComprasService)
        {
            this.ComprasConfigService = ComprasConfigService;
            this.contabilidadService = contabilidadService;
            this.ComprasService = ComprasService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(IList<ImputacionCompra> Imputaciones)
        {
            try
            {
                ClearNotValidatedProperties(Imputaciones);
                this.ComprasService.AddImputaciones(Imputaciones);
                return Json(new { Success = true });
            }
            catch(BusinessException bs)
            {
                return Json(new { Success = false, ErrorMessage = bs.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.ComprasService.GetAllImputaciones(), Success = true });
            }
            else
            {
                PagingResponse<ImputacionCompra> resp = new PagingResponse<ImputacionCompra>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetAllImputaciones();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }


        [HttpPost]
        public JsonResult GetImputacion(int IdImputacion)
        {
            return Json(new { Data = this.ComprasService.GetImputacion(IdImputacion), Success = true });
        }

        public ActionResult NotasCredito(int IdProveedor, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Pagada)
        {
            ViewBag.IdProveedor = IdProveedor;
            ViewBag.Tipo = Tipo;
            ViewBag.NoTipo = NoTipo;
            ViewBag.Pagada = (int)Pagada;
            return View();
        }

        public ActionResult CDescontar(int IdProveedor, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Pagada)
        {
            ViewBag.IdProveedor = IdProveedor;
            ViewBag.Tipo = Tipo;
            ViewBag.NoTipo = NoTipo;
            ViewBag.Pagada = (int)Pagada;
            return View();
        }

        [HttpPost]
        public JsonResult GetAllByProveedor(int IdProveedor, DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.ComprasService.GetAllImputacionesByProveedor(IdProveedor, range.Start, range.End),
                    Success = true
                });
            }
            else
            {
                PagingResponse<ImputacionCompra> resp = new PagingResponse<ImputacionCompra>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetAllImputacionesByProveedor(IdProveedor, range.Start, range.End).Select(x => new ImputacionCompra()
                {
                    Id = x.Id,
                    Fecha = x.Fecha,
                    Importe = x.Importe,
                    NotaCredito = x.NotaCredito,
                    ComprobanteADescontar = x.ComprobanteADescontar
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult AnularImputacion(int IdImputacion)
        {
            try
            {
                this.ComprasService.AnularImputacion(IdImputacion);
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

    }
}
