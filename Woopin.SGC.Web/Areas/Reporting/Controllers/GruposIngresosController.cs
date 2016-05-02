using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Reporting;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.Filters;

namespace Woopin.SGC.Web.Areas.Reporting.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class GruposIngresosController : BaseController
    {
        private readonly IReportingService ReportingService;
        private readonly ICommonConfigService commonConfigService;
        public GruposIngresosController(ICommonConfigService commonConfigService, IReportingService ReportingService)
        {
            this.commonConfigService = commonConfigService;
            this.ReportingService = ReportingService;
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
        public JsonResult Nuevo(GrupoIngreso GrupoIngreso)
        {
            try
            {
                this.ReportingService.AddGrupoIngreso(GrupoIngreso);
                return Json(new { Success = true, GrupoIngreso = GrupoIngreso });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }
        public ActionResult Detalle(int Id, bool? opensDialog)
        {
            GrupoIngreso GrupoIngreso = this.ReportingService.GetGrupoIngreso(Id);
            if (GrupoIngreso == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(GrupoIngreso);
        }


        public ActionResult Editar(int Id)
        {
            GrupoIngreso GrupoIngreso = this.ReportingService.GetGrupoIngreso(Id);
            return View(GrupoIngreso);
        }


        [HttpPost]
        public JsonResult Editar(GrupoIngreso GrupoIngreso)
        {
            try
            {
                    this.ReportingService.UpdateGrupoIngreso(GrupoIngreso);
                    return Json(new { Success = true, GrupoIngreso = GrupoIngreso });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        [HttpPost]
        public JsonResult GetAllTree(PagingRequest paging, DateTime start, DateTime end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ReportingService.GetAllGruposIngresosTree(range.Start, range.End), Success = true });
            }
            else
            {
                IList<GrupoIngreso> gruposIngresos = this.ReportingService.GetAllGruposIngresosTree(range.Start, range.End);
                PagingResponse<dynamic> resp = new PagingResponse<dynamic>();
                resp.Page = paging.page;
                resp.Records = gruposIngresos.Select(x => new
                {
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    DescripcionPadre = (x.NodoPadre != null ? x.NodoPadre.Descripcion : null),
                    DescripcionArticulo = (x.Articulo != null ? x.Articulo.Descripcion : null),
                    level = x.Level,
                    parent = (x.NodoPadre != null ? x.NodoPadre.Id : 0),
                    isLeaf = (x.Articulo != null ? true : false),
                    loaded = true,
                    expanded = x.Articulo == null,
                    codigo = x.Raiz + "" +  x.Level
                }).OrderBy(x => x.codigo).ToList<dynamic>();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetGruposIngresos(SelectComboRequest req)
        {
            return Json(new { Data = this.ReportingService.GetAllGruposIngresosNoHoja(req), Success = true });
        }

        [HttpPost]
        public JsonResult Eliminar(int Id)
        {
            try
            {
                this.ReportingService.DeleteGrupoIngreso(Id);
                return Json(new { Success = true });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

    }
}
