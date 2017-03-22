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
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Bolos;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.Scheduler;

namespace Woopin.SGC.Web.Areas.Bolos.Controllers
{
    public class BolosController : BaseController
    {
        private readonly IBolosConfigService BolosConfigService;
        private readonly ICommonConfigService commonConfigService;

        public BolosController(IBolosConfigService BolosConfigService, ICommonConfigService commonConfigService)
        {
            this.BolosConfigService = BolosConfigService; 
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
            List<SelectListItem> Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.Localizaciones = Localizaciones;
            
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Bolo Bolo)
        {
            try
            {
                ClearNotValidatedProperties(Bolo);
                if (ModelState.IsValid)
                {

                    if (Bolo.Localizacion.Id == 0)
                    {
                        Bolo.Localizacion = null;
                    }
                    this.BolosConfigService.AddBolo(Bolo);
                    return Json(new { Success = true, Bolo = Bolo });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Bolo", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch (BusinessException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
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
                    this.BolosConfigService.DeleteBolos(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Bolo, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Bolo Bolo = this.BolosConfigService.GetBolo(Id);
            List<SelectListItem> Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            ViewBag.Localizaciones = Localizaciones;

            return View(Bolo);
        }


        [HttpPost]
        public JsonResult Editar(Bolo Bolo)
        {
            try
            {
                ClearNotValidatedProperties(Bolo);
                if (ModelState.IsValid)
                {

                    if (Bolo.Localizacion.Id == 0)
                    {
                        Bolo.Localizacion = null;
                    }
                    this.BolosConfigService.UpdateBolo(Bolo);
                    return Json(new { Success = true, Bolo = Bolo });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de edición de Bolo", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch (BusinessException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
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
                return Json(new { Data = this.BolosConfigService.GetAllBolos(), Success = true });
            }
            else
            {
                PagingResponse<Bolo> resp = new PagingResponse<Bolo>();
                resp.Page = paging.page;
                resp.Records = this.BolosConfigService.GetAllBolos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        //[HttpPost]
        //public JsonResult GetBolos(SelectComboRequest req)
        //{
        //    return Json(new { Data = this.BolosConfigService.GetAllBolosByFilterCombo(req), Success = true });
        //}

        [HttpPost]
        public JsonResult GetBolosCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.BolosConfigService.GetAllBolosByFilterCombo(req), Success = true });
        }


        [HttpPost]
        public JsonResult GetBolo(int idBolo)
        {
            Bolo a = this.BolosConfigService.GetBolo(idBolo);
            //Bolo a = this.BolosConfigService.GetBoloCompleto(idBolo);
            return Json(new { Data = (a != null && a.Activo) ? a : null, Success = true });
        }

        public ActionResult Importar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Importar(HttpPostedFileBase archivo)
        {
            try
            {
                if (archivo != null)
                {
                    string filename = Security.GetOrganizacion().RazonSocial.Replace(" ", "") + "_" + DateTime.Now.ToString("ddMMyyyy-HHm") + Path.GetExtension(archivo.FileName);
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/Bolos", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    //BackgroundJob.Enqueue<BolosJobs>(x => x.ImportarBolos(absolutePath, Security.GetJobHeader()));
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "No se detecto el archivo para importar" });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        public void DescargarTemplate()
        {
            DescargarArchivo("App_Data/Importaciones/Templates/Bolos.xlsx");
        }

    }
}
