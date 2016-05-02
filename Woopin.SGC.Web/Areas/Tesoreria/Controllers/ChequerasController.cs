using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Tesoreria.Controllers
{
    public class ChequerasController : BaseController
    {
        private readonly ITesoreriaConfigService TesoreriaConfigService;
        private readonly ITesoreriaService TesoreriaService;
        private readonly ICommonConfigService commonConfigService;

        public ChequerasController(ITesoreriaConfigService TesoreriaConfigService, ICommonConfigService commonConfigService,
                                        ITesoreriaService TesoreriaService)
        {
            this.TesoreriaConfigService = TesoreriaConfigService;
            this.commonConfigService = commonConfigService;
            this.TesoreriaService = TesoreriaService;
        }

        //
        // Chequeras //

        public ActionResult Index()
        {
            return View();
        }
        //Nueva Chequera
        public ActionResult Nuevo()
        {
            ViewBag.CuentasBancarias = this.TesoreriaConfigService.GetAllCuentasBancarias();
            return View();
        }
        //Nueva Chequera
        [HttpPost]
        public JsonResult Nuevo(Chequera Chequera)
        {
            try
            {
                ClearNotValidatedProperties(Chequera);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.AddChequera(Chequera);
                    Chequera.CuentaBancaria = this.TesoreriaConfigService.GetCuentaBancaria(Chequera.CuentaBancaria.Id);
                    return Json(new { Success = true, Chequera = Chequera });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Chequera", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch (BusinessException bs)
            {
                return Json(new { Success = false, ErrorMessage = bs.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        //Eliminar Chequeras
        [HttpPost]
        public JsonResult Eliminar(List<int> Ids)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.DeleteChequeras(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Chequera, vuelva a intentarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        //Editar Chequera
        public ActionResult Editar(int Id)
        {
            Chequera Chequera = this.TesoreriaConfigService.GetChequera(Id);
            ViewBag.CuentasBancarias = this.TesoreriaConfigService.GetAllCuentasBancarias();
            return View(Chequera);
        }

        //Editar Chequera
        [HttpPost]
        public JsonResult Editar(Chequera Chequera)
        {
            try
            {
                ClearNotValidatedProperties(Chequera);
                if (ModelState.IsValid)
                {
                    this.TesoreriaConfigService.UpdateChequera(Chequera);
                    Chequera.CuentaBancaria = this.TesoreriaConfigService.GetCuentaBancaria(Chequera.CuentaBancaria.Id);
                    return Json(new { Success = true, Chequera = Chequera });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Algunos de los campos no se encuentran correctamente introducidos.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch (BusinessException be)
            {
                return Json(new { Success = false, ErrorMessage = be.ErrorMessage });    
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }
        //Para el index de chequeras - mostrar solo las activas
        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.TesoreriaConfigService.GetAllChequeras(), Success = true });
            }
            else
            {
                PagingResponse<Chequera> resp = new PagingResponse<Chequera>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaConfigService.GetAllChequeras();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }




        // Una Chequera (Cheques) //

        public ActionResult IndexChequera(int Id)
        {
            Chequera Chequera = this.TesoreriaConfigService.GetChequera(Id);
            return View(Chequera);
        }

        //Anular Cheque
        public ActionResult AnularCheques(int IdChequera, int IdCuentaBancaria)
        {
            ChequePropio ChequePropio = new ChequePropio();
            ChequePropio.CuentaBancaria = this.TesoreriaConfigService.GetCuentaBancaria(IdCuentaBancaria);
            Chequera chequera = this.TesoreriaConfigService.GetChequera(IdChequera);
            ViewBag.NumeroDesde = chequera.NumeroDesde;
            ViewBag.NumeroHasta = chequera.NumeroHasta;
            return View(ChequePropio);
        }

        //Anular Cheques
        [HttpPost]
        public JsonResult AnularCheques(ChequePropio cp)
        {
            try
            {
                    this.TesoreriaService.AnularChequePropio(cp.CuentaBancaria.Id, Convert.ToInt32(cp.Numero));
                    return Json(new { Success = true, ChequePropio = cp });
            }
            catch(BusinessException be)
            {
                return Json(new { Success = false, ErrorMessage = be.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }



        [HttpPost]
        [OutputCache(CacheProfile="SmallOrgCache")]
        public JsonResult GetChequera(int Id)
        {
            return Json(new { Data = this.TesoreriaConfigService.GetChequera(Id), Success = true });
        }

        [HttpPost]
        public JsonResult GetAllChequesInChequera(int IdChequera, PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.TesoreriaConfigService.GetAllChequesInChequera(IdChequera), Success = true });
            }
            else
            {
                PagingResponse<ChequePropio> resp = new PagingResponse<ChequePropio>();
                resp.Page = paging.page;
                resp.Records = this.TesoreriaConfigService.GetAllChequesInChequera(IdChequera);
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }


    }
}
