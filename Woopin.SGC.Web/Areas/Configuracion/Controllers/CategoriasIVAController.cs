using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Configuracion.Controllers
{
    public class CategoriasIVAController : BaseController
    {
        private readonly ICommonConfigService commonConfigService;
        public CategoriasIVAController(ICommonConfigService commonConfigService)
        {
            this.commonConfigService = commonConfigService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nueva()
        {
            ViewBag.LetraComprobante = this.commonConfigService.GetItemsByCombo(ComboType.LetraComprobante);
            return View();
        }

        [HttpPost]
        public JsonResult Nueva(CategoriaIVA categoriaIVA)
        {
            try
            {
                ClearNotValidatedProperties(categoriaIVA);
                if (ModelState.IsValid)
                {
                    this.commonConfigService.AddCategoriaIVA(categoriaIVA);
                    categoriaIVA.LetraVentas = this.commonConfigService.GetComboItem(categoriaIVA.LetraVentas.Id);
                    categoriaIVA.LetraCompras = this.commonConfigService.GetComboItem(categoriaIVA.LetraCompras.Id);
                    return Json(new { Success = true, CategoriaIVA = categoriaIVA });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de categoría de IVA", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.commonConfigService.DeleteCategoriaIVAs(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar las categorías de IVA, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            CategoriaIVA categoriaIVA = this.commonConfigService.GetCategoriaIVA(Id);
            ViewBag.LetraComprobantes = this.commonConfigService.GetItemsByCombo(ComboType.LetraComprobante).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            return View(categoriaIVA);
        }


        [HttpPost]
        public JsonResult Editar(CategoriaIVA categoriaIVA)
        {
            try
            {
                ClearNotValidatedProperties(categoriaIVA);
                if (ModelState.IsValid)
                {
                    this.commonConfigService.UpdateCategoriaIVA(categoriaIVA);
                    categoriaIVA.LetraVentas = this.commonConfigService.GetComboItem(categoriaIVA.LetraVentas.Id);
                    categoriaIVA.LetraCompras = this.commonConfigService.GetComboItem(categoriaIVA.LetraCompras.Id);
                    return Json(new { Success = true, CategoriaIVA = categoriaIVA });
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
        [OutputCache(CacheProfile="LongOrgCache")]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.commonConfigService.GetAllCategoriaIVAs(), Success = true });
            }
            else
            {
                PagingResponse<CategoriaIVA> resp = new PagingResponse<CategoriaIVA>();
                resp.Page = paging.page;
                resp.Records = this.commonConfigService.GetAllCategoriaIVAs();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult SetDefault(int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.commonConfigService.SetDefaultCategoriaIVA(Id);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

    }


}




