using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Stock;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Ventas.Controllers
{
    public class ListaPreciosController : BaseController
    {
        private readonly IVentasConfigService ventasConfigService;
        private readonly IStockConfigService stockConfigService;
        public ListaPreciosController(IVentasConfigService ventasConfigService, IStockConfigService stockConfigService)
        {
            this.ventasConfigService = ventasConfigService;
            this.stockConfigService = stockConfigService;
        }

        public ActionResult VerListaGrupos(int? Id)
        {
            ViewBag.IdCliente = Id.HasValue ? Id.Value : 0;
            return View();
        }
        public ActionResult VerLista(string Id)
        {
            ViewBag.Id = Id;
            if (Id.Contains(TipoListaPrecios.Cliente))
            {
                int IdCliente = Convert.ToInt32(Id.Replace("C", ""));
                ViewBag.Nombre = this.ventasConfigService.GetCliente(IdCliente).RazonSocial;
            }
            else if (Id.Contains(TipoListaPrecios.Grupo))
            {
                int IdCliente = Convert.ToInt32(Id.Replace("G", ""));
                ViewBag.Nombre = this.ventasConfigService.GetGrupoEconomico(IdCliente).Nombre;
            }
            return View();
        }
        public ActionResult VerListaDefault()
        {
            return View();
        }

        public ActionResult VerListaCliente(int? Id)
        {
            ViewBag.IdCliente = Id.HasValue ? Id.Value : 0;
            return View();
        }

        public ActionResult Editar(int Id, string IdCliente)
        {
            ListaPreciosItem item = this.ventasConfigService.GetListaPreciosItem(Id);
            ViewBag.IdCliente = IdCliente;
            return View(item);
        }

        [HttpPost]
        public ActionResult Editar(ListaPreciosItem ListaPreciosItem, string IdCliente)
        {
            try
            {
                this.ventasConfigService.SaveListaPrecios(ListaPreciosItem, IdCliente);
                return Json(new { Success = true, ListaPreciosItem = ListaPreciosItem });
            }
            catch(Exception)
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error inesperado, vuelva a intentarlo" });
            }
            
        }

        [HttpPost]
        public JsonResult GetAllById(PagingRequest paging, string Id)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.ventasConfigService.GetAllPreciosById(Id), Success = true });
            }
            else
            {
                PagingResponse<ListaPreciosItem> resp = new PagingResponse<ListaPreciosItem>();
                resp.Page = paging.page;
                resp.Records = this.ventasConfigService.GetAllPreciosById(Id);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public ActionResult GetArticuloConPrecio(int IdArticulo, int IdCliente)
        {
            try
            {
                Articulo articulo = this.stockConfigService.GetArticulo(IdArticulo);
                ListaPreciosItem precio = this.ventasConfigService.GetPrecioForArticulo(IdArticulo, IdCliente);
                return Json(new { 
                    Success = true, 
                    Articulo = articulo,
                    Precio = precio
                });
            }
            catch
            {
                return Json(new { Success = false });
            }
        }
    }
}
