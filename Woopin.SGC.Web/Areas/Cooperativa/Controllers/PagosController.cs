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
    public class PagosController : BaseController
    {
        private readonly ICooperativaService CooperativaService;
        private readonly ICommonConfigService commonConfigService;
        private readonly ISystemService SystemService;

        public PagosController(ICooperativaService CooperativaService, ICommonConfigService commonConfigService,
                                ISystemService SystemService)
        {
            this.CooperativaService = CooperativaService; 
            this.commonConfigService = commonConfigService;
            this.SystemService = SystemService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexDetalle()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Pago Pago, int CantidadCuotasAPagar)
        {
            try
            {
                ClearNotValidatedProperties(Pago);
                if (ModelState.IsValid)
                {
                    this.CooperativaService.AddPagos(Pago, CantidadCuotasAPagar);
                    return Json(new { Success = true, Pago = Pago });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Pago", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
        //            this.CooperativaService.DeletePagos(Ids);
        //            return Json(new { Success = true });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Pago, vuelva a inetntarlo." });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }
        //}

        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.CooperativaService.GetAllPagos(), Success = true });
            }
            else
            {
                PagingResponse<Pago> resp = new PagingResponse<Pago>();
                resp.Page = paging.page;
                resp.Records = this.CooperativaService.GetAllPagos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        [HttpPost]
        public JsonResult GetAllByAsociado(PagingRequest paging, int IdAsociado)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.CooperativaService.GetAllPagosByAsociado(IdAsociado), Success = true });
            }
            else
            {
                PagingResponse<Pago> resp = new PagingResponse<Pago>();
                resp.Page = paging.page;
                resp.Records = this.CooperativaService.GetAllPagosByAsociado(IdAsociado);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetPago(int idPago)
        {
            Pago a = this.CooperativaService.GetPago(idPago);
            return Json(new { Data = (a != null) ? a : null, Success = true });
        }
        public void DescargarPDF(int Id)
        {
            string outpath = this.ArmarComprobantePDF(Id);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=\"" + Path.GetFileName(outpath) + "\"");
            Response.WriteFile(outpath);
            Response.ContentType = "";
            Response.End();
        }

        private string ArmarComprobantePDF(int IdPago)
        {
            Pago r = this.CooperativaService.GetPago(IdPago);
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(r.Organizacion.Id);
            if (o.ImagePath != null)
            {
                string logoorgpath = "/" + o.ImagePath;
                ViewBag.BaseLogoOrg = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, logoorgpath);
            }

            string html = RenderViewToString("VersionImprimible", r);
            string filename = r.Asociado + " " + r.NumeroCuota + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "PagosAsociados/" + o.Id.ToString());
            return OutputPath;
        }

        public ActionResult VersionImprimible(int Id, bool? opensDialog)
        {
            Pago r = this.CooperativaService.GetPago(Id);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }


    }
}
