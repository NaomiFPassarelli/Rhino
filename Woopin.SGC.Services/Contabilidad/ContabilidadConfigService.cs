using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Repositories.Contabilidad;

namespace Woopin.SGC.Services
{
    public class ContabilidadConfigService : IContabilidadConfigService
    {
        #region VariablesyConstructor
        private readonly ICuentaRepository cuentaRepository;
        private readonly IEjercicioRepository EjercicioRepository;
        private readonly IBloqueoContableRepository BloqueoContableRepository;
        private readonly IRetencionRepository RetencionRepository;

        public ContabilidadConfigService(ICuentaRepository cuentaRepository, IEjercicioRepository EjercicioRepository, IBloqueoContableRepository BloqueoContableRepository, IRetencionRepository RetencionRepository)
        {
            this.cuentaRepository = cuentaRepository;
            this.EjercicioRepository = EjercicioRepository;
            this.BloqueoContableRepository = BloqueoContableRepository;
            this.RetencionRepository = RetencionRepository;
        }
        #endregion

        #region Cuenta
        public Cuenta GetCuenta(int Id)
        {
            Cuenta cuenta = null;
            this.cuentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                cuenta = this.cuentaRepository.Get(Id);
            });
            return cuenta;
        }

        public void AddCuenta(Cuenta cuenta)
        {
            this.cuentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddCuentaNT(cuenta);
            });
        }

        public void AddCuentaNT(Cuenta cuenta)
        {
            this.cuentaRepository.Create(cuenta);
        }


        public void UpdateCuenta(Cuenta cuenta)
        {
            this.cuentaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.cuentaRepository.Update(cuenta);
            });
        }


        public IList<Cuenta> GetAllCuentas()
        {
            IList<Cuenta> cuentas = null;
            this.cuentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                cuentas = this.cuentaRepository.GetAll();
            });
            return cuentas;
        }

        public IList<Cuenta> GetRubros()
        {
            IList<Cuenta> cuentas = null;
            this.cuentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                cuentas = this.cuentaRepository.GetRubros();
            });
            return cuentas;
        }

        public IList<Cuenta> GetCorrientes(int Rubro)
        {
            IList<Cuenta> cuentas = null;
            this.cuentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                cuentas = this.cuentaRepository.GetCorriente(Rubro);
            });
            return cuentas;
        }

        public IList<Cuenta> GetSubRubros(int Rubro, int Corriente)
        {
            IList<Cuenta> cuentas = null;
            this.cuentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                cuentas = this.cuentaRepository.GetSubRubros(Rubro,Corriente);
            });
            return cuentas;
        }
        
        public SelectCombo GetCuentaIngresosCombo()
        {
            SelectCombo SelectCuentaCombos = new SelectCombo();
            this.cuentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectCuentaCombos.Items = this.cuentaRepository.GetCuentasdeIngresos()
                                                                .Select(x => new SelectComboItem(){
                                                                    id = x.Id,
                                                                    text = x.Nombre + " (" + x.Codigo + ")"
                                                                }).ToList();
            });
            return SelectCuentaCombos;
        }

        public SelectCombo GetCuentaEgresosCombo()
        {
            SelectCombo SelectCuentaCombos = new SelectCombo();
            this.cuentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectCuentaCombos.Items = this.cuentaRepository.GetCuentasdeEgresos()
                                                                .Select(x => new SelectComboItem()
                                                                {
                                                                    id = x.Id,
                                                                    text = x.Nombre + " (" + x.Codigo + ")"
                                                                }).ToList();
            });
            return SelectCuentaCombos;
        }

        public IList<Cuenta> GetSubRubrosEgresosCombo()
        {
            IList<Cuenta> List = null;
            this.cuentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                List = this.cuentaRepository.GetSubRubrosEgresos();
            });
            return List;
        }

        public SelectCombo GetAllCuentasByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectCombos = new SelectCombo();
            this.cuentaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectCombos.Items = this.cuentaRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + '(' + x.Codigo + ')'
                                                              }).ToList();
            });
            return SelectCombos;
        }

        
        #endregion

        #region Ejercicios Contable
        public Ejercicio GetEjercicio(int Id)
        {
            Ejercicio ejercicio = null;
            this.EjercicioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ejercicio = this.EjercicioRepository.Get(Id);
            });
            return ejercicio;
        }
        public Ejercicio GetEjercicioCompleto(int Id)
        {
            Ejercicio ejercicio = null;
            this.EjercicioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ejercicio = this.EjercicioRepository.GetCompleto(Id);
            });
            return ejercicio;
        }

        public void AddEjercicio(Ejercicio ejercicio)
        {
            this.EjercicioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {

                this.EjercicioRepository.Add(ejercicio);
            });
        }

        public void UpdateEjercicio(Ejercicio ejercicio)
        {
            this.EjercicioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Ejercicio ToUpdate = this.EjercicioRepository.Get(ejercicio.Id);
                ToUpdate.Nombre = ejercicio.Nombre;
                ToUpdate.Inicio = ejercicio.Inicio;
                ToUpdate.Final = ejercicio.Final;
                this.EjercicioRepository.Update(ToUpdate);
            });
        }

        public IList<Ejercicio> GetAllEjercicios()
        {
            IList<Ejercicio> ejercicios = null;
            this.EjercicioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ejercicios = this.EjercicioRepository.GetAll();
            });
            return ejercicios;
        }
        public IList<Ejercicio> GetAllAvailableEjercicios()
        {
            IList<Ejercicio> ejercicios = null;
            this.EjercicioRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ejercicios = this.EjercicioRepository.GetAllAvailable();
            });
            return ejercicios;
        }
        
        public void DeleteEjercicio(int Id)
        {
            this.EjercicioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Ejercicio e = this.EjercicioRepository.Get(Id);
                e.Activo = false;
                this.EjercicioRepository.Update(e);
            });
        }

        public void EjercicioCambiarCerrado(int Id, bool Cerrado)
        {
            this.EjercicioRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Ejercicio e = this.EjercicioRepository.Get(Id);
                e.Cerrado = Cerrado;
                this.EjercicioRepository.Update(e);
            });
        }

        #endregion

        #region Bloqueos Contables

        public void AddBloqueoContable(BloqueoContable bloqueo)
        {
            this.BloqueoContableRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if(this.BloqueoContableRepository.GetByDates(bloqueo.Inicio,bloqueo.Final).Count > 0)
                {
                    throw new ValidationException("Ya se encuentra un bloqueo contable creado en ese periodo");
                }
                this.BloqueoContableRepository.Add(bloqueo);
            });
        }

        public void UpdateBloqueoContable(int Id, bool Cerrado)
        {
            this.BloqueoContableRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                BloqueoContable b = this.BloqueoContableRepository.Get(Id);
                b.Activo = Cerrado;
                this.BloqueoContableRepository.Update(b);
            });
        }
        #endregion

        #region Retenciones
        public Retencion GetRetencion(int Id)
        {
            Retencion Retencion = null;
            this.RetencionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Retencion = this.RetencionRepository.Get(Id);
            });
            return Retencion;
        }
        public void AddRetencion(Retencion Retencion)
        {
            this.RetencionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Retencion.CuentaContable = CuentaContableHelper.GetCuentaRetencion();
                Retencion.CuentaContable.Nombre = Retencion.Descripcion;
                this.cuentaRepository.Create(Retencion.CuentaContable);
                Retencion.CuentaADepositar = CuentaContableHelper.GetCuentaRetencionADepositar();
                Retencion.CuentaADepositar.Nombre = Retencion.Descripcion + " a Depositar";
                this.cuentaRepository.Create(Retencion.CuentaADepositar);
                this.RetencionRepository.Add(Retencion);
            });
        }
        public void UpdateRetencion(Retencion Retencion)
        {
            this.RetencionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Retencion ToUpdate = this.RetencionRepository.Get(Retencion.Id);
                ToUpdate.Abreviatura = Retencion.Abreviatura;
                ToUpdate.Descripcion = Retencion.Descripcion;
                ToUpdate.Juridiccion = Retencion.Juridiccion;
                ToUpdate.EsValor = Retencion.EsValor;
                this.RetencionRepository.Update(ToUpdate);
            });
        }
        public IList<Retencion> GetAllRetenciones()
        {
            IList<Retencion> Retenciones = null;
            this.RetencionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Retenciones = this.RetencionRepository.GetAll();
            });
            return Retenciones;
        }
        public void DeleteRetenciones(List<int> Ids)
        {
            this.RetencionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Retencion Retencion = this.RetencionRepository.Get(Id);
                    Retencion.Activo = false;
                    this.RetencionRepository.Update(Retencion);
                }
            });
        }
        public IList<Retencion> GetAllRetencionesValor()
        {
            IList<Retencion> Retenciones = null;
            this.RetencionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Retenciones = this.RetencionRepository.GetAllValor();
            });
            return Retenciones;
        }

        public SelectCombo GetRetencionCombos(SelectComboRequest req)
        {
            SelectCombo SelectCombosRetencion = new SelectCombo();
            this.RetencionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectCombosRetencion.Items = this.RetencionRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion + '(' + x.Abreviatura + ')'
                                                              }).ToList();
            });
            return SelectCombosRetencion;
        }
        
        #endregion
    }
}
