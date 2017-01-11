using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Cooperativa;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Cooperativa.Controllers
{
    public class ConceptosController : BaseController
    {
        private readonly ICooperativaConfigService CooperativaConfigService;
        private readonly ICommonConfigService commonConfigService;

        public ConceptosController(ICooperativaConfigService CooperativaConfigService, ICommonConfigService commonConfigService)
        {
            this.CooperativaConfigService = CooperativaConfigService; 
            this.commonConfigService = commonConfigService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.TipoConceptos = Enum.GetNames(typeof(TypeConcepto)).Select(x => new SelectListItem() { Text = x, Value = (Enum.Parse(typeof(TypeConcepto), x)).ToString() }).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Concepto Concepto, IList<Concepto> Conceptos = null)
        {
            try
            {

                //TODO mejorar este validador 
                if (Concepto.Descripcion != null && Concepto.Valor != null && Concepto.Suma != null && Concepto.TipoConcepto != null)
                {
                    //if (Conceptos != null && Conceptos.Count > 0)
                    //{
                    //    this.CooperativaConfigService.AddConceptoConConceptos(Concepto, Conceptos);
                    //}
                    //else
                    //{
                        this.CooperativaConfigService.AddConcepto(Concepto);
                    //}
                    return Json(new { Success = true, Concepto = Concepto });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Concepto", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }

                //ClearNotValidatedProperties(Concepto);
                //for (int j = 0; j < Conceptos.Count(); j++ )
                //{
                //    string cleanmodel = "Conceptos[" + j + "]Descripcion";
                //    ModelState[cleanmodel].Errors.Clear();
                //}
                

                //if (ModelState.IsValid)
                //{
                    
                //}
                //else
                //{
                //    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación de Concepto", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                //}
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
                    this.CooperativaConfigService.DeleteConceptos(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Concepto, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }


        public ActionResult Editar(int Id)
        {
            Concepto Concepto = this.CooperativaConfigService.GetConcepto(Id);
            ViewBag.TypeConceptos = Enum.GetNames(typeof(TypeConcepto)).Select(x => new SelectListItem() { Text = x, Value = (Enum.Parse(typeof(TypeConcepto), x)).ToString(), Selected = (x == Concepto.TipoConcepto.ToString() ? true : false) }).ToList();
            return View(Concepto);
        }


        [HttpPost]
        public JsonResult Editar(Concepto Concepto)
        {
            try
            {
                ClearNotValidatedProperties(Concepto);
                if (ModelState.IsValid)
                {
                    this.CooperativaConfigService.UpdateConcepto(Concepto);
                    return Json(new { Success = true, Concepto = Concepto });
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
                return Json(new { Data = this.CooperativaConfigService.GetAllConceptos(), Success = true });
            }
            else
            {
                PagingResponse<Concepto> resp = new PagingResponse<Concepto>();
                resp.Page = paging.page;
                resp.Records = this.CooperativaConfigService.GetAllConceptos();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetConceptos(SelectComboRequest req)
        {
            return Json(new { Data = this.CooperativaConfigService.GetAllConceptosByFilterCombo(req), Success = true });
        }

        [HttpPost]
        public JsonResult GetConceptoByFilter(string DescripcionConcepto)
        {
            return Json(new { Data = this.CooperativaConfigService.GetConceptoByFilterCombo(DescripcionConcepto), Success = true });
        }


        [HttpPost]
        public JsonResult GetConcepto(int idConcepto)
        {
            Concepto a = this.CooperativaConfigService.GetConcepto(idConcepto);
            //TODO buscar sobre los que va
            //IList<ConceptoConceptos> AAs = this.CooperativaConfigService.GetConceptoConceptosByConcepto(idConcepto);
            //IEnumerable<Concepto> AAs = this.CooperativaConfigService.GetConceptoConceptosByConcepto(idConcepto).Select(x => x.ConceptoSobre);
            return Json(new { Data = (a != null ) ? a : null, Success = true });
        }

        public ActionResult ConceptosDefault(/*int IdConcepto*/)
        {
            return View();
        }

    }
}
