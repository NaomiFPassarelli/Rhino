using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Ventas.Controllers
{
    public class GrupoEconomicoController : BaseController
    {
        private readonly IVentasConfigService ventasConfigService;
        public GrupoEconomicoController(IVentasConfigService ventasConfigService)
        {
            this.ventasConfigService = ventasConfigService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(GrupoEconomico GrupoEconomico)
        {
            try
            {
                ClearNotValidatedProperties(GrupoEconomico);
                if (ModelState.IsValid)
                {
                    this.ventasConfigService.AddGrupoEconomico(GrupoEconomico);
                    return Json(new { Success = true, GrupoEconomico = GrupoEconomico });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Grupo Economico", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        [HttpPost]
        public JsonResult Eliminar(List<int> Ids)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.ventasConfigService.DeleteGrupoEconomico(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la GrupoEconomico, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        public ActionResult Editar(int Id)
        {
            GrupoEconomico GrupoEconomico = this.ventasConfigService.GetGrupoEconomico(Id);
            return View(GrupoEconomico);
        }


        [HttpPost]
        public JsonResult Editar(GrupoEconomico GrupoEconomico)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.ventasConfigService.UpdateGrupoEconomico(GrupoEconomico);
                    return Json(new { Success = true, GrupoEconomico = GrupoEconomico });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Algunos de los campos no se encuentran correctamente introducidos.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
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
                return Json(new { Data = this.ventasConfigService.GetAllGrupoEconomicos(), Success = true });
            }
            else
            {
                PagingResponse<GrupoEconomico> resp = new PagingResponse<GrupoEconomico>();
                resp.Page = paging.page;
                resp.Records = this.ventasConfigService.GetAllGrupoEconomicos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetCombo()
        {
            return Json(new { Data = this.ventasConfigService.GetGrupoEconomicoCombos(), Success = true });
        }

        [HttpPost]
        public JsonResult GetGruposCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.ventasConfigService.GetAllGruposByFilterCombo(req), Success = true });
        }

        [HttpPost]
        [OutputCache(CacheProfile="SmallOrgCache")]
        public JsonResult GetGrupo(int Id)
        {
            GrupoEconomico g = this.ventasConfigService.GetGrupoEconomico(Id);
            return Json(new { Data = g.Activo ? g : null, Success = true });
        }
    }
}
