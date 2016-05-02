using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Contabilidad.Controllers
{
    public class CuentasController : BaseController
    {
        private readonly IContabilidadConfigService contabilidadConfigService;
        private readonly IContabilidadService contabilidadService;
        public CuentasController(IContabilidadConfigService contabilidadConfigService, IContabilidadService contabilidadService)
        {
            this.contabilidadConfigService = contabilidadConfigService;
            this.contabilidadService = contabilidadService;
        }

        //
        // GET: /Contabilidad/Cuentas/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPlanCuentas()
        {
            IList<Cuenta> cuentas = this.contabilidadConfigService.GetAllCuentas();
            return View(cuentas);
        }

       
        public ActionResult NuevaCodigo(string Codigo)
        {
            ViewBag.Codigo = Codigo;
            return View();
        }
        
        // Creacion de Cuenta Contable desde Cero
        [HttpPost]
        public JsonResult NuevaCodigo(Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                this.contabilidadConfigService.AddCuenta(cuenta);
                return Json(new { Success = true });
            }
            return Json(new { Success = false, ErrorMessage = "Falta completar algun campo.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        // Creacion de Cuenta Contable desde Cero
        
        public ActionResult Nueva()
        {
            ViewBag.Rubros = this.contabilidadConfigService.GetRubros().Select(c => new SelectListItem() { Value = c.Rubro.ToString(), Text = c.Nombre}).ToList();
            return View();
        }


        // Creacion de Cuenta Contable desde Cero
        [HttpPost]
        public JsonResult Nueva(Cuenta cuenta)
        {
            if(ModelState.IsValid)
            {
                this.contabilidadConfigService.AddCuenta(cuenta);
                return Json(new { Success = true });
            }
            return Json(new { Success = false, ErrorMessage = "Falta completar algun campo.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }

        public void ExportarExcel()
        {
            IList<Cuenta> cuentas = this.contabilidadConfigService.GetAllCuentas();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=PlanDeCuentas.xls");
            Response.Write(RenderViewToString("ExportarExcel", cuentas));
            Response.ContentType = "";
            Response.End();
        }


        public ActionResult EjemploGrillaArbol()
        {
            return View();
        }






        public JsonResult GetAllCuentas(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.contabilidadConfigService.GetAllCuentas(), Success = true });
            }
            else
            {
                IList<Cuenta> cuentas = this.contabilidadConfigService.GetAllCuentas();
                PagingResponse<dynamic> resp = new PagingResponse<dynamic>();
                resp.Page = paging.page;
                resp.Records = cuentas.Select(x => new
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    Codigo = x.Codigo,
                    Saldo = x.Numero > 0 ? x.Id * 132 : 0,
                    level = CuentaContableHelper.GetLevel(x),
                    parent = CuentaContableHelper.GetParent(x,cuentas),
                    isLeaf = x.Numero > 0,
                    loaded = true,
                    expanded = false
                }).OrderBy(x => x.Codigo).ToList<dynamic>();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        public JsonResult GetRubros()
        {
            return Json(this.contabilidadConfigService.GetRubros());
        }
        public JsonResult GetCorrientes(int Rubro)
        {
            return Json(this.contabilidadConfigService.GetCorrientes(Rubro));
        }

        public JsonResult GetSubRubros(int Rubro,int Corriente)
        {
            return Json(this.contabilidadConfigService.GetSubRubros(Rubro, Corriente));
        }

        [HttpPost]
        public JsonResult GetIngresosCombo()
        {
            return Json(new { Data = this.contabilidadConfigService.GetCuentaIngresosCombo(), Success = true });
        }

        [HttpPost]
        public JsonResult GetEgresosCombo()
        {
            return Json(new { Data = this.contabilidadConfigService.GetCuentaEgresosCombo(), Success = true });
        }

        [HttpPost]
        public JsonResult GetCuenta(int IdCuenta)
        {
            return Json(new { Data = this.contabilidadConfigService.GetCuenta(IdCuenta), Success = true });
        }

        [HttpPost]
        public JsonResult GetCuentasCombo(SelectComboRequest req)
        {
            return Json(new { Data = this.contabilidadConfigService.GetAllCuentasByFilterCombo(req), Success = true });
        }
    }
}
