using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Tesoreria.Controllers
{
    public class DepositosController : BaseController
    {
        private readonly ITesoreriaService TesoreriaService;
        private readonly ICommonConfigService commonConfigService;
        private readonly IContabilidadService ContabilidadService;

        public DepositosController(ITesoreriaService TesoreriaService, ICommonConfigService commonConfigService,
            IContabilidadService ContabilidadService)
        {
            this.TesoreriaService = TesoreriaService;
            this.commonConfigService = commonConfigService;
            this.ContabilidadService = ContabilidadService;
        }

        //
        // GET: /Tesoreria/Depositos/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.NumeroRef = this.TesoreriaService.GetProximoNumeroReferenciaDeposito();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Deposito Deposito)
        {
            try
            {
                ClearNotValidatedProperties(Deposito);
                this.TesoreriaService.AddDeposito(Deposito);
                Deposito.Asiento = null;
                Deposito.Cheques = null;
                Deposito.Usuario = null;
                return Json(new { Success = true, Deposito = Deposito });
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
        public JsonResult GetDepositoFilter(PagingRequest paging, int idCuentaBancaria, DateTime? start, DateTime? end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.TesoreriaService.GetDepositoFilter(idCuentaBancaria, range.Start, range.End )
                    .Select( x => new Deposito()
                    { 
                        NumeroBoleta = x.NumeroBoleta,
                        Cuenta = x.Cuenta,
                        FechaCreacion = x.FechaCreacion,
                        Id = x.Id,
                        Concepto = x.Concepto,
                        Total = x.Total
                    }), Success = true });
            }
            else
            {
                PagingResponse<Deposito> resp = new PagingResponse<Deposito>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetDepositoFilter(idCuentaBancaria, range.Start, range.End).Select( x => new Deposito()
                    { 
                        NumeroBoleta = x.NumeroBoleta,
                        Cuenta = x.Cuenta,
                        FechaCreacion = x.FechaCreacion,
                        Id = x.Id,
                        Concepto = x.Concepto,
                        Total = x.Total
                    }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult ChequesEnCartera()
        {
            return View();
        }
        public JsonResult GetAllChequesEnCartera(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.TesoreriaService.GetAllChequesEnCartera()
                    .Select( x => new Cheque()
                    { 
                        Banco = x.Banco,
                        Cliente = x.Cliente,
                        Estado = x.Estado,
                        Fecha = x.Fecha,
                        FechaCreacion = x.FechaCreacion,
                        FechaVencimiento = x.FechaVencimiento,
                        Id = x.Id,
                        Importe = x.Importe,
                        Numero = x.Numero,
                        NumeroCuenta = x.NumeroCuenta,
                        Propio = x.Propio
                    }), Success = true });
            }
            else
            {
                PagingResponse<Cheque> resp = new PagingResponse<Cheque>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaService.GetAllChequesEnCartera().Select(x => new Cheque()
                {
                    Banco = x.Banco,
                    Cliente = x.Cliente,
                    Estado = x.Estado,
                    Fecha = x.Fecha,
                    FechaCreacion = x.FechaCreacion,
                    FechaVencimiento = x.FechaVencimiento,
                    Id = x.Id,
                    Importe = x.Importe,
                    Numero = x.Numero,
                    NumeroCuenta = x.NumeroCuenta,
                    Propio = x.Propio
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        public ActionResult Detalle(int Id, bool? opensDialog)
        {
            Deposito d = this.TesoreriaService.GetDepositoCompleto(Id);
            if (d == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(d);
        }

    }
}
