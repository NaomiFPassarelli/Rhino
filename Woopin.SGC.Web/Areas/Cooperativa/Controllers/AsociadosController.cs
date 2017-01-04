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
        private readonly ICooperativaService CooperativaService;
        private readonly ICommonConfigService commonConfigService;
        private readonly ISystemService SystemService;

        public AsociadosController(ICooperativaConfigService CooperativaConfigService, ICommonConfigService commonConfigService,
                                    ISystemService SystemService, ICooperativaService CooperativaService)
        {
            this.CooperativaConfigService = CooperativaConfigService;
            this.CooperativaService = CooperativaService;
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
            ViewBag.NumeroRef = this.CooperativaConfigService.GetProximoNumeroReferencia();
            ViewBag.EstadosCivil = this.commonConfigService.GetItemsByCombo(ComboType.EstadoCivil).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Nacionalidades = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Asociado Asociado, HttpPostedFileBase ImagePath)
        {
            try
            {
                ClearNotValidatedProperties(Asociado);
                DateTime endOfMonthIngreso = new DateTime(Asociado.FechaIngreso.Year, Asociado.FechaIngreso.Month, DateTime.DaysInMonth(Asociado.FechaIngreso.Year, Asociado.FechaIngreso.Month));
                Asociado.FechaActaIngreso = endOfMonthIngreso;
                Acta AI = this.CooperativaService.GetActaByFecha(endOfMonthIngreso);
                Acta AE = null;
                if (Asociado.FechaEgreso != null)
                {
                    DateTime fechaEgreso = (DateTime)Asociado.FechaEgreso;
                    DateTime endOfMonthEgreso = new DateTime(fechaEgreso.Year, fechaEgreso.Month, DateTime.DaysInMonth(fechaEgreso.Year, fechaEgreso.Month));
                    AE = this.CooperativaService.GetActaByFecha(endOfMonthEgreso);
                }
                if (AI != null || AE != null)
                {
                    return Json(new { Success = false, ErrorMessage = "Existe un acta creada para la fecha de ingreso o egreso seleccionado", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
                if (Asociado.FechaCreacion.Year < DateTime.Now.Year)
                {
                    Asociado.FechaCreacion = DateTime.Now;
                }

                string Documento = null;
                //Debe tener alguno de los 5 de identida
                Documento = (Asociado.CUIT != null ? Asociado.CUIT : (Asociado.DNI != null ? Asociado.DNI : (Asociado.CI != null ? Asociado.CI : (Asociado.LC != null ? Asociado.LC : Asociado.LE))));
            
                if(Documento == null || Documento == "")
                //if ((Asociado.CUIT == null || Asociado.CUIT == "") &&
                //    (Asociado.DNI == null || Asociado.DNI == "") &&
                //    (Asociado.CI == null || Asociado.CI == "") &&
                //    (Asociado.LE == null || Asociado.LE == "") &&
                //    (Asociado.LC == null || Asociado.LC == ""))
                {
                    return Json(new { Success = false, ErrorMessage = "El asociado debe tener algun identificador CUIT, DNI, CI, LE, LC", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
                
                //else {
                //    if (Asociado.CUIT != null)
                //    {
                //        Documento = Asociado.CUIT;
                //    }else if(Asociado.DNI != null)
                //    {
                //        Documento = Asociado.DNI;
                //    }
                //    else if (Asociado.CI != null)
                //    {
                //        Documento = Asociado.CI;
                //    }
                //    else if (Asociado.LE != null)
                //    {
                //        Documento = Asociado.LE;
                //    }
                //    else {
                //        Documento = Asociado.LC;                    
                //    }
                //}

                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string filename = Documento + Path.GetExtension(ImagePath.FileName);
                        string path = GuardarArchivo(ImagePath, "Images/Organizacion", filename);

                        Asociado.ImagePath = path;
                    }

                    Asociado.NumeroReferencia = this.CooperativaConfigService.GetProximoNumeroReferencia();
                    this.CooperativaConfigService.AddAsociado(Asociado);
                    if (Asociado.NumeroReferencia != Asociado.NumeroReferencia)
                    {
                        return Json(new { Success = true, NumeroRef = Asociado.Id, Asociado = Asociado });
                    }
                
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

        public ActionResult Bajar(int Id)
        {
            Asociado Asociado = this.CooperativaConfigService.GetAsociado(Id);
            return View(Asociado);
        }

        [HttpPost]
        public JsonResult Bajar(Asociado Asociado)
        {
            try
            {
                if (Asociado.FechaEgreso != null)
                {
                    Acta AE = null;
                    DateTime fechaEgreso = (DateTime)Asociado.FechaEgreso;
                    DateTime endOfMonthEgreso = new DateTime(fechaEgreso.Year, fechaEgreso.Month, DateTime.DaysInMonth(fechaEgreso.Year, fechaEgreso.Month));
                    AE = this.CooperativaService.GetActaByFecha(endOfMonthEgreso);
                    
                    if (AE != null)
                    {
                        return Json(new { Success = false, ErrorMessage = "Existe un acta creada para la fecha de ingreso o egreso seleccionado" });
                    }
                    Asociado = this.CooperativaConfigService.GetAsociado(Asociado.Id);
                    Asociado.FechaEgreso = fechaEgreso;
                    if(Asociado.FechaIngreso > Asociado.FechaEgreso)
                    {
                        return Json(new { Success = false, ErrorMessage = "La fecha de ingreso debe ser menor a la de egreso" });
                    }
                    this.CooperativaConfigService.BajarAsociado(Asociado);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Debe seleccionar la fecha de egreso" });
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


        public ActionResult Editar(int Id)
        {
            ViewBag.EstadosCivil = this.commonConfigService.GetItemsByCombo(ComboType.EstadoCivil).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Nacionalidades = this.commonConfigService.GetItemsByCombo(ComboType.Paises).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            Asociado Asociado = this.CooperativaConfigService.GetAsociado(Id);
            if (Asociado.ActaAlta != null)
            {
                ViewBag.EditableAlta = false;
            }
            else {
                ViewBag.EditableAlta = true;            
            }
            if (Asociado.ActaBaja != null)
            {
                ViewBag.EditableBaja = false;
            }
            else
            {
                ViewBag.EditableBaja = true;
            }
            return View(Asociado);
        }


        [HttpPost]
        public JsonResult Editar(Asociado Asociado, HttpPostedFileBase ImagePath)
        {
            try
            {
                ClearNotValidatedProperties(Asociado);
                DateTime endOfMonthIngreso = new DateTime(Asociado.FechaIngreso.Year, Asociado.FechaIngreso.Month, DateTime.DaysInMonth(Asociado.FechaIngreso.Year, Asociado.FechaIngreso.Month));
                Asociado.FechaActaIngreso = endOfMonthIngreso;
                Acta AI = this.CooperativaService.GetActaByFecha(endOfMonthIngreso);
                Acta AE = null;
                if (Asociado.FechaEgreso != null)
                {
                    DateTime fechaEgreso = (DateTime)Asociado.FechaEgreso;
                    DateTime endOfMonthEgreso = new DateTime(fechaEgreso.Year, fechaEgreso.Month, DateTime.DaysInMonth(fechaEgreso.Year, fechaEgreso.Month));
                    AE = this.CooperativaService.GetActaByFecha(endOfMonthEgreso);
                }
                if (AI != null || AE != null)
                {
                    return Json(new { Success = false, ErrorMessage = "Existe un acta creada para la fecha de ingreso o egreso seleccionado", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }

                string Documento = null;
                Documento = (Asociado.CUIT != null ? Asociado.CUIT : (Asociado.DNI != null ? Asociado.DNI : (Asociado.CI != null ? Asociado.CI : (Asociado.LC != null ? Asociado.LC : Asociado.LE))));

                //Debe tener alguno de los 5 de identida
                if (Documento == null || Documento == "")
                //if ((Asociado.CUIT == null || Asociado.CUIT == "") &&
                //    (Asociado.DNI == null || Asociado.DNI == "") &&
                //    (Asociado.CI == null || Asociado.CI == "") &&
                //    (Asociado.LE == null || Asociado.LE == "") &&
                //    (Asociado.LC == null || Asociado.LC == ""))
                {
                    return Json(new { Success = false, ErrorMessage = "El asociado debe tener algun identificador CUIT, DNI, CI, LE, LC", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            
                if (ModelState.IsValid)
                {
                    if (ImagePath != null)
                    {
                        string filename = Documento + Path.GetExtension(ImagePath.FileName);
                        EliminarArchivo(Asociado.ImagePath); //elimina el viejo
                        string path = GuardarArchivo(ImagePath, "Images/Organizacion", filename);
                        Asociado.ImagePath = path;
                    }

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
                    string path = GuardarArchivo(archivo, "App_Data/Importaciones/Asociados", filename);
                    string absolutePath = Server.MapPath("~/" + path);
                    BackgroundJob.Enqueue<CooperativaJobs>(x => x.ImportarAsociados(absolutePath, Security.GetJobHeader()));
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
            DescargarArchivo("App_Data/Importaciones/Templates/Asociados.xlsx");
        }

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
            
            string html = RenderViewToString("SolicitudIngreso", r);
            string filename = r.Nombre + " " + r.Apellido + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "SolicitudIngreso/" + o.Id.ToString());
            return OutputPath;
        }

        public ActionResult SolicitudIngreso(int Id, bool? opensDialog)
        {
            Asociado r = this.CooperativaConfigService.GetAsociado(Id);

            DateTime dateToday = new DateTime();
            int dayToday = dateToday.Day;
            int monthToday = dateToday.Month + 1;
            int yearToday = dateToday.Year;
            int dayNacimiento, monthNacimiento, yearNacimiento, cantYears, cantMonths, cantDays = 0;

            if (r.FechaNacimiento.HasValue)
            {
                dayNacimiento = r.FechaNacimiento.Value.Day;
                monthNacimiento = r.FechaNacimiento.Value.Month;
                yearNacimiento = r.FechaNacimiento.Value.Year;
                cantYears = yearToday - yearNacimiento;
                if (cantYears != 0)
                {
                    //distinto año
                    cantMonths = monthToday - monthNacimiento;
                    if (cantMonths == 0)
                    {
                        //mismo mes
                        cantDays = dayToday - dayNacimiento;
                        if (cantDays < 0)
                        {
                            //resto un año
                            cantYears -= 1;
                        }
                        //else nada
                    }
                    else if (cantMonths < 0)
                    {
                        //distinto mes resto un año
                        cantYears -= 1;
                    }
                }
                ViewBag.Edad = cantYears;
            }

            ViewBag.Documento = (r.CUIT != null ? r.CUIT : (r.DNI != null ? r.DNI : (r.CI != null ? r.CI : (r.LC != null ? r.LC : r.LE))));
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }

        //public ActionResult CompletarActaAlta(int Mes, int Anio)
        //{
        //    IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMes(Mes, Anio);
        //    Asociado Asociado = rs.First();
        //    return View(Asociado);
        //}

        //[HttpPost]
        //public JsonResult CompletarActaAlta(Asociado Asociado, int Mes, int Anio)
        //{
        //    try
        //    {
        //        if(Asociado.FechaActaIngreso == null || Asociado.FechaFinalizacionAlta == null
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


        //Egreso o renuncia
        //[HttpPost]
        //public JsonResult ValidarDescargaEgreso(int Mes, int Año)
        //{
        //    DateTime endOfMonth = new DateTime(Año, Mes, DateTime.DaysInMonth(Año, Mes));
        //    DateTime today = DateTime.Now;
        //    if (today <= endOfMonth)
        //    {
        //        //return con error porque debe terminar el mes antes de generar el pdf
        //        return Json(new { Success = false, ErrorMessage = "La fecha seleccionada no puede ser de un mes no finalizado", FaltaInformacion = false });
        //    }
        //    else
        //    {
        //        IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMesEgreso(Mes, Año);
        //        if (rs.Count > 0 && (rs.First().ActaAlta == 0 || rs.First().ActaAlta == null))
        //        {
        //            //colocar actaAlta/presidente/secretario/tesorero/hora de finalizada la reunion
        //            return Json(new { Success = false, ErrorMessage = "Se debe seleccionar el Presidente, Secretario, Tesorero, Otro Presente y Nro Acta", FaltaInformacion = true });
        //        }
        //    }

        //    return Json(new { Success = true });
        //}

        //Acta Egreso
        //public void DescargarPDFActaEgreso(int Mes, int Año)
        //{
        //    string outpath = this.ArmarComprobantePDFActaEgreso(Mes, Año);
        //    Response.Clear();
        //    Response.AddHeader("content-disposition", "attachment; filename=\"" + Path.GetFileName(outpath) + "\"");
        //    Response.WriteFile(outpath);
        //    Response.ContentType = "";
        //    Response.End();
        //}

        //private string ArmarComprobantePDFActaEgreso(int Mes, int Año)
        //{
        //    IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMesEgreso(Mes, Año);
        //    ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
        //    Organizacion o = this.SystemService.GetOrganizacion(rs.First().Organizacion.Id);

        //    string html = RenderViewToString("ActaEgreso", rs);
        //    string filename = rs.First().Organizacion.RazonSocial + " " + rs.First().FechaEgreso.Value.Month + ".pdf";
        //    string OutputPath = HtmlToPDF.Generate(html, filename, "ActaEgreso/" + o.Id.ToString());
        //    return OutputPath;
        //    //}
        //}

        //public ActionResult ActaEgreso(int Id, bool? opensDialog)
        //public ActionResult ActaEgreso(IList<int> Ids, bool? opensDialog, int Mes, int Año)
        //{
        //    DateTime endOfMonth = new DateTime(Año, Mes, DateTime.DaysInMonth(Año, Mes));
        //    DateTime today = DateTime.Now;
        //    if (today > endOfMonth)
        //    {
        //        //return con error porque debe terminar el mes antes de generar el pdf
        //        return RedirectToAction("Index", "Asociados");
        //    }

        //    IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMesEgreso(Mes, Año);
        //    if (rs.Count > 0 && (rs.First().ActaAlta == 0 || rs.First().ActaAlta == null))
        //    {
        //        //colocar actaAlta/presidente/secretario/tesorero/hora de finalizada la reunion
        //        return null;
        //    }
        //    else
        //    {
        //        ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
        //        return View(rs);
        //    }
        //}


        public ActionResult CompletarActaBaja(int Mes, int Anio)
        {
            IList<Asociado> rs = this.CooperativaConfigService.GetAsociadosMesEgreso(Mes, Anio);
            Asociado Asociado = rs.First();
            return View(Asociado);
        }

        [HttpPost]
        public JsonResult CompletarActaBaja(Asociado Asociado, int Mes, int Anio)
        {
            try
            {
                if (Asociado.ActaBaja == null)
                {
                    return Json(new { Success = false, ErrorMessage = "Debe completar toda la informacion" });
                }
                else
                {
                    this.CooperativaConfigService.ActualizarBajaAsociados(Asociado, Mes, Anio);
                    return Json(new { Success = true });
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
            IList<Asociado> r = this.CooperativaConfigService.GetAsociados(Ids);
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(r.First().Organizacion.Id);
            if (o.ImagePath != null)
            {
                string logoorgpath = "/" + o.ImagePath;
                ViewBag.BaseLogoOrg = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, logoorgpath);
            }

            string html = RenderViewToString("VersionImprimibles", r);
            string filename = "AsociadosCombinados" + " " + r.First().Organizacion.RazonSocial + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "AsociadosCombinados/" + o.Id.ToString());
            return OutputPath;
        }

        public ActionResult VersionImprimibles(IList<int> Ids, bool? opensDialog)
        {
            IList<Asociado> r = this.CooperativaConfigService.GetAsociados(Ids);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }


    }
}
