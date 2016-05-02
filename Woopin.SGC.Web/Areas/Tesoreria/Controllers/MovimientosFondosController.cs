using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Common.Models;

namespace Woopin.SGC.Web.Areas.Tesoreria.Controllers
{
    public class MovimientosFondosController : BaseController
    {
        private readonly ITesoreriaService TesoreriaService;
        private readonly IContabilidadConfigService ContabilidadConfigService;
        private readonly ICommonConfigService CommonConfigService;
        private readonly IContabilidadService ContabilidadService;
        public MovimientosFondosController(ITesoreriaService TesoreriaService, IContabilidadConfigService ContabilidadConfigService, ICommonConfigService CommonConfigService,
            IContabilidadService ContabilidadService)
        {
            this.TesoreriaService = TesoreriaService;
            this.ContabilidadConfigService = ContabilidadConfigService;
            this.CommonConfigService = CommonConfigService;
            this.ContabilidadService = ContabilidadService;
        }
        //
        // GET: /Tesoreria/MovimientosFondos/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(MovimientoFondo MovimientoFondo)
        {
            try
            {
                MovimientoFondo.FechaCreacion = DateTime.Now;
                ClearNotValidatedProperties(MovimientoFondo);
                if (ModelState.IsValid && ((MovimientoFondo.CuentaBancaria != null && (MovimientoFondo.CuentaDestino != null || MovimientoFondo.Caja != null))))
                {
                    this.TesoreriaService.AddMovimientoFondo(MovimientoFondo);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Movimiento de Fondo", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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

        [HttpPost]
        public JsonResult GetComboMovimiento(PagingRequest paging)
        {
            return Json(new { Data = this.CommonConfigService.GetSelectItemsByComboId(ComboType.MovimientoFondo) });
        }



        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
       {
            if (paging.page == 0)
            {
                return Json(new { Data = this.TesoreriaService.GetAllMovimientosFondos(), Success = true });
            }
            else
            {
                PagingResponse<MovimientoFondo> resp = new PagingResponse<MovimientoFondo>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetAllMovimientosFondos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public JsonResult GetAllByDates(DateTime? start,DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.TesoreriaService.GetAllMovimientosFondosByDates(range.Start,range.End), Success = true });
            }
            else
            {
                PagingResponse<MovimientoFondo> resp = new PagingResponse<MovimientoFondo>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetAllMovimientosFondosByDates(range.Start,range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }


    }
}
