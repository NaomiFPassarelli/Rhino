using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;

namespace Woopin.SGC.Web.Areas.Ventas.Controllers
{
    public class CobranzasController : BaseController
    {
        private readonly IVentasConfigService ventasConfigService;
        private readonly IVentasService ventasService;
        private readonly ITesoreriaConfigService tesoreriaConfigService;
        private readonly ICommonConfigService commonConfigService;
        private readonly IContabilidadService ContabilidadService;

        public CobranzasController(IVentasService ventasService, IVentasConfigService ventasConfigService,
            ICommonConfigService commonConfigService, IContabilidadConfigService contabilidadService,
            ITesoreriaConfigService tesoreriaConfigService, IContabilidadService ContabilidadService)
        {
            this.ventasConfigService = ventasConfigService;
            this.ventasService = ventasService;
            this.tesoreriaConfigService = tesoreriaConfigService;
            this.commonConfigService = commonConfigService;
            this.ContabilidadService = ContabilidadService;
        }

        //
        // GET: 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            IList<ComboItem> tipos = this.commonConfigService.GetItemsByCombo(ComboType.TipoCobranza);
            ViewBag.Tipo = tipos.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data, Selected = tipos.Count() == 1 }).ToList();
            ViewBag.NumeroRef = this.ventasService.GetProximoIdCobranza();
            ViewBag.Valores = new SelectCombo() { Items = this.tesoreriaConfigService.GetAllValores().Select(x => new SelectComboItem() { id = x.Id, text = x.Nombre, additionalData = x.TipoValor.Data }).ToList() };
            return View();
        }


        [HttpPost]
        public JsonResult Nuevo(Cobranza Cobranza)
        {
            try
            {
                string Numero = Cobranza.Numero;
                this.ventasService.AddCobranza(Cobranza);
                Cobranza.Asiento = null;
                if (Numero != Cobranza.Numero)
                {
                    return Json(new { Success = true, NuevoNumero = Cobranza.Numero, Cobranza = Cobranza });
                }
                return Json(new { Success = true, Cobranza = Cobranza });
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
            Cobranza Cobranza = this.ventasService.GetCobranzaCompleto(Id);
            if (Cobranza == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.EjercicioDesbloqueado = this.ContabilidadService.ControlarIngreso(Cobranza.Fecha);
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(Cobranza);
        }

        [HttpPost]
        public JsonResult GetNumeroByTalonario(string Talonario)
        {
            try 
            { 
                string numero = this.ventasService.GetProximoRecibo(Talonario);
                return Json(new { Numero = numero, Success = true });
            }
            catch
            {
                return Json(new { Success = false });
            }
        }

        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.ventasService.GetAllCobranzas(), Success = true });
            }
            else
            {
                PagingResponse<Cobranza> resp = new PagingResponse<Cobranza>();
                resp.Page = paging.page;
                resp.Records = this.ventasService.GetAllCobranzas().Select(x => new Cobranza()
                    {
                        Id = x.Id,
                        Cliente = x.Cliente,
                        Fecha = x.Fecha,
                        Numero = x.Numero,
                        Tipo = x.Tipo,
                        Total = x.Total,
                        Detalle = x.Detalle,
                        Estado = x.Estado
                    }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAllByCliente(int IdCliente, DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.ventasService.GetAllCobranzaByCliente(IdCliente, range.Start, range.End).Select(x => new Cobranza()
                {
                    Id = x.Id,
                    Cliente = x.Cliente,
                    Fecha = x.Fecha,
                    Numero = x.Numero,
                    Tipo = x.Tipo,
                    Total = x.Total,
                    Detalle = x.Detalle,
                    Estado = x.Estado
                }).ToList(), Success = true });
            }
            else
            {
                PagingResponse<Cobranza> resp = new PagingResponse<Cobranza>();
                resp.Page = paging.page;
                resp.Records = this.ventasService.GetAllCobranzaByCliente(IdCliente, range.Start, range.End).Select(x => new Cobranza()
                {
                    Id = x.Id,
                    Cliente = x.Cliente,
                    Fecha = x.Fecha,
                    Numero = x.Numero,
                    Tipo = x.Tipo,
                    Total = x.Total,
                    Detalle = x.Detalle,
                    Estado = x.Estado
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult AnularCobranza(int IdCobranza)
        {
            try
            {
                this.ventasService.AnularCobranza(IdCobranza);
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