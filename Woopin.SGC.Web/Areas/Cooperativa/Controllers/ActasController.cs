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
    public class ActasController : BaseController
    {
        private readonly ICooperativaService CooperativaService;
        private readonly ICooperativaConfigService CooperativaConfigService;
        private readonly ICommonConfigService commonConfigService;
        private readonly ISystemService SystemService;

        public ActasController(ICooperativaService CooperativaService, ICooperativaConfigService CooperativaConfigService, 
            ICommonConfigService commonConfigService, ISystemService SystemService)
        {
            this.CooperativaService = CooperativaService; 
            this.commonConfigService = commonConfigService;
            this.SystemService = SystemService;
            this.CooperativaConfigService = CooperativaConfigService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Nuevo(string FechaActa)
        {
            ViewBag.FechaActa = FechaActa;
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Acta Acta)
        {
            try
            {
                DateTime endOfMonth = new DateTime(Acta.FechaActa.Year, Acta.FechaActa.Month, DateTime.DaysInMonth(Acta.FechaActa.Year, Acta.FechaActa.Month));
                if(this.CooperativaService.GetActaByFecha(endOfMonth) != null)
                {
                    return Json(new { Success = false, ErrorMessage = "Ya existe un acta para el mes seleccionado", FaltaInformacion = false });
                }
                DateTime today = DateTime.Now;
                if (today < endOfMonth)
                {
                    //return con error porque debe terminar el mes antes de generar el pdf
                    return Json(new { Success = false, ErrorMessage = "La fecha seleccionada no puede ser de un mes no finalizado", FaltaInformacion = false });
                }
                Acta.FechaActa = endOfMonth;
                ClearNotValidatedProperties(Acta);
                if (ModelState.IsValid)
                {
                    this.CooperativaService.AddActa(Acta);
                    return Json(new { Success = true, Acta = Acta });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Acta", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
        //            this.CooperativaService.DeleteActas(Ids);
        //            return Json(new { Success = true });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Acta, vuelva a inetntarlo." });
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
                return Json(new { Data = this.CooperativaService.GetAllActasCompletas(), Success = true });
            }
            else
            {
                PagingResponse<Acta> resp = new PagingResponse<Acta>();
                resp.Page = paging.page;
                resp.Records = this.CooperativaService.GetAllActasCompletas().Select(x => new Acta()
                {
                    Id = x.Id,
                    AsociadosEgreso = x.AsociadosEgreso,
                    AsociadosIngreso = x.AsociadosIngreso,
                    FechaActa = x.FechaActa,
                    FechaFinalizacionActa = x.FechaFinalizacionActa,
                    NumeroActa = x.NumeroActa,
                    OtroPresente = x.OtroPresente,
                    OtrosPuntos = x.OtrosPuntos,
                    Presidente = x.Presidente,
                    Secretario = x.Secretario,
                    Tesorero = x.Tesorero,
                    Organizacion = x.Organizacion
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetActa(int idActa)
        {
            Acta a = this.CooperativaService.GetActa(idActa);
            return Json(new { Data = (a != null) ? a : null, Success = true });
        }



        [HttpPost]
        public JsonResult ValidarDescarga(int Mes, int Año)
        {
            //return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Asociado", Errors = ModelState.Values.SelectMany(v => v.Errors) });
            DateTime endOfMonth = new DateTime(Año, Mes, DateTime.DaysInMonth(Año, Mes));
            DateTime today = DateTime.Now;
            if (today < endOfMonth)
            {
                //return con error porque debe terminar el mes antes de generar el pdf
                return Json(new { Success = false, ErrorMessage = "La fecha seleccionada no puede ser de un mes no finalizado", FaltaInformacion = false });
            }
            else
            {
                Acta A = this.CooperativaService.GetActaByFecha(endOfMonth);

                if (A == null)
                {
                    return Json(new { Success = false, ErrorMessage = "Crear el Acta", FaltaInformacion = true, FechaActa = endOfMonth.ToString("dd/MM/yyyy") });
                }
                else {
                    return Json(new { Success = false, ErrorMessage = "Ya existe un acta para este periodo" });
                    //IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMesEgreso(Mes, Año);
                    //IList<Asociado> rss = this.CooperativaConfigService.GetAsociadosMes(Mes, Año);
                    //rs.Concat(rss);
                    //if (rs.Count > 0 && (rs.First().ActaAlta == 0 || rs.First().ActaAlta == null))
                    //{
                    //    //colocar actaAlta/presidente/secretario/tesorero/hora de finalizada la reunion
                    //    //colocar otros puntos u observaciones
                    //    return Json(new { Success = false, ErrorMessage = "Se debe seleccionar el Presidente, Secretario, Tesorero, Otro Presente y Nro Acta", FaltaInformacion = true });
                    //}
                }
            }
            return Json(new { Success = true });
        }

        //Acta 
        //public void DescargarPDF(int Mes, int Año)
        public void DescargarPDF(int Id)
        {
            //DateTime endOfMonth = new DateTime(Año, Mes, DateTime.DaysInMonth(Año, Mes));
            //DateTime today = DateTime.Now;
            //if (today > endOfMonth)
            //{
            //    //return con error porque debe terminar el mes antes de generar el pdf
            //    return;
            //}
            //else {
            //string outpath = this.ArmarComprobantePDF(Mes, Año);
            string outpath = this.ArmarComprobantePDF(Id);
            //if (outpath == null)
            //{
            //    RedirectToAction("Index", "Asociados");
            //    return;
            //}
            //else {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=\"" + Path.GetFileName(outpath) + "\"");
            Response.WriteFile(outpath);
            Response.ContentType = "";
            Response.End();
            //}

            //}
        }

        //private string ArmarComprobantePDF(int Mes, int Año)
        private string ArmarComprobantePDF(int ActaId)
        {
            Acta Acta = this.CooperativaService.GetActaCompleta(ActaId);
            Acta.OtrosPuntos = this.CooperativaService.GetActaPuntoExtraByActa(ActaId);
            //IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMes(Mes, Año);
            //if (rs.Count > 0 && (rs.First().ActaAlta == 0 || rs.First().ActaAlta == null))
            //{
            //    //colocar actaAlta/presidente/secretario/tesorero/hora de finalizada la reunion
            //    return null;
            //}
            //else {
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(Acta.Organizacion.Id);

            string html = RenderViewToString("Acta", Acta);
            string filename = Acta.Organizacion.RazonSocial + " " + Acta.FechaActa.Month + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "Acta/" + o.Id.ToString());
            return OutputPath;
            //}
        }

        //public ActionResult Acta(int Id, bool? opensDialog)
        public ActionResult Acta(int Id, bool? opensDialog)
        {
            Acta Acta = this.CooperativaService.GetActaCompleta(Id);
            //DateTime endOfMonth = new DateTime(Año, Mes, DateTime.DaysInMonth(Año, Mes));
            //DateTime today = DateTime.Now;
            //if (today > endOfMonth)
            //{
            //    //return con error porque debe terminar el mes antes de generar el pdf
            //    return RedirectToAction("Index", "Asociados");
            //}

            //IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMes(Mes, Año);
            //if (rs.Count > 0 && (rs.First().ActaAlta == 0 || rs.First().ActaAlta == null))
            //{
            //    //colocar actaAlta/presidente/secretario/tesorero/hora de finalizada la reunion
            //    return null;
            //}
            //else
            //{
                ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
                return View(Acta);
            //}
        }


        [HttpPost]
        public JsonResult Eliminar(List<int> Ids)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.CooperativaService.DeleteActas(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Acta, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }




        //public ActionResult CompletarActa(int Mes, int Anio)
        //{
        //    IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMes(Mes, Anio);
        //    Asociado Asociado = rs.First();
        //    return View(Asociado);
        //}

        //[HttpPost]
        //public JsonResult CompletarActa(Asociado Asociado, int Mes, int Anio)
        //{
        //    try
        //    {
        //        if (Asociado.FechaActaIngreso == null || Asociado.FechaFinalizacionAlta == null
        //            || Asociado.Presidente == null || Asociado.Tesorero == null ||
        //            Asociado.OtroPresente == null || Asociado.ActaAlta == null)
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Debe completar toda la informacion" });
        //        }
        //        else
        //        {
        //            this.CooperativaConfigService.ActualizarAltaAsociados(Asociado, Mes, Anio);
        //            return Json(new { Success = true });
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


        //TODOS o seleccionados LOS PDF
        public void DescargarPDFs(string IdsString)
        {
            IList<int> Ids = IdsString.Split(',').Select(Int32.Parse).ToList();
            string outpath = this.ArmarComprobantePDFs(Ids);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=\"" + Path.GetFileName(outpath) + "\"");
            Response.WriteFile(outpath);
            Response.ContentType = "";
            Response.End();

        }


        private string ArmarComprobantePDFs(IList<int> Ids)
        {
            IList<Acta> r = this.CooperativaService.GetActas(Ids);
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(r.First().Organizacion.Id);
            if (o.ImagePath != null)
            {
                string logoorgpath = "/" + o.ImagePath;
                ViewBag.BaseLogoOrg = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, logoorgpath);
            }

            string html = RenderViewToString("VersionImprimibles", r);
            string filename = "ActasCombinados" + " " + r.First().Organizacion.RazonSocial + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "ActasCombinados/" + o.Id.ToString());
            return OutputPath;
        }

        public ActionResult VersionImprimibles(IList<int> Ids, bool? opensDialog)
        {
            IList<Acta> r = this.CooperativaService.GetActas(Ids);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }


    }
}
