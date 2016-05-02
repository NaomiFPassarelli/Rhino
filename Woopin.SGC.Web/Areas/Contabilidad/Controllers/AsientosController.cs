using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Contabilidad.Controllers
{
    public class AsientosController : BaseController
    {
        private readonly IContabilidadService contabilidadService;
        private readonly IContabilidadConfigService contabilidadConfigService;
        public AsientosController(IContabilidadService contabilidadService, IContabilidadConfigService contabilidadConfigService)
        {
            this.contabilidadService = contabilidadService;
            this.contabilidadConfigService = contabilidadConfigService;
        }

        //
        // GET: /Contabilidad/Asientos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.Ejercicios = this.contabilidadConfigService.GetAllAvailableEjercicios().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre }).ToList();
            return View();
        }


        [HttpPost]
        public JsonResult Nuevo(Asiento Asiento)
        {
            try
            {
                Asiento.FechaCreacion = DateTime.Now; //esto puede ir en el helper Asiento
                Asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
                int Numero = Asiento.NumeroReferencia;
                this.contabilidadService.AddAsiento(Asiento);
                Asiento.Items = null;
                // Problema de referencia circular, no lo devuelvo
                if (Numero != Asiento.NumeroReferencia)
                {
                    return Json(new { Success = true, NuevoNumero = Asiento.NumeroReferencia, Asiento = Asiento });
                }
                return Json(new { Success = true, Asiento = Asiento });
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

        public JsonResult GetProximoAsiento()
        {
            return Json(new { Data = this.contabilidadService.GetProximoIdAsiento() });
        }

        public ActionResult Detalle(int Id, bool? opensDialog)
        {
            Asiento a = this.contabilidadService.GetAsientoCompleto(Id);
            if (a == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            foreach (AsientoItem Item in a.Items)
            {
                Item.Asiento = null;
                Item.Cuenta.Organizacion = null;
            }
            //a.Fecha.ToShortDateString();
            a.NumeroReferencia = a.Id;
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(a);
        }

        public JsonResult GetAsientosFilter( DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.contabilidadService.GetAsientosFilter( range.Start, range.End)
                    .Select( x => new Asiento()
                    { Id = x.Id,
                      Fecha = x.Fecha,
                      Leyenda = x.Leyenda,
                      NumeroReferencia = x.Id
                    }), Success = true });
            }
            else
            {
                PagingResponse<Asiento> resp = new PagingResponse<Asiento>();
                resp.Page = paging.page;
                resp.Records = this.contabilidadService.GetAsientosFilter( range.Start, range.End).Select(x => new Asiento()
                {
                    Id = x.Id,
                    Fecha = x.Fecha,
                    Leyenda = x.Leyenda,
                    NumeroReferencia = x.Id
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        public ActionResult LibroMayor(int? CuentaId, string start, string end, bool? opensDialog)
        {
            ViewBag.IdCuenta = CuentaId.HasValue ? CuentaId.Value : 0;
            ViewBag.start = start;
            ViewBag.end = end;
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View();
        }

        [HttpPost]
        public JsonResult GetAsientosHeaderFilterCuenta(int id, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            return Json(new { Data = this.contabilidadService.GetAsientosHeaderFilterCuenta(id, range.Start, range.End), Success = true });
        }

        public JsonResult GetAsientosFilterCuenta(int id, DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.contabilidadService.GetAsientosFilterCuenta(id, range.Start, range.End),
                    Success = true
                });
            }
            else
            {
                PagingResponse<LibroMayor> resp = new PagingResponse<LibroMayor>();
                resp.Page = paging.page;
                resp.Records = this.contabilidadService.GetAsientosFilterCuenta(id, range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }


        [HttpPost]
        public JsonResult Anular(int IdAsiento)
        {
            try
            {
                this.contabilidadService.DeleteAsiento(IdAsiento);
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
