using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Stock;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Stock.Controllers
{
    public class ArticulosController : BaseController
    {
        private readonly IStockConfigService StockConfigService;
        public ArticulosController(IStockConfigService stockConfigService)
        {
            this.StockConfigService = stockConfigService;
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
        public JsonResult Nuevo(Articulo Articulo)
        {
            try
            {
                ClearNotValidatedProperties(Articulo);
                if (ModelState.IsValid)
                {
                    if (Articulo.Inventario && Articulo.Estado < 0)
                    {
                        return Json(new { Success = false, ErrorMessage = "Cuando el articulo es para inventario se debe seleccionar el Estado" });
                    }

                    this.StockConfigService.AddArticulo(Articulo);
                    return Json(new { Success = true, Articulo = Articulo });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Articulo", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
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
                    this.StockConfigService.DeleteArticulos(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Articulo, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Articulo Articulo = this.StockConfigService.GetArticulo(Id);
            return View(Articulo);
        }


        [HttpPost]
        public JsonResult Editar(Articulo Articulo)
        {
            try
            {
                ClearNotValidatedProperties(Articulo);
                if (ModelState.IsValid)
                {
                    if (Articulo.Inventario && Articulo.Estado < 0)
                    {
                        return Json(new { Success = false, ErrorMessage = "Cuando el articulo es para inventario se debe seleccionar el Estado" });
                    }
                    this.StockConfigService.UpdateArticulo(Articulo);
                    return Json(new { Success = true, Articulo = Articulo });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Algunos de los campos no se encuentran correctamente introducidos.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
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
                return Json(new { Data = this.StockConfigService.GetAllArticulos(), Success = true });
            }
            else
            {
                PagingResponse<Articulo> resp = new PagingResponse<Articulo>();
                resp.Page = paging.page;
                resp.Records = this.StockConfigService.GetAllArticulos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetArticulos(SelectComboRequest req)
        {
            return Json(new { Data = this.StockConfigService.GetAllArticulosByFilterCombo(req), Success = true });
        }

        [HttpPost]
        public JsonResult GetArticulo(int idArticulo)
        {
            Articulo a = this.StockConfigService.GetArticulo(idArticulo);
            return Json(new { Data = (a != null && a.Activo) ? a : null, Success = true });
        }

        [HttpPost]
        public JsonResult GetArticulosConStock(SelectComboRequest req)
        {
            return Json(new { Data = this.StockConfigService.GetAllArticulosConStockByFilterCombo(req), Success = true });
        }

        [HttpPost]
        public JsonResult GetArticuloConStock(int idArticulo)
        {
            Articulo a = this.StockConfigService.GetArticuloConStock(idArticulo);
            return Json(new { Data = (a != null && a.Activo) ? a : null, Success = true });
        }


    }
}
