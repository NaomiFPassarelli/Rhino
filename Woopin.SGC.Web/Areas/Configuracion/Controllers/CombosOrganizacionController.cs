using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Configuracion.Controllers
{
    public class CombosOrganizacionController : BaseController
    {
        private readonly ICommonConfigService commonConfigService;
        //
        // GET: /Configuracion/CombosOrganizacion/
        public CombosOrganizacionController(ICommonConfigService commonConfigService)
        {
            this.commonConfigService = commonConfigService;
        }

        [HttpPost]
        public JsonResult Eliminar(List<int> Ids)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.commonConfigService.DeleteCombosItemsOrganizacion(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar los CombosOrganizacion items, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }



        //Seccion de CombosOrganizacion Individual
        //CombosOrganizacion/IndexCombo/Id
        public ActionResult IndexCombo(int Id)
        {
            ComboOrganizacion combo = this.commonConfigService.GetComboOrganizacion(Id);
            return View(combo);
        }



        //Seccion de CombosOrganizacion Default (Principales)
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [OutputCache(CacheProfile="LongOrgCache")]
        public JsonResult GetByComboId(PagingRequest paging, int ComboId)
        {
            try
            {
                IList<ComboItemOrganizacion> CombosItemOrganizacion = this.commonConfigService.GetItemsByComboOrganizacion((ComboOrganizacionType)Enum.Parse(typeof(ComboOrganizacionType), ComboId.ToString()));
                if (paging.page == 0)
                {
                    return Json(new { Data = CombosItemOrganizacion, Success = true });
                }
                else
                {
                    PagingResponse<ComboItemOrganizacion> resp = new PagingResponse<ComboItemOrganizacion>();
                    resp.Page = paging.page;
                    resp.Records = CombosItemOrganizacion.Select(x => new ComboItemOrganizacion { Data = x.Data, AdditionalData = x.AdditionalData, Id = x.Id }).ToList<ComboItemOrganizacion>();
                    resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                    resp.TotalRecords = resp.Records.Count;
                    return Json(resp);
                }
            }

            catch
            {
                return Json(new { Message = "Ha ocurrido un error, vuelva a intentarlo.", Success = false });
            }
        }

        // Devuelve todos los CombosOrganizacion que tiene el sistema
        [HttpPost]
        [OutputCache(CacheProfile="LongOrgCache")]
        public JsonResult GetAll(PagingRequest paging)
        {
            try
            {
                IList<ComboOrganizacion> CombosOrganizacion = this.commonConfigService.GetAllCombosOrganizacion();
                if (paging.page == 0)
                {
                    return Json(new { Data = CombosOrganizacion, Success = true });
                }
                else
                {
                    PagingResponse<ComboOrganizacion> resp = new PagingResponse<ComboOrganizacion>();
                    resp.Page = paging.page;
                    resp.Records = CombosOrganizacion;
                    resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                    resp.TotalRecords = resp.Records.Count;
                    return Json(resp);
                }
            }

            catch
            {
                return Json(new { Message = "Ha ocurrido un error, vuelva a intentarlo.", Success = false });
            }
        }

        //NuevoComboItemOrganizacion

        public ActionResult NuevoItem(int ComboId)
        {
            ComboItemOrganizacion ComboItemOrganizacion = new ComboItemOrganizacion() { Combo = new ComboOrganizacion() { Id = ComboId } };
            return View(ComboItemOrganizacion);
        }

        [HttpPost]
        public JsonResult NuevoItem(ComboItemOrganizacion ComboItemOrganizacion)
        {
            try
            {
                ClearNotValidatedProperties(ComboItemOrganizacion);
                if (ModelState.IsValid)
                {
                    this.commonConfigService.AddComboItemOrganizacion(ComboItemOrganizacion);
                    return Json(new { Success = true, ComboItemOrganizacion = ComboItemOrganizacion });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de combo item", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        //Editar ComboItemOrganizacion
        public ActionResult EditarItem(int IdItem)
        {
            ComboItemOrganizacion ComboItemOrganizacion = this.commonConfigService.GetComboItemOrganizacion(IdItem);
            return View(ComboItemOrganizacion);
        }


        [HttpPost]
        public JsonResult EditarItem(ComboItemOrganizacion ComboItemOrganizacion)
        {
            try
            {
                ClearNotValidatedProperties(ComboItemOrganizacion);
                if (ModelState.IsValid)
                {
                    this.commonConfigService.UpdateComboItemOrganizacion(ComboItemOrganizacion);
                    return Json(new { Success = true, ComboItemOrganizacion = ComboItemOrganizacion });
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
        public JsonResult GetSelectCombo(int type)
        {
            SelectCombo combo = new SelectCombo(){
                Items = this.commonConfigService.GetItemsByComboOrganizacion((ComboOrganizacionType)Enum.Parse(typeof(ComboOrganizacionType), type.ToString()))
                                                                    .Select(x => new SelectComboItem() { 
                                                                        id = x.Id, 
                                                                        text = x.Data, 
                                                                        additionalData = x.AdditionalData 
                                                                    })
                                                                    .ToList()
            };
             
            return Json(new { Success = true, Data = combo });
        }


    }
}
