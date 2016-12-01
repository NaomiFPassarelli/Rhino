using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common;
using Woopin.SGC.CommonApp.Security;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Ventas;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Common.App.Logging;
using Woopin.SGC.Repositories.Cooperativa;
using Woopin.SGC.Model.Cooperativa;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Services
{
    
    public class CooperativaService : ICooperativaService
    {

        #region VariablesyConstructor

        private readonly IPagoRepository PagoRepository;
        private readonly IAsociadoRepository AsociadoRepository;
        private readonly IActaRepository ActaRepository;
        private readonly IActaPuntoExtraRepository ActaPuntoExtraRepository;
        private readonly IAdicionalPagoRepository AdicionalPagoRepository;
        private readonly IAporteRepository AporteRepository;

        public CooperativaService(IPagoRepository PagoRepository, IAsociadoRepository AsociadoRepository,
            IActaRepository ActaRepository, IActaPuntoExtraRepository ActaPuntoExtraRepository,
            IAdicionalPagoRepository AdicionalPagoRepository, IAporteRepository AporteRepository)
        {
            this.PagoRepository = PagoRepository;
            this.AsociadoRepository = AsociadoRepository;
            this.ActaRepository = ActaRepository;
            this.ActaPuntoExtraRepository = ActaPuntoExtraRepository;
            this.AdicionalPagoRepository = AdicionalPagoRepository;
            this.AporteRepository = AporteRepository;
        }

        #endregion


        #region Pago
        public Pago GetPago(int Id)
        {
            Pago Pago = null;
            this.PagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Pago = this.PagoRepository.Get(Id);
                Pago.AdicionalesPago = this.AdicionalPagoRepository.GetAllAdicionalesByPago(Pago.Id);
            });
            return Pago;
        }
        public IList<Pago> GetPagos(IList<int> Ids)
        {
            IList<Pago> Pagos = null;
            this.PagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Pagos = this.PagoRepository.GetPagos(Ids);
                foreach(Pago Pago in Pagos)
                {
                    Pago.AdicionalesPago = this.AdicionalPagoRepository.GetAllAdicionalesByPago(Pago.Id);
                }
            });
            return Pagos;
        
        }


        public IList<Pago> GetAllPagos(DateTime? start, DateTime? end)
        {
            IList<Pago> Pagos = null;
            this.PagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Pagos = this.PagoRepository.GetAll( _start, _end);
            });
            return Pagos;
        }

        public IList<Asociado> GetAllPorVencer()
        {
            IList<Asociado> Asociados = null;
            this.PagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociados = this.PagoRepository.GetAllPorVencer();
            });
            return Asociados;
        }


        public IList<Asociado> GetAllVencidos()
        {
            IList<Asociado> Asociados = null;
            this.PagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociados = this.PagoRepository.GetAllVencidos();
            });
            return Asociados;
        }

        //[Loggable]
        //public void AddPagos(Pago Pago, int CantidadCuotasAPagar)
        //{
        //    this.PagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        Pago.FechaPago = Pago.FechaCreacion;
        //        Asociado a = this.AsociadoRepository.Get(Pago.Asociado.Id);
        //        int NumeroCuota = a.CantidadCuotasAbonadas;
        //        for (int i = 1; i <= CantidadCuotasAPagar; i++) {
        //            Pago.NumeroCuota = NumeroCuota;
        //            this.PagoRepository.Add(Pago);
        //            NumeroCuota++;
        //        }
        //        a.CantidadCuotasAbonadas += CantidadCuotasAPagar;
        //        this.AsociadoRepository.Update(a);
        //    });
        //}


        [Loggable]
        public void AddPago(Pago Pago)
        {
            this.PagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddPagoNT(Pago);
            });
        }

        public void AddPagoNT(Pago Pago)
        {

            foreach (var adicional in Pago.AdicionalesPago)
            {
                adicional.Pago = Pago;
                adicional.Organizacion = Security.GetOrganizacion();
                //todo calcular totales
            }

            Asociado E = this.AsociadoRepository.Get(Pago.Asociado.Id);
            //if (E.CantidadPagosAbonados == E.CantidadPagos || E.AbonoTotalmente)
            //{
            //    return;
            //}
            //else
            //{
                int cantidadDePagosAbonadas = ++E.CantidadPagosAbonados;
                E.CantidadPagosAbonados = cantidadDePagosAbonadas;
                Pago.NumeroPago = cantidadDePagosAbonadas;
            //}
            //if (E.CantidadPagosAbonados == E.CantidadPagos)
            //{
            //    E.AbonoTotalmente = true;
            //}
            this.AsociadoRepository.Update(E);
            this.PagoRepository.Add(Pago);
        }

        public int GetProximoNumeroReferenciaPago() 
        {
            int proximo = 1;
            this.PagoRepository.GetSessionFactory().SessionInterceptor(() =>
                {
                    proximo = this.PagoRepository.GetProximoNumeroReferencia();
                });
            return proximo;
        }

        public IList<Pago> GetAllPagosByAsociado(int IdAsociado)
        {
            IList<Pago> Pagos = null;
            this.PagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Pagos = this.PagoRepository.GetAllPagosByAsociado(IdAsociado);
            });
            return Pagos;
        }

        public void DeletePagos(List<int> Ids)
        {
            this.PagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Pago Pago = this.PagoRepository.Get(Id);
                    //Pago.Asociado.AbonoTotalmente = false;
                    Pago.Asociado.CantidadPagosAbonados--;
                    this.AsociadoRepository.Update(Pago.Asociado);
                    Pago.Activo = false;
                    this.PagoRepository.Update(Pago);
                }
            });
        }
 

        #endregion

        #region Aporte
        public Aporte GetAporte(int Id)
        {
            Aporte Aporte = null;
            this.AporteRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Aporte = this.AporteRepository.Get(Id);
            });
            return Aporte;
        }

        public IList<Aporte> GetAllAportes(DateTime? start, DateTime? end)
        {
            IList<Aporte> Aportes = null;
            this.AporteRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                Aportes = this.AporteRepository.GetAll(_start, _end);
            });
            return Aportes;
        }

        //public IList<Asociado> GetAllPorVencer()
        //{
        //    IList<Asociado> Asociados = null;
        //    this.AporteRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        Asociados = this.AporteRepository.GetAllPorVencer();
        //    });
        //    return Asociados;
        //}


        //public IList<Asociado> GetAllVencidos()
        //{
        //    IList<Asociado> Asociados = null;
        //    this.AporteRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        Asociados = this.AporteRepository.GetAllVencidos();
        //    });
        //    return Asociados;
        //}

        [Loggable]
        public void AddAportes(Aporte Aporte, int CantidadCuotasAPagar)
        {
            this.AporteRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                //Aporte.FechaAporte = Aporte.FechaCreacion;
                Asociado a = this.AsociadoRepository.Get(Aporte.Asociado.Id);
                int NumeroCuota = ++a.CantidadAbonosEfectivos;
                for (int i = 1; i <= CantidadCuotasAPagar; i++)
                {
                    Aporte.NumeroAbono = NumeroCuota;
                    Aporte.FechaPeriodo = this.AporteRepository.GetProximoPeriodo(Aporte.Asociado.Id);
                    //Aporte.Total = 
                    Aporte.NumeroReferencia = this.AporteRepository.GetProximoNumeroReferencia();
                    this.AporteRepository.Add(Aporte);
                    NumeroCuota++;
                }
                a.CantidadAbonosEfectivos += CantidadCuotasAPagar;
                if(a.CantidadAbonosEfectivos == a.CantidadAbonos)
                {
                    a.AbonoFinalizado = true;
                }
                this.AsociadoRepository.Update(a);
            });
        }


        [Loggable]
        public void AddAporte(Aporte Aporte)
        {
            this.AporteRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddAporteNT(Aporte);
            });
        }

        public void AddAporteNT(Aporte Aporte)
        {

            //foreach (var adicional in Aporte.AdicionalesAporte)
            //{
            //    adicional.Aporte = Aporte;
            //    adicional.Organizacion = Security.GetOrganizacion();
            //    //todo calcular totales
            //}

            Asociado E = this.AsociadoRepository.Get(Aporte.Asociado.Id);
            if (E.CantidadPagosAbonados == E.CantidadAbonos || E.AbonoFinalizado)
            {
                return;
            }
            else
            {
                int cantidadDeAbonosAbonadas = ++E.CantidadPagosAbonados;
                E.CantidadPagosAbonados = cantidadDeAbonosAbonadas;
                Aporte.NumeroAbono = cantidadDeAbonosAbonadas;
            }
            if (E.CantidadPagosAbonados == E.CantidadAbonos)
            {
                E.AbonoFinalizado = true;
            }
            this.AsociadoRepository.Update(E);
            this.AporteRepository.Add(Aporte);
        }

        public int GetProximoNumeroReferenciaAporte()
        {
            int proximo = 1;
            this.AporteRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                proximo = this.AporteRepository.GetProximoNumeroReferencia();
            });
            return proximo;
        }

        public IList<Aporte> GetAllAportesByAsociado(int IdAsociado)
        {
            IList<Aporte> Aportes = null;
            this.AporteRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Aportes = this.AporteRepository.GetAllAportesByAsociado(IdAsociado);
            });
            return Aportes;
        }

        public void DeleteAportes(List<int> Ids)
        {
            this.AporteRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Aporte Aporte = this.AporteRepository.Get(Id);
                    Aporte.Asociado.AbonoFinalizado = false;
                    Aporte.Asociado.CantidadAbonosEfectivos--;
                    this.AsociadoRepository.Update(Aporte.Asociado);
                    Aporte.Activo = false;
                    this.AporteRepository.Update(Aporte);
                }
            });
        }


        #endregion


        #region Acta
        public Acta GetActa(int Id)
        {
            Acta Acta = null;
            this.ActaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Acta = this.ActaRepository.Get(Id);
            });
            return Acta;
        }

        public IList<Acta> GetActas(IList<int> Ids)
        {
            IList<Acta> Actas = null;
            this.ActaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Actas = this.ActaRepository.GetActas(Ids);
                foreach (Acta Acta in Actas)
                {
                    Acta.AsociadosIngreso = this.AsociadoRepository.GetAsociadosMes(Acta.FechaActa.Month, Acta.FechaActa.Year);
                    Acta.AsociadosEgreso = this.AsociadoRepository.GetAsociadosMesEgreso(Acta.FechaActa.Month, Acta.FechaActa.Year);
                    Acta.OtrosPuntos = this.ActaPuntoExtraRepository.GetActaPuntoExtraByActa(Acta.Id);
                }
            });
            return Actas;

        }

        public IList<Acta> GetAllActas()
        {
            IList<Acta> Actas = null;
            this.ActaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Actas = this.ActaRepository.GetAll();
                foreach (Acta Acta in Actas)
                {
                    Acta.AsociadosEgreso = null;
                    Acta.AsociadosIngreso = null;
                    Acta.OtrosPuntos = null;
                }
            });
            return Actas;
        }

        public Acta GetActaCompleta(int ActaId)
        {
            Acta Acta = null;
            this.ActaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Acta = this.ActaRepository.GetActaCompleta(ActaId);
                //foreach (Acta Acta in Actas)
                //{
                    //TODO null o GetAsociadosMesEgreso() + GetAsociadosMes() + getOtrospuntos
                Acta.AsociadosIngreso = this.AsociadoRepository.GetAsociadosMes(Acta.FechaActa.Month, Acta.FechaActa.Year);
                Acta.AsociadosEgreso = this.AsociadoRepository.GetAsociadosMesEgreso(Acta.FechaActa.Month, Acta.FechaActa.Year);
                    Acta.OtrosPuntos = this.ActaPuntoExtraRepository.GetActaPuntoExtraByActa(Acta.Id);
                //}
            });
            return Acta;
        }
        

        public IList<Acta> GetAllActasCompletas()
        {
            IList<Acta> Actas = null;
            this.ActaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Actas = this.ActaRepository.GetAllActasCompletas();
                foreach (Acta Acta in Actas)
                {
                    //TODO null o GetAsociadosMesEgreso() + GetAsociadosMes() + getOtrospuntos
                    Acta.AsociadosEgreso = this.AsociadoRepository.GetAsociadosMes(Acta.FechaActa.Month, Acta.FechaActa.Year);
                    Acta.AsociadosIngreso = this.AsociadoRepository.GetAsociadosMesEgreso(Acta.FechaActa.Month, Acta.FechaActa.Year);
                    Acta.OtrosPuntos = this.ActaPuntoExtraRepository.GetActaPuntoExtraByActa(Acta.Id);
                }
            });
            return Actas;
        }

        
        [Loggable]
        public void AddActa(Acta Acta)
        {
            this.ActaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Acta.AsociadosIngreso = this.AsociadoRepository.GetAsociadosMes(Acta.FechaActa.Month, Acta.FechaActa.Year);
                Acta.AsociadosEgreso = this.AsociadoRepository.GetAsociadosMesEgreso(Acta.FechaActa.Month, Acta.FechaActa.Year);
                this.ActaRepository.Add(Acta);
            });
        }

        public Acta GetActaByFecha(DateTime endOfMonth)
        { 
            Acta Acta = null;
            this.ActaRepository.GetSessionFactory().SessionInterceptor(() =>
                {
                    Acta = this.ActaRepository.GetActaByFecha(endOfMonth);
                });
            return Acta;
        }


        public void DeleteActas(List<int> Ids)
        {
            this.ActaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Acta Acta = this.ActaRepository.Get(Id);

                    IList<Asociado> AsociadosIngreso = this.AsociadoRepository.GetAsociadosMes(Acta.FechaActa.Month, Acta.FechaActa.Year);
                    IList<Asociado> AsociadosEgreso = this.AsociadoRepository.GetAsociadosMesEgreso(Acta.FechaActa.Month, Acta.FechaActa.Year);
                    foreach (Asociado AsociadoIng in AsociadosIngreso) {
                        AsociadoIng.ActaAlta = null;
                        this.AsociadoRepository.Update(AsociadoIng);
                    }
                    foreach (Asociado AsociadoEg in AsociadosEgreso)
                    {
                        AsociadoEg.ActaBaja = null;
                        this.AsociadoRepository.Update(AsociadoEg);
                    }
                    Acta.AsociadosEgreso = null;
                    Acta.AsociadosIngreso = null;
                    
                    this.ActaRepository.Delete(Acta);
                }
            });
        }




        #endregion
        #region
        public IList<ActaPuntoExtra> GetActaPuntoExtraByActa(int ActaId)
        {
            IList<ActaPuntoExtra> APE = null;
            this.ActaPuntoExtraRepository.GetSessionFactory().SessionInterceptor(() =>
                {
                    APE = this.ActaPuntoExtraRepository.GetActaPuntoExtraByActa(ActaId);
                });
            return APE;
        }

        #endregion


    }
}
