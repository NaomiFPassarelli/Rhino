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
using Woopin.SGC.Model.Sueldos;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.Scheduler;

namespace Woopin.SGC.Web.Areas.Sueldos.Controllers
{
    public class EmpleadosController : BaseController
    {
        private readonly ISueldosConfigService SueldosConfigService;
        private readonly ICommonConfigService commonConfigService;

        public EmpleadosController(ISueldosConfigService SueldosConfigService, ICommonConfigService commonConfigService)
        {
            this.SueldosConfigService = SueldosConfigService; 
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
            Localizaciones.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una localizacion" }));
            ViewBag.Localizaciones = Localizaciones;
            
            List<SelectListItem> Nacionalidades = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data, Selected = (x.Id == 40 ? true : false) }).ToList();
            Nacionalidades.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una nacionalidad" }));
            ViewBag.Nacionalidades = Nacionalidades;

            List<SelectListItem> Categorias = this.commonConfigService.GetItemsByCombo(ComboType.CategoriasEmpleados).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Categorias.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una categoria" }));
            ViewBag.Categorias = Categorias;

            List<SelectListItem> Sindicatos = this.commonConfigService.GetItemsByCombo(ComboType.Sindicato).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Sindicatos.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione un sindicato" }));
            ViewBag.Sindicatos = Sindicatos;
            
            List<SelectListItem> ObrasSociales = this.commonConfigService.GetItemsByCombo(ComboType.ObraSocial).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ObrasSociales.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una obra social" }));
            ViewBag.ObrasSociales = ObrasSociales;
            
            List<SelectListItem> EstadosCivil = this.commonConfigService.GetItemsByCombo(ComboType.EstadoCivil).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            EstadosCivil.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione un estado civil" }));
            ViewBag.EstadosCivil = EstadosCivil;
            
            List<SelectListItem> Sexos = this.commonConfigService.GetItemsByCombo(ComboType.Sexo).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Sexos.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione un genero" }));
            ViewBag.Sexos = Sexos;
            
            List<SelectListItem> Tareas = this.commonConfigService.GetItemsByCombo(ComboType.TareasEmpleados).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Tareas.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una tarea" }));
            ViewBag.Tareas = Tareas;
            
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Empleado Empleado)
        {
            try
            {
                ClearNotValidatedProperties(Empleado);
                if (ModelState.IsValid)
                {

                    if (Empleado.Localizacion.Id == 0)
                    {
                        Empleado.Localizacion = null;
                    }
                    if (Empleado.Nacionalidad.Id == 0)
                    {
                        Empleado.Nacionalidad = null;
                    }
                    if (Empleado.EstadoCivil.Id == 0)
                    {
                        Empleado.EstadoCivil = null;
                    }
                    if (Empleado.Sindicato.Id == 0)
                    {
                        Empleado.Sindicato = null;
                    }
                    if (Empleado.ObraSocial.Id == 0)
                    {
                        Empleado.ObraSocial = null;
                    }
                    if (Empleado.Sexo.Id == 0)
                    {
                        Empleado.Sexo = null;
                    }
                    if (Empleado.Categoria.Id == 0)
                    {
                        Empleado.Categoria = null;
                    }
                    if (Empleado.Tarea.Id == 0)
                    {
                        Empleado.Tarea = null;
                    }
                    this.SueldosConfigService.AddEmpleado(Empleado);
                    return Json(new { Success = true, Empleado = Empleado });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Empleado", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.SueldosConfigService.DeleteEmpleados(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Empleado, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Empleado Empleado = this.SueldosConfigService.GetEmpleado(Id);
            List<SelectListItem> Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            Localizaciones.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una localizacion" }));
            ViewBag.Localizaciones = Localizaciones;

            List<SelectListItem> Nacionalidades = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Nacionalidades.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una nacionalidad" }));
            ViewBag.Nacionalidades = Nacionalidades;

            List<SelectListItem> Categorias = this.commonConfigService.GetItemsByCombo(ComboType.CategoriasEmpleados).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Categorias.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una categoria" }));
            ViewBag.Categorias = Categorias;

            List<SelectListItem> Sindicatos = this.commonConfigService.GetItemsByCombo(ComboType.Sindicato).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Sindicatos.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione un sindicato" }));
            ViewBag.Sindicatos = Sindicatos;

            List<SelectListItem> ObrasSociales = this.commonConfigService.GetItemsByCombo(ComboType.ObraSocial).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ObrasSociales.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una obra social" }));
            ViewBag.ObrasSociales = ObrasSociales;

            List<SelectListItem> EstadosCivil = this.commonConfigService.GetItemsByCombo(ComboType.EstadoCivil).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            EstadosCivil.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione un estado civil" }));
            ViewBag.EstadosCivil = EstadosCivil;

            List<SelectListItem> Sexos = this.commonConfigService.GetItemsByCombo(ComboType.Sexo).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Sexos.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione un genero" }));
            ViewBag.Sexos = Sexos;

            List<SelectListItem> Tareas = this.commonConfigService.GetItemsByCombo(ComboType.TareasEmpleados).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            Tareas.Insert(0, (new SelectListItem() { Value = "0", Text = "Seleccione una tarea" }));
            ViewBag.Tareas = Tareas;

            ViewBag.FechaNacimiento = Empleado.FechaNacimiento;
            return View(Empleado);
        }


        [HttpPost]
        public JsonResult Editar(Empleado Empleado)
        {
            try
            {
                ClearNotValidatedProperties(Empleado);
                if (ModelState.IsValid)
                {

                    if (Empleado.Localizacion.Id == 0)
                    {
                        Empleado.Localizacion = null;
                    }
                    if (Empleado.Nacionalidad.Id == 0)
                    {
                        Empleado.Nacionalidad = null;
                    }
                    if (Empleado.EstadoCivil.Id == 0)
                    {
                        Empleado.EstadoCivil = null;
                    }
                    if (Empleado.Sindicato.Id == 0)
                    {
                        Empleado.Sindicato = null;
                    }
                    if (Empleado.ObraSocial.Id == 0)
                    {
                        Empleado.ObraSocial = null;
                    }
                    if (Empleado.Sexo.Id == 0)
                    {
                        Empleado.Sexo = null;
                    }
                    if (Empleado.Categoria.Id == 0)
                    {
                        Empleado.Categoria = null;
                    }
                    if (Empleado.Tarea.Id == 0)
                    {
                        Empleado.Tarea = null;
                    }
                    this.SueldosConfigService.UpdateEmpleado(Empleado);
                    return Json(new { Success = true, Empleado = Empleado });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Empleado", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                return Json(new { Data = this.SueldosConfigService.GetAllEmpleados(), Success = true });
            }
            else
            {
                PagingResponse<Empleado> resp = new PagingResponse<Empleado>();
                resp.Page = paging.page;
                resp.Records = this.SueldosConfigService.GetAllEmpleados();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetEmpleados(SelectComboRequest req)
        {
            return Json(new { Data = this.SueldosConfigService.GetAllEmpleadosByFilterCombo(req), Success = true });
        }


        [HttpPost]
        public JsonResult GetEmpleado(int idEmpleado)
        {
            Empleado a = this.SueldosConfigService.GetEmpleado(idEmpleado);
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
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/Empleados", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    BackgroundJob.Enqueue<SueldosJobs>(x => x.ImportarEmpleados(absolutePath, Security.GetJobHeader()));
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
            DescargarArchivo("App_Data/Importaciones/Templates/Empleados.xlsx");
        }

    }
}
