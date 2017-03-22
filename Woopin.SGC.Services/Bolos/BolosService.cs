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
using Woopin.SGC.Repositories.Bolos;
using Woopin.SGC.Model.Bolos;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Services
{
    
    public class BolosService : IBolosService
    {

        #region VariablesyConstructor

        private readonly ITrabajadorRepository TrabajadorRepository;
        private readonly IEscalafonRepository EscalafonRepository;
        private readonly IContabilidadService ContabilidadService;
        private readonly ILiquidadorRepository LiquidadorRepository;

        public BolosService(ITrabajadorRepository TrabajadorRepository, IEscalafonRepository EscalafonRepository,
                            IContabilidadService ContabilidadService, ILiquidadorRepository LiquidadorRepository)
        {
            this.TrabajadorRepository = TrabajadorRepository;
            this.EscalafonRepository = EscalafonRepository;
            this.ContabilidadService = ContabilidadService;
            this.LiquidadorRepository = LiquidadorRepository;
        }

        #endregion


        #region Liquidador
        public Liquidador GetLiquidador(int Id)
        {
            Liquidador Liquidador = null;
            this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Liquidador = this.LiquidadorRepository.Get(Id);
            });
            return Liquidador;
        }

        public Liquidador GetLiquidadorCompleto(int Id)
        {
            Liquidador Liquidador = null;
            this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
                {
                    Liquidador = this.LiquidadorRepository.GetCompleto(Id);
                });
            return Liquidador;
        }
        public IList<Liquidador> GetLiquidadores(IList<int> Ids)
        {
            IList<Liquidador> Liquidadores = null;
            this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Liquidadores = this.LiquidadorRepository.GetLiquidadores(Ids);
            });
            return Liquidadores;

        }


        //public IList<Liquidador> GetAllLiquidadores(DateTime? start, DateTime? end)
        //{
        //    IList<Liquidador> Liquidadores = null;
        //    this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        DateTime _start = start.HasValue ? start.Value : DateTime.Now;
        //        DateTime _end = end.HasValue ? end.Value : DateTime.Now;
        //        if (!start.HasValue && !end.HasValue)
        //        {
        //            _start = _start.AddMonths(-1);
        //        }
        //        Liquidadores = this.LiquidadorRepository.GetAllByDates(_start, _end);
        //    });
        //    return Liquidadores;
        //}

        [Loggable]
        public void AddLiquidador(Liquidador Liquidador)
        {
            this.LiquidadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {

                this.AddLiquidadorNT(Liquidador);
            });
        }
        public void AddLiquidadorNT(Liquidador Liquidador)
        {

            //foreach (var adicional in Liquidador.AdicionalesLiquidador)
            //{
            //    adicional.Liquidador = Liquidador;
            //    adicional.Organizacion = Security.GetOrganizacion();
            //    //todo calcular totales
            //    if (adicional.Id == 8011) //Adicional es Vacaciones entonces sumar al empleado
            //    {
            //        Empleado E = this.EmpleadoRepository.Get(Liquidador.Empleado.Id);
            //        E.VacacionesYaGozadas += adicional.Unidades;
            //        this.EmpleadoRepository.Update(E);
            //    }
            //}
            //TODO
            //Liquidador.Asiento = this.ContabilidadService.NuevoAsientoLiquidadoresueldo(Liquidador);
            this.LiquidadorRepository.Add(Liquidador);
        }

        public void DeleteLiquidadores(List<int> Ids)
        {
            this.LiquidadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Liquidador Liquidador = this.LiquidadorRepository.Get(Id);
                    //foreach (AdicionalLiquidador AR in Liquidador.AdicionalesLiquidador)
                    //{
                    //    if (AR.Id == 8011) //Adicional es Vacaciones entonces sumar al empleado
                    //    {
                    //        Empleado E = this.EmpleadoRepository.Get(Liquidador.Empleado.Id);
                    //        E.VacacionesYaGozadas -= AR.Unidades;
                    //        this.EmpleadoRepository.Update(E);
                    //    }
                    //}

                    Liquidador.Activo = false;
                    this.LiquidadorRepository.Update(Liquidador);
                }
            });
        }

        public SelectCombo GetLiquidadorCombos()
        {
            SelectCombo SelectLiquidadorCombos = new SelectCombo();
            this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectLiquidadorCombos.Items = this.LiquidadorRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Trabajador.Nombre
                                                              }).ToList();
            });
            return SelectLiquidadorCombos;
        }

        public SelectCombo GetAllLiquidadoresByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectLiquidadorCombos = new SelectCombo();
            this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectLiquidadorCombos.Items = this.LiquidadorRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Trabajador.Nombre
                                                              }).ToList();
            });
            return SelectLiquidadorCombos;
        }

        public int GetProximoNumeroReferencia()
        {
            int ProximoNumeroReferencia = 1;
            this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ProximoNumeroReferencia = this.LiquidadorRepository.GetProximoNumeroReferencia();
            });
            return ProximoNumeroReferencia;
        }

        public Liquidador GetLiquidadorAnterior()
        {
            Liquidador LiquidadorAnterior = null;
            this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                LiquidadorAnterior = this.LiquidadorRepository.GetLiquidadorAnterior();
            });
            return LiquidadorAnterior;
        }

        //public decimal GetMejorRemuneracion(int IdEmpleado)
        //{
        //    decimal MejorRemuneracion = 0;
        //    this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        MejorRemuneracion = this.LiquidadorRepository.GetMejorRemuneracion(IdEmpleado);
        //    });
        //    return MejorRemuneracion;
        //}

        //public decimal[] GetPromedioRemunerativo(int IdEmpleado)
        //{
        //    decimal[] Promedio = null;
        //    this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
        //        {
        //            Promedio = this.LiquidadorRepository.GetPromedioRemunerativo(IdEmpleado);
        //        });
        //    return Promedio;

        //}

        public void UpdateLiquidadorNT(Liquidador Liquidador)
        {
            this.LiquidadorRepository.Update(Liquidador);
        }

        [Loggable]
        public void AnularLiquidador(int IdLiquidador)
        {
            Liquidador l = this.GetLiquidador(IdLiquidador);
            this.LiquidadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                //Controlar fecha contable
                //this.ContabilidadService.TryControlarIngresoNT(l.FechaContable);

                //l.TryAnular();

                //l.Estado = EstadoComprobante.Anulada;
                //int asientoId = l.Asiento.Id;
                //l.Asiento = null;
                l.Activo = false;
                this.UpdateLiquidadorNT(l);
                //this.ContabilidadService.DeleteAsientoNT(asientoId);
            });

        }


        [Loggable]
        public void EliminarLiquidador(int IdLiquidador)
        {
            Liquidador l = this.GetLiquidador(IdLiquidador);
            this.LiquidadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                //Controlar fecha contable
                //this.ContabilidadService.TryControlarIngresoNT(l.FechaContable);

                //l.TryAnular();

                //int asientoId = l.Asiento.Id;
                //l.Asiento = null;
                //this.LiquidadorRepository.Delete(l);
                l.Activo = false;
                this.UpdateLiquidadorNT(l);
                //this.ContabilidadService.DeleteAsientoNT(asientoId);
            });

        }

        public IList<Liquidador> GetAllLiquidadores()
        {
            IList<Liquidador> Liquidadores = null;
            this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Liquidadores = this.LiquidadorRepository.GetAll();
            });
            return Liquidadores;
        }

        //public IList<Liquidador> GetAllLiquidadoresByBolo(int IdBolo, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, Model.Common.CuentaCorrienteFilter filter)
        public IList<Liquidador> GetAllLiquidadoresByBolo(int IdBolo, DateTime? start, DateTime? end)
        {
            IList<Liquidador> Liquidadores = null;
            this.LiquidadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                DateTime _start = start.HasValue ? start.Value : DateTime.Now;
                DateTime _end = end.HasValue ? end.Value : DateTime.Now;
                //DateTime _startvenc = startvenc.HasValue ? startvenc.Value : DateTime.Parse("1970-01-01");
                //DateTime _endvenc = endvenc.HasValue ? endvenc.Value : DateTime.Parse("9998-12-31");
                if (!start.HasValue && !end.HasValue)
                {
                    _start = _start.AddMonths(-1);
                }
                //Liquidadores = this.LiquidadorRepository.GetAllByBolo(IdBolo, _start, _end, _startvenc, _endvenc, filter);
                Liquidadores = this.LiquidadorRepository.GetAllByBolo(IdBolo, _start, _end);
            });
            return Liquidadores;
        }


        #endregion


    }
}
