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
    public class TarjetasCreditosController : BaseController
    {
        private readonly ITesoreriaConfigService TesoreriaConfigService;
        private readonly ICommonConfigService commonConfigService;

        public TarjetasCreditosController(ITesoreriaConfigService TesoreriaConfigService, ICommonConfigService commonConfigService)
        {
            this.TesoreriaConfigService = TesoreriaConfigService;
            this.commonConfigService = commonConfigService;
        }

        //
        // GET: /Tesoreria/TarjetaCreditos/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.CuentasBancarias = this.TesoreriaConfigService.GetAllCuentasBancarias();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(TarjetaCredito TarjetaCredito)
        {
            try
            {
                ClearNotValidatedProperties(TarjetaCredito);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.AddTarjetaCredito(TarjetaCredito);
                    TarjetaCredito.CuentaBancaria = this.TesoreriaConfigService.GetCuentaBancaria(TarjetaCredito.CuentaBancaria.Id);
                    return Json(new { Success = true, TarjetaCredito = TarjetaCredito });
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
                    this.TesoreriaConfigService.DeleteTarjetaCreditos(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Tarjeta de credito, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            TarjetaCredito TarjetaCredito = this.TesoreriaConfigService.GetTarjetaCredito(Id);
            ViewBag.CuentasBancarias = this.TesoreriaConfigService.GetAllCuentasBancarias();
            return View(TarjetaCredito);
        }


        [HttpPost]
        public JsonResult Editar(TarjetaCredito TarjetaCredito)
        {
            try
            {
                ClearNotValidatedProperties(TarjetaCredito);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.UpdateTarjetaCredito(TarjetaCredito);
                    TarjetaCredito.CuentaBancaria = this.TesoreriaConfigService.GetCuentaBancaria(TarjetaCredito.CuentaBancaria.Id);
                    return Json(new { Success = true, TarjetaCredito = TarjetaCredito });
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
                return Json(new { Data = this.TesoreriaConfigService.GetAllTarjetaCreditos(), Success = true });
            }
            else
            {
                PagingResponse<TarjetaCredito> resp = new PagingResponse<TarjetaCredito>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaConfigService.GetAllTarjetaCreditos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }









        [HttpPost]
        public JsonResult GetComboTarjetaCredito(PagingRequest paging)
        {
            return Json(new { Data = this.TesoreriaConfigService.GetTarjetaCreditoCombos(), Success = true });
        }

        [HttpPost]
        public JsonResult GetTarjetaCredito(int Id)
        {
            return Json(new { Data = this.TesoreriaConfigService.GetTarjetaCredito(Id), Success = true });
        }



    }
}
