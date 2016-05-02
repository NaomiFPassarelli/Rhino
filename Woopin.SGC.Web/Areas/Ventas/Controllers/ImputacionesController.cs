using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Ventas.Controllers
{
    public class ImputacionesController : BaseController
    {
        private readonly IVentasConfigService VentasConfigService;
        private readonly IContabilidadConfigService contabilidadService;
        private readonly IVentasService VentasService;
        public ImputacionesController(IVentasConfigService VentasConfigService, IContabilidadConfigService contabilidadService, IVentasService VentasService)
        {
            this.VentasConfigService = VentasConfigService;
            this.contabilidadService = contabilidadService;
            this.VentasService = VentasService;
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
        public JsonResult Nuevo(IList<ImputacionVenta> Imputaciones)
        {
            try
            {
                ClearNotValidatedProperties(Imputaciones);
                this.VentasService.AddImputaciones(Imputaciones);
                return Json(new { Success = true });
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


        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.VentasService.GetAllImputaciones(), Success = true });
            }
            else
            {
                PagingResponse<ImputacionVenta> resp = new PagingResponse<ImputacionVenta>();
                resp.Page = paging.page;
                resp.Records = this.VentasService.GetAllImputaciones();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }


        [HttpPost]
        public JsonResult GetImputacion(int IdImputacion)
        {
            return Json(new { Data = this.VentasService.GetImputacion(IdImputacion), Success = true });
        }

        public ActionResult NotasCredito(int IdCliente, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Cobrada)
        {
            ViewBag.IdCliente = IdCliente;
            ViewBag.Tipo = Tipo;
            ViewBag.NoTipo = NoTipo;
            ViewBag.Cobrada = (int)Cobrada;
            return View();
        }

        public ActionResult CDescontar(int IdCliente, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Cobrada)
        {
            ViewBag.IdCliente = IdCliente;
            ViewBag.Tipo = Tipo;
            ViewBag.NoTipo = NoTipo;
            ViewBag.Cobrada = (int)Cobrada;
            return View();
        }

        [HttpPost]
        public JsonResult GetAllByCliente(int IdCliente, DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.VentasService.GetAllImputacionesByCliente(IdCliente, range.Start, range.End),
                    Success = true
                });
            }
            else
            {
                PagingResponse<ImputacionVenta> resp = new PagingResponse<ImputacionVenta>();
                resp.Page = paging.page;
                resp.Records = this.VentasService.GetAllImputacionesByCliente(IdCliente, range.Start, range.End).Select(x => new ImputacionVenta()
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
                this.VentasService.AnularImputacion(IdImputacion);
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
