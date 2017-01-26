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
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.Scheduler;

namespace Woopin.SGC.Web.Areas.Ventas.Controllers
{
    public class ClientesController : BaseController
    {
        private readonly IVentasConfigService ventasConfigService;
        private readonly ICommonConfigService commonConfigService;
        public ClientesController(IVentasConfigService ventasConfigService, ICommonConfigService commonConfigService)
        {
            this.ventasConfigService = ventasConfigService;
            this.commonConfigService = commonConfigService;
        }

        //
        // GET: 
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
        public JsonResult Nuevo(Cliente Cliente)
        {
            try
            {
                ClearNotValidatedProperties(Cliente);
                Cliente.Master = Cliente.Master == null ? null : (Cliente.Master.Id == 0 ? null : Cliente.Master);
                if (ModelState.IsValid)
                {
                    this.ventasConfigService.AddCliente(Cliente);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Cliente", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.ventasConfigService.DeleteClientes(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Cliente, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Cliente Cliente = this.ventasConfigService.GetCliente(Id);
            ViewBag.DireccionesEntrega = Cliente.DireccionesEntrega;
            ViewBag.CategoriasIva = this.commonConfigService.GetAllCategoriaIVAs().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Abreviatura + " (" + x.Nombre + ")", Selected = x.Predeterminado }).ToList();
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.Localidades = this.commonConfigService.GetAllLocalidades().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.Condiciones = this.commonConfigService.GetItemsByCombo(ComboType.CondicionCompraVenta).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Paises = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View(Cliente);
        }
        

        [HttpPost]
        public JsonResult Editar(Cliente Cliente)
        {
            try
            {
                ClearNotValidatedProperties(Cliente);
                Cliente.Master = Cliente.Master == null ? null : Cliente.Master;
                if (ModelState.IsValid)
                {
                    this.ventasConfigService.UpdateCliente(Cliente);
                    return Json(new { Success = true, Cliente = Cliente });
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
            if(paging.page == 0)
            {
                return Json(new { Data = this.ventasConfigService.GetAllClientes(), Success = true });
            }
            else
            {
                PagingResponse<Cliente> resp = new PagingResponse<Cliente>();
                resp.Page = paging.page;
                resp.Records = this.ventasConfigService.GetAllClientes();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double) resp.Records.Count/paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetClientesCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.ventasConfigService.GetAllClientesByFilterCombo(req), Success = true });
        }

        [HttpPost]
        public JsonResult GetCliente(int IdCliente)
        {
            Cliente c = this.ventasConfigService.GetCliente(IdCliente);
            return Json(new { Data = (c != null && c.Activo) ? c : null, Success = true });
        }


        #region Importacion de Clientes
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
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/Clientes", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    BackgroundJob.Enqueue<VentasJobs>(x => x.ImportarClientes(absolutePath, Security.GetJobHeader()));
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
            DescargarArchivo("App_Data/Importaciones/Templates/Clientes.xlsx");
        }
        #endregion
    }
}
