using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.Filters;

namespace Woopin.SGC.Web.Areas.Configuracion.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class MonedasController : BaseController
    {
        private readonly ICommonConfigService commonConfigService;
        public MonedasController(ICommonConfigService commonConfigService)
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
            return View();
        }

        [HttpPost]
        public JsonResult Nueva(Moneda moneda)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.commonConfigService.AddMoneda(moneda);
                    return Json(new { Success = true, Moneda = moneda });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de moneda", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.commonConfigService.DeleteMonedas(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar moneda, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Moneda moneda = this.commonConfigService.GetMoneda(Id);
            return View(moneda);
        }


        [HttpPost]
        public JsonResult Editar(Moneda moneda)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.commonConfigService.UpdateMoneda(moneda);
                    return Json(new { Success = true, Moneda = moneda });
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
                return Json(new { Data = this.commonConfigService.GetAllMonedas(), Success = true });
            }
            else
            {
                PagingResponse<Moneda> resp = new PagingResponse<Moneda>();
                resp.Page = paging.page;
                resp.Records = this.commonConfigService.GetAllMonedas();
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
                    this.commonConfigService.SetDefaultMoneda(Id);
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
