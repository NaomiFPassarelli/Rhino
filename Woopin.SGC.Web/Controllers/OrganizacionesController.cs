using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Services;
using System.IO;

namespace Woopin.SGC.Web.Controllers
{
    public class OrganizacionesController : BaseController
    {
        private readonly ICommonConfigService commonConfigService;
        private readonly ISystemService SystemService;

        public OrganizacionesController(SystemService SystemService, ICommonConfigService commonConfigService)
        {
            this.SystemService = SystemService;
            this.commonConfigService = commonConfigService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.Categorias = this.commonConfigService.GetItemsByCombo(ComboType.TipoIVAOrganizacion).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.Actividades = this.commonConfigService.GetItemsByCombo(ComboType.ActividadOrganizacion).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Organizacion Organizacion, HttpPostedFileBase ImagePath)
        {
            try
            {
                ClearNotValidatedProperties(Organizacion);
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string filename = Organizacion.CUIT + Path.GetExtension(ImagePath.FileName);
                        string path = GuardarArchivo(ImagePath, "Images/Organizacion", filename);

                        Organizacion.ImagePath = path;
                    }

                    this.SystemService.AddOrganizacion(Organizacion);
                    return Json(new { Success = true, Organizacion = Organizacion });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Organizacion", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch (BusinessException bs)
            {
                return Json(new { Success = false, ErrorMessage = bs.ErrorMessage });
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
                    this.SystemService.DeleteOrganizaciones(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Organizacion, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Organizacion Organizacion = this.SystemService.GetOrganizacion(Id);
            ViewBag.Categorias = this.commonConfigService.GetItemsByCombo(ComboType.TipoIVAOrganizacion).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.Actividades = this.commonConfigService.GetItemsByCombo(ComboType.ActividadOrganizacion).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View(Organizacion);
        }


        [HttpPost]
        public JsonResult Editar(Organizacion Organizacion, HttpPostedFileBase ImagePath)
        {
            try
            {
                ClearNotValidatedProperties(Organizacion);
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string filename = Organizacion.CUIT + Path.GetExtension(ImagePath.FileName);
                        EliminarArchivo(Organizacion.ImagePath); //elimina el viejo
                        string path = GuardarArchivo(ImagePath, "Images/Organizacion", filename);
                        Organizacion.ImagePath = path;
                    }
                    
                    this.SystemService.UpdateOrganizacion(Organizacion);
                    return Json(new { Success = true, Organizacion = Organizacion });
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
                return Json(new { Data = this.SystemService.GetAllOrganizaciones(), Success = true });
            }
            else
            {
                PagingResponse<Organizacion> resp = new PagingResponse<Organizacion>();
                resp.Page = paging.page;
                resp.Records = this.SystemService.GetAllOrganizaciones();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        [HttpPost]
        public JsonResult GetAllMisOrganizaciones(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.SystemService.GetMisOrganizaciones(), Success = true });
            }
            else
            {
                PagingResponse<Organizacion> resp = new PagingResponse<Organizacion>();
                resp.Page = paging.page;
                resp.Records = this.SystemService.GetMisOrganizaciones();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetOrganizacion(int IdOrganizacion)
        {
            Organizacion p = this.SystemService.GetOrganizacion(IdOrganizacion);
            return Json(new { Data = p.Activo ? p : null, Success = true });
        }

        public ActionResult Administrar(int Id)
        {
            Organizacion org = this.SystemService.GetOrganizacion(Id);
            // TODO - Cambiar administrador por un CanAdministrar
            // Despues todos los que tengan permisos van a poder administrarla.
            if (org.Administrador.Id != SessionDataManager.Get().CurrentUser.Id)
            {
                return RedirectToAction("Forbidden", "Error");
            }
            return View(org);
        }

        [HttpPost]
        public JsonResult RemoverUsuarios(List<int> Ids, int IdOrganizacion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.SystemService.RemoverUsuariosOrganizacion(Ids, IdOrganizacion);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de remover los usuaros, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        [HttpPost]
        public JsonResult AgregarUsuarios(List<int> Ids, int IdOrganizacion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.SystemService.AgregarUsuariosOrganizacion(Ids, IdOrganizacion);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de agregar los usuaros, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        public ActionResult AgregarUsuarios(int IdOrganizacion)
        {
            ViewBag.IdOrganizacion = IdOrganizacion;
            return View();
        }

        [HttpPost]
        public JsonResult GetAllAAgregar(PagingRequest paging, int IdOrganizacion)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.SystemService.GetAllUsuariosMisOrganizaciones(IdOrganizacion), Success = true });
            }
            else
            {
                PagingResponse<Usuario> resp = new PagingResponse<Usuario>();
                resp.Page = paging.page;
                resp.Records = this.SystemService.GetAllUsuariosMisOrganizaciones(IdOrganizacion);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult CambiarActual()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CambiarActual(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.SystemService.SetCurrentOrganizacion(Id);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema al querer actualizar la organización, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

    }
}
