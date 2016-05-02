using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Tesoreria.Controllers
{
    public class BancosController : BaseController
    {
        private readonly ITesoreriaConfigService TesoreriaConfigService;
        private readonly ICommonConfigService commonConfigService;

        public BancosController(ITesoreriaConfigService TesoreriaConfigService, ICommonConfigService commonConfigService)
        {
            this.TesoreriaConfigService = TesoreriaConfigService;
            this.commonConfigService = commonConfigService;
        }

        //
        // GET: /Tesoreria/Bancos/

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
        public JsonResult Nuevo(Banco Banco)
        {
            try
            {
                ClearNotValidatedProperties(Banco);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.AddBanco(Banco);
                    Banco.Lugar = this.commonConfigService.GetLocalizacion(Banco.Lugar.Id);
                    return Json(new { Success = true, Banco = Banco });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Banco", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.TesoreriaConfigService.DeleteBancos(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Banco, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Banco Banco = this.TesoreriaConfigService.GetBanco(Id);
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            return View(Banco);
        }


        [HttpPost]
        public JsonResult Editar(Banco Banco)
        {
            try
            {
                ClearNotValidatedProperties(Banco);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.UpdateBanco(Banco);
                    Banco.Lugar = this.commonConfigService.GetLocalizacion(Banco.Lugar.Id);
                    return Json(new { Success = true, Banco = Banco });
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
                return Json(new { Data = this.TesoreriaConfigService.GetAllBancos(), Success = true });
            }
            else
            {
                PagingResponse<Banco> resp = new PagingResponse<Banco>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaConfigService.GetAllBancos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

       


    }
}
