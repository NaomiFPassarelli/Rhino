using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.Filters;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Web.Areas.Contabilidad.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class RetencionesController : BaseController
    {
        private readonly IContabilidadConfigService ContabilidadConfigService;
        private readonly ICommonConfigService commonConfigService;
        private readonly ITesoreriaService TesoreriaService;
        public RetencionesController(IContabilidadConfigService ContabilidadConfigService, ICommonConfigService commonConfigService, ITesoreriaService TesoreriaService)
        {
            this.ContabilidadConfigService = ContabilidadConfigService;
            this.commonConfigService = commonConfigService;
            this.TesoreriaService = TesoreriaService;
        }

        //
        // GET: /Configuracion/Retenciones
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Retencion Retencion)
        {
            try
            {
                ClearNotValidatedProperties(Retencion);
                if (ModelState.IsValid)
                {
                    this.ContabilidadConfigService.AddRetencion(Retencion);
                    Retencion.Juridiccion = this.commonConfigService.GetLocalizacion(Retencion.Juridiccion.Id);
                    return Json(new { Success = true, Retencion = Retencion });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Retencion", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.ContabilidadConfigService.DeleteRetenciones(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar retencion, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Retencion Retencion = this.ContabilidadConfigService.GetRetencion(Id);
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            return View(Retencion);
        }


        [HttpPost]
        public JsonResult Editar(Retencion Retencion)
        {
            try
            {
                ClearNotValidatedProperties(Retencion);
                if (ModelState.IsValid)
                {
                    this.ContabilidadConfigService.UpdateRetencion(Retencion);
                    Retencion.Juridiccion = this.commonConfigService.GetLocalizacion(Retencion.Juridiccion.Id);
                    return Json(new { Success = true, Retencion = Retencion });
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
        public JsonResult GetAll(PagingRequest paging )
        {
            if(paging.page == 0)
            {
                return Json(new { Data = this.ContabilidadConfigService.GetAllRetenciones(), Success = true });
            }
            else
            {
                PagingResponse<Retencion> resp = new PagingResponse<Retencion>();
                resp.Page = paging.page;
                resp.Records = this.ContabilidadConfigService.GetAllRetenciones();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double) resp.Records.Count/paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetRetencionesCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.ContabilidadConfigService.GetRetencionCombos(req), Success = true });
        }

        [HttpPost]
        public JsonResult GetRetencion(int Id)
        {
            return Json(new { Data = this.ContabilidadConfigService.GetRetencion(Id), Success = true });
        }

    }
}
