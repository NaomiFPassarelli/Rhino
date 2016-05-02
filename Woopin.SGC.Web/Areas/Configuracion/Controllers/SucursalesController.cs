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
    public class SucursalesController : BaseController
    {
        private readonly ICommonConfigService commonConfigService;
        public SucursalesController(ICommonConfigService commonConfigService)
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
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nueva(Sucursal sucursal)
        {
            try
            {
                ClearNotValidatedProperties(sucursal);
                if (ModelState.IsValid)
                {
                    this.commonConfigService.AddSucursal(sucursal);
                    return Json(new { Success = true, Sucursal = sucursal });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de sucursal", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.commonConfigService.DeleteSucursales(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la sucursal, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Sucursal sucursal = this.commonConfigService.GetSucursal(Id);
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            return View(sucursal);
        }


        [HttpPost]
        public JsonResult Editar(Sucursal sucursal)
        {
            try
            {
                ClearNotValidatedProperties(sucursal);
                if (ModelState.IsValid)
                {
                    this.commonConfigService.UpdateSucursal(sucursal);
                    return Json(new { Success = true, Sucursal = sucursal });
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
                return Json(new { Data = this.commonConfigService.GetAllSucursales(), Success = true });
            }
            else
            {
                PagingResponse<Sucursal> resp = new PagingResponse<Sucursal>();
                resp.Page = paging.page;
                resp.Records = this.commonConfigService.GetAllSucursales();
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
                    this.commonConfigService.SetDefaultSucursal(Id);
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
