using Hangfire;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Sueldos;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.Scheduler;

namespace Woopin.SGC.Web.Areas.Sueldos.Controllers
{
    public class RecibosController : BaseController
    {
        private readonly ISueldosService SueldosService;
        private readonly ICommonConfigService commonConfigService;

        public RecibosController(ISueldosService SueldosService, ICommonConfigService commonConfigService)
        {
            this.SueldosService = SueldosService; 
            this.commonConfigService = commonConfigService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            //ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            //ViewBag.Nacionalidades = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.NumeroRef = this.SueldosService.GetProximoNumeroReferencia();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Recibo Recibo)
        {
            try
            {
                ClearNotValidatedProperties(Recibo);
                if (ModelState.IsValid)
                {
                    this.SueldosService.AddRecibo(Recibo);
                    return Json(new { Success = true, Recibo = Recibo });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Recibo", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        //[HttpPost]
        //public JsonResult Eliminar(List<int> Ids)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            this.SueldosService.DeleteRecibos(Ids);
        //            return Json(new { Success = true });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Recibo, vuelva a inetntarlo." });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }
        //}


        //public ActionResult Editar(int Id)
        //{
        //    Recibo Recibo = this.SueldosService.GetRecibo(Id);
        //    ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
        //    ViewBag.Nacionalidades = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    ViewBag.Categorias = this.commonConfigService.GetItemsByCombo(ComboType.CategoriasRecibos).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    ViewBag.EstadosCivil = this.commonConfigService.GetItemsByCombo(ComboType.EstadoCivil).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    ViewBag.Sexos = this.commonConfigService.GetItemsByCombo(ComboType.Sexo).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    ViewBag.Tareas = this.commonConfigService.GetItemsByCombo(ComboType.TareasRecibos).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    return View(Recibo);
        //}


        //[HttpPost]
        //public JsonResult Editar(Recibo Recibo)
        //{
        //    try
        //    {
        //        ClearNotValidatedProperties(Recibo);
        //        if (ModelState.IsValid)
        //        {
        //            this.SueldosService.UpdateRecibo(Recibo);
        //            return Json(new { Success = true, Recibo = Recibo });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Algunos de los campos no se encuentran correctamente introducidos.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }
        //}

        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.SueldosService.GetAllRecibos(), Success = true });
            }
            else
            {
                PagingResponse<Recibo> resp = new PagingResponse<Recibo>();
                resp.Page = paging.page;
                resp.Records = this.SueldosService.GetAllRecibos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetRecibos(SelectComboRequest req)
        {
            return Json(new { Data = this.SueldosService.GetAllRecibosByFilterCombo(req), Success = true });
        }

        [HttpPost]
        public JsonResult GetRecibo(int idRecibo)
        {
            Recibo a = this.SueldosService.GetRecibo(idRecibo);
            return Json(new { Data = (a != null) ? a : null, Success = true });
        }

        public ActionResult SueldoBruto(decimal? SueldoCategoria, decimal? SueldoMes, decimal? SueldoHora)
        {
            ViewBag.SueldoCategoria = SueldoCategoria;
            ViewBag.SueldoMes = SueldoMes;
            ViewBag.SueldoHora = SueldoHora;
            return View();
        }

    }
}
