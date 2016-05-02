using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Tesoreria.Controllers
{
    public class CuentasBancariasController : BaseController
    {
        private readonly ITesoreriaConfigService TesoreriaConfigService;
        private readonly ICommonConfigService commonConfigService;

        public CuentasBancariasController(ITesoreriaConfigService TesoreriaConfigService, ICommonConfigService commonConfigService)
        {
            this.TesoreriaConfigService = TesoreriaConfigService;
            this.commonConfigService = commonConfigService;
        }

        //
        // GET: /Tesoreria/CuentaBancarias/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            IList <Banco> bancos = this.TesoreriaConfigService.GetAllBancos();
            if (bancos.Count == 0)
                return RedirectToAction("SinBancos", "ConfiguracionErrors", new { Area = "" });

            ViewBag.Bancos = bancos;
            ViewBag.Monedas = this.commonConfigService.GetAllMonedas();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(CuentaBancaria CuentaBancaria)
        {
            try
            {
                ClearNotValidatedProperties(CuentaBancaria);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.AddCuentaBancaria(CuentaBancaria);
                    CuentaBancaria.Banco = this.TesoreriaConfigService.GetBanco(CuentaBancaria.Banco.Id);
                    return Json(new { Success = true, CuentaBancaria = CuentaBancaria });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Cuenta Bancaria", Errors = ModelState.Values.SelectMany(v => v.Errors) });
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
                    this.TesoreriaConfigService.DeleteCuentasBancarias(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Cuenta Bancaria, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            CuentaBancaria CuentaBancaria = this.TesoreriaConfigService.GetCuentaBancaria(Id);
            ViewBag.Bancos = this.TesoreriaConfigService.GetAllBancos();
            ViewBag.Monedas = this.commonConfigService.GetAllMonedas();
            return View(CuentaBancaria);
        }


        [HttpPost]
        public JsonResult Editar(CuentaBancaria CuentaBancaria)
        {
            try
            {
                ClearNotValidatedProperties(CuentaBancaria);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.UpdateCuentaBancaria(CuentaBancaria);
                    CuentaBancaria.Banco = this.TesoreriaConfigService.GetBanco(CuentaBancaria.Banco.Id);
                    return Json(new { Success = true, CuentaBancaria = CuentaBancaria });
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
                return Json(new { Data = this.TesoreriaConfigService.GetAllCuentasBancarias(), Success = true });
            }
            else
            {
                PagingResponse<CuentaBancaria> resp = new PagingResponse<CuentaBancaria>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaConfigService.GetAllCuentasBancarias();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetComboCuentaBancaria(PagingRequest paging)
        {
            return Json(new { Data = this.TesoreriaConfigService.GetCuentaBancariaCombos(), Success = true });
        }

        [HttpPost]
        public JsonResult GetCuentaBancaria(int Id)
        {
            CuentaBancaria c = this.TesoreriaConfigService.GetCuentaBancaria(Id);
            return Json(new { Data = (c != null && c.Activo) ? c : null, Success = true });
        }




    }
}
