using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.App.Logging;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Services.Afip;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.AFIP.Controllers
{
    public class WSFEVController : BaseController
    {
        private readonly IAfipService AfipService;
        public WSFEVController(IAfipService AfipService)
        {
            this.AfipService = AfipService;
        }
        //
        // GET: /AFIP/WSFEV/

        public async Task<ActionResult> Index()
        {
            return View();
        }

        [OutputCache(CacheProfile="LongOrgCache")]
        public async Task<JsonResult> GetMonedas()
        {
            try
            {
                List<string> items = await this.AfipService.GetAllMonedas();
                SelectCombo sl = new SelectCombo();
                sl.Items = items.Select(x => new SelectComboItem() { text = x }).ToList();
                return Json(new { Data = sl });
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }

        [OutputCache(CacheProfile="LongOrgCache")]
        public async Task<JsonResult> GetComprobantes()
        {
            try
            {
                List<string> items = await this.AfipService.GetAllTiposComprobantes();
                SelectCombo sl = new SelectCombo();
                sl.Items = items.Select(x => new SelectComboItem() { text = x }).ToList();
                return Json(new { Data = sl });
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }

        [OutputCache(CacheProfile="LongOrgCache")]
        public async Task<JsonResult> GetPuntosVenta()
        {
            try
            {
                List<string> items = await this.AfipService.GetAllPuntosVentas();
                SelectCombo sl = new SelectCombo();
                sl.Items = items.Select(x => new SelectComboItem() { text = x }).ToList();
                return Json(new { Data = sl });
            }
            catch (Exception e)
            {
                Logger.LogError("Ocurrio un problema al buscar los puntos de venta",e);
                return null;
            }

        }

        [OutputCache(CacheProfile="LongOrgCache")]
        public async Task<JsonResult> GetPaises()
        {
            try
            {
                List<string> items = await this.AfipService.GetAllPaises();
                SelectCombo sl = new SelectCombo();
                sl.Items = items.Select(x => new SelectComboItem() { text = x }).ToList();
                return Json(new { Data = sl });
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }

        [OutputCache(CacheProfile="LongOrgCache")]
        public async Task<JsonResult> GetDocumentos()
        {
            try
            {
                List<string> items = await this.AfipService.GetAllTiposDocumentos();
                SelectCombo sl = new SelectCombo();
                sl.Items = items.Select(x => new SelectComboItem() { text = x }).ToList();
                return Json(new { Data = sl });
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }

        [OutputCache(CacheProfile="LongOrgCache")]
        public async Task<JsonResult> GetTiposIvas()
        {
            try
            {
                List<string> items = await this.AfipService.GetAllTiposIvas();
                SelectCombo sl = new SelectCombo();
                sl.Items = items.Select(x => new SelectComboItem() { text = x }).ToList();
                return Json(new { Data = sl });
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }

        [OutputCache(CacheProfile="LongOrgCache")]
        public async Task<JsonResult> GetTributos()
        {
            try
            {
                List<string> items = await this.AfipService.GetAllTributos();
                SelectCombo sl = new SelectCombo();
                sl.Items = items.Select(x => new SelectComboItem() { text = x }).ToList();
                return Json(new { Data = sl });
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }

        [OutputCache(CacheProfile="LongOrgCache")]
        public async Task<JsonResult> GetOpcionales()
        {
            try
            {
                List<string> items = await this.AfipService.GetAllTiposOpcional();
                SelectCombo sl = new SelectCombo();
                sl.Items = items.Select(x => new SelectComboItem() { text = x }).ToList();
                return Json(new { Data = sl });
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }

        [OutputCache(CacheProfile="LongOrgCache")]
        public async Task<JsonResult> GetConceptos()
        {
            try
            {
                List<string> items = await this.AfipService.GetAllTiposConceptos();
                SelectCombo sl = new SelectCombo();
                sl.Items = items.Select(x => new SelectComboItem() { text = x }).ToList();
                return Json(new { Data = sl });
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }


        public async Task<JsonResult> GetComprobanteEmitido(long NroComprobante, int PuntoVenta, int TipoComprobante)
        {
            try
            {
                return Json(new { Data = await this.AfipService.ConsultarComprobante(NroComprobante,PuntoVenta,TipoComprobante) });
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }
        public async Task<JsonResult> GetUltimoComprobante(int PuntoVenta, int TipoComprobante)
        {
            try
            {
                return Json(new { Data = await this.AfipService.ConsultarUltimoComprobante(PuntoVenta, TipoComprobante) });
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message,e);
                return null;
            }

        }
    }
}
