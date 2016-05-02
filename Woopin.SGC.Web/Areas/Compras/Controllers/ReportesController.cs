using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Compras.Controllers
{
    public class ReportesController : BaseController
    {
        private readonly IComprasReportService comprasReportService;
        public ReportesController(IComprasReportService comprasReportService)
        {
            this.comprasReportService = comprasReportService;
        }

        #region Compras por Rubros

        public ActionResult ComprasRubros()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllComprasRubros(PagingRequest paging, int IdProveedor, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.comprasReportService.GetReporteRubros(IdProveedor, range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<ReporteComprasRubros> resp = new PagingResponse<ReporteComprasRubros>();
                resp.Page = paging.page;
                resp.Records = this.comprasReportService.GetReporteRubros(IdProveedor, range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        #endregion

        #region Vencimientos A Pagar

        public ActionResult VencimientosAPagar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllVencimientosAPagar(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.comprasReportService.GetVencimientosAPagar(), Success = true });
            }
            else
            {
                PagingResponse<ReporteCompra> resp = new PagingResponse<ReporteCompra>();
                resp.Page = paging.page;
                resp.Records = this.comprasReportService.GetVencimientosAPagar();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        #endregion

        #region Citi Compras

        public ActionResult Citi()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllCiti(PagingRequest paging, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.comprasReportService.GetCitiCompras(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<ReporteCitiItem> resp = new PagingResponse<ReporteCitiItem>();
                resp.Page = paging.page;
                resp.Records = this.comprasReportService.GetCitiCompras(range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult GetCitiPlainFile(DateTime? start, DateTime? end, TipoReporteCiti tipo)
        {
            DateRange range = new DateRange(start, end);
            IList<ReporteCitiItem> items = this.comprasReportService.GetCitiCompras(range.Start, range.End);

            StringBuilder sb = new StringBuilder();

            foreach (var item in items)
            {
                if (tipo == TipoReporteCiti.ComprasAlicuotas && (item.Letra == "B" || item.Letra == "C"))
                    continue;

                string[] records = item.GetCitiRows(tipo);
                foreach (var record in records)
                {
                    sb.Append(record);
                    sb.Append("\r\n");
                }

            }

            string text = sb.ToString();

            Response.Clear();
            Response.ClearHeaders();

            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "text/plain";
            string filename = "Citi_" + tipo.ToString() + DateTime.Now.ToString("ddMMyyyy");
            Response.AppendHeader("content-disposition", "attachment;filename=\"" + filename + ".txt\"");

            Response.Write(text);
            Response.End();
            return View();
        }
        #endregion
    }
}
