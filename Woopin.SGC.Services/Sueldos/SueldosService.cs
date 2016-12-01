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
using Woopin.SGC.Repositories.Sueldos;
using Woopin.SGC.Model.Sueldos;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Services
{
    
    public class SueldosService : ISueldosService
    {

        #region VariablesyConstructor

        private readonly IEmpleadoRepository EmpleadoRepository;
        private readonly IReciboRepository ReciboRepository;
        private readonly IContabilidadService ContabilidadService;

        public SueldosService(IEmpleadoRepository EmpleadoRepository, IReciboRepository ReciboRepository,
                            IContabilidadService ContabilidadService)
        {
            this.EmpleadoRepository = EmpleadoRepository;
            this.ReciboRepository = ReciboRepository;
            this.ContabilidadService = ContabilidadService;
        }

        #endregion


        #region Recibo
        public Recibo GetRecibo(int Id)
        {
            Recibo Recibo = null;
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Recibo = this.ReciboRepository.Get(Id);
            });
            return Recibo;
        }

        public Recibo GetReciboCompleto(int Id)
        {
            Recibo Recibo = null;
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
                {
                    Recibo = this.ReciboRepository.GetCompleto(Id);
                });
            return Recibo;
        }
        public IList<Recibo> GetRecibos(IList<int> Ids)
        {
            IList<Recibo> Recibos = null;
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Recibos = this.ReciboRepository.GetRecibos(Ids);
            });
            return Recibos;

        }


        public IList<Recibo> GetAllRecibos()
        {
            IList<Recibo> Recibos = null;
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Recibos = this.ReciboRepository.GetAll();
            });
            return Recibos;
        }

        [Loggable]
        public void AddRecibo(Recibo Recibo)
        {
            this.ReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                
                this.AddReciboNT(Recibo);
            });
        }
        public void AddReciboNT(Recibo Recibo)
        {

            foreach (var adicional in Recibo.AdicionalesRecibo)
            {
                adicional.Recibo = Recibo;
                adicional.Organizacion = Security.GetOrganizacion();
                //todo calcular totales
                if(adicional.Id == 8011) //Adicional es Vacaciones entonces sumar al empleado
                {
                    Empleado E = this.EmpleadoRepository.Get(Recibo.Empleado.Id);
                    E.VacacionesYaGozadas += adicional.Unidades;
                    this.EmpleadoRepository.Update(E);
                }
            }
            //TODO
            //Recibo.Asiento = this.ContabilidadService.NuevoAsientoReciboSueldo(Recibo);
            this.ReciboRepository.Add(Recibo);
        }

        //[Loggable]
        //public void UpdateRecibo(Recibo Recibo)
        //{
        //    this.ReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        Recibo ToUpdate = this.ReciboRepository.Get(Recibo.Id);
        //        ToUpdate.Apellido = Recibo.Apellido;
        //        ToUpdate.CodigoPostal = Recibo.CodigoPostal;
        //        ToUpdate.CUIT = Recibo.CUIT;
        //        ToUpdate.DNI = Recibo.DNI;
        //        ToUpdate.Departamento = Recibo.Departamento;
        //        ToUpdate.Direccion = Recibo.Direccion;
        //        ToUpdate.Email = Recibo.Email;
        //        ToUpdate.Localizacion = Recibo.Localizacion;
        //        ToUpdate.Nombre = Recibo.Nombre;
        //        ToUpdate.Numero = Recibo.Numero;
        //        ToUpdate.Piso = Recibo.Piso;
        //        ToUpdate.SueldoBrutoHora = Recibo.SueldoBrutoHora;
        //        ToUpdate.SueldoBrutoMensual = Recibo.SueldoBrutoMensual;
        //        ToUpdate.Telefono = Recibo.Telefono;
        //        ToUpdate.FechaIngreso = Recibo.FechaIngreso;
        //        ToUpdate.FechaNacimiento = Recibo.FechaNacimiento;
        //        if ((ToUpdate.Categoria == null && Recibo.Categoria != null) || (ToUpdate.Categoria != null && Recibo.Categoria != null && ToUpdate.Categoria.Id != Recibo.Categoria.Id))
        //        {
        //            ToUpdate.Categoria = new ComboItem();
        //            ToUpdate.Categoria.Id = Recibo.Categoria.Id;
        //        }
        //        else
        //        {
        //            ToUpdate.Categoria = Recibo.Categoria;
        //        }
        //        if ((ToUpdate.Sexo == null && Recibo.Sexo != null) || (ToUpdate.Sexo != null && Recibo.Sexo != null && ToUpdate.Sexo.Id != Recibo.Sexo.Id))
        //        {
        //            ToUpdate.Sexo = new ComboItem();
        //            ToUpdate.Sexo.Id = Recibo.Sexo.Id;
        //        }
        //        else
        //        {
        //            ToUpdate.Sexo = Recibo.Sexo;
        //        }
        //        if ((ToUpdate.EstadoCivil == null && Recibo.EstadoCivil != null) || (ToUpdate.EstadoCivil != null && Recibo.EstadoCivil != null && ToUpdate.EstadoCivil.Id != Recibo.EstadoCivil.Id))
        //        {
        //            ToUpdate.EstadoCivil = new ComboItem();
        //            ToUpdate.EstadoCivil.Id = Recibo.EstadoCivil.Id;
        //        }
        //        else
        //        {
        //            ToUpdate.EstadoCivil = Recibo.EstadoCivil;
        //        }
        //        if ((ToUpdate.Tarea == null && Recibo.Tarea != null) || (ToUpdate.Tarea != null && Recibo.Tarea != null && ToUpdate.Tarea.Id != Recibo.Tarea.Id))
        //        {
        //            ToUpdate.Tarea = new ComboItem();
        //            ToUpdate.Tarea.Id = Recibo.Tarea.Id;
        //        }
        //        else
        //        {
        //            ToUpdate.Tarea = Recibo.Tarea;
        //        }
        //        if (ToUpdate.Nacionalidad.Id != Recibo.Nacionalidad.Id)
        //        {
        //            ToUpdate.Nacionalidad = new ComboItem();
        //            ToUpdate.Nacionalidad.Id = Recibo.Nacionalidad.Id;
        //        }
        //        this.ReciboRepository.Update(ToUpdate);
        //    });
        //}
        public void DeleteRecibos(List<int> Ids)
        {
            this.ReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Recibo Recibo = this.ReciboRepository.Get(Id);
                    foreach(AdicionalRecibo AR in Recibo.AdicionalesRecibo)
                    {
                        if(AR.Id == 8011) //Adicional es Vacaciones entonces sumar al empleado
                        {
                            Empleado E = this.EmpleadoRepository.Get(Recibo.Empleado.Id);
                            E.VacacionesYaGozadas -= AR.Unidades;
                            this.EmpleadoRepository.Update(E);
                        }
                    }

                    Recibo.Activo = false;
                    this.ReciboRepository.Update(Recibo);
                }
            });
        }

        public SelectCombo GetReciboCombos()
        {
            SelectCombo SelectReciboCombos = new SelectCombo();
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectReciboCombos.Items = this.ReciboRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Empleado.Nombre
                                                              }).ToList();
            });
            return SelectReciboCombos;
        }

        public SelectCombo GetAllRecibosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectReciboCombos = new SelectCombo();
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectReciboCombos.Items = this.ReciboRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Empleado.Nombre
                                                              }).ToList();
            });
            return SelectReciboCombos;
        }

        public int GetProximoNumeroReferencia()
        {
            int ProximoNumeroReferencia = 1;
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ProximoNumeroReferencia = this.ReciboRepository.GetProximoNumeroReferencia();
            });
            return ProximoNumeroReferencia;
        }

        public Recibo GetReciboAnterior(int IdEmpleado)
        {
            Recibo ReciboAnterior = null;
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ReciboAnterior = this.ReciboRepository.GetReciboAnterior(IdEmpleado);
                if (ReciboAnterior != null)
                {
                    ReciboAnterior.AdicionalesRecibo = null;
                }
            });
            return ReciboAnterior;
        }

        public decimal GetMejorRemuneracion(int IdEmpleado)
        {
            decimal MejorRemuneracion = 0;
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                MejorRemuneracion = this.ReciboRepository.GetMejorRemuneracion(IdEmpleado);
            });
            return MejorRemuneracion;
        }

        public decimal[] GetPromedioRemunerativo(int IdEmpleado)
        {
            decimal[] Promedio = null;
            this.ReciboRepository.GetSessionFactory().SessionInterceptor(() =>
                {
                    Promedio = this.ReciboRepository.GetPromedioRemunerativo(IdEmpleado);
                });
            return Promedio;
        
        }
        

        #endregion


    }
}
