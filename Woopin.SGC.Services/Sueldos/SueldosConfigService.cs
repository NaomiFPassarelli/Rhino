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
    
    public class SueldosConfigService : ISueldosConfigService
    {

        #region VariablesyConstructor

        private readonly IEmpleadoRepository EmpleadoRepository;
        private readonly IAdicionalRepository AdicionalRepository;
        private readonly IAdicionalReciboRepository AdicionalReciboRepository;
        private readonly IAdicionalAdicionalesRepository AdicionalAdicionalesRepository;
        private readonly IComboItemRepository ComboItemRepository;
        private readonly ILocalizacionRepository LocalizacionRepository;

        public SueldosConfigService(IEmpleadoRepository EmpleadoRepository, IAdicionalRepository AdicionalRepository,
                                        IComboItemRepository ComboItemRepository, ILocalizacionRepository LocalizacionRepository,
                                        IAdicionalAdicionalesRepository AdicionalAdicionalesRepository, IAdicionalReciboRepository AdicionalReciboRepository)
        {
            this.EmpleadoRepository = EmpleadoRepository;
            this.AdicionalRepository = AdicionalRepository;
            this.AdicionalReciboRepository = AdicionalReciboRepository;
            this.AdicionalAdicionalesRepository = AdicionalAdicionalesRepository;
            this.ComboItemRepository = ComboItemRepository;
            this.LocalizacionRepository = LocalizacionRepository;
        }

        #endregion

        #region Empleado
        public Empleado GetEmpleado(int Id)
        {
            Empleado Empleado = null;
            this.EmpleadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Empleado = this.EmpleadoRepository.Get(Id);
            });
            return Empleado;
        }

        public Empleado GetEmpleadoCompleto(int Id)
        {
            Empleado Empleado = null;
            this.EmpleadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Empleado = this.EmpleadoRepository.GetCompleto(Id);
            });
            return Empleado;
        }

        public IList<Empleado> GetAllEmpleados()
        {
            IList<Empleado> Empleados = null;
            this.EmpleadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Empleados = this.EmpleadoRepository.GetAll();
            });
            return Empleados;
        }

        [Loggable]
        public void AddEmpleado(Empleado Empleado)
        {
            this.EmpleadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (!ComprobanteHelper.IsCUITValid(Empleado.CUIT))
                    throw new BusinessException("El CUIT no es valido");

                if (this.ExistCUITNT(Empleado.CUIT, null))
                    throw new BusinessException("El CUIT ya existe");

                this.AddEmpleadoNT(Empleado); 
            });
        }
        public void AddEmpleadoNT(Empleado Empleado)
        {
            this.EmpleadoRepository.Add(Empleado);
        }

        [Loggable]
        public void UpdateEmpleado(Empleado Empleado)
        {
            this.EmpleadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (!ComprobanteHelper.IsCUITValid(Empleado.CUIT))
                    throw new BusinessException("El CUIT no es valido");

                if (this.ExistCUITNT(Empleado.CUIT, Empleado.Id))
                    throw new BusinessException("El CUIT ya existe");


                Empleado ToUpdate = this.EmpleadoRepository.Get(Empleado.Id);
                ToUpdate.Apellido = Empleado.Apellido;
                ToUpdate.CodigoPostal = Empleado.CodigoPostal;
                ToUpdate.CUIT = Empleado.CUIT;
                ToUpdate.DNI = Empleado.DNI;
                ToUpdate.Departamento = Empleado.Departamento;
                ToUpdate.Direccion = Empleado.Direccion;
                ToUpdate.Email = Empleado.Email;
                ToUpdate.Nombre = Empleado.Nombre;
                ToUpdate.Numero = Empleado.Numero;
                ToUpdate.Piso = Empleado.Piso;
                ToUpdate.SueldoBrutoHora = Empleado.SueldoBrutoHora;
                ToUpdate.SueldoBrutoMensual = Empleado.SueldoBrutoMensual;
                ToUpdate.Telefono = Empleado.Telefono;
                ToUpdate.FechaIngreso = Empleado.FechaIngreso;
                ToUpdate.FechaNacimiento = Empleado.FechaNacimiento;
                ToUpdate.FechaAntiguedadReconocida = Empleado.FechaAntiguedadReconocida;
                if ((ToUpdate.Localizacion == null && Empleado.Localizacion != null) || (ToUpdate.Localizacion != null && Empleado.Localizacion != null && ToUpdate.Localizacion.Id != Empleado.Localizacion.Id))
                {
                    ToUpdate.Localizacion = new Localizacion();
                    ToUpdate.Localizacion.Id = Empleado.Localizacion.Id;
                }
                else
                {
                    ToUpdate.Localizacion = Empleado.Localizacion;
                }
                if ((ToUpdate.Categoria == null && Empleado.Categoria != null) || (ToUpdate.Categoria != null && Empleado.Categoria != null && ToUpdate.Categoria.Id != Empleado.Categoria.Id))
                {
                    ToUpdate.Categoria = new ComboItem();
                    ToUpdate.Categoria.Id = Empleado.Categoria.Id;
                }
                else
                {
                    ToUpdate.Categoria = Empleado.Categoria;
                }
                if ((ToUpdate.Sexo == null && Empleado.Sexo != null) || (ToUpdate.Sexo != null && Empleado.Sexo != null && ToUpdate.Sexo.Id != Empleado.Sexo.Id))
                {
                    ToUpdate.Sexo = new ComboItem();
                    ToUpdate.Sexo.Id = Empleado.Sexo.Id;
                }
                else {
                    ToUpdate.Sexo = Empleado.Sexo;
                }
                if ((ToUpdate.EstadoCivil == null && Empleado.EstadoCivil != null) || (ToUpdate.EstadoCivil != null && Empleado.EstadoCivil != null && ToUpdate.EstadoCivil.Id != Empleado.EstadoCivil.Id))
                {
                    ToUpdate.EstadoCivil = new ComboItem();
                    ToUpdate.EstadoCivil.Id = Empleado.EstadoCivil.Id;
                }
                else
                {
                    ToUpdate.EstadoCivil = Empleado.EstadoCivil;
                }
                if ((ToUpdate.Sindicato == null && Empleado.Sindicato != null) || (ToUpdate.Sindicato != null && Empleado.Sindicato != null && ToUpdate.Sindicato.Id != Empleado.Sindicato.Id))
                {
                    ToUpdate.Sindicato = new ComboItem();
                    ToUpdate.Sindicato.Id = Empleado.Sindicato.Id;
                }
                else
                {
                    ToUpdate.Sindicato = Empleado.Sindicato;
                }
                if ((ToUpdate.ObraSocial == null && Empleado.ObraSocial != null) || (ToUpdate.ObraSocial != null && Empleado.ObraSocial != null && ToUpdate.ObraSocial.Id != Empleado.ObraSocial.Id))
                {
                    ToUpdate.ObraSocial = new ComboItem();
                    ToUpdate.ObraSocial.Id = Empleado.ObraSocial.Id;
                }
                else
                {
                    ToUpdate.ObraSocial = Empleado.ObraSocial;
                }
                if ((ToUpdate.BancoDeposito == null && Empleado.BancoDeposito != null) || (ToUpdate.BancoDeposito != null && Empleado.BancoDeposito != null && ToUpdate.BancoDeposito.Id != Empleado.BancoDeposito.Id))
                {
                    ToUpdate.BancoDeposito = new ComboItem();
                    ToUpdate.BancoDeposito.Id = Empleado.BancoDeposito.Id;
                }
                else
                {
                    ToUpdate.BancoDeposito = Empleado.BancoDeposito;
                }
                if ((ToUpdate.Tarea == null && Empleado.Tarea != null) || (ToUpdate.Tarea != null && Empleado.Tarea != null && ToUpdate.Tarea.Id != Empleado.Tarea.Id))
                {
                    ToUpdate.Tarea = new ComboItem();
                    ToUpdate.Tarea.Id = Empleado.Tarea.Id;
                }
                else
                {
                    ToUpdate.Tarea = Empleado.Tarea;
                }
                if ((ToUpdate.Nacionalidad == null && Empleado.Nacionalidad != null) || ToUpdate.Nacionalidad != null && Empleado.Nacionalidad != null && (ToUpdate.Nacionalidad.Id != Empleado.Nacionalidad.Id))
                {
                    ToUpdate.Nacionalidad = new ComboItem();
                    ToUpdate.Nacionalidad.Id = Empleado.Nacionalidad.Id;
                }
                else
                {
                    ToUpdate.Nacionalidad = Empleado.Nacionalidad;
                }
                this.EmpleadoRepository.Update(ToUpdate);
            });
        }
        public void DeleteEmpleados(List<int> Ids)
        {
            this.EmpleadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Empleado Empleado = this.EmpleadoRepository.Get(Id);
                    Empleado.Activo = false;
                    this.EmpleadoRepository.Update(Empleado);
                }
            });
        }

        public SelectCombo GetEmpleadoCombos()
        {
            SelectCombo SelectEmpleadoCombos = new SelectCombo();
            this.EmpleadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectEmpleadoCombos.Items = this.EmpleadoRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + " " + x.Apellido
                                                              }).ToList();
            });
            return SelectEmpleadoCombos;
        }

        public SelectCombo GetAllEmpleadosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectEmpleadoCombos = new SelectCombo();
            this.EmpleadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectEmpleadoCombos.Items = this.EmpleadoRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + " " + x.Apellido + " " + "(" + x.CUIT + ")"
                                                              }).ToList();
            });
            return SelectEmpleadoCombos;
        }

        [Loggable]
        public void ImportEmpleados(List<Empleado> Empleados)
        {
            this.EmpleadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Empleado in Empleados)
                {
                    this.ImportEmpleado(Empleado);
                }
            });
        }

        [Loggable]
        public void ImportEmpleado(Empleado Empleado)
        {
            this.EmpleadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ImportEmpleadoNT(Empleado);
            });
        }

        [Loggable]
        public void ImportEmpleadoNT(Empleado Empleado)
        {
            if (!ComprobanteHelper.IsCUITValid(Empleado.CUIT))
                throw new BusinessException("El CUIT es invalido.");

            if (this.ExistCUITNT(Empleado.CUIT, null))
                throw new BusinessException("El CUIT coincide con uno ya creado");

            if (Empleado.Categoria != null)
            {
                Empleado.Categoria = this.ComboItemRepository.GetByComboAndName(ComboType.CategoriasEmpleados, Empleado.Categoria.Data);
            }
            if (Empleado.EstadoCivil != null)
            {
                Empleado.EstadoCivil = this.ComboItemRepository.GetByComboAndName(ComboType.EstadoCivil, Empleado.EstadoCivil.Data);
            }
            if (Empleado.Sindicato != null)
            {
                Empleado.Sindicato = this.ComboItemRepository.GetByComboAndName(ComboType.Sindicato, Empleado.Sindicato.Data);
            }
            if (Empleado.ObraSocial != null)
            {
                Empleado.ObraSocial = this.ComboItemRepository.GetByComboAndName(ComboType.ObraSocial, Empleado.ObraSocial.Data);
            }
            if (Empleado.BancoDeposito != null)
            {
                Empleado.BancoDeposito = this.ComboItemRepository.GetByComboAndName(ComboType.BancoDeposito, Empleado.BancoDeposito.Data);
            }
            if (Empleado.Sexo != null)
            {
                Empleado.Sexo = this.ComboItemRepository.GetByComboAndName(ComboType.Sexo, Empleado.Sexo.Data);            
            }
            if (Empleado.Tarea != null)
            {
                Empleado.Tarea = this.ComboItemRepository.GetByComboAndName(ComboType.TareasEmpleados, Empleado.Tarea.Data);            
            }
            if (Empleado.Nacionalidad != null)
            {
                Empleado.Nacionalidad = this.ComboItemRepository.GetByComboAndName(ComboType.Paises, Empleado.Nacionalidad.Data);
            }
            if (Empleado.Localizacion != null)
            {
                Empleado.Localizacion = this.LocalizacionRepository.GetByNombre(Empleado.Localizacion.Nombre);
            } 

            this.EmpleadoRepository.Add(Empleado);
        }

        public bool ExistCUITNT(string cuit, int? IdUpdate)
        {
            return this.EmpleadoRepository.ExistCUIT(cuit, IdUpdate);
        }

        #endregion

        #region Adicional
        public Adicional GetAdicionalNT(int Id)
        {
            Adicional Adicional = null;
            Adicional = this.AdicionalRepository.Get(Id);
            return Adicional;
        }

        public Adicional GetAdicional(int Id)
        {
            Adicional Adicional = null;
            this.AdicionalRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Adicional = this.AdicionalRepository.Get(Id);
            });
            return Adicional;
        }

        public IList<Adicional> GetAllAdicionales()
        {
            IList<Adicional> Adicionales = null;
            this.AdicionalRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Adicionales = this.AdicionalRepository.GetAll();
            });
            return Adicionales;
        }

        [Loggable]
        public void AddAdicional(Adicional Adicional)
        {
            this.AdicionalRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddAdicionalNT(Adicional);
            });
        }
        public void AddAdicionalNT(Adicional Adicional)
        {
            this.AdicionalRepository.Add(Adicional);
        }

        [Loggable]
        public void UpdateAdicional(Adicional Adicional, IList<AdicionalAdicionales> AdicionalesAdicional = null)
        {
            this.AdicionalRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Adicional ToUpdate = this.AdicionalRepository.Get(Adicional.Id);
                IList<AdicionalAdicionales> AdicionalesToUpdate = this.AdicionalAdicionalesRepository.GetByAdicional(Adicional.Id, true);

                if (AdicionalesToUpdate.Count == 0 && AdicionalesAdicional.Count > 0)
                {
                    foreach (AdicionalAdicionales Adic in AdicionalesAdicional)
                    {
                        AdicionalAdicionales RAAS = new AdicionalAdicionales();
                        RAAS.EsDefault = true;
                        RAAS.Adicional = new Adicional();
                        RAAS.Adicional.Id = Adicional.Id;

                        RAAS.AdicionalSobre = new Adicional();
                        RAAS.AdicionalSobre.Id = Adic.Id;

                        this.AddAdicionalAdicionalesNT(RAAS);
                    }
                } else if(AdicionalesToUpdate.Count > 0 && AdicionalesAdicional.Count == 0)
                {
                    foreach(AdicionalAdicionales Adic in AdicionalesToUpdate)
                    {
                        this.AdicionalAdicionalesRepository.Delete(Adic);                    
                    }
                }
                else if (AdicionalesToUpdate.Count > 0 && AdicionalesAdicional.Count > 0)
                {
                    foreach(AdicionalAdicionales Adic in AdicionalesToUpdate)
                    {
                        //int AdicionalAgregar = AdicionalesAdicional.IndexOf(Adic);
                        //si estaba pero ahora no esta mas, eliminar
                        AdicionalAdicionales AdicionalAgregar = new AdicionalAdicionales();
                        //es cn el id porque AdicionalSobre no lo seteamos en la vista
                        AdicionalAgregar = AdicionalesAdicional.Where(x => x.Id == Adic.AdicionalSobre.Id).SingleOrDefault();
                        if (AdicionalAgregar == null)
                        {
                            this.AdicionalAdicionalesRepository.Delete(Adic);                    
                        }
                    }
                    foreach (AdicionalAdicionales Adic in AdicionalesAdicional)
                    {
                        AdicionalAdicionales AdicionalAgregar = new AdicionalAdicionales();
                        AdicionalAgregar = AdicionalesToUpdate.Where(x => x.AdicionalSobre.Id == Adic.Id).SingleOrDefault();
                        //si esta pero antes no estaba, agreagr
                        //int AdicionalAgregar = AdicionalesToUpdate.IndexOf(Adic);
                        //if (AdicionalAgregar < 0)
                        if (AdicionalAgregar == null )
                        {
                            AdicionalAdicionales RAAS = new AdicionalAdicionales();
                            RAAS.EsDefault = true;
                            RAAS.Adicional = new Adicional();
                            RAAS.Adicional.Id = Adicional.Id;

                            RAAS.AdicionalSobre = new Adicional();
                            RAAS.AdicionalSobre.Id = Adic.Id;

                            this.AddAdicionalAdicionalesNT(RAAS);
                        }
                    }
                }

                ToUpdate.Descripcion = Adicional.Descripcion;
                ToUpdate.AdditionalDescription = Adicional.AdditionalDescription;
                ToUpdate.Porcentaje = Adicional.Porcentaje;
                ToUpdate.Valor = Adicional.Valor;
                ToUpdate.Suma = Adicional.Suma;
                ToUpdate.TipoLiquidacion = Adicional.TipoLiquidacion;
                this.AdicionalRepository.Update(ToUpdate);
            });
        }
        public void DeleteAdicionales(List<int> Ids)
        {
            this.AdicionalRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Adicional Adicional = this.AdicionalRepository.Get(Id);
                    IList<AdicionalAdicionales> AdicAdicionales = this.AdicionalAdicionalesRepository.GetByAdicional(Id, false);
                    foreach(AdicionalAdicionales AA in AdicAdicionales)
                    {
                        this.AdicionalAdicionalesRepository.Delete(AA);
                    }
                    IList<AdicionalAdicionales> AdicSobreAdicionales = this.AdicionalAdicionalesRepository.GetSobreByAdicional(Id);
                    foreach (AdicionalAdicionales AA in AdicSobreAdicionales)
                    {
                        this.AdicionalAdicionalesRepository.Delete(AA);
                    }

                    this.AdicionalRepository.Delete(Adicional);
                }
            });
        }

        public SelectCombo GetAdicionalCombos()
        {
            SelectCombo SelectAdicionalCombos = new SelectCombo();
            this.AdicionalRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectAdicionalCombos.Items = this.AdicionalRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion + x.AdditionalDescription
                                                              }).ToList();
            });
            return SelectAdicionalCombos;
        }

        public SelectCombo GetAllAdicionalesByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectAdicionalCombos = new SelectCombo();
            this.AdicionalRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectAdicionalCombos.Items = this.AdicionalRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion + x.AdditionalDescription
                                                              }).ToList();
            });
            return SelectAdicionalCombos;
        }

        public void AddAdicionalConAdicionales(Adicional Adicional, IList<Adicional> Adicionales)
        {
            this.AdicionalRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddAdicionalNT(Adicional);
                foreach (Adicional A in Adicionales)
                {
                    AdicionalAdicionales RAAS = new AdicionalAdicionales();
                    RAAS.EsDefault = true;
                    RAAS.Adicional = new Adicional();
                    RAAS.Adicional.Id = Adicional.Id;

                    RAAS.AdicionalSobre = new Adicional();
                    RAAS.AdicionalSobre.Id = A.Id;

                    this.AddAdicionalAdicionalesNT(RAAS);
                }
            });
        }


        #endregion

        #region AdicionalRecibo
        public AdicionalRecibo GetAdicionalReciboNT(int Id)
        {
            AdicionalRecibo AdicionalRecibo = null;
            AdicionalRecibo = this.AdicionalReciboRepository.Get(Id);
            return AdicionalRecibo;
        }

        public AdicionalRecibo GetAdicionalRecibo(int Id)
        {
            AdicionalRecibo AdicionalRecibo = null;
            this.AdicionalReciboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                AdicionalRecibo = this.AdicionalReciboRepository.Get(Id);
            });
            return AdicionalRecibo;
        }

        //public IList<AdicionalRecibo> GetAllAdicionalReciboes()
        //{
        //    IList<AdicionalRecibo> AdicionalReciboes = null;
        //    this.AdicionalReciboRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        AdicionalReciboes = this.AdicionalReciboRepository.GetAll();
        //    });
        //    return AdicionalReciboes;
        //}

        [Loggable]
        public void AddAdicionalRecibo(AdicionalRecibo AdicionalRecibo)
        {
            this.AdicionalReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddAdicionalReciboNT(AdicionalRecibo);
            });
        }
        public void AddAdicionalReciboNT(AdicionalRecibo AdicionalRecibo)
        {
            this.AdicionalReciboRepository.Add(AdicionalRecibo);
        }

        //[Loggable]
        //public void UpdateAdicionalRecibo(AdicionalRecibo AdicionalRecibo, IList<AdicionalAdicionales> AdicionalReciboesAdicionalRecibo = null)
        //{
        //    this.AdicionalReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        AdicionalRecibo ToUpdate = this.AdicionalReciboRepository.Get(AdicionalRecibo.Id);
        //        IList<AdicionalAdicionales> AdicionalReciboesToUpdate = this.AdicionalAdicionalesRepository.GetByAdicional(AdicionalRecibo.Id, true);

        //        if (AdicionalReciboesToUpdate.Count == 0 && AdicionalReciboesAdicionalRecibo.Count > 0)
        //        {
        //            foreach (AdicionalAdicionales Adic in AdicionalReciboesAdicionalRecibo)
        //            {
        //                AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //                RAAS.EsDefault = true;
        //                RAAS.Adicional = new Adicional();
        //                RAAS.Adicional.Id = AdicionalRecibo.Id;

        //                RAAS.AdicionalSobre = new Adicional();
        //                RAAS.AdicionalSobre.Id = Adic.Id;

        //                this.AddAdicionalAdicionalesNT(RAAS);
        //            }
        //        }
        //        else if (AdicionalReciboesToUpdate.Count > 0 && AdicionalReciboesAdicionalRecibo.Count == 0)
        //        {
        //            foreach (AdicionalAdicionales Adic in AdicionalReciboesToUpdate)
        //            {
        //                this.AdicionalAdicionalesRepository.Delete(Adic);
        //            }
        //        }
        //        else if (AdicionalReciboesToUpdate.Count > 0 && AdicionalReciboesAdicionalRecibo.Count > 0)
        //        {
        //            foreach (AdicionalAdicionales Adic in AdicionalReciboesToUpdate)
        //            {
        //                //int AdicionalReciboAgregar = AdicionalReciboesAdicionalRecibo.IndexOf(Adic);
        //                //si estaba pero ahora no esta mas, eliminar
        //                AdicionalAdicionales AdicionalReciboAgregar = new AdicionalAdicionales();
        //                //es cn el id porque AdicionalSobre no lo seteamos en la vista
        //                AdicionalReciboAgregar = AdicionalReciboesAdicionalRecibo.Where(x => x.Id == Adic.AdicionalSobre.Id).SingleOrDefault();
        //                if (AdicionalReciboAgregar == null)
        //                {
        //                    this.AdicionalAdicionalesRepository.Delete(Adic);
        //                }
        //            }
        //            foreach (AdicionalAdicionales Adic in AdicionalReciboesAdicionalRecibo)
        //            {
        //                AdicionalAdicionales AdicionalReciboAgregar = new AdicionalAdicionales();
        //                AdicionalReciboAgregar = AdicionalReciboesToUpdate.Where(x => x.AdicionalSobre.Id == Adic.Id).SingleOrDefault();
        //                //si esta pero antes no estaba, agreagr
        //                //int AdicionalReciboAgregar = AdicionalReciboesToUpdate.IndexOf(Adic);
        //                //if (AdicionalReciboAgregar < 0)
        //                if (AdicionalReciboAgregar == null)
        //                {
        //                    AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //                    RAAS.EsDefault = true;
        //                    RAAS.Adicional = new Adicional();
        //                    RAAS.Adicional.Id = AdicionalRecibo.Id;

        //                    RAAS.AdicionalSobre = new Adicional();
        //                    RAAS.AdicionalSobre.Id = Adic.Id;

        //                    this.AddAdicionalAdicionalesNT(RAAS);
        //                }
        //            }
        //        }

        //        //ToUpdate.Descripcion = AdicionalRecibo.Descripcion;
        //        //ToUpdate.Porcentaje = AdicionalRecibo.Porcentaje;
        //        //ToUpdate.Valor = AdicionalRecibo.Valor;
        //        //ToUpdate.Suma = AdicionalRecibo.Suma;
        //        //ToUpdate.TipoLiquidacion = AdicionalRecibo.TipoLiquidacion;
        //        this.AdicionalReciboRepository.Update(ToUpdate);
        //    });
        //}
        public void DeleteAdicionalRecibos(List<int> Ids)
        {
            this.AdicionalReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    AdicionalRecibo AdicionalRecibo = this.AdicionalReciboRepository.Get(Id);
                    IList<AdicionalAdicionales> AdicAdicionalReciboes = this.AdicionalAdicionalesRepository.GetByAdicional(Id, false);
                    foreach (AdicionalAdicionales AA in AdicAdicionalReciboes)
                    {
                        this.AdicionalAdicionalesRepository.Delete(AA);
                    }
                    IList<AdicionalAdicionales> AdicSobreAdicionalReciboes = this.AdicionalAdicionalesRepository.GetSobreByAdicional(Id);
                    foreach (AdicionalAdicionales AA in AdicSobreAdicionalReciboes)
                    {
                        this.AdicionalAdicionalesRepository.Delete(AA);
                    }

                    this.AdicionalReciboRepository.Delete(AdicionalRecibo);
                }
            });
        }

        //public SelectCombo GetAdicionalReciboCombos()
        //{
        //    SelectCombo SelectAdicionalReciboCombos = new SelectCombo();
        //    this.AdicionalReciboRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectAdicionalReciboCombos.Items = this.AdicionalReciboRepository.GetAll()
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                      }).ToList();
        //    });
        //    return SelectAdicionalReciboCombos;
        //}

        //public SelectCombo GetAllAdicionalReciboesByFilterCombo(SelectComboRequest req)
        //{
        //    SelectCombo SelectAdicionalReciboCombos = new SelectCombo();
        //    this.AdicionalReciboRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectAdicionalReciboCombos.Items = this.AdicionalReciboRepository.GetAllByFilter(req)
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                      }).ToList();
        //    });
        //    return SelectAdicionalReciboCombos;
        //}

        //public void AddAdicionalReciboConAdicionalReciboes(AdicionalRecibo AdicionalRecibo, IList<AdicionalRecibo> AdicionalReciboes)
        //{
        //    this.AdicionalReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        this.AddAdicionalReciboNT(AdicionalRecibo);
        //        foreach (AdicionalRecibo A in AdicionalReciboes)
        //        {
        //            AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //            RAAS.EsDefault = true;
        //            RAAS.Adicional = new Adicional();
        //            RAAS.Adicional.Id = AdicionalRecibo.Id;

        //            RAAS.AdicionalSobre = new Adicional();
        //            RAAS.AdicionalSobre.Id = A.Id;

        //            this.AddAdicionalAdicionalesNT(RAAS);
        //        }
        //    });
        //}


        #endregion



        #region AdicionalAdicionales
        public AdicionalAdicionales GetAdicionalAdicionales(int Id)
        {
            AdicionalAdicionales AdicionalAdicionales = null;
            this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                AdicionalAdicionales = this.AdicionalAdicionalesRepository.Get(Id);
            });
            return AdicionalAdicionales;
        }

        public IList<AdicionalAdicionales> GetAdicionalAdicionalesByAdicional(int Id)
        {
            IList<AdicionalAdicionales> AdicionalAdicionales = null;
            this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                AdicionalAdicionales = this.AdicionalAdicionalesRepository.GetByAdicional(Id, true);
            });
            return AdicionalAdicionales;
        }

        public IList<AdicionalAdicionales> GetAllAdicionalAdicionaleses()
        {
            IList<AdicionalAdicionales> AdicionalAdicionaleses = null;
            this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                AdicionalAdicionaleses = this.AdicionalAdicionalesRepository.GetAll();
            });
            return AdicionalAdicionaleses;
        }

        [Loggable]
        public void AddAdicionalAdicionales(AdicionalAdicionales AdicionalAdicionales)
        {
            this.AdicionalAdicionalesRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddAdicionalAdicionalesNT(AdicionalAdicionales);
            });
        }
        public void AddAdicionalAdicionalesNT(AdicionalAdicionales AdicionalAdicionales)
        {
            this.AdicionalAdicionalesRepository.Add(AdicionalAdicionales);
        }

        //[Loggable]
        //public void UpdateAdicionalAdicionales(AdicionalAdicionales AdicionalAdicionales)
        //{
        //    this.AdicionalAdicionalesRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        AdicionalAdicionales ToUpdate = this.AdicionalAdicionalesRepository.Get(AdicionalAdicionales.Id);
        //        ToUpdate.Descripcion = AdicionalAdicionales.Descripcion;
        //        ToUpdate.Porcentaje = AdicionalAdicionales.Porcentaje;
        //        ToUpdate.Valor = AdicionalAdicionales.Valor;
        //        this.AdicionalAdicionalesRepository.Update(ToUpdate);
        //    });
        //}
        //public void DeleteAdicionalAdicionaleses(List<int> Ids)
        //{
        //    this.AdicionalAdicionalesRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        foreach (var Id in Ids)
        //        {
        //            AdicionalAdicionales AdicionalAdicionales = this.AdicionalAdicionalesRepository.Get(Id);
        //            AdicionalAdicionales.Activo = false;
        //            this.AdicionalAdicionalesRepository.Update(AdicionalAdicionales);
        //        }
        //    });
        //}

        //public SelectCombo GetAdicionalAdicionalesCombos()
        //{
        //    SelectCombo SelectAdicionalAdicionalesCombos = new SelectCombo();
        //    this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectAdicionalAdicionalesCombos.Items = this.AdicionalAdicionalesRepository.GetAll()
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                          text = x.Descripcion
        //                                                      }).ToList();
        //    });
        //    return SelectAdicionalAdicionalesCombos;
        //}

        //public SelectCombo GetAllAdicionalAdicionalesesByFilterCombo(SelectComboRequest req)
        //{
        //    SelectCombo SelectAdicionalAdicionalesCombos = new SelectCombo();
        //    this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectAdicionalAdicionalesCombos.Items = this.AdicionalAdicionalesRepository.GetAllByFilter(req)
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                          text = x.Descripcion
        //                                                      }).ToList();
        //    });
        //    return SelectAdicionalAdicionalesCombos;
        //}

        #endregion


    }
}
