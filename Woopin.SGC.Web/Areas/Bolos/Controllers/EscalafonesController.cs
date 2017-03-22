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
    public class EscalafonesController : BaseController
    {
        private readonly IBolosConfigService BolosConfigService;
        private readonly ICommonConfigService commonConfigService;

        public EscalafonesController(IBolosConfigService BolosConfigService, ICommonConfigService commonConfigService)
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
            //List<SelectListItem> Categorias = this.commonConfigService.GetItemsByCombo(ComboType.CategoriasEscalafones).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            //Categorias.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una categoria" }));
            //ViewBag.Categorias = Categorias;

            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Escalafon Escalafon)
        {
            try
            {
                ClearNotValidatedProperties(Escalafon);
                if (ModelState.IsValid)
                {

                    //if (Escalafon.Categoria.Id == 0)
                    //{
                    //    Escalafon.Categoria = null;
                    //}
                    //if (Escalafon.Tarea.Id == 0)
                    //{
                    //    Escalafon.Tarea = null;
                    //}
                    this.BolosConfigService.AddEscalafon(Escalafon);
                    return Json(new { Success = true, Escalafon = Escalafon });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Escalafon", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.BolosConfigService.DeleteEscalafones(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Escalafon, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Escalafon Escalafon = this.BolosConfigService.GetEscalafon(Id);
            
            //List<SelectListItem> Categorias = this.commonConfigService.GetItemsByCombo(ComboType.CategoriasEscalafones).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            //Categorias.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una categoria" }));
            //ViewBag.Categorias = Categorias;
            //List<SelectListItem> Tareas = this.commonConfigService.GetItemsByCombo(ComboType.TareasEscalafones).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            //Tareas.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una tarea" }));
            //ViewBag.Tareas = Tareas;

            return View(Escalafon);
        }


        [HttpPost]
        public JsonResult Editar(Escalafon Escalafon)
        {
            try
            {
                ClearNotValidatedProperties(Escalafon);
                if (ModelState.IsValid)
                {

                    //if (Escalafon.Localizacion.Id == 0)
                    //{
                    //    Escalafon.Localizacion = null;
                    //}
                    //if (Escalafon.Nacionalidad.Id == 0)
                    //{
                    //    Escalafon.Nacionalidad = null;
                    //}
                    //if (Escalafon.EstadoCivil.Id == 0)
                    //{
                    //    Escalafon.EstadoCivil = null;
                    //}
                    //if (Escalafon.Sindicato.Id == 0)
                    //{
                    //    Escalafon.Sindicato = null;
                    //}
                    //if (Escalafon.ObraSocial.Id == 0)
                    //{
                    //    Escalafon.ObraSocial = null;
                    //}
                    //if (Escalafon.BancoDeposito.Id == 0)
                    //{
                    //    Escalafon.BancoDeposito = null;
                    //}
                    //if (Escalafon.Sexo.Id == 0)
                    //{
                    //    Escalafon.Sexo = null;
                    //}
                    //if (Escalafon.Categoria.Id == 0)
                    //{
                    //    Escalafon.Categoria = null;
                    //}
                    //if (Escalafon.Tarea.Id == 0)
                    //{
                    //    Escalafon.Tarea = null;
                    //}
                    this.BolosConfigService.UpdateEscalafon(Escalafon);
                    return Json(new { Success = true, Escalafon = Escalafon });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Escalafon", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                return Json(new { Data = this.BolosConfigService.GetAllEscalafones(), Success = true });
            }
            else
            {
                PagingResponse<Escalafon> resp = new PagingResponse<Escalafon>();
                resp.Page = paging.page;
                resp.Records = this.BolosConfigService.GetAllEscalafones();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        //[HttpPost]
        //public JsonResult GetEscalafones(SelectComboRequest req)
        //{
        //    return Json(new { Data = this.BolosConfigService.GetAllEscalafonesByFilterCombo(req), Success = true });
        //}


        [HttpPost]
        public JsonResult GetEscalafon(int idEscalafon)
        {
            Escalafon a = this.BolosConfigService.GetEscalafon(idEscalafon);
            //Escalafon a = this.BolosConfigService.GetEscalafonCompleto(idEscalafon);
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
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/Escalafones", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    //BackgroundJob.Enqueue<BolosJobs>(x => x.ImportarEscalafones(absolutePath, Security.GetJobHeader()));
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
            DescargarArchivo("App_Data/Importaciones/Templates/Escalafones.xlsx");
        }

    }
}
