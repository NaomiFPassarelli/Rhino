using Hangfire;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Sueldos;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.PDF;
using Woopin.SGC.Web.Scheduler;

namespace Woopin.SGC.Web.Areas.Sueldos.Controllers
{
    public class RecibosController : BaseController
    {
        private readonly ISueldosService SueldosService;
        private readonly ISueldosConfigService SueldosConfigService;
        private readonly ISystemService SystemService;
        private readonly ICommonConfigService commonConfigService;

        public RecibosController(ISueldosService SueldosService, ICommonConfigService commonConfigService,
            ISystemService SystemService, ISueldosConfigService SueldosConfigService)
        {
            this.SueldosService = SueldosService;
            this.SystemService = SystemService;
            this.commonConfigService = commonConfigService;
            this.SueldosConfigService = SueldosConfigService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            //ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            //ViewBag.Nacionalidades = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.DomicilioEmpresa = Security.GetOrganizacion().Domicilio;
            ViewBag.NumeroRef = this.SueldosService.GetProximoNumeroReferencia();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Recibo Recibo)
        {
            try
            {
                foreach (AdicionalRecibo AR in Recibo.AdicionalesRecibo)
                {
                    decimal n;
                    bool isNumeric = decimal.TryParse(AR.Total.ToString(), out n);
                    if (!isNumeric)
                    {
                        return Json(new { Success = false });                    
                    }
                }
                if (Recibo.FechaPagoAnterior.HasValue && Convert.ToInt32(Recibo.FechaPagoAnterior.Value.Year.ToString()) < 2000 && Recibo.FechaPagoAnterior.Value.Year.ToString() != "null")
                {
                    Recibo.FechaPagoAnterior = DateTime.Now;
                }
                ClearNotValidatedProperties(Recibo);
                
                //if (ModelState.IsValid)
                //{
                int Numero = Recibo.NumeroReferencia;
                    this.SueldosService.AddRecibo(Recibo);
                    if (Numero != Recibo.NumeroReferencia)
                    {
                        return Json(new { Success = true, NumeroRef = Recibo.NumeroReferencia, Recibo = Recibo });
                    }
                    return Json(new { Success = true, Recibo = Recibo });
                //}
                //else
                //{
                //    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Recibo", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                //}
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
                    this.SueldosService.DeleteRecibos(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Recibo, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        //public ActionResult Editar(int Id)
        //{
        //    Recibo Recibo = this.SueldosService.GetRecibo(Id);
        //    ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
        //    ViewBag.Nacionalidades = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    ViewBag.Categorias = this.commonConfigService.GetItemsByCombo(ComboType.CategoriasRecibos).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    ViewBag.EstadosCivil = this.commonConfigService.GetItemsByCombo(ComboType.EstadoCivil).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    ViewBag.Sexos = this.commonConfigService.GetItemsByCombo(ComboType.Sexo).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    ViewBag.Tareas = this.commonConfigService.GetItemsByCombo(ComboType.TareasRecibos).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
        //    return View(Recibo);
        //}


        //[HttpPost]
        //public JsonResult Editar(Recibo Recibo)
        //{
        //    try
        //    {
        //        ClearNotValidatedProperties(Recibo);
        //        if (ModelState.IsValid)
        //        {
        //            this.SueldosService.UpdateRecibo(Recibo);
        //            return Json(new { Success = true, Recibo = Recibo });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Algunos de los campos no se encuentran correctamente introducidos.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }
        //}


        public ActionResult Detalle(int Id, bool? opensDialog)
        {
            Recibo r = this.SueldosService.GetReciboCompleto(Id);
            if (r == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }


        [HttpPost]
        public JsonResult GetAll(PagingRequest paging, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.SueldosService.GetAllRecibos(range.Start, range.End).Select(x => new Recibo()
                {
                    Id = x.Id,
                    Total = x.Total,
                    TotalDescuento = x.TotalDescuento,
                    TotalNoRemunerativo = x.TotalNoRemunerativo,
                    TotalRemunerativo = x.TotalRemunerativo,
                    FechaCreacion = x.FechaCreacion,
                    NumeroReferencia = x.NumeroReferencia,
                    Periodo = x.Periodo,
                    Empleado = x.Empleado
                }).ToList()
                , Success = true });
            }
            else
            {
                PagingResponse<Recibo> resp = new PagingResponse<Recibo>();
                resp.Page = paging.page;
                resp.Records = this.SueldosService.GetAllRecibos(range.Start, range.End).Select(x => new Recibo()
                {
                    Id = x.Id,
                    Total = x.Total,
                    TotalDescuento = x.TotalDescuento,
                    TotalNoRemunerativo = x.TotalNoRemunerativo,
                    TotalRemunerativo = x.TotalRemunerativo,
                    FechaCreacion = x.FechaCreacion,
                    NumeroReferencia = x.NumeroReferencia,
                    Periodo = x.Periodo,
                    Empleado = x.Empleado
                }).ToList();
                
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetRecibos(SelectComboRequest req)
        {
            return Json(new { Data = this.SueldosService.GetAllRecibosByFilterCombo(req), Success = true });
        }

        [HttpPost]
        public JsonResult GetRecibo(int idRecibo)
        {
            Recibo a = this.SueldosService.GetRecibo(idRecibo);
            return Json(new { Data = (a != null) ? a : null, Success = true });
        }

        public ActionResult SueldoBruto(decimal? SueldoCategoria, decimal? SueldoMes, decimal? SueldoHora)
        {
            ViewBag.SueldoCategoria = SueldoCategoria;
            ViewBag.SueldoMes = SueldoMes;
            ViewBag.SueldoHora = SueldoHora;
            return View();
        }

        public ActionResult TipoRecibo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetReciboAnterior(int IdEmpleado)
        {
            Recibo rAnterior = this.SueldosService.GetReciboAnterior(IdEmpleado);
            return Json(new { Data = rAnterior, Success = true });
        }

        [HttpPost]
        public JsonResult GetPromedioRemunerativo(int IdEmpleado)
        { 
            decimal[] Promedio = this.SueldosService.GetPromedioRemunerativo(IdEmpleado);
            return Json(new { Data = Promedio, Success = true });
        }

        /* Funcion para impresion de Recibo. */
        public void DescargarPDF(int Id)
        {
            string outpath = this.ArmarComprobantePDF(Id);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=\"" + Path.GetFileName(outpath) + "\"");
            Response.WriteFile(outpath);
            Response.ContentType = "";
            Response.End();
        }

        private string ArmarComprobantePDF(int IdRecibo)
        {
            Recibo r = this.SueldosService.GetReciboCompleto(IdRecibo);
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(r.Organizacion.Id);
            if (o.ImagePath != null)
            {
                string logoorgpath = "/" + o.ImagePath;
                ViewBag.BaseLogoOrg = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, logoorgpath);
            }

            string html = RenderViewToString("VersionImprimible", r);
            string filename = r.NumeroReferencia + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "Recibos/" + o.Id.ToString());
            return OutputPath;
        }

        public ActionResult VersionImprimible(int Id, bool? opensDialog)
        {
            Recibo r = this.SueldosService.GetReciboCompleto(Id);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }

        public JsonResult MejorRemuneracion(int IdEmpleado)
        {
            decimal mejorRemuneracion = this.SueldosService.GetMejorRemuneracion(IdEmpleado);
            return Json(new { Data = mejorRemuneracion, Success = true });
        }

        public JsonResult GetAdicionalesDelPeriodoByEmpleado(string Periodo, int IdEmpleado)
        {
            IList<AdicionalRecibo> ARs = this.SueldosConfigService.GetAdicionalesDelPeriodoByEmpleado(Periodo, IdEmpleado);
            if (ARs.Count > 0)
            {
                foreach (AdicionalRecibo AR in ARs)
                {
                    AR.Recibo = null;
                }
            }
            return Json(new { Data = ARs , Success = true });            
        }


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
            IList<Recibo> r = this.SueldosService.GetRecibos(Ids);
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(r.First().Organizacion.Id);
            if (o.ImagePath != null)
            {
                string logoorgpath = "/" + o.ImagePath;
                ViewBag.BaseLogoOrg = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, logoorgpath);
            }

            string html = RenderViewToString("VersionImprimibles", r);
            string filename = "RecibosCombinados" + " " + r.First().Organizacion.RazonSocial + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "RecibosCombinados/" + o.Id.ToString());
            return OutputPath;
        }

        public ActionResult VersionImprimibles(IList<int> Ids, bool? opensDialog)
        {
            IList<Recibo> r = this.SueldosService.GetRecibos(Ids);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }


    }
}
