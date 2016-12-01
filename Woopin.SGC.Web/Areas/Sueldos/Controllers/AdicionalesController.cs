using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Sueldos;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Sueldos.Controllers
{
    public class AdicionalesController : BaseController
    {
        private readonly ISueldosConfigService SueldosConfigService;
        private readonly ICommonConfigService commonConfigService;

        public AdicionalesController(ISueldosConfigService SueldosConfigService, ICommonConfigService commonConfigService)
        {
            this.SueldosConfigService = SueldosConfigService; 
            this.commonConfigService = commonConfigService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            //ViewBag.TypeLiquidacion = new SelectCombo() { Items = this.tesoreriaSerivce.GetAllValores().Select(x => new SelectComboItem() { id = x.Id, text = x.Nombre, additionalData = x.TipoValor.Data }).ToList() };
            //ViewBag.TypeLiquidacion = (TypeLiquidacion).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Data }).ToList();
            //new SelectComboItem(){ text = TypeLiquidacion, id =  }
            ViewBag.TypeLiquidaciones = Enum.GetNames(typeof(TypeLiquidacion)).Select(x => new SelectListItem() { Text = x, Value = (Enum.Parse(typeof(TypeLiquidacion), x)).ToString() }).ToList();
            //ViewBag.Localizaciones = this.commonConfigService.GetAllLocalizaciones().Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nombre, Selected = x.Predeterminado }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Adicional Adicional)
        {
            try
            {
                //TODO mejorar este validador 
                if (Adicional.Descripcion != null &&
                    Adicional.Suma != null && Adicional.TipoLiquidacion != null)
                {
                    this.SueldosConfigService.AddAdicional(Adicional);
                    return Json(new { Success = true, Adicional = Adicional });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Adicional", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        //[HttpPost]
        //public JsonResult Nuevo(Adicional Adicional, IList<Adicional> Adicionales = null)
        //{
        //    try
        //    {

        //        //TODO mejorar este validador 
        //        if (Adicional.Descripcion != null && 
        //            (Adicional.Porcentaje != null || Adicional.Valor != null) 
        //            && Adicional.Suma != null && Adicional.TipoLiquidacion != null)
        //        {
        //            if (Adicionales != null && Adicionales.Count > 0)
        //            {
        //                this.SueldosConfigService.AddAdicionalConAdicionales(Adicional, Adicionales);
        //            }
        //            else
        //            {
        //                this.SueldosConfigService.AddAdicional(Adicional);
        //            }
        //            return Json(new { Success = true, Adicional = Adicional });
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Adicional", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        //        }

        //        //ClearNotValidatedProperties(Adicional);
        //        //for (int j = 0; j < Adicionales.Count(); j++ )
        //        //{
        //        //    string cleanmodel = "Adicionales[" + j + "]Descripcion";
        //        //    ModelState[cleanmodel].Errors.Clear();
        //        //}
                

        //        //if (ModelState.IsValid)
        //        //{
                    
        //        //}
        //        //else
        //        //{
        //        //    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Adicional", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        //        //}
        //    }
        //    catch
        //    {
        //        return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
        //    }
        //}

        [HttpPost]
        public JsonResult Eliminar(List<int> Ids)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.SueldosConfigService.DeleteAdicionales(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Adicional, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Adicional Adicional = this.SueldosConfigService.GetAdicional(Id, 0, false);
            ViewBag.TypeLiquidaciones = Enum.GetNames(typeof(TypeLiquidacion)).Select(x => new SelectListItem() { Text = x, Value = (Enum.Parse(typeof(TypeLiquidacion), x)).ToString(), Selected = (x == Adicional.TipoLiquidacion.ToString() ? true : false)  }).ToList();
            return View(Adicional);
        }


        [HttpPost]
        public JsonResult Editar(Adicional Adicional, IList<AdicionalAdicionales> Adicionales = null)
        {
            try
            {
                ClearNotValidatedProperties(Adicional);
                if (ModelState.IsValid)
                {
                    this.SueldosConfigService.UpdateAdicional(Adicional, Adicionales);
                    return Json(new { Success = true, Adicional = Adicional });
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
                return Json(new { Data = this.SueldosConfigService.GetAllAdicionales(), Success = true });
            }
            else
            {
                PagingResponse<Adicional> resp = new PagingResponse<Adicional>();
                resp.Page = paging.page;
                resp.Records = this.SueldosConfigService.GetAllAdicionales();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetAdicionales(SelectComboRequest req, int IdSindicato, bool OnlyManual)
        {
            return Json(new { Data = this.SueldosConfigService.GetAllAdicionalesByFilterCombo(req, IdSindicato, OnlyManual), Success = true });
        }

        [HttpPost]
        public JsonResult GetAdicional(int idAdicional, int IdSindicato, bool OnlyManual)
        {
            Adicional a = this.SueldosConfigService.GetAdicional(idAdicional, IdSindicato, OnlyManual);
            //TODO buscar sobre los que va
            //IList<AdicionalAdicionales> AAs = this.SueldosConfigService.GetAdicionalAdicionalesByAdicional(idAdicional);
            //IEnumerable<Adicional> AAs = this.SueldosConfigService.GetAdicionalAdicionalesByAdicional(idAdicional).Select(x => x.AdicionalSobre);
            //return Json(new { Data = (a != null) ? a : null, Data_AAs = (AAs.Count() > 0) ? AAs : null, Success = true });
            return Json(new { Data = (a != null) ? a : null, Success = true });
        }

        public ActionResult AdicionalesDefault(/*int IdAdicional*/)
        {
            return View();
        }

    }
}
