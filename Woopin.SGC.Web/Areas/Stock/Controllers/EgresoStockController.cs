using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Stock;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Stock.Controllers
{
    public class EgresoStockController : BaseController
    {
        private readonly IStockService StockService;
        public EgresoStockController(IStockService StockService)
        {
            this.StockService = StockService;
        }

        //
        // GET: /Stock/Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(EgresoStock EgresoStock)
        {
            try
            {
                ClearNotValidatedProperties(EgresoStock);
                if (ModelState.IsValid)
                {
                    this.StockService.AddEgresoStock(EgresoStock);
                    return Json(new { Success = true, EgresoStock = EgresoStock });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Egreso de Stock", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
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
        //            this.StockService.DeleteEgresosStock(Ids);
        //            return Json(new { Success = true });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la EgresoStock, vuelva a inetntarlo." });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }
        //}


        //public ActionResult Editar(int Id)
        //{
        //    EgresoStock EgresoStock = this.StockService.GetEgresoStock(Id);
        //    return View(EgresoStock);
        //}


        //[HttpPost]
        //public JsonResult Editar(EgresoStock EgresoStock)
        //{
        //    try
        //    {
        //        ClearNotValidatedProperties(EgresoStock);
        //        if (ModelState.IsValid)
        //        {
        //            if (EgresoStock.Inventario && EgresoStock.Estado < 0)
        //            {
        //                return Json(new { Success = false, ErrorMessage = "Cuando el EgresoStock es para inventario se debe seleccionar el Estado" });
        //            }
        //            this.StockService.UpdateEgresoStock(EgresoStock);
        //            return Json(new { Success = true, EgresoStock = EgresoStock });
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

        [HttpPost]
        public JsonResult GetAll(int Id, DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new { Data = this.StockService.GetAllEgresosStock(Id, range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<EgresoStock> resp = new PagingResponse<EgresoStock>();
                resp.Page = paging.page;
                resp.Records = this.StockService.GetAllEgresosStock(Id, range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        //[HttpPost]
        //public JsonResult GetEgresosStock(SelectComboRequest req)
        //{
        //    return Json(new { Data = this.StockService.GetAllEgresosStockByFilterCombo(req), Success = true });
        //}

        [HttpPost]
        public JsonResult GetEgresoStock(int idEgresoStock)
        {
            return Json(new { Data = this.StockService.GetEgresoStock(idEgresoStock), Success = true });
        }
        



    }
}
