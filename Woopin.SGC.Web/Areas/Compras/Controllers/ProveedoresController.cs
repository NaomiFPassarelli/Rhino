using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Exceptions;
using Hangfire;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Web.Scheduler;
using System.IO;

namespace Woopin.SGC.Web.Areas.Compras.Controllers
{
    public class ProveedoresController : BaseController
    {
        private readonly IComprasConfigService ComprasConfigService;
        private readonly ICommonConfigService commonConfigService;

        public ProveedoresController(IComprasConfigService ComprasConfigService, ICommonConfigService commonConfigService)
        {
            this.ComprasConfigService = ComprasConfigService;
            this.commonConfigService = commonConfigService;
        }

        //
        // GET: /Compras/Proveedores/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.CategoriasIva = this.commonConfigService.GetAllCategoriaIVAs().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Abreviatura + " (" + x.Nombre + ")", Selected = x.Predeterminado }).ToList();
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.Localidades = this.commonConfigService.GetAllLocalidades().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.Condiciones = this.commonConfigService.GetItemsByCombo(ComboType.CondicionCompraVenta).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Paises = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Proveedor Proveedor)
        {
            try
            {
                ClearNotValidatedProperties(Proveedor);
                if (ModelState.IsValid)
                {
                    this.ComprasConfigService.AddProveedor(Proveedor);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Proveedor", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.ComprasConfigService.DeleteProveedores(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Proveedor, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Proveedor Proveedor = this.ComprasConfigService.GetProveedor(Id);
            ViewBag.CategoriasIva = this.commonConfigService.GetAllCategoriaIVAs().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Abreviatura + " (" + x.Nombre + ")", Selected = x.Predeterminado }).ToList();
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.Localidades = this.commonConfigService.GetAllLocalidades().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.Condiciones = this.commonConfigService.GetItemsByCombo(ComboType.CondicionCompraVenta).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Paises = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View(Proveedor);
        }


        [HttpPost]
        public JsonResult Editar(Proveedor Proveedor)
        {
            try
            {
                ClearNotValidatedProperties(Proveedor);
                if (ModelState.IsValid)
                {
                    this.ComprasConfigService.UpdateProveedor(Proveedor);
                    return Json(new { Success = true, Proveedor = Proveedor });
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
        public JsonResult GetCombo(PagingRequest paging)
        {
            return Json(new { Data = this.ComprasConfigService.GetProveedorCombos(), Success = true });
        }

        [HttpPost]
        [OutputCache(CacheProfile="SmallOrgCache")]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.ComprasConfigService.GetAllProveedores(), Success = true });
            }
            else
            {
                PagingResponse<Proveedor> resp = new PagingResponse<Proveedor>();
                resp.Page = paging.page;
                resp.Records = this.ComprasConfigService.GetAllProveedores();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetProveedoresCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.ComprasConfigService.GetAllProveedoresByFilterCombo(req), Success = true });
        }

        [HttpPost]
        [OutputCache(CacheProfile="SmallOrgCache")]
        public JsonResult GetProveedor(int IdProveedor)
        {
            Proveedor p = this.ComprasConfigService.GetProveedor(IdProveedor);
            return Json(new { Data = (p != null && p.Activo) ? p : null, Success = true });
        }


        #region Importacion de Proveedores
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
                    string filename = Security.GetOrganizacion().RazonSocial.Replace(" ","") + "_" + DateTime.Now.ToString("ddMMyyyy-HHm") + Path.GetExtension(archivo.FileName);
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/Proveedores", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    BackgroundJob.Enqueue<ComprasJobs>(x => x.ImportarProveedores(absolutePath, Security.GetJobHeader()));
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
            DescargarArchivo("App_Data/Importaciones/Templates/Proveedores.xlsx");
        }
        #endregion

    }
}
