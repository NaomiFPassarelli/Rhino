using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Configuracion.Controllers
{
    public class LocalizacionesController : BaseController
    {
        private readonly ICommonConfigService commonConfigService;
        public LocalizacionesController(ICommonConfigService commonConfigService)
        {
            this.commonConfigService = commonConfigService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nueva()
        {
            ViewBag.Paises = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nueva(Localizacion localizacion)
        {
            try
            {
                ClearNotValidatedProperties(localizacion);
                if (ModelState.IsValid)
                {
                    this.commonConfigService.AddLocalizacion(localizacion);
                    localizacion.Pais = this.commonConfigService.GetComboItem(localizacion.Pais.Id);
                    return Json(new { Success = true, Localizacion = localizacion });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de localizacion", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.commonConfigService.DeleteLocalizaciones(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar las localizaciones, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Localizacion localizacion = this.commonConfigService.GetLocalizacion(Id);
            ViewBag.Paises = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View(localizacion);
        }


        [HttpPost]
        public JsonResult Editar(Localizacion localizacion)
        {
            try
            {
                ClearNotValidatedProperties(localizacion);
                if (ModelState.IsValid)
                {
                    Localizacion l = this.commonConfigService.UpdateLocalizacion(localizacion);
                    l.Pais = this.commonConfigService.GetComboItem(localizacion.Pais.Id);
                    return Json(new { Success = true, Localizacion = l });
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
        [OutputCache(CacheProfile="LongOrgCache")]
        public JsonResult GetAll(PagingRequest paging )
        {
            if(paging.page == 0)
            {
                return Json(new { Data = this.commonConfigService.GetAllLocalizaciones(), Success = true });
            }
            else
            {
                PagingResponse<Localizacion> resp = new PagingResponse<Localizacion>();
                resp.Page = paging.page;
                resp.Records = this.commonConfigService.GetAllLocalizaciones();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double) resp.Records.Count/paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }




        [HttpPost]
        public JsonResult SetDefault(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.commonConfigService.SetDefaultLocalizacion(Id);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

    }
}
