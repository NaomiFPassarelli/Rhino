using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Bolos;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Services;
using Woopin.SGC.Web.Controllers;

namespace Woopin.SGC.Web.Areas.Bolos.Controllers
{
    public class LiquidadoresController : BaseController
    {
        private readonly IBolosConfigService BolosConfigService;
        private readonly IBolosService BolosService;
        private readonly ICommonConfigService commonConfigService;
        public LiquidadoresController(IBolosConfigService BolosConfigService, ICommonConfigService commonConfigService,
                                            IBolosService BolosService)
        {
            this.BolosConfigService = BolosConfigService;
            this.commonConfigService = commonConfigService;
            this.BolosService = BolosService;
        }


        //public ActionResult CuentaCorriente(int? Id)
        //{
        //    ViewBag.IdProveedor = Id.HasValue ? Id.Value : 0;
        //    return View();
        //}

        //public ActionResult Acumulados()
        //{
        //    return View();
        //}


        //
        // GET: /Configuracion/Monedas
        public ActionResult Index(string start, string end, string statusType, string startvenc, string endvenc)
        {
            //ViewBag.start = start;
            //ViewBag.end = end;
            //ViewBag.startvenc = startvenc;
            //ViewBag.endvenc = endvenc;
            //ViewBag.statusType = statusType;
            return View();
        }

        public ActionResult Nuevo()
        {
            ViewBag.NumeroRef = this.BolosService.GetProximoNumeroReferencia();
            //ViewBag.IVAs = new SelectCombo() { Items = this.commonConfigService.GetItemsByCombo(ComboType.TipoIva).Select(x => new SelectComboItem() { id = x.Id, text = x.Data, additionalData = x.AdditionalData }).ToList() };
            Liquidador lanterior = this.BolosService.GetLiquidadorAnterior();
            if(lanterior != null)
            {
                ViewBag.FechaUltimoDeposito = lanterior.FechaHasta;
            }
            return View();
        }


        [HttpPost]
        public JsonResult Nuevo(Liquidador Liquidador)
        {
            try
            {
                int Numero = Liquidador.NumeroReferencia;
                this.BolosService.AddLiquidador(Liquidador);
                // Problema de referencia circular, no lo devuelvo
                //Liquidador.Asiento = null;
                Liquidador.Detalle = null;
                if (Numero != Liquidador.NumeroReferencia)
                {
                    return Json(new { Success = true, NumeroRef = Liquidador.NumeroReferencia, Liquidador = Liquidador });
                }
                return Json(new { Success = true, Liquidador = Liquidador });
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch (BusinessException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }

        public ActionResult Detalle(int Id, bool? opensDialog)
        {
            Liquidador c = this.BolosService.GetLiquidadorCompleto(Id);
            if (c == null)
            {
                return RedirectToAction("NotFound", "Error", new { Area = "" });
            }
            ViewBag.OpensDialog = opensDialog.HasValue ? opensDialog.Value : false;
            return View(c);
        }

        public ActionResult LiquidadoresAPagar(/*int IdProveedor*/)
        {
            //ViewBag.IdProveedor = IdProveedor;
            return View();
        }

        //public ActionResult LiquidadoresAPagarNC(int IdProveedor, int? Tipo, int? NoTipo,LiquidadoresACancelarFilter Pagada)
        //{
        //    ViewBag.IdProveedor = IdProveedor;
        //    ViewBag.Tipo = Tipo.HasValue ? Tipo.Value : 0;
        //    ViewBag.NoTipo = NoTipo.HasValue ? NoTipo.Value : 0;
        //    ViewBag.Pagada = (int) Pagada;
        //    return View();
        //}

        [HttpPost]
        public JsonResult AnularLiquidador(int IdLiquidador)
        {
            try
            {
                this.BolosService.AnularLiquidador(IdLiquidador);
                return Json(new { Success = true, Data = "Fue anulada con exito" });
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch (BusinessException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }
            
        }

        [HttpPost]
        public JsonResult EliminarLiquidador(int IdLiquidador)
        {
            try
            {
                this.BolosService.EliminarLiquidador(IdLiquidador);
                return Json(new { Success = true, Data = "Fue eliminado con exito" });
            }
            catch (ValidationException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch (BusinessException e)
            {
                return Json(new { Success = false, ErrorMessage = e.ErrorMessage });
            }
            catch
            {
                return Json(new { Success = false, ErrorMessage = "Ha ocurrido un error, vuelva a intentarlo." });
            }

        }

        //[HttpPost]
        //public JsonResult LoadCtaCorrienteHead(int id, DateTime? start, DateTime? end, CuentaCorrienteFilter filter)
        //{
        //    DateRange range = new DateRange(start, end);
        //    return Json(new { Data = this.BolosService.LoadCtaCorrienteHead(id,range.Start, range.End, filter), Success = true });
        //}

        //[HttpPost]
        //public JsonResult AcumuladosHead(int id, int idRubro, DateTime? start, DateTime? end, CuentaCorrienteFilter filter)
        //{
        //    DateRange range = new DateRange(start, end);
        //    return Json(new { Data = this.BolosService.AcumuladosHead(id, idRubro, range.Start, range.End, filter), Success = true });
        //}

        [HttpPost]
        public JsonResult GetAll(PagingRequest paging)
        {
            if (paging.page == 0)
            {
                return Json(new { Data = this.BolosService.GetAllLiquidadores(), Success = true });
            }
            else
            {
                PagingResponse<Liquidador> resp = new PagingResponse<Liquidador>();
                resp.Page = paging.page;
                resp.Records = this.BolosService.GetAllLiquidadores().Select(x => new Liquidador()
                                                                                            {
                                                                                                Id = x.Id,
                                                                                                Bolo = x.Bolo,
                                                                                                Numero = x.Numero,
                                                                                                IVA = x.IVA,
                                                                                                Subtotal = x.Subtotal,
                                                                                                Total = x.Total
                                                                                            }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        [HttpPost]
        //public JsonResult GetAllByBolo(int Id, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, CuentaCorrienteFilter filter, PagingRequest paging)
        public JsonResult GetAllByBolo(int Id, DateTime? start, DateTime? end, PagingRequest paging)
        {
            DateRange range = new DateRange(start, end);
            if (paging.page == 0)
            {
                return Json(new
                {
                    Data = this.BolosService.GetAllLiquidadoresByBolo(Id, range.Start, range.End).Select(x => new Liquidador()
                    {
                        Id = x.Id,
                        Bolo = x.Bolo,
                        //Estado = x.Estado,
                        FechaCreacion = x.FechaCreacion,
                        Numero = x.Numero,
                        //CondicionCompra = x.CondicionCompra,
                        //Letra = x.Letra,
                        IVA = x.IVA,
                        //Tipo = x.Tipo,
                        Subtotal = x.Subtotal,
                        Total = x.Total,
                        //TotalPagado = x.TotalPagado
                    }).ToList(),
                    Success = true
                });
            }
            else
            {
                PagingResponse<Liquidador> resp = new PagingResponse<Liquidador>();
                resp.Page = paging.page;
                resp.Records = this.BolosService.GetAllLiquidadoresByBolo(Id, range.Start, range.End).Select(x => new Liquidador()
                {
                    Id = x.Id,
                    Bolo = x.Bolo,
                    //Estado = x.Estado,
                    FechaCreacion = x.FechaCreacion,
                    Numero = x.Numero,
                    //CondicionCompra = x.CondicionCompra,
                    //Letra = x.Letra,
                    IVA = x.IVA,
                    //Tipo = x.Tipo,
                    Subtotal = x.Subtotal,
                    Total = x.Total,
                    //TotalPagado = x.TotalPagado
                }).ToList();
                resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
                resp.TotalRecords = resp.Records.Count;
                return Json(resp);
            }
        }

        //[HttpPost]
        //public JsonResult GetAllAcumulados(int Id, int idRubro, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, CuentaCorrienteFilter filter, PagingRequest paging)
        //{
        //    DateRange range = new DateRange(start, end);
        //    DateRange rangevenc = new DateRange(startvenc, endvenc);
        //    if (paging.page == 0)
        //    {
        //        return Json(new { Data = this.BolosService.GetAllCCAcumulados(Id, idRubro, range.Start, range.End, filter), Success = true });
        //    }
        //    else
        //    {
        //        PagingResponse<CuentaCorrienteItem> resp = new PagingResponse<CuentaCorrienteItem>();
        //        resp.Page = paging.page;
        //        resp.Records = this.BolosService.GetAllCCAcumulados(Id, idRubro, range.Start, range.End, filter);
        //        resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
        //        resp.TotalRecords = resp.Records.Count;
        //        return Json(resp);
        //    }
        //}

        //[HttpPost]
        //public JsonResult GetCuentaCorrienteByDates(int id, DateTime? start, DateTime? end, CuentaCorrienteFilter filter, PagingRequest paging)
        //{
        //    DateRange range = new DateRange(start, end);
        //    if (paging.page == 0)
        //    {
        //        return Json(new { Data = this.BolosService.GetCuentaCorrienteByDates(range.Start, range.End, id, filter), Success = true });
        //    }
        //    else
        //    {
        //        PagingResponse<CuentaCorrienteItem> resp = new PagingResponse<CuentaCorrienteItem>();
        //        resp.Page = paging.page;
        //        resp.Records = this.BolosService.GetCuentaCorrienteByDates(range.Start, range.End, id, filter);
        //        resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
        //        resp.TotalRecords = resp.Records.Count;
        //        return Json(resp);
        //    }
        //}

        //[HttpPost]
        //public JsonResult GetAllAPagarByProv(int IdProveedor, PagingRequest paging)
        //{
        //    if (paging.page == 0)
        //    {
        //        return Json(new
        //        {
        //            Data = this.BolosService.GetAllLiquidadorAPagarByProv(IdProveedor).Select(x => new Liquidador()
        //            {
        //                Id = x.Id,
        //                Proveedor = x.Proveedor,
        //                Estado = x.Estado,
        //                Fecha = x.Fecha,
        //                Numero = x.Numero,
        //                CondicionCompra = x.CondicionCompra,
        //                Letra = x.Letra,
        //                IVA = x.IVA,
        //                Tipo = x.Tipo,
        //                Subtotal = x.Subtotal,
        //                Total = x.Total,
        //                TotalPagado = x.TotalPagado
        //            }).ToList(),
        //            Success = true
        //        });
        //    }
        //    else
        //    {
        //        PagingResponse<Liquidador> resp = new PagingResponse<Liquidador>();
        //        resp.Page = paging.page;
        //        resp.Records = this.BolosService.GetAllLiquidadorAPagarByProv(IdProveedor).Select(x => new Liquidador()
        //        {
        //            Id = x.Id,
        //            Proveedor = x.Proveedor,
        //            Estado = x.Estado,
        //            Fecha = x.Fecha,
        //            Numero = x.Numero,
        //            CondicionCompra = x.CondicionCompra,
        //            Letra = x.Letra,
        //            IVA = x.IVA,
        //            Tipo = x.Tipo,
        //            Subtotal = x.Subtotal,
        //            Total = x.Total,
        //            TotalPagado = x.TotalPagado
        //        }).ToList();
        //        resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
        //        resp.TotalRecords = resp.Records.Count;
        //        return Json(resp);
        //    }
        //}

        //[HttpPost]
        //public JsonResult GetAllByProvFilterNC(int? IdProveedor, int? Tipo, int? NoTipo, LiquidadoresACancelarFilter Pagada, PagingRequest paging)
        //{
        //    if (paging.page == 0)
        //    {
        //        return Json(new
        //        {
        //            Data = this.BolosService.GetAllByProvFilterNC(IdProveedor, Tipo, NoTipo, Pagada),
        //            Success = true
        //        });
        //    }
        //    else
        //    {
        //        PagingResponse<Liquidador> resp = new PagingResponse<Liquidador>();
        //        resp.Page = paging.page;
        //        resp.Records = this.BolosService.GetAllByProvFilterNC(IdProveedor, Tipo, NoTipo, Pagada).Select(x => new Liquidador()
        //        {
        //            Id = x.Id,
        //            Proveedor = x.Proveedor,
        //            Estado = x.Estado,
        //            Fecha = x.Fecha,
        //            Numero = x.Numero,
        //            CondicionCompra = x.CondicionCompra,
        //            Letra = x.Letra,
        //            IVA = x.IVA,
        //            Tipo = x.Tipo,
        //            Subtotal = x.Subtotal,
        //            Total = x.Total,
        //            TotalPagado = x.TotalPagado
        //        }).ToList();
        //        resp.TotalPages = Convert.ToInt32(Math.Ceiling((double)resp.Records.Count / paging.rows));
        //        resp.TotalRecords = resp.Records.Count;
        //        return Json(resp);
        //    }
        //}




    }
}
