using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Tesoreria.Controllers
{
    public class PagosTarjetasController : BaseController
    {
        private readonly ITesoreriaConfigService TesoreriaConfigService;
        private readonly ITesoreriaService TesoreriaService;
        private readonly ICommonConfigService commonConfigService;

        public PagosTarjetasController(ITesoreriaConfigService TesoreriaConfigService, ICommonConfigService commonConfigService, ITesoreriaService TesoreriaService)
        {
            this.TesoreriaConfigService = TesoreriaConfigService;
            this.commonConfigService = commonConfigService;
            this.TesoreriaService = TesoreriaService;
        }

        public ActionResult Index(string start, string end, string statusType)
        {
            ViewBag.start = start;
            ViewBag.end = end;
            ViewBag.statusType = statusType;
            ViewBag.Tarjetas = this.TesoreriaConfigService.GetAllTarjetaCreditos();
            return View();
        }

        public ActionResult Detalle(int Id)
        {
            PagoTarjeta pago = this.TesoreriaService.GetPagoTarjetaCompleta(Id);
            if (pago == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            return View(pago);
        }

        public ActionResult CancelarDeudas(int Id)
        {
            ViewBag.IdPagoTarjeta = Id;
            ViewBag.PagoTarjeta = this.TesoreriaService.GetPagoTarjeta(Id);
            return View();
        }


        [HttpPost]
        public JsonResult CancelarDeudas(CancelacionTarjeta CancelacionTarjeta)
        {
            try
            {
                ClearNotValidatedProperties(CancelacionTarjeta);
                if (ModelState.IsValid)
                {
                    this.TesoreriaService.AddCancelacionTarjeta(CancelacionTarjeta);
                    CancelacionTarjeta.Asiento = null;
                    CancelacionTarjeta.Pago.Cancelaciones = null;
                    CancelacionTarjeta.Pago.Usuario = null;
                    return Json(new { Success = true, CancelacionTarjeta = CancelacionTarjeta });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de cancelación de deuda con tarjeta", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch(ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }





        public JsonResult GetAllPagosByDates(int Id, DateTime? start, DateTime? end, PagingRequest paging, PagoTarjetaFilter filter)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.TesoreriaService.GetAllPagoTarjetasByDates(Id, range.Start, range.End,filter), Success = true });
            }
            else
            {
                PagingResponse<PagoTarjeta> resp = new PagingResponse<PagoTarjeta>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetAllPagoTarjetasByDates(Id, range.Start, range.End,filter);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
    }
}
