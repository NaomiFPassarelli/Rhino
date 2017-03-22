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
    public class TrabajadoresController : BaseController
    {
        private readonly IBolosConfigService BolosConfigService;
        private readonly ICommonConfigService commonConfigService;

        public TrabajadoresController(IBolosConfigService BolosConfigService, ICommonConfigService commonConfigService)
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
            ViewBag.NumeroRef = this.BolosConfigService.GetProximoNumeroReferencia();

            List<SelectListItem> Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            Localizaciones.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una localizacion" }));
            ViewBag.Localizaciones = Localizaciones;

            List<SelectListItem> EstadosCivil = this.commonConfigService.GetItemsByCombo(ComboType.EstadoCivil).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.EstadosCivil = EstadosCivil;

            List<SelectListItem> Sindicatos = this.commonConfigService.GetItemsByComboOrganizacion(ComboOrganizacionType.Sindicato).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Sindicatos = Sindicatos;

            List<SelectListItem> Escalafones = this.BolosConfigService.GetAllEscalafones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Descripcion }).ToList();
            ViewBag.Escalafones = Escalafones;
            
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Trabajador Trabajador)
        {
            try
            {
                ClearNotValidatedProperties(Trabajador);
                if (ModelState.IsValid)
                {

                    if (Trabajador.Localizacion.Id == 0)
                    {
                        Trabajador.Localizacion = null;
                    }
                    //if (Trabajador.Nacionalidad.Id == 0)
                    //{
                    //    Trabajador.Nacionalidad = null;
                    //}
                    if (Trabajador.EstadoCivil.Id == 0)
                    {
                        Trabajador.EstadoCivil = null;
                    }
                    if (Trabajador.Sindicato.Id == 0)
                    {
                        Trabajador.Sindicato = null;
                    }
                    //if (Trabajador.ObraSocial.Id == 0)
                    //{
                    //    Trabajador.ObraSocial = null;
                    //}
                    //if (Trabajador.BancoDeposito.Id == 0)
                    //{
                    //    Trabajador.BancoDeposito = null;
                    //}
                    //if (Trabajador.Sexo.Id == 0)
                    //{
                    //    Trabajador.Sexo = null;
                    //}
                    //if (Trabajador.Categoria.Id == 0)
                    //{
                    //    Trabajador.Categoria = null;
                    //}
                    //if (Trabajador.Tarea.Id == 0)
                    //{
                    //    Trabajador.Tarea = null;
                    //}
                    int Numero = Trabajador.NumeroReferencia;
                    this.BolosConfigService.AddTrabajador(Trabajador);
                    if (Numero != Trabajador.NumeroReferencia)
                    {
                        return Json(new { Success = true, NumeroRef = Trabajador.NumeroReferencia, Trabajador = Trabajador });
                    }
                    return Json(new { Success = true, Trabajador = Trabajador });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Trabajador", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.BolosConfigService.DeleteTrabajadores(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Trabajador, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Trabajador Trabajador = this.BolosConfigService.GetTrabajador(Id);

            List<SelectListItem> Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            Localizaciones.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una localizacion" }));
            ViewBag.Localizaciones = Localizaciones;

            List<SelectListItem> EstadosCivil = this.commonConfigService.GetItemsByCombo(ComboType.EstadoCivil).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.EstadosCivil = EstadosCivil;

            List<SelectListItem> Sindicatos = this.commonConfigService.GetItemsByComboOrganizacion(ComboOrganizacionType.Sindicato).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Sindicatos = Sindicatos;

            List<SelectListItem> Escalafones = this.BolosConfigService.GetAllEscalafones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Descripcion }).ToList();
            ViewBag.Escalafones = Escalafones;
            
            ViewBag.FechaNacimiento = Trabajador.FechaNacimiento.ToString();
            ViewBag.NumeroRef = Trabajador.NumeroReferencia;
            return View(Trabajador);
        }


        [HttpPost]
        public JsonResult Editar(Trabajador Trabajador)
        {
            try
            {
                ClearNotValidatedProperties(Trabajador);
                if (ModelState.IsValid)
                {

                    if (Trabajador.Localizacion.Id == 0)
                    {
                        Trabajador.Localizacion = null;
                    }
                    //if (Trabajador.Nacionalidad.Id == 0)
                    //{
                    //    Trabajador.Nacionalidad = null;
                    //}
                    if (Trabajador.EstadoCivil.Id == 0)
                    {
                        Trabajador.EstadoCivil = null;
                    }
                    if (Trabajador.Sindicato.Id == 0)
                    {
                        Trabajador.Sindicato = null;
                    }
                    //if (Trabajador.ObraSocial.Id == 0)
                    //{
                    //    Trabajador.ObraSocial = null;
                    //}
                    //if (Trabajador.BancoDeposito.Id == 0)
                    //{
                    //    Trabajador.BancoDeposito = null;
                    //}
                    //if (Trabajador.Sexo.Id == 0)
                    //{
                    //    Trabajador.Sexo = null;
                    //}
                    //if (Trabajador.Categoria.Id == 0)
                    //{
                    //    Trabajador.Categoria = null;
                    //}
                    //if (Trabajador.Tarea.Id == 0)
                    //{
                    //    Trabajador.Tarea = null;
                    //}
                    this.BolosConfigService.UpdateTrabajador(Trabajador);
                    return Json(new { Success = true, Trabajador = Trabajador });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Trabajador", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
        public JsonResult GetTrabajadoresCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.BolosConfigService.GetAllTrabajadoresByFilterCombo(req), Success = true });
        }


        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.BolosConfigService.GetAllTrabajadores(), Success = true });
            }
            else
            {
                PagingResponse<Trabajador> resp = new PagingResponse<Trabajador>();
                resp.Page = paging.page;
                resp.Records = this.BolosConfigService.GetAllTrabajadores();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetTrabajadores(SelectComboRequest req)
        {
            return Json(new { Data = this.BolosConfigService.GetAllTrabajadoresByFilterCombo(req), Success = true });
        }


        [HttpPost]
        public JsonResult GetTrabajador(int idTrabajador)
        {
            //Trabajador a = this.BolosConfigService.GetTrabajadorCompleto(idTrabajador);
            Trabajador a = this.BolosConfigService.GetTrabajador(idTrabajador);
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
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/Trabajadores", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    //BackgroundJob.Enqueue<BolosJobs>(x => x.ImportarTrabajadores(absolutePath, Security.GetJobHeader()));
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
            DescargarArchivo("App_Data/Importaciones/Templates/Trabajadores.xlsx");
        }

    }
}
