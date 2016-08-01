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
using Woopin.SGC.Model.Cooperativa;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.Scheduler;
using Woopin.SGC.Web.PDF;

namespace Woopin.SGC.Web.Areas.Cooperativa.Controllers
{
    public class AsociadosController : BaseController
    {
        private readonly ICooperativaConfigService CooperativaConfigService;
        private readonly ICommonConfigService commonConfigService;
        private readonly ISystemService SystemService;

        public AsociadosController(ICooperativaConfigService CooperativaConfigService, ICommonConfigService commonConfigService,
                                    ISystemService SystemService)
        {
            this.CooperativaConfigService = CooperativaConfigService; 
            this.commonConfigService = commonConfigService;
            this.SystemService = SystemService;
        }

        //
        // GET: 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Asociado Asociado)
        {
            try
            {
                ClearNotValidatedProperties(Asociado);
                if (ModelState.IsValid)
                {
                    this.CooperativaConfigService.AddAsociado(Asociado);
                    return Json(new { Success = true, Asociado = Asociado });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Asociado", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.CooperativaConfigService.DeleteAsociados(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Asociado, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        public ActionResult Editar(int Id)
        {
            Asociado Asociado = this.CooperativaConfigService.GetAsociado(Id);
            //ViewBag.FechaNacimiento = Asociado.FechaNacimiento;
            //ViewBag.FechaAntiguedadReconocida = Asociado.FechaAntiguedadReconocida;
            return View(Asociado);
        }


        [HttpPost]
        public JsonResult Editar(Asociado Asociado)
        {
            try
            {
                ClearNotValidatedProperties(Asociado);
                if (ModelState.IsValid)
                {
                    this.CooperativaConfigService.UpdateAsociado(Asociado);
                    return Json(new { Success = true, Asociado = Asociado });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Asociado", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                return Json(new { Data = this.CooperativaConfigService.GetAllAsociados(), Success = true });
            }
            else
            {
                PagingResponse<Asociado> resp = new PagingResponse<Asociado>();
                resp.Page = paging.page;
                resp.Records = this.CooperativaConfigService.GetAllAsociados();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAsociados(SelectComboRequest req)
        {
            return Json(new { Data = this.CooperativaConfigService.GetAllAsociadosByFilterCombo(req), Success = true });
        }


        [HttpPost]
        public JsonResult GetAsociado(int idAsociado)
        {
            Asociado a = this.CooperativaConfigService.GetAsociadoCompleto(idAsociado);
            return Json(new { Data = (a != null && a.Activo) ? a : null, Success = true });
        }

        //public ActionResult Importar()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult Importar(HttpPostedFileBase archivo)
        //{
        //    try
        //    {
        //        if (archivo != null)
        //        {
        //            string filename = Security.GetOrganizacion().RazonSocial.Replace(" ", "") + "_" + DateTime.Now.ToString("ddMMyyyy-HHm") + Path.GetExtension(archivo.FileName);
        //            string path = GuardarArchivo(archivo, "App_Data/Importaciones/Asociados", filename);
        //            string absolutePath = Server.MapPath("~/" + path);
        //            BackgroundJob.Enqueue<CooperativaJobs>(x => x.ImportarAsociados(absolutePath, Security.GetJobHeader()));
        //            return Json(new { Success = true });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "No se detecto el archivo para importar" });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }
        //}

        //public void DescargarTemplate()
        //{
        //    DescargarArchivo("App_Data/Importaciones/Templates/Asociados.xlsx");
        //}

        //SolicitudIngreso
        public void DescargarPDFSolicitud(int Id)
        {
            string outpath = this.ArmarComprobantePDFSolicitud(Id);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=\"" + Path.GetFileName(outpath) + "\"");
            Response.WriteFile(outpath);
            Response.ContentType = "";
            Response.End();
        }

        private string ArmarComprobantePDFSolicitud(int IdAsociado)
        {
            Asociado r = this.CooperativaConfigService.GetAsociado(IdAsociado);
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(r.Organizacion.Id);
            if (o.ImagePath != null)
            {
                string logoorgpath = "/" + o.ImagePath;
                ViewBag.BaseLogoOrg = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, logoorgpath);
            }

            string html = RenderViewToString("SolicitudIngreso", r);
            string filename = r.Nombre + " " + r.Apellido + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "Asociados/SolicitudIngreso" + o.Id.ToString());
            return OutputPath;
        }

        public ActionResult SolicitudIngreso(int Id, bool? opensDialog)
        {
            Asociado r = this.CooperativaConfigService.GetAsociado(Id);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }

        //Acta Ingreso
        public void DescargarPDFActaIngreso(int Mes, int Año)
        {
            DateTime endOfMonth = new DateTime(Año, Mes, DateTime.DaysInMonth(Año, Mes));
            DateTime today = new DateTime();
            if (today > endOfMonth)
            {
                //return con error porque debe terminar el mes antes de generar el pdf
                return;
            }
            else {
                string outpath = this.ArmarComprobantePDFActaIngreso(Mes, Año);
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=\"" + Path.GetFileName(outpath) + "\"");
                Response.WriteFile(outpath);
                Response.ContentType = "";
                Response.End();
            }
        }

        private string ArmarComprobantePDFActaIngreso(int Mes, int Año)
        {
            IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMes(Mes, Año);
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(rs.First().Organizacion.Id);
            if (o.ImagePath != null)
            {
                string logoorgpath = "/" + o.ImagePath;
                ViewBag.BaseLogoOrg = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, logoorgpath);
            }

            string html = RenderViewToString("ActaIngreso", rs);
            string filename = rs.First().Organizacion + " " + rs.First().FechaIngreso.Month + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "Asociados/ActaIngreso" + o.Id.ToString());
            return OutputPath;
        }

        //public ActionResult ActaIngreso(int Id, bool? opensDialog)
        public ActionResult ActaIngreso(IList<int> Ids, bool? opensDialog, int Mes, int Año)
        {
            IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMes(Mes, Año);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(rs);
        }



    }
}
