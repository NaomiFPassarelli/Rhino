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

        public CooperativaService(IPagoRepository PagoRepository, IAsociadoRepository AsociadoRepository)
        {
            this.PagoRepository = PagoRepository;
            this.AsociadoRepository = AsociadoRepository;
        }

        #endregion


        #region Pago
        public Pago GetPago(int Id)
        {
            Pago Pago = null;
            this.PagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Pago = this.PagoRepository.Get(Id);
            });
            return Pago;
        }

        public IList<Pago> GetAllPagos()
        {
            IList<Pago> Pagos = null;
            this.PagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Pagos = this.PagoRepository.GetAll();
            });
            return Pagos;
        }

        [Loggable]
        public void AddPagos(Pago Pago, int CantidadCuotasAPagar)
        {
            this.PagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Pago.FechaPago = Pago.FechaCreacion;
                Asociado a = this.AsociadoRepository.Get(Pago.Asociado.Id);
                int NumeroCuota = a.CantidadCuotasAbonadas;
                for (int i = 1; i <= CantidadCuotasAPagar; i++) {
                    Pago.NumeroCuota = NumeroCuota;
                    this.PagoRepository.Add(Pago);
                    NumeroCuota++;
                }
                a.CantidadCuotasAbonadas += CantidadCuotasAPagar;
                this.AsociadoRepository.Update(a);
            });
        }


        [Loggable]
        public void AddPago(Pago Pago)
        {
            this.PagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.PagoRepository.Add(Pago);
            });
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

        //public void DeletePagos(List<int> Ids)
        //{
        //    this.PagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        foreach (var Id in Ids)
        //        {
        //            Pago Pago = this.PagoRepository.Get(Id);
        //            this.PagoRepository.Update(Pago);
        //        }
        //    });
        //}


        #endregion

    }
}
