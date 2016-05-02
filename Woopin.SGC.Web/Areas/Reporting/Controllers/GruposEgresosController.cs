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
    public class GruposEgresosController : BaseController
    {
        private readonly IReportingService ReportingService;
        private readonly ICommonConfigService commonConfigService;
        public GruposEgresosController(ICommonConfigService commonConfigService, IReportingService ReportingService)
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
        public JsonResult Nuevo(GrupoEgreso GrupoEgreso)
        {
            try
            {
                this.ReportingService.AddGrupoEgreso(GrupoEgreso);
                return Json(new { Success = true, GrupoEgreso = GrupoEgreso });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }
        public ActionResult Detalle(int Id, bool? opensDialog)
        {
            GrupoEgreso GrupoEgreso = this.ReportingService.GetGrupoEgreso(Id);
            if (GrupoEgreso == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(GrupoEgreso);
        }


        public ActionResult Editar(int Id)
        {
            GrupoEgreso GrupoEgreso = this.ReportingService.GetGrupoEgreso(Id);
            return View(GrupoEgreso);
        }


        [HttpPost]
        public JsonResult Editar(GrupoEgreso GrupoEgreso)
        {
            try
            {
                    this.ReportingService.UpdateGrupoEgreso(GrupoEgreso);
                    return Json(new { Success = true, GrupoEgreso = GrupoEgreso });
                
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
                return Json(new { Data = this.ReportingService.GetAllGruposEgresosTree(range.Start, range.End), Success = true });
            }
            else
            {
                IList<GrupoEgreso> gruposEgresos = this.ReportingService.GetAllGruposEgresosTree(range.Start, range.End);
                PagingResponse<dynamic> resp = new PagingResponse<dynamic>();
                resp.Page = paging.page;
                resp.Records = gruposEgresos.Select(x => new
                {
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    DescripcionPadre = (x.NodoPadre != null ? x.NodoPadre.Descripcion : null),
                    DescripcionRubro = (x.Rubro != null ? x.Rubro.Descripcion : null),
                    level = x.Level,
                    parent = (x.NodoPadre != null ? x.NodoPadre.Id : 0),
                    isLeaf = (x.Rubro != null ? true : false),
                    loaded = true,
                    expanded = x.Rubro == null,
                    codigo = x.Raiz + "" + x.Level 
                }).OrderBy(x => x.codigo).ToList<dynamic>();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetGruposEgresos(SelectComboRequest req)
        {
            return Json(new { Data = this.ReportingService.GetAllGruposEgresosNoHoja(req), Success = true });
        }


        [HttpPost]
        public JsonResult Eliminar(int Id)
        {
            try
            {
                this.ReportingService.DeleteGrupoEgreso(Id);
                return Json(new { Success = true });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }
    }
}
