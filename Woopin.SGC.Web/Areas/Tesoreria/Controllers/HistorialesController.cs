using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Tesoreria.Controllers
{
    public class HistorialesController : BaseController
    {
        //
        // GET: /Tesoreria/Historiales/

        private readonly ITesoreriaConfigService TesoreriaConfigService;
        private readonly ITesoreriaService TesoreriaService;

        public HistorialesController(ITesoreriaConfigService TesoreriaConfigService, ITesoreriaService TesoreriaService)
        {
            this.TesoreriaConfigService = TesoreriaConfigService;
            this.TesoreriaService = TesoreriaService;
        }


        public ActionResult Cajas()
        {
            return View();
        }

        public JsonResult GetAllCajasByDates(int IdCaja, DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.TesoreriaService.GetAllHistorialCajaByDates(IdCaja,range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<HistorialCaja> resp = new PagingResponse<HistorialCaja>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetAllHistorialCajaByDates(IdCaja,range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult Cuenta(int? Id)
        {
            return View();
        }

        public JsonResult GetAllCuentasByDates(int Id,DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.TesoreriaService.GetAllHistorialCuentasByDates(Id,range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<HistorialCuentaBancaria> resp = new PagingResponse<HistorialCuentaBancaria>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetAllHistorialCuentasByDates(Id,range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

    }
}
