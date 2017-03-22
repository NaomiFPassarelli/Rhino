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
    //Es como el rubro del area de compras

    public class ConceptosBoloController : BaseController
    {
        private readonly IBolosConfigService BolosConfigService;
        private readonly ICommonConfigService commonConfigService;

        public ConceptosBoloController(IBolosConfigService BolosConfigService, ICommonConfigService commonConfigService)
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
        public JsonResult Nuevo(ConceptoBolo ConceptoBolo)
        {
            try
            {
                ClearNotValidatedProperties(ConceptoBolo);
                if (ModelState.IsValid)
                {
                    this.BolosConfigService.AddConceptoBolo(ConceptoBolo);
                    return Json(new { Success = true, ConceptoBolo = ConceptoBolo });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de ConceptoBolo", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.BolosConfigService.DeleteConceptosBolo(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la ConceptoBolo, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            ConceptoBolo ConceptoBolo = this.BolosConfigService.GetConceptoBolo(Id);
            return View(ConceptoBolo);
        }


        [HttpPost]
        public JsonResult Editar(ConceptoBolo ConceptoBolo)
        {
            try
            {
                ClearNotValidatedProperties(ConceptoBolo);
                if (ModelState.IsValid)
                {
                    this.BolosConfigService.UpdateConceptoBolo(ConceptoBolo);
                    return Json(new { Success = true, ConceptoBolo = ConceptoBolo });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de edición de ConceptoBolo", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                return Json(new { Data = this.BolosConfigService.GetAllConceptosBolo(), Success = true });
            }
            else
            {
                PagingResponse<ConceptoBolo> resp = new PagingResponse<ConceptoBolo>();
                resp.Page = paging.page;
                resp.Records = this.BolosConfigService.GetAllConceptosBolo();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        //[HttpPost]
        //public JsonResult GetConceptosBolo(SelectComboRequest req)
        //{
        //    return Json(new { Data = this.BolosConfigService.GetAllConceptosBoloByFilterCombo(req), Success = true });
        //}


        [HttpPost]
        public JsonResult GetConceptoBolo(int idConceptoBolo)
        {
            ConceptoBolo a = this.BolosConfigService.GetConceptoBolo(idConceptoBolo);
            //ConceptoBolo a = this.BolosConfigService.GetConceptoBoloCompleto(idConceptoBolo);
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
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/ConceptosBolo", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    //BackgroundJob.Enqueue<BolosJobs>(x => x.ImportarConceptosBolo(absolutePath, Security.GetJobHeader()));
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
            DescargarArchivo("App_Data/Importaciones/Templates/ConceptosBolo.xlsx");
        }

    }
}
