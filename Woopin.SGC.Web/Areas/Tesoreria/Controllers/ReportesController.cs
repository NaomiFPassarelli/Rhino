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
    public class ReportesController : BaseController
    {
        private readonly ITesoreriaReportService tesoreriaReportService;
        private readonly ITesoreriaService tesoreriaService;
        public ReportesController(ITesoreriaReportService tesoreriaReportService, ITesoreriaService tesoreriaService)
        {
            this.tesoreriaReportService = tesoreriaReportService;
            this.tesoreriaService = tesoreriaService;
        }


        #region Ingresos por Periodo
        public ActionResult Ingresos()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllIngresos(PagingRequest paging, DateTime start, DateTime end)
        {
            DateRange range = new DateRange(start, end, 7);
            if (paging.page == 0)
            {
                return Json(new { Data = this.tesoreriaReportService.GetIngresosByDates(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<ReporteTesoreria> resp = new PagingResponse<ReporteTesoreria>();
                resp.Page = paging.page;
                resp.Records = this.tesoreriaReportService.GetIngresosByDates(range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                resp.userdata = new
                {
                    Monto = resp.Records.Sum(x => x.Monto)
                };
                return Json(resp);
            }
        }
        #endregion

        
        #region Egresos por Periodo
        public ActionResult Egresos()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllEgresos(PagingRequest paging, DateTime start, DateTime end)
        {
            DateRange range = new DateRange(start, end, 7);
            if (paging.page == 0)
            {
                return Json(new { Data = this.tesoreriaReportService.GetEgresosByDates(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<ReporteTesoreria> resp = new PagingResponse<ReporteTesoreria>();
                resp.Page = paging.page;
                resp.Records = this.tesoreriaReportService.GetEgresosByDates(range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                resp.userdata = new
                {
                    Monto = resp.Records.Sum(x => x.Monto)
                };
                return Json(resp);
            }
        }
        #endregion

        #region Comprobate de Retenciones
        public ActionResult ComprobantesRetenciones()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllComprobantesRetenciones(PagingRequest paging, DateTime start, DateTime end, int TipoRetencion)
        {
            int IdProveedor = 0;
            int IdCliente = 0;
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                { Data = this.tesoreriaService.GetRetencionFilterReporte(TipoRetencion, IdProveedor, IdCliente, range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<ComprobanteRetencionReporte> resp = new PagingResponse<ComprobanteRetencionReporte>();
                resp.Page = paging.page;
                resp.Records = this.tesoreriaService.GetRetencionFilterReporte(TipoRetencion, IdProveedor, IdCliente, range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                resp.userdata = new
                {
                    ProveedorCUIT = "Totales:",
                    Debe = resp.Records.Sum(x => x.Debe),
                    Haber = resp.Records.Sum(x => x.Haber)
                };
                return Json(resp);
            }
        }
        #endregion

    }
}
