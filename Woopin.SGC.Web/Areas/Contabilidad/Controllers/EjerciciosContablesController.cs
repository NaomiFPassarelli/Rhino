using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Contabilidad.Controllers
{
    public class EjerciciosContablesController : BaseController
    {
        private readonly IContabilidadConfigService contabilidadConfigService;
        private readonly IContabilidadService contabilidadService;
        public EjerciciosContablesController(IContabilidadConfigService contabilidadConfigService, IContabilidadService contabilidadService)
        {
            this.contabilidadConfigService = contabilidadConfigService;
            this.contabilidadService = contabilidadService;
        }

        //
        // GET: /Contabilidad/EjerciciosContables/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Ejercicio ejercicio)
        {
            try
            {
                ClearNotValidatedProperties(ejercicio);
                if (ModelState.IsValid)
                {
                    this.contabilidadConfigService.AddEjercicio(ejercicio);
                    return Json(new { Success = true, Ejercicio = ejercicio });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Ejercicio Contable", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch(ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        [HttpPost]
        public JsonResult Eliminar(int Ids)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.contabilidadConfigService.DeleteEjercicio(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Ejercicio Contable, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        public ActionResult Editar(int Id)
        {
            Ejercicio Ejercicio = this.contabilidadConfigService.GetEjercicio(Id);
            return View(Ejercicio);
        }


        [HttpPost]
        public JsonResult Editar(Ejercicio Ejercicio)
        {
            try
            {
                ClearNotValidatedProperties(Ejercicio);
                if (ModelState.IsValid)
                {
                    this.contabilidadConfigService.UpdateEjercicio(Ejercicio);
                    return Json(new { Success = true, Ejercicio = Ejercicio });
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

        public ActionResult Detalle(int Id)
        {
            Ejercicio e = this.contabilidadConfigService.GetEjercicioCompleto(Id);
            if (e == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            return View(e);
        }


        public ActionResult NuevoBloqueo(int Id)
        {
            ViewBag.IdEjercicio = Id;
            return View();
        }

        [HttpPost]
        public JsonResult NuevoBloqueo(BloqueoContable BloqueoContable)
        {
            try
            {
                ClearNotValidatedProperties(BloqueoContable);
                if (ModelState.IsValid)
                {
                    this.contabilidadConfigService.AddBloqueoContable(BloqueoContable);
                    return Json(new { Success = true, BloqueoContable = BloqueoContable });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Bloqueo Contable", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        [HttpPost]
        public JsonResult CambiarBloqueo(int Id, bool Bloqueado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.contabilidadConfigService.UpdateBloqueoContable(Id, !Bloqueado);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el cambio de estado del bloqueo contable, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        [HttpPost]
        public JsonResult CambiarCerrado(int Id, bool Cerrado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.contabilidadConfigService.EjercicioCambiarCerrado(Id, !Cerrado);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el cambio de estado del ejercicio contable, vuelva a inetntarlo." });
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
                return Json(new { Data = this.contabilidadConfigService.GetAllEjercicios(), Success = true });
            }
            else
            {
                PagingResponse<Ejercicio> resp = new PagingResponse<Ejercicio>();
                resp.Page = paging.page;
                resp.Records = this.contabilidadConfigService.GetAllEjercicios();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
    }
}
