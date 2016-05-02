using Hangfire;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.App.Logging;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Services.Afip.Model;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.PDF;
using Woopin.SGC.Web.Scheduler;

namespace Woopin.SGC.Web.Areas.Ventas.Controllers
{
    public class ComprobantesVentasController : BaseController
    {
        private readonly IVentasConfigService ventasConfigService;
        private readonly IVentasService ventasService;
        private readonly ICommonConfigService commonConfigService;
        private readonly IContabilidadConfigService ContabilidadConfigService;
        private readonly IContabilidadService ContabilidadService;
        private readonly ISystemService SystemService;
        public ComprobantesVentasController(IVentasService ventasService, IVentasConfigService ventasConfigService, 
            ICommonConfigService commonConfigService, IContabilidadConfigService ContabilidadConfigService,
            IContabilidadService ContabilidadService, ISystemService SystemService)
        {
            this.ventasConfigService = ventasConfigService;
            this.ventasService = ventasService;
            this.commonConfigService = commonConfigService;
            this.ContabilidadConfigService = ContabilidadConfigService;
            this.ContabilidadService = ContabilidadService;
            this.SystemService = SystemService;
        }

        #region Listados
        public ActionResult Index(string start, string end, string statusType, string startvenc, string endvenc)
        {
            ViewBag.start = start;
            ViewBag.end = end;
            ViewBag.startvenc = startvenc;
            ViewBag.endvenc = endvenc;
            ViewBag.statusType = statusType;
            return View();
        }
        public ActionResult CuentaCorriente(int? Id)
        {
            ViewBag.IdCliente = Id.HasValue ? Id.Value : 0;
            return View();
        }
        public ActionResult PendientesAfip()
        {
            return View();
        }

        public ActionResult Acumulados()
        {
            return View();
        }
        
        #endregion

        #region Nuevo Comprobante Venta
        public ActionResult Nuevo()
        {
            ViewBag.Tipos = new SelectCombo() { Items = this.commonConfigService.GetItemsByCombo(ComboType.TipoComprobanteVenta).Select(x => new SelectComboItem() { id = x.Id, text = x.Data, additionalData = x.AdditionalData }).ToList() };
            ViewBag.Monedas = this.commonConfigService.GetAllMonedas().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            ViewBag.CondicionesVentas = this.commonConfigService.GetItemsByCombo(ComboType.CondicionCompraVenta).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            ViewBag.IVAs = new SelectCombo() { Items = this.commonConfigService.GetItemsByCombo(ComboType.TipoIva).Select(x => new SelectComboItem() { id = x.Id, text = x.Data, selected = x.AdditionalData == "21", additionalData = x.AdditionalData }).ToList() };
            ViewBag.EsServicio = Security.GetOrganizacion().Actividad.AfipData != "1";
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(ComprobanteVenta ComprobanteVenta)
        {
            
            try
            {
                string Numero = ComprobanteVenta.Numero;
                this.ventasService.AddComprobanteVenta(ComprobanteVenta);
                if (ComprobanteVenta.Tipo.AfipData != null)
                {
                    BackgroundJob.Enqueue<VentasJobs>(x => x.SolicitarCAE(ComprobanteVenta.Id, Security.GetJobHeader()));
                }
                // Problema de referencia circular, no lo devuelvo
                ComprobanteVenta.Asiento = null;
                ComprobanteVenta.Detalle = null;
                if (Numero != ComprobanteVenta.Numero)
                {
                    return Json(new { Success = true, NuevoNumero = ComprobanteVenta.Letra + ComprobanteVenta.Numero, Comprobante = ComprobanteVenta });
                }
                return Json(new { Success = true, Comprobante = ComprobanteVenta });
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch(BusinessException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }
        #endregion

        #region Detalles e Impresiones
        public ActionResult Detalle(int Id, bool? opensDialog)
        {
            ComprobanteVenta c = this.ventasService.GetComprobanteVentaCompleto(Id);
            if (c == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.EsServicio = Security.GetOrganizacion().Actividad.AfipData != "1";
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(c);
        }
        public ActionResult VersionImprimible(int Id, bool? opensDialog)
        {
            ComprobanteVenta c = this.ventasService.GetComprobanteVentaCompleto(Id);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(c);
        }

        /* Funcion para impresion de FC Electronicas. */
        public void DescargarPDF(int Id)
        {
            string outpath = this.ArmarComprobantePDF(Id);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=\"" + Path.GetFileName(outpath) + "\"");
            Response.WriteFile(outpath);
            Response.ContentType = "";
            Response.End();
        }

        private string ArmarComprobantePDF(int IdComprobante)
        {
            ComprobanteVenta c = this.ventasService.GetComprobanteVentaCompleto(IdComprobante);
            ViewBag.BarCode = GetBarcode(c);
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(c.Organizacion.Id);
            if (o.ImagePath != null)
            {
                string logoorgpath = "/" + o.ImagePath;
                ViewBag.BaseLogoOrg = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, logoorgpath);
            }

            string html = RenderViewToString("VersionImprimible", c);
            string filename = c.GetLetraNumero() + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "ComprobantesVenta/" + o.Id.ToString());
            return OutputPath;
        }

        /* Funcion para descargar pdf para agentes impresores */
        public void DescargarAgImpresor(int Id)
        {
            ComprobanteVenta c = this.ventasService.GetComprobanteVentaCompleto(Id);
            ComprobanteVentaPDF pdf = new ComprobanteVentaPDF(c);
            MemoryStream stream = pdf.GenereatePDF();
            string filename = c.GetLetraNumero() + ".pdf";
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AddHeader("content-disposition", "attachment; filename=\"" + filename + "\"");
            Response.BinaryWrite(stream.ToArray());
            Response.Flush();
            stream.Close();
            Response.End();
        }

        [HttpPost]
        public JsonResult EnviarMail(int Id)
        {
            try
            {
                this.ArmarComprobantePDF(Id);
                BackgroundJob.Enqueue<ComprobanteVentaJobs>(x => x.EnviarComprobante(Id,Security.GetJobHeader()));
                return Json(new { Success = true });
            }
            catch
            {
                return Json(new { Success = false });
            }

        }
        #endregion

        #region Dialogs
        public ActionResult ComprobantesACobrar(int IdCliente)
        {
            ViewBag.IdCliente = IdCliente;
            return View();
        }
        public ActionResult ComprobantesACobrarNC(int IdCliente, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Cobrada)
        {
            ViewBag.IdCliente = IdCliente;
            ViewBag.Tipo = Tipo.HasValue ? Tipo.Value : 0;
            ViewBag.NoTipo = NoTipo.HasValue ? NoTipo.Value : 0;
            ViewBag.Cobrada = (int)Cobrada;
            return View();
        }
        #endregion

        #region Updates & Operaciones
        [HttpPost]
        public JsonResult AnularComprobante(int IdComprobante)
        {
            try
            {
                this.ventasService.AnularComprobante(IdComprobante);
                return Json(new { Success = true, Data = "Fue anulada con exito" });
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
        public JsonResult EliminarComprobante(int IdComprobante)
        {
            try
            {
                this.ventasService.EliminarComprobante(IdComprobante);
                return Json(new { Success = true, Data = "Fue eliminado con exito" });
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
        public JsonResult Editar(int IdComprobante, string Observacion)
        {
            try
            {
                this.ventasService.UpdateObservacion(IdComprobante, Observacion);
                return Json(new { Success = true, Data = "Fue modificada con exito" });
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
        public JsonResult CreateCAEJob(int Id)
        {
            BackgroundJob.Enqueue<VentasJobs>(x => x.SolicitarCAE(Id, Security.GetJobHeader()));
            return Json(true);
        }
        
        #endregion

        #region Busquedas de datos
        [HttpPost]
        public JsonResult GetProximoComprobante(string LetraComprobante, int TipoComprobante, int Talonario)
        {
            return Json(new { Data = this.ventasService.GetProximoComprobante(LetraComprobante, TipoComprobante, Talonario), Success = true });
        }

        [HttpPost]
        public JsonResult GetAllByCliente(int Id, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, CuentaCorrienteFilter filter, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ventasService.GetAllComprobantesVentasByCliente(Id, range.Start, range.End, startvenc, endvenc, filter), Success = true });
            }
            else
            {
                PagingResponse<ComprobanteVenta> resp = new PagingResponse<ComprobanteVenta>();
                resp.Page = paging.page;
                resp.Records = this.ventasService.GetAllComprobantesVentasByCliente(Id, range.Start, range.End, startvenc, endvenc, filter).Select(x => new ComprobanteVenta()
                {
                    Id = x.Id,
                    Cliente = x.Cliente,
                    Estado = x.Estado,
                    Fecha = x.Fecha,
                    Numero = x.Numero,
                    CondicionVenta = x.CondicionVenta,
                    Letra = x.Letra,
                    IVA = x.IVA,
                    Tipo = x.Tipo,
                    Subtotal = x.Subtotal,
                    Total = x.Total,
                    TotalCobrado = x.TotalCobrado,
                    MesPrestacion = x.MesPrestacion,
                    Observacion = x.Observacion
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAllACobrar(PagingRequest paging, int IdCliente)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.ventasService.GetComprobantesVentasACobrar(IdCliente), Success = true });
            }
            else
            {
                PagingResponse<ComprobanteVenta> resp = new PagingResponse<ComprobanteVenta>();
                resp.Page = paging.page;
                resp.Records = this.ventasService.GetComprobantesVentasACobrar(IdCliente).Select(x => new ComprobanteVenta()
                {
                    Id = x.Id,
                    Cliente = x.Cliente,
                    Estado = x.Estado,
                    Fecha = x.Fecha,
                    Numero = x.Numero,
                    CondicionVenta = x.CondicionVenta,
                    Letra = x.Letra,
                    IVA = x.IVA,
                    Tipo = x.Tipo,
                    Subtotal = x.Subtotal,
                    Total = x.Total,
                    TotalCobrado = x.TotalCobrado,
                    MesPrestacion = x.MesPrestacion
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult LoadCtaCorrienteHead(int id, DateTime? start, DateTime? end, CuentaCorrienteFilter filter)
        {
            DateRange range = new DateRange(start, end);
            return Json(new { Data = this.ventasService.LoadCtaCorrienteHead(id, range.Start, range.End, filter), Success = true });
        }
        
        [HttpPost]
        public JsonResult GetCuentaCorrienteByDates(int id, DateTime? start, DateTime? end, CuentaCorrienteFilter filter, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ventasService.GetCuentaCorrienteByDates(range.Start, range.End, id, filter), Success = true });
            }
            else
            {
                PagingResponse<CuentaCorrienteItem> resp = new PagingResponse<CuentaCorrienteItem>();
                resp.Page = paging.page;
                resp.Records = this.ventasService.GetCuentaCorrienteByDates(range.Start, range.End, id, filter);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAllByClienteFilterNC(int? IdCliente, int? Tipo, int? NoTipo, ComprobantesACancelarFilter Cobrada, PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.ventasService.GetAllByClienteFilterNC(IdCliente, Tipo, NoTipo, Cobrada),
                    Success = true
                });
            }
            else
            {
                PagingResponse<ComprobanteVenta> resp = new PagingResponse<ComprobanteVenta>();
                resp.Page = paging.page;
                resp.Records = this.ventasService.GetAllByClienteFilterNC(IdCliente, Tipo, NoTipo, Cobrada).Select(x => new ComprobanteVenta()
                {
                    Id = x.Id,
                    Cliente = x.Cliente,
                    Estado = x.Estado,
                    Fecha = x.Fecha,
                    Numero = x.Numero,
                    CondicionVenta = x.CondicionVenta,
                    Letra = x.Letra,
                    IVA = x.IVA,
                    Tipo = x.Tipo,
                    Subtotal = x.Subtotal,
                    Total = x.Total,
                    TotalCobrado = x.TotalCobrado
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        #endregion

        #region CommonUtils
        public string GetBarcode(ComprobanteVenta c)
        {
            if (c.CAE == null) return null;
            if (!c.Talonario.PuntoVenta.HasValue) return null;

            string data = "";
            data += c.Organizacion.CUIT.Replace("-", "");
            data += Convert.ToInt32(c.Tipo.AfipData).ToString("00");
            data += c.Talonario.PuntoVenta.Value.ToString("0000");
            data += c.CAE;
            data += c.VencimientoCAE.Value.ToString("yyyyMMdd");
            data += AfipUtils.CalcularDigitoVerificadorCB(data).ToString();

            BarcodeLib.Barcode b = new BarcodeLib.Barcode();

            // Configuration of Barcode-
            BarcodeLib.TYPE type = BarcodeLib.TYPE.Interleaved2of5;
            int width = 400;
            int height = 50;
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
            b.IncludeLabel = true;
            System.Drawing.Color fore = System.Drawing.ColorTranslator.FromHtml("#000000");
            System.Drawing.Color bg = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            System.Drawing.Image barcodeImage = null;

            try
            {
                string filename = "/Images/CB/" + Security.GetOrganizacion().Id.ToString() + "_" + c.GetLetraNumero() + ".png";
                barcodeImage = b.Encode(type, data.Trim(), fore, bg, width, height);
                barcodeImage.Save(Server.MapPath("~"+filename), ImageFormat.Png);
                return filename;
            }
            catch(Exception e)
            {
                Logger.LogError("Error al generar el codigo de barras", e);
                return null;
            }
            finally
            {
                if (barcodeImage != null)
                {
                    //Clean up / Dispose...
                    barcodeImage.Dispose();
                }
            }
        }
        #endregion

    }
}
