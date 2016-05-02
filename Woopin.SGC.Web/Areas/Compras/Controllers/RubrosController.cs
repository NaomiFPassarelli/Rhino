using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Compras.Controllers
{
    public class RubrosController : BaseController
    {
        private readonly IComprasConfigService ComprasConfigService;
        private readonly IContabilidadConfigService contabilidadService;
        public RubrosController(IComprasConfigService ComprasConfigService,IContabilidadConfigService contabilidadService)
        {
            this.ComprasConfigService = ComprasConfigService;
            this.contabilidadService = contabilidadService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.SubRubros = this.contabilidadService.GetSubRubrosEgresosCombo().Select(x => new SelectListItem() { Value = x.Codigo, Text = x.Nombre }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(RubroCompra Rubro)
        {
            try
            {
                ClearNotValidatedProperties(Rubro);
                if (ModelState.IsValid)
                {
                    this.ComprasConfigService.AddRubro(Rubro);
                    return Json(new { Success = true, Rubro = Rubro });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Rubro", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.ComprasConfigService.DeleteRubros(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Rubro, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            RubroCompra Rubro = this.ComprasConfigService.GetRubro(Id);
            return View(Rubro);
        }


        [HttpPost]
        public JsonResult Editar(RubroCompra Rubro)
        {
            try
            {
                ClearNotValidatedProperties(Rubro);
                if (ModelState.IsValid)
                {
                    this.ComprasConfigService.UpdateRubro(Rubro);
                    return Json(new { Success = true, Rubro = Rubro });
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
                return Json(new { Data = this.ComprasConfigService.GetAllRubros(), Success = true });
            }
            else
            {
                PagingResponse<RubroCompra> resp = new PagingResponse<RubroCompra>();
                resp.Page = paging.page;
                resp.Records = this.ComprasConfigService.GetAllRubros();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetRubrosCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.ComprasConfigService.GetAllRubrosByFilterCombo(req), Success = true });
        }

        [HttpPost]
        public JsonResult GetRubrosSinPercepcionesCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.ComprasConfigService.GetAllRubrosSinPerceByFilterCombo(req), Success = true });
        }

        [HttpPost]
        [OutputCache(CacheProfile="SmallOrgCache")]
        public JsonResult GetRubroCompra(int IdRubroCompra)
        {
            RubroCompra r = this.ComprasConfigService.GetRubro(IdRubroCompra);
            return Json(new { Data = (r != null && r.Activo) ? r : null, Success = true });
        }

    }
}
