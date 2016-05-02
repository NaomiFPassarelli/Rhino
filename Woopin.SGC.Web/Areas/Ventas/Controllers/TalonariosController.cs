using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Ventas.Controllers
{
    public class TalonarioController : BaseController
    {
        private readonly IVentasConfigService ventasConfigService;
        public TalonarioController(IVentasConfigService ventasConfigService)
        {
            this.ventasConfigService = ventasConfigService;
        }

        //
        // GET: /Configuracion/Monedas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Nuevo(Talonario Talonario, HttpPostedFileBase Archivo, bool FE)
        {
            try
            {
                ClearNotValidatedProperties(Talonario);
                if (ModelState.IsValid)
                {
                    Talonario.Validate(Archivo != null, FE);
                    if (Archivo != null)
                    {
                        string filename = Security.GetOrganizacion().CUIT + "-" + Talonario.Prefijo + Path.GetExtension(Archivo.FileName);
                        string path = GuardarArchivo(Archivo, "Certificados", filename);

                        Talonario.CertificadoPath = path;
                    }
                    this.ventasConfigService.AddTalonario(Talonario);
                    return Json(new { Success = true, Talonario = Talonario });

                }else
                {
                    return Json(new { Success = false, ErrorMessage = "Hay errores en el formulario de creación del talonario", Errors = ModelState.Values.SelectMany(v => v.Errors) });
                }
            }
            catch (ValidationException ve)
            {
                return Json(new { Success = false, ErrorMessage = ve.ErrorMessage });
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
                    this.ventasConfigService.DeleteTalonario(Ids);
                    return Json(new { Success = true });
                }
                else
                {
                    return Json(new { Success = false, ErrorMessage = "Hubo un problema en el pedido de eliminar la Talonario, vuelva a inetntarlo." });
                }
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
        }

        public ActionResult Editar(int Id)
        {
            Talonario Talonario = this.ventasConfigService.GetTalonario(Id);
            return View(Talonario);
        }

        [HttpPost]
        public JsonResult Editar(Talonario Talonario, HttpPostedFileBase Archivo, bool FE)
        {
            try
            {
                Talonario.Validate(Archivo != null, FE);
                if (Archivo != null)
                {
                    string filename = Security.GetOrganizacion().CUIT + "-" + Talonario.Prefijo + Path.GetExtension(Archivo.FileName);
                    string path = GuardarArchivo(Archivo, "Certificados", filename);

                    Talonario.CertificadoPath = path;
                }    
                
                this.ventasConfigService.UpdateTalonario(Talonario);
                return Json(new { Success = true, Talonario = Talonario });
            }
            catch (ValidationException ve)
            {
                return Json(new { Success = false, ErrorMessage = ve.ErrorMessage });
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
                return Json(new { Data = this.ventasConfigService.GetAllTalonarios(), Success = true });
            }
            else
            {
                PagingResponse<Talonario> resp = new PagingResponse<Talonario>();
                resp.Page = paging.page;
                resp.Records = this.ventasConfigService.GetAllTalonarios();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        public JsonResult GetCombo()
        {
            SelectCombo combo = new SelectCombo(){
                Items = this.ventasConfigService.GetAllTalonarios()
                                                .Select(x => new SelectComboItem(){
                                                    id = x.Id,
                                                    text = x.Prefijo,
                                                    additionalData = x.PuntoVenta.HasValue ? 
                                                            x.PuntoVenta.Value.ToString() : null
                                                }).ToList()
            };
            return Json(new { Data = combo, Success = true });
        }

    }
}
