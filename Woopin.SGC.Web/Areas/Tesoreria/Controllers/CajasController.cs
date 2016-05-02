using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Tesoreria.Controllers
{
    public class CajasController : BaseController
    {
        private readonly ITesoreriaConfigService TesoreriaConfigService;
        private readonly ICommonConfigService commonConfigService;

        public CajasController(ITesoreriaConfigService TesoreriaConfigService, ICommonConfigService commonConfigService)
        {
            this.TesoreriaConfigService = TesoreriaConfigService;
            this.commonConfigService = commonConfigService;
        }

        //
        // GET: /Tesoreria/Cajas/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Caja Caja)
        {
            try
            {
                ClearNotValidatedProperties(Caja);
                this.TesoreriaConfigService.AddCaja(Caja);
                return Json(new { Success = true, Caja = Caja });
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
                    this.TesoreriaConfigService.DeleteCajas(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar el Caja, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Caja Caja = this.TesoreriaConfigService.GetCaja(Id);
            return View(Caja);
        }


        [HttpPost]
        public JsonResult Editar(Caja Caja)
        {
            try
            {
                ClearNotValidatedProperties(Caja);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.UpdateCaja(Caja);
                    return Json(new { Success = true, Caja = Caja });
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
                return Json(new { Data = this.TesoreriaConfigService.GetAllCajas(), Success = true });
            }
            else
            {
                PagingResponse<Caja> resp = new PagingResponse<Caja>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaConfigService.GetAllCajas();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }


        [HttpPost]
        [OutputCache(CacheProfile="SmallOrgCache")]
        public JsonResult GetCombo(PagingRequest paging)
        {
            SelectCombo combo = new SelectCombo(){
                Items = this.TesoreriaConfigService.GetAllCajas().Select(x => new SelectComboItem() { id = x.Id, text = x.Nombre }).ToList()
            };
            return Json(new { Data = combo, Success = true });
        }

        [HttpPost]
        [OutputCache(CacheProfile="SmallOrgCache")]
        public JsonResult GetCaja(int Id)
        {
            Caja c = this.TesoreriaConfigService.GetCaja(Id);
            return Json(new { Data = c.Activo ? c : null, Success = true });
        }

    }
}
