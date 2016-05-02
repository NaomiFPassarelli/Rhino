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
    public class RubroArticulosController : BaseController
    {
        private readonly IStockConfigService StockConfigService;
        public RubroArticulosController(IStockConfigService StockConfigService)
        {
            this.StockConfigService = StockConfigService;
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
        public JsonResult Nuevo(RubroArticulo RubroArticulo)
        {
            try
            {
                ClearNotValidatedProperties(RubroArticulo);
                if (ModelState.IsValid)
                {
                    this.StockConfigService.AddRubroArticulo(RubroArticulo);
                    return Json(new { Success = true, RubroArticulo = RubroArticulo });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del Rubro", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.StockConfigService.DeleteRubroArticulos(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Rubro, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            RubroArticulo RubroArticulo = this.StockConfigService.GetRubroArticulo(Id);
            return View(RubroArticulo);
        }


        [HttpPost]
        public JsonResult Editar(RubroArticulo RubroArticulo)
        {
            try
            {
                ClearNotValidatedProperties(RubroArticulo);
                if (ModelState.IsValid)
                {
                    this.StockConfigService.UpdateRubroArticulo(RubroArticulo);
                    return Json(new { Success = true, RubroArticulo = RubroArticulo });
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
                return Json(new { Data = this.StockConfigService.GetAllRubroArticulos(), Success = true });
            }
            else
            {
                PagingResponse<RubroArticulo> resp = new PagingResponse<RubroArticulo>();
                resp.Page = paging.page;
                resp.Records = this.StockConfigService.GetAllRubroArticulos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.StockConfigService.GetAllRubroArticulosByFilterCombo(req), Success = true });
        }

        [HttpPost]
        public JsonResult Get(int idRubroArticulo)
        {
            return Json(new { Data = this.StockConfigService.GetRubroArticulo(idRubroArticulo), Success = true });
        }
        



    }
}
