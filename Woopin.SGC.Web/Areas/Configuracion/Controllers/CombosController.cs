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
    public class CombosController : BaseController
    {
        private readonly ICommonConfigService commonConfigService;
        //
        // GET: /Configuracion/Combos/
        public CombosController(ICommonConfigService commonConfigService)
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
                    this.commonConfigService.DeleteCombosItems(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar los combos items, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }



        //Seccion de Combos Individual
        //Combos/IndexCombo/Id
        public ActionResult IndexCombo(int Id)
        {
            Combo combo = this.commonConfigService.GetCombo(Id);
            return View(combo);
        }



        //Seccion de Combos Default (Principales)
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
                IList<ComboItem> ComboItems = this.commonConfigService.GetItemsByCombo((ComboType)Enum.Parse(typeof(ComboType), ComboId.ToString()));
                if (paging.page == 0)
                {
                    return Json(new { Data = ComboItems, Success = true });
                }
                else
                {
                    PagingResponse<ComboItem> resp = new PagingResponse<ComboItem>();
                    resp.Page = paging.page;
                    resp.Records = ComboItems.Select(x => new ComboItem { Data = x.Data, AdditionalData = x.AdditionalData, Id = x.Id }).ToList<ComboItem>();
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

        // Devuelve todos los combos que tiene el sistema
        [HttpPost]
        [OutputCache(CacheProfile="LongOrgCache")]
        public JsonResult GetAll(PagingRequest paging)
        {
            try
            {
                IList<Combo> combos = this.commonConfigService.GetAllCombos();
                if (paging.page == 0)
                {
                    return Json(new { Data = combos, Success = true });
                }
                else
                {
                    PagingResponse<Combo> resp = new PagingResponse<Combo>();
                    resp.Page = paging.page;
                    resp.Records = combos;
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

        //NuevoComboItem

        public ActionResult NuevoItem(int ComboId)
        {
            ComboItem comboItem = new ComboItem() { Combo = new Combo() { Id = ComboId } };
            return View(comboItem);
        }

        [HttpPost]
        public JsonResult NuevoItem(ComboItem ComboItem)
        {
            try
            {
                ClearNotValidatedProperties(ComboItem);
                if (ModelState.IsValid)
                {
                    this.commonConfigService.AddComboItem(ComboItem);
                    return Json(new { Success = true, ComboItem = ComboItem });
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

        //Editar ComboItem
        public ActionResult EditarItem(int IdItem)
        {
            ComboItem ComboItem = this.commonConfigService.GetComboItem(IdItem);
            return View(ComboItem);
        }


        [HttpPost]
        public JsonResult EditarItem(ComboItem ComboItem)
        {
            try
            {
                ClearNotValidatedProperties(ComboItem);
                if (ModelState.IsValid)
                {
                    this.commonConfigService.UpdateComboItem(ComboItem);
                    return Json(new { Success = true, ComboItem = ComboItem });
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
                Items = this.commonConfigService.GetItemsByCombo((ComboType)Enum.Parse(typeof(ComboType), type.ToString()))
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
