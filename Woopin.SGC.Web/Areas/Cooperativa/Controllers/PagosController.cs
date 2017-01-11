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
using Woopin.SGC.Common.Models;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace Woopin.SGC.Web.Areas.Cooperativa.Controllers
{
    public class PagosController : BaseController
    {
        private readonly ICooperativaService CooperativaService;
        private readonly ICooperativaConfigService CooperativaConfigService;
        private readonly ICommonConfigService commonConfigService;
        private readonly ISystemService SystemService;

        public PagosController(ICooperativaService CooperativaService, ICommonConfigService commonConfigService,
                                ISystemService SystemService, ICooperativaConfigService CooperativaConfigService)
        {
            this.CooperativaService = CooperativaService; 
            this.commonConfigService = commonConfigService;
            this.SystemService = SystemService;
            this.CooperativaConfigService = CooperativaConfigService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PorVencer()
        {
            return View();
        }

        public ActionResult Vencidos()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.NumeroRef = this.CooperativaService.GetProximoNumeroReferenciaPago();
            ViewBag.Organizacion = Security.GetOrganizacion();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Pago Pago)
        {
            try
            {
                foreach (AdicionalPago AP in Pago.AdicionalesPago)
                {
                    decimal n;
                    bool isNumeric = decimal.TryParse(AP.Total.ToString(), out n);
                    if (!isNumeric)
                    {
                        return Json(new { Success = false });
                    }
                }
                ClearNotValidatedProperties(Pago);

                //if (ModelState.IsValid)
                //{
                this.CooperativaService.AddPago(Pago);
                if (Pago.Id != Pago.NumeroReferencia)
                {
                    return Json(new { Success = true, NumeroRef = Pago.Id, Pago = Pago });
                }
                
                return Json(new { Success = true, Pago = Pago });
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


        //[HttpPost]
        //public JsonResult NuevoAutomatico(Pago Pago, int CantidadCuotasAPagar)
        //{
        //    try
        //    {
        //        ClearNotValidatedProperties(Pago);
        //        if (ModelState.IsValid)
        //        {
        //            this.CooperativaService.AddPagos(Pago, CantidadCuotasAPagar);
        //            return Json(new { Success = true, Pago = Pago });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Pago", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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

        //public JsonResult ImprimirGrid(List<int> Ids, string outPathname, string password, string folderName)
        //{
        //    foreach (int Id in Ids)
        //    {
        //        //Crear la carpeta y todos los archivos adentro
        //        string outpath = this.ArmarComprobantePDF(Id);
        //        Response.Clear();
        //        Response.AddHeader("content-disposition", "attachment; filename=\"" + Path.GetFileName(outpath) + "\"");
        //        Response.WriteFile(outpath);
        //        Response.ContentType = "";
        //        Response.End();

        //    }



        //    //FileStream fsOut = File.Create(outPathname);
        //    FileStream fsOut = new FileStream();
        //    fsOut.Write(Response)

        //    //FileStream fsOut = Response.OutputStream;
        //    //ZipOutputStream zipStream = new ZipOutputStream(fsOut);

        //    //ZipOutputStream zipStream = new ZipOutputStream(Response.OutputStream);
        //    ZipOutputStream zipStream = new ZipOutputStream(fsOut);

        //    zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

        //    zipStream.Password = password;  // optional. Null is the same as not setting. Required if using AES.

        //    // This setting will strip the leading part of the folder path in the entries, to
        //    // make the entries relative to the starting folder.
        //    // To include the full path for each entry up to the drive root, assign folderOffset = 0.
        //    int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

        //    CompressFolder(folderName, zipStream, folderOffset);

        //    zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
        //    zipStream.Close();




        //    return Json(new { Success = true });
            
        //}

        // Recurses down the folder structure
        //
        //private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
        //{

        //    string[] files = Directory.GetFiles(path);

        //    foreach (string filename in files)
        //    {

        //        FileInfo fi = new FileInfo(filename);

        //        string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
        //        entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
        //        ZipEntry newEntry = new ZipEntry(entryName);
        //        newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

        //        // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
        //        // A password on the ZipOutputStream is required if using AES.
        //        //   newEntry.AESKeySize = 256;

        //        // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
        //        // you need to do one of the following: Specify UseZip64.Off, or set the Size.
        //        // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
        //        // but the zip will be in Zip64 format which not all utilities can understand.
        //        //   zipStream.UseZip64 = UseZip64.Off;
        //        newEntry.Size = fi.Length;

        //        zipStream.PutNextEntry(newEntry);

        //        // Zip the file in buffered chunks
        //        // the "using" will close the stream even if an exception occurs
        //        byte[] buffer = new byte[4096];
        //        //using (FileStream streamReader = File.OpenRead(filename))
        //        //{
        //        //    StreamUtils.Copy(streamReader, zipStream, buffer);
        //        //}
        //        zipStream.CloseEntry();
        //    }
        //    string[] folders = Directory.GetDirectories(path);
        //    foreach (string folder in folders)
        //    {
        //        CompressFolder(folder, zipStream, folderOffset);
        //    }
        //}



        [HttpPost]
        public JsonResult Eliminar(List<int> Ids)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.CooperativaService.DeletePagos(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Pago, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        [HttpPost]
        public JsonResult GetAll(PagingRequest paging, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.CooperativaService.GetAllPagos(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<Pago> resp = new PagingResponse<Pago>();
                resp.Page = paging.page;
                resp.Records = this.CooperativaService.GetAllPagos(range.Start, range.End).Select(x => new Pago()
                {
                    Id = x.Id,
                    Total = x.Total,
                    TotalDescuentos = x.TotalDescuentos,
                    TotalAnticipo = x.TotalAnticipo,
                    FechaCreacion = x.FechaCreacion,
                    NumeroReferencia = x.NumeroReferencia,
                    Asociado = x.Asociado,
                    FechaPago = x.FechaPago,
                    FechaPeriodo = x.FechaPeriodo
                    //AdicionalesPago = x.AdicionalesPago
                }).ToList();
                

                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }


        [HttpPost]
        public JsonResult GetAllPorVencer(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.CooperativaService.GetAllPorVencer(), Success = true });
            }
            else
            {
                PagingResponse<Pago> resp = new PagingResponse<Pago>();
                resp.Page = paging.page;
                resp.Records = this.CooperativaService.GetAllPorVencer()
                    .Select(x => new Pago()
                {
                //    Id = x.Id,
                //    Total = x.Total,
                //    TotalDescuentos = x.TotalDescuentos,
                //    TotalAnticipo = x.TotalAnticipo,
                //    FechaCreacion = x.FechaCreacion,
                //    NumeroReferencia = x.NumeroReferencia,
                    Asociado = x,
                //    FechaPago = x.FechaPago,
                //    FechaPeriodo = x.FechaPeriodo
                //    //AdicionalesPago = x.AdicionalesPago
                }).ToList();


                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAllVencidos(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.CooperativaService.GetAllVencidos(), Success = true });
            }
            else
            {
                PagingResponse<Pago> resp = new PagingResponse<Pago>();
                resp.Page = paging.page;
                resp.Records = this.CooperativaService.GetAllVencidos()
                    .Select(x => new Pago()
                    {
                        //    Id = x.Id,
                        //    Total = x.Total,
                        //    TotalDescuentos = x.TotalDescuentos,
                        //    TotalAnticipo = x.TotalAnticipo,
                        //    FechaCreacion = x.FechaCreacion,
                        //    NumeroReferencia = x.NumeroReferencia,
                        Asociado = x,
                        //    FechaPago = x.FechaPago,
                        //    FechaPeriodo = x.FechaPeriodo
                        //    //AdicionalesPago = x.AdicionalesPago
                    }).ToList();


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
            string filename = r.Asociado + " " + r.NumeroPago + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "PagosAsociados/" + o.Id.ToString());
            return OutputPath;
        }

        public ActionResult VersionImprimible(int Id, bool? opensDialog)
        {
            Pago r = this.CooperativaService.GetPago(Id);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }

        //public JsonResult LoadHeader()
        //{ 
        //    Asociado Header = this.CooperativaConfigService.LoadHeader();
        //    //return Json( new { Data =  Success = true });
        //    return Json(new { Data = (Header != null ? Header : null ), Success = true });
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
            IList<Pago> r = this.CooperativaService.GetPagos(Ids);
            ViewBag.BaseURL = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            Organizacion o = this.SystemService.GetOrganizacion(r.First().Organizacion.Id);
            if (o.ImagePath != null)
            {
                string logoorgpath = "/" + o.ImagePath;
                ViewBag.BaseLogoOrg = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, logoorgpath);
            }

            string html = RenderViewToString("VersionImprimibles", r);
            string filename = "PagosCombinados" + " " + r.First().Organizacion.RazonSocial + ".pdf";
            string OutputPath = HtmlToPDF.Generate(html, filename, "PagosCombinados/" + o.Id.ToString());
            return OutputPath;
        }

        public ActionResult VersionImprimibles(IList<int> Ids, bool? opensDialog)
        {
            IList<Pago> r = this.CooperativaService.GetPagos(Ids);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(r);
        }


    }
}
