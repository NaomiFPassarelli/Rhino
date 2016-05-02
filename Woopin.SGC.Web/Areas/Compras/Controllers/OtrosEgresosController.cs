using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Compras.Controllers
{
    public class OtrosEgresosController : BaseController
    {
        private readonly IComprasService ComprasService;
        private readonly ICommonConfigService commonConfigService;
        private readonly ITesoreriaConfigService tesoreriaSerivce;
        private readonly IContabilidadService ContabilidadService; 

        public OtrosEgresosController(IComprasService ComprasService, ICommonConfigService commonConfigService, ITesoreriaConfigService tesoreriaSerivce,
            IContabilidadService ContabilidadService)
        {
            this.ComprasService = ComprasService;
            this.commonConfigService = commonConfigService;
            this.tesoreriaSerivce = tesoreriaSerivce;
            this.ContabilidadService = ContabilidadService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.NumeroRef = this.ComprasService.GetOtroEgresoProximoNumeroReferencia();
            ViewBag.Valores = new SelectCombo() { Items = this.tesoreriaSerivce.GetAllValores().Select(x => new SelectComboItem() { id = x.Id, text = x.Nombre, additionalData = x.TipoValor.Data }).ToList() };
            return View();
        }


        [HttpPost]
        public JsonResult Nuevo(OtroEgreso OtroEgreso)
        {
            try
            {
                int NumRef = OtroEgreso.NumeroReferencia;
                this.ComprasService.AddOtroEgreso(OtroEgreso);
                OtroEgreso.Asiento = null;
                if (OtroEgreso.Id != NumRef)
                {
                    return Json(new { Success = true, NumeroRef = OtroEgreso.Id, OtroEgreso = OtroEgreso });
                }
                return Json(new { Success = true, OtroEgreso = OtroEgreso });
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


        public ActionResult Detalle(int Id, bool? opensDialog)
        {
            OtroEgreso oe = this.ComprasService.GetOtroEgresoCompleto(Id);
            if (oe == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(oe);
        }



        [HttpPost]
        public JsonResult GetAllByProveedor(int Id, DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.ComprasService.GetAllOtroEgresoByProveedor(Id, range.Start, range.End).Select(x => new OtroEgreso()
                    {
                        Id = x.Id,
                        Proveedor = x.Proveedor,
                        Estado = x.Estado,
                        Fecha = x.Fecha,
                        Total = x.Total
                    }).ToList(),
                    Success = true
                });
            }
            else
            {
                PagingResponse<OtroEgreso> resp = new PagingResponse<OtroEgreso>();
                resp.Page = paging.page;
                resp.Records = this.ComprasService.GetAllOtroEgresoByProveedor(Id, range.Start, range.End).Select(x => new OtroEgreso()
                {
                    Id = x.Id,
                    Proveedor = x.Proveedor,
                    Estado = x.Estado,
                    Fecha = x.Fecha,
                    Total = x.Total
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult AnularOE(int IdOE)
        {
            try
            {
                this.ComprasService.AnularOE(IdOE);
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
    
    
    
    }
}
