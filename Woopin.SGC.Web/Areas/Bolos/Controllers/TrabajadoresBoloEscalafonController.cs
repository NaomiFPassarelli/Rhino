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
    public class TrabajadoresBoloEscalafonController : BaseController
    {
        private readonly IBolosConfigService BolosConfigService;
        private readonly ICommonConfigService commonConfigService;

        public TrabajadoresBoloEscalafonController(IBolosConfigService BolosConfigService, ICommonConfigService commonConfigService)
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
            List<SelectListItem> Escalafones = this.BolosConfigService.GetAllEscalafones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Descripcion }).ToList();
            Escalafones.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una Escalafon" }));
            ViewBag.Escalafones = Escalafones;

            List<SelectListItem> Bolos = this.BolosConfigService.GetAllBolos().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.DenominacionPelicula + x.DenominacionProducto + x.Nombre }).ToList();
            Bolos.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una Bolo" }));
            ViewBag.Bolos = Bolos;

            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(TrabajadorBoloEscalafon TrabajadorBoloEscalafon)
        {
            try
            {
                ClearNotValidatedProperties(TrabajadorBoloEscalafon);
                if (ModelState.IsValid)
                {

                    //if (TrabajadorBoloEscalafon.Categoria.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.Categoria = null;
                    //}
                    //if (TrabajadorBoloEscalafon.Tarea.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.Tarea = null;
                    //}
                    this.BolosConfigService.AddTrabajadorBoloEscalafon(TrabajadorBoloEscalafon);
                    return Json(new { Success = true, TrabajadorBoloEscalafon = TrabajadorBoloEscalafon });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de TrabajadorBoloEscalafon", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.BolosConfigService.DeleteTrabajadoresBoloEscalafon(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la TrabajadorBoloEscalafon, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            TrabajadorBoloEscalafon TrabajadorBoloEscalafon = this.BolosConfigService.GetTrabajadorBoloEscalafon(Id);

            List<SelectListItem> Escalafones = this.BolosConfigService.GetAllEscalafones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Descripcion }).ToList();
            Escalafones.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una Escalafon" }));
            ViewBag.Escalafones = Escalafones;

            List<SelectListItem> Bolos = this.BolosConfigService.GetAllBolos().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.DenominacionPelicula + x.DenominacionProducto + x.Nombre }).ToList();
            Bolos.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una Bolo" }));
            ViewBag.Bolos = Bolos;

            return View(TrabajadorBoloEscalafon);
        }


        [HttpPost]
        public JsonResult Editar(TrabajadorBoloEscalafon TrabajadorBoloEscalafon)
        {
            try
            {
                ClearNotValidatedProperties(TrabajadorBoloEscalafon);
                if (ModelState.IsValid)
                {

                    //if (TrabajadorBoloEscalafon.Localizacion.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.Localizacion = null;
                    //}
                    //if (TrabajadorBoloEscalafon.Nacionalidad.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.Nacionalidad = null;
                    //}
                    //if (TrabajadorBoloEscalafon.EstadoCivil.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.EstadoCivil = null;
                    //}
                    //if (TrabajadorBoloEscalafon.Sindicato.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.Sindicato = null;
                    //}
                    //if (TrabajadorBoloEscalafon.ObraSocial.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.ObraSocial = null;
                    //}
                    //if (TrabajadorBoloEscalafon.BancoDeposito.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.BancoDeposito = null;
                    //}
                    //if (TrabajadorBoloEscalafon.Sexo.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.Sexo = null;
                    //}
                    //if (TrabajadorBoloEscalafon.Categoria.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.Categoria = null;
                    //}
                    //if (TrabajadorBoloEscalafon.Tarea.Id == 0)
                    //{
                    //    TrabajadorBoloEscalafon.Tarea = null;
                    //}
                    this.BolosConfigService.UpdateTrabajadorBoloEscalafon(TrabajadorBoloEscalafon);
                    return Json(new { Success = true, TrabajadorBoloEscalafon = TrabajadorBoloEscalafon });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de TrabajadorBoloEscalafon", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                return Json(new { Data = this.BolosConfigService.GetAllTrabajadoresBoloEscalafon(), Success = true });
            }
            else
            {
                PagingResponse<TrabajadorBoloEscalafon> resp = new PagingResponse<TrabajadorBoloEscalafon>();
                resp.Page = paging.page;
                resp.Records = this.BolosConfigService.GetAllTrabajadoresBoloEscalafon();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        //[HttpPost]
        //public JsonResult GetTrabajadoresBoloEscalafon(SelectComboRequest req)
        //{
        //    return Json(new { Data = this.BolosConfigService.GetAllTrabajadoresBoloEscalafonByFilterCombo(req), Success = true });
        //}


        [HttpPost]
        public JsonResult GetTrabajadorBoloEscalafon(int idTrabajadorBoloEscalafon)
        {
            TrabajadorBoloEscalafon a = this.BolosConfigService.GetTrabajadorBoloEscalafon(idTrabajadorBoloEscalafon);
            //TrabajadorBoloEscalafon a = this.BolosConfigService.GetTrabajadorBoloEscalafonCompleto(idTrabajadorBoloEscalafon);
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
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/TrabajadoresBoloEscalafon", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    //BackgroundJob.Enqueue<BolosJobs>(x => x.ImportarTrabajadoresBoloEscalafon(absolutePath, Security.GetJobHeader()));
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
            DescargarArchivo("App_Data/Importaciones/Templates/TrabajadoresBoloEscalafon.xlsx");
        }

    }
}
