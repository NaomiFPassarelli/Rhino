using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Ventas.Controllers
{
    public class ReportesController : BaseController
    {
        private readonly IVentasReportService ventasReportService;
        public ReportesController(IVentasReportService ventasReportService)
        {
            this.ventasReportService = ventasReportService;
        }

        //
        // GET: /Ventas/Reportes/
        #region Vencimientos A Cobrar

        public ActionResult VencimientosACobrar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllVencimientosACobrar(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.ventasReportService.GetVencimientosACobrar(), Success = true });
            }
            else
            {
                PagingResponse<ReporteVenta> resp = new PagingResponse<ReporteVenta>();
                resp.Page = paging.page;
                resp.Records = this.ventasReportService.GetVencimientosACobrar();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        #endregion

        #region Cobranzas Por Semana

        public ActionResult CobranzasPorSemana()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllCobranzasPorSemana(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.ventasReportService.GetVencimientosACobrar().OrderBy(x => x.FechaEstipuladaCobro).ToList(), Success = true });
            }
            else
            {
                PagingResponse<ReporteVenta> resp = new PagingResponse<ReporteVenta>();
                resp.Page = paging.page;
                resp.Records = this.ventasReportService.GetVencimientosACobrar().OrderBy(x => x.FechaEstipuladaCobro).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        #endregion

        #region Ventas por Articulos

        public ActionResult VentasArticulos()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllVentasArticulos(PagingRequest paging, int IdCliente, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ventasReportService.GetReporteVentasArticulo(IdCliente, range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<ReporteVentasArticulo> resp = new PagingResponse<ReporteVentasArticulo>();
                resp.Page = paging.page;
                resp.Records = this.ventasReportService.GetReporteVentasArticulo(IdCliente, range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        #endregion

        #region Citi Ventas

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
                return Json(new { Data = this.ventasReportService.GetCitiVentas(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<ReporteCitiItem> resp = new PagingResponse<ReporteCitiItem>();
                resp.Page = paging.page;
                resp.Records = this.ventasReportService.GetCitiVentas(range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult GetCitiPlainFile(DateTime? start, DateTime? end, TipoReporteCiti tipo)
        {
            DateRange range = new DateRange(start, end);
            IList<ReporteCitiItem> items = this.ventasReportService.GetCitiVentas(range.Start, range.End);

            StringBuilder sb = new StringBuilder();

            foreach (var item in items)
            {
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
            Response.AppendHeader("content-disposition", "attachment;filename=\""+ filename + ".txt\"");

            Response.Write(text);
            Response.End();
            return View();
        }
        #endregion

        #region Ventas Por Clientes

        public ActionResult VentasPorCliente()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAllVentasAcumPorClientes(DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ventasReportService.GetVentasPorClientes(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<ReporteAcumulado> resp = new PagingResponse<ReporteAcumulado>();
                resp.Page = paging.page;
                resp.Records = this.ventasReportService.GetVentasPorClientes(range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        #endregion
    }
}
