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
    public class IngresoStockController : BaseController
    {
        private readonly IStockService StockService;
        public IngresoStockController(IStockService StockService)
        {
            this.StockService = StockService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(IngresoStock IngresoStock)
        {
            try
            {
                ClearNotValidatedProperties(IngresoStock);
                if (ModelState.IsValid)
                {
                    this.StockService.AddIngresoStock(IngresoStock);
                    return Json(new { Success = true, IngresoStock = IngresoStock });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Ingreso de Stock", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
        //            this.StockService.DeleteIngresosStock(Ids);
        //            return Json(new { Success = true });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la IngresoStock, vuelva a inetntarlo." });
        //        }
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }
        //}


        //public ActionResult Editar(int Id)
        //{
        //    IngresoStock IngresoStock = this.StockService.GetIngresoStock(Id);
        //    return View(IngresoStock);
        //}


        //[HttpPost]
        //public JsonResult Editar(IngresoStock IngresoStock)
        //{
        //    try
        //    {
        //        ClearNotValidatedProperties(IngresoStock);
        //        if (ModelState.IsValid)
        //        {
        //            this.StockService.UpdateIngresoStock(IngresoStock);
        //            return Json(new { Success = true, IngresoStock = IngresoStock });
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
                return Json(new { Data = this.StockService.GetAllIngresosStock(Id, range.Start, range.End), Success = true });
            }
            else
            {
                PagingResponse<IngresoStock> resp = new PagingResponse<IngresoStock>();
                resp.Page = paging.page;
                resp.Records = this.StockService.GetAllIngresosStock(Id, range.Start, range.End);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        //[HttpPost]
        //public JsonResult GetIngresosStock(SelectComboRequest req)
        //{
        //    return Json(new { Data = this.StockService.GetAllIngresosStockByFilterCombo(req), Success = true });
        //}

        [HttpPost]
        public JsonResult GetIngresoStock(int idIngresoStock)
        {
            return Json(new { Data = this.StockService.GetIngresoStock(idIngresoStock), Success = true });
        }
        



    }
}
