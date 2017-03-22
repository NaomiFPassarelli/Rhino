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
    public class EmpresasController : BaseController
    {
        private readonly IBolosConfigService BolosConfigService;
        private readonly ICommonConfigService commonConfigService;

        public EmpresasController(IBolosConfigService BolosConfigService, ICommonConfigService commonConfigService)
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
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Empresa Empresa)
        {
            try
            {
                ClearNotValidatedProperties(Empresa);
                if (ModelState.IsValid)
                {
                    this.BolosConfigService.AddEmpresa(Empresa);
                    return Json(new { Success = true, Empresa = Empresa });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Empresa", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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

        //[HttpPost]
        //public JsonResult Eliminar(List<int> Ids)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            this.BolosConfigService.DeleteEmpresas(Ids);
        //            return Json(new { Success = true });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Empresa, vuelva a inetntarlo." });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }
        //}


        public ActionResult Editar(int Id)
        {
            Empresa Empresa = this.BolosConfigService.GetEmpresa(Id);
            return View(Empresa);
        }


        //[HttpPost]
        //public JsonResult Editar(Empresa Empresa)
        //{
        //    try
        //    {
        //        ClearNotValidatedProperties(Empresa);
        //        if (ModelState.IsValid)
        //        {
        //            this.BolosConfigService.UpdateEmpresa(Empresa);
        //            return Json(new { Success = true, Empresa = Empresa });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de edición de Empresa", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        //        }
        //    }
        //    catch (ValidationException e)
        //    {
        //        return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
        //    }
        //    catch (BusinessException e)
        //    {
        //        return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }

        //}


        //[HttpPost]
        //public JsonResult GetAll(PagingRequest paging)
        //{
        //    if (paging.page == 0)
        //    {
        //        return Json(new { Data = this.BolosConfigService.GetAllEmpresas(), Success = true });
        //    }
        //    else
        //    {
        //        PagingResponse<Empresa> resp = new PagingResponse<Empresa>();
        //        resp.Page = paging.page;
        //        resp.Records = this.BolosConfigService.GetAllEmpresas();
        //        resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
        //        resp.TotalRecords = resp.Records.Count;
        //        return Json(resp);
        //    }
        //}

        //[HttpPost]
        //public JsonResult GetEmpresas(SelectComboRequest req)
        //{
        //    return Json(new { Data = this.BolosConfigService.GetAllEmpresasByFilterCombo(req), Success = true });
        //}


        [HttpPost]
        public JsonResult GetEmpresa(int idEmpresa)
        {
            Empresa a = this.BolosConfigService.GetEmpresa(idEmpresa);
            //Empresa a = this.BolosConfigService.GetEmpresaCompleto(idEmpresa);
            return Json(new { Data = (a != null) ? a : null, Success = true });
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
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/Empresas", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    //BackgroundJob.Enqueue<BolosJobs>(x => x.ImportarEmpresas(absolutePath, Security.GetJobHeader()));
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
            DescargarArchivo("App_Data/Importaciones/Templates/Empresas.xlsx");
        }

    }
}
