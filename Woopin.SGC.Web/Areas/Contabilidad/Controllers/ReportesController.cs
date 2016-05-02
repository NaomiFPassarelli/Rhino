using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;
using Woopin.SGC.Web.Filters;

namespace Woopin.SGC.Web.Areas.Contabilidad.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ReportesController : BaseController
    {

        private readonly IContabilidadReportService ContabilidadReportingService;
        private readonly IContabilidadConfigService contabilidadConfigService;
        public ReportesController(IContabilidadReportService ContabilidadReportingService, IContabilidadConfigService contabilidadConfigService)
        {
            this.ContabilidadReportingService = ContabilidadReportingService;
            this.contabilidadConfigService = contabilidadConfigService;
        }

        #region Libro IVA Compras

        public ActionResult LibroIVACompras()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllLibroIVACompras(DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ContabilidadReportingService.GetAllLibroIVACompras(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<LibroIVA> resp = new PagingResponse<LibroIVA>();
                resp.Page = paging.page;
                resp.Records = this.ContabilidadReportingService.GetAllLibroIVACompras(range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                resp.userdata = new
                {
                    IVA = resp.Records.Sum(x => x.IVA),
                    Total = resp.Records.Sum(x => x.Total),
                    LetraNumero = "Totales:",
                    ImporteIVA27 = resp.Records.Sum(x => x.ImporteIVA27),
                    ImporteIVA21 = resp.Records.Sum(x => x.ImporteIVA21),
                    ImporteIVA105 = resp.Records.Sum(x => x.ImporteIVA105),
                    ImporteExento = resp.Records.Sum(x => x.ImporteExento),
                    ImporteGravado = resp.Records.Sum(x => x.ImporteGravado)
                };
                return Json(resp);
            }
        }
        #endregion

        #region Libro IVA Ventas

        public ActionResult LibroIVAVentas()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllLibroIVAVentas(DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ContabilidadReportingService.GetAllLibroIVAVentas(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<LibroIVA> resp = new PagingResponse<LibroIVA>();
                resp.Page = paging.page;
                resp.Records = this.ContabilidadReportingService.GetAllLibroIVAVentas(range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                resp.userdata = new
                {
                    IVA = resp.Records.Sum(x => x.IVA),
                    Total = resp.Records.Sum(x => x.Total),
                    LetraNumero = "Totales:",
                    ImporteIVA27 = resp.Records.Sum(x => x.ImporteIVA27),
                    ImporteIVA21 = resp.Records.Sum(x => x.ImporteIVA21),
                    ImporteIVA105 = resp.Records.Sum(x => x.ImporteIVA105),
                    ImporteExento = resp.Records.Sum(x => x.ImporteExento),
                    ImporteGravado = resp.Records.Sum(x => x.ImporteGravado)
                };
                return Json(resp);
            }
        }
        #endregion

        #region Sumas y Saldos

        public ActionResult SumasYSaldos()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllSumasYSaldos(DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ContabilidadReportingService.GetAllSumasYSaldos(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<SumaSaldo> resp = new PagingResponse<SumaSaldo>();
                resp.Page = paging.page;
                resp.Records = this.ContabilidadReportingService.GetAllSumasYSaldos(range.Start, range.End).Select(
                    x => new SumaSaldo() { 
                    Codigo = x.Codigo,
                    Corriente = x.Corriente,
                    CuentaId = x.CuentaId,
                    Debe = x.Debe,
                    Haber = x.Haber,
                    NombreCuenta = x.NombreCuenta,
                    Numero = x.Numero,
                    Rubro = x.Rubro,
                    Saldo = x.Saldo,
                    SaldoActual = x.SaldoAnterior + x.Saldo,
                    SaldoAnterior = x.SaldoAnterior,
                    SubRubro = x.SubRubro
                    }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public ActionResult SumasYSaldosArbol()
        {
            return View();
        }

        public JsonResult GetSumasYSaldosArbol(PagingRequest paging, DateTime start, DateTime end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ContabilidadReportingService.GetAllSumasYSaldosTree(range.Start, range.End), Success = true });
            }
            else
            {
                IList<SumaSaldo> ssa = this.ContabilidadReportingService.GetAllSumasYSaldosTree(range.Start, range.End);
                PagingResponse<dynamic> resp = new PagingResponse<dynamic>();
                resp.Page = paging.page;
                resp.Records = ssa.Select(x => new
                {
                    Codigo = x.Codigo,
                    Corriente = x.Corriente,
                    CuentaId = x.CuentaId,
                    Debe = x.Debe,
                    Haber = x.Haber,
                    NombreCuenta = x.NombreCuenta,
                    Numero = x.Numero,
                    Rubro = x.Rubro,
                    Saldo = x.Saldo,
                    SaldoActual = x.SaldoAnterior + x.Saldo,
                    SaldoAnterior = x.SaldoAnterior,
                    SubRubro = x.SubRubro,
                    level = CuentaContableHelper.GetLevel(x.Numero, x.SubRubro, x.Corriente, x.Rubro),
                    parent = CuentaContableHelper.GetParentSumaYSaldo(x, ssa),
                    isLeaf = x.Numero > 0,
                    loaded = true,
                    expanded = CuentaContableHelper.GetLevel(x.Numero, x.SubRubro, x.Corriente, x.Rubro) <= 1
                }).OrderBy(x => x.Codigo).ToList<dynamic>();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        #endregion

        #region Balance
        public ActionResult Balance()
        {
            return View();
        }

        public JsonResult GetBalance(PagingRequest paging, DateTime start, DateTime end)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ContabilidadReportingService.GetBalance(range.Start, range.End), Success = true });
            }
            else
            {
                IList<SumaSaldo> balance = this.ContabilidadReportingService.GetBalance(range.Start, range.End);
                PagingResponse<dynamic> resp = new PagingResponse<dynamic>();
                resp.Page = paging.page;
                resp.Records = balance.Select(x => new
                {
                    Corriente = x.Corriente,
                    Codigo = x.Codigo,
                    CuentaId = x.CuentaId,
                    NombreCuenta = x.NombreCuenta,
                    Debe = x.Debe,
                    Haber = x.Haber,
                    Saldo = x.Saldo,
                    SubRubro = x.SubRubro,
                    Numero = x.Numero,
                    Rubro = x.Rubro,
                    level = CuentaContableHelper.GetLevel(x.Numero, x.SubRubro, x.Corriente, x.Rubro),
                    parent = CuentaContableHelper.GetParentSumaYSaldo(x, balance),
                    isLeaf = x.Numero > 0,
                    loaded = true,
                    expanded = CuentaContableHelper.GetLevel(x.Numero, x.SubRubro, x.Corriente, x.Rubro) <= 1
                }).OrderBy(x => x.Codigo).ToList<dynamic>();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        #endregion

        #region Libro Mayor de Proveedores

        public ActionResult MayorProveedores()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllMayorProveedores(DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ContabilidadReportingService.GetAllMayorProveedores(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<MayorItem> resp = new PagingResponse<MayorItem>();
                resp.Page = paging.page;
                resp.Records = this.ContabilidadReportingService.GetAllMayorProveedores(range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        #endregion

        #region Libro Mayor de Clientes

        public ActionResult MayorClientes()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllMayorClientes(DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.ContabilidadReportingService.GetAllMayorClientes(range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<MayorItem> resp = new PagingResponse<MayorItem>();
                resp.Page = paging.page;
                resp.Records = this.ContabilidadReportingService.GetAllMayorClientes(range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }
        #endregion
    }
}
