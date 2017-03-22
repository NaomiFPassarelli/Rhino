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
    
    public class BolosConfigService : IBolosConfigService
    {

        #region VariablesyConstructor

        private readonly IEscalafonRepository EscalafonRepository;
        private readonly ITrabajadorRepository TrabajadorRepository;
        private readonly IComboItemRepository ComboItemRepository;
        private readonly ILocalizacionRepository LocalizacionRepository;
        private readonly IEmpresaRepository EmpresaRepository;
        private readonly IConceptoBoloRepository ConceptoBoloRepository;
        private readonly ITrabajadorBoloEscalafonRepository TrabajadorBoloEscalafonRepository;
        private readonly IBoloRepository BoloRepository; 

        public BolosConfigService(IEscalafonRepository EscalafonRepository, IComboItemRepository ComboItemRepository,
            ILocalizacionRepository LocalizacionRepository, ITrabajadorRepository TrabajadorRepository,
                    IEmpresaRepository EmpresaRepository,
                    IConceptoBoloRepository ConceptoBoloRepository, ITrabajadorBoloEscalafonRepository TrabajadorBoloEscalafonRepository,
                        IBoloRepository BoloRepository)
        {
            this.EscalafonRepository = EscalafonRepository;
            this.TrabajadorRepository = TrabajadorRepository;
            this.ComboItemRepository = ComboItemRepository;
            this.LocalizacionRepository = LocalizacionRepository;
            this.EmpresaRepository = EmpresaRepository;
            this.ConceptoBoloRepository = ConceptoBoloRepository;
            this.TrabajadorBoloEscalafonRepository = TrabajadorBoloEscalafonRepository;
            this.BoloRepository = BoloRepository;
        }

        #endregion

        #region Trabajador
        public Trabajador GetTrabajador(int Id)
        {
            Trabajador Trabajador = null;
            this.TrabajadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Trabajador = this.TrabajadorRepository.Get(Id);
            });
            return Trabajador;
        }

        public Trabajador GetTrabajadorCompleto(int Id)
        {
            Trabajador Trabajador = null;
            this.TrabajadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Trabajador = this.TrabajadorRepository.GetCompleto(Id);
            });
            return Trabajador;
        }

        public IList<Trabajador> GetAllTrabajadores()
        {
            IList<Trabajador> Trabajadores = null;
            this.TrabajadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Trabajadores = this.TrabajadorRepository.GetAll();
            });
            foreach (Trabajador trabajador in Trabajadores)
            {
                trabajador.TrabajadorBoloEscalafon = null;
            }
            return Trabajadores;
        }

        [Loggable]
        public void AddTrabajador(Trabajador Trabajador)
        {
            this.TrabajadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (!ComprobanteHelper.IsCUITValid(Trabajador.CUIT))
                    throw new BusinessException("El CUIT no es valido");

                if (this.ExistCUITNT(Trabajador.CUIT, null))
                    throw new BusinessException("El CUIT ya existe");

                this.AddTrabajadorNT(Trabajador); 
            });
        }
        public void AddTrabajadorNT(Trabajador Trabajador)
        {
            this.TrabajadorRepository.Add(Trabajador);
        }

        [Loggable]
        public void UpdateTrabajador(Trabajador Trabajador)
        {
            this.TrabajadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (!ComprobanteHelper.IsCUITValid(Trabajador.CUIT))
                    throw new BusinessException("El CUIT no es valido");

                if (this.ExistCUITNT(Trabajador.CUIT, Trabajador.Id))
                    throw new BusinessException("El CUIT ya existe");


                Trabajador ToUpdate = this.TrabajadorRepository.Get(Trabajador.Id);
                ToUpdate.Apellido = Trabajador.Apellido;
                ToUpdate.CodigoPostal = Trabajador.CodigoPostal;
                ToUpdate.CUIT = Trabajador.CUIT;
                ToUpdate.Departamento = Trabajador.Departamento;
                ToUpdate.Direccion = Trabajador.Direccion;
                ToUpdate.Email = Trabajador.Email;
                ToUpdate.Hijos = Trabajador.Hijos;
                ToUpdate.Nombre = Trabajador.Nombre;
                ToUpdate.Numero = Trabajador.Numero;
                ToUpdate.Piso = Trabajador.Piso;
                ToUpdate.SalarioEspecial = Trabajador.SalarioEspecial;
                ToUpdate.Telefono = Trabajador.Telefono;
                //ToUpdate.FechaIngreso = Trabajador.FechaIngreso;
                ToUpdate.FechaNacimiento = Trabajador.FechaNacimiento;
                //ToUpdate.FechaAntiguedadReconocida = Trabajador.FechaAntiguedadReconocida;
                //ToUpdate.BeneficiarioObraSocial = Trabajador.BeneficiarioObraSocial;
                //ToUpdate.VacacionesYaGozadas = Trabajador.VacacionesYaGozadas;
                //ToUpdate.VacacionesInicial = Trabajador.VacacionesInicial;
                //ToUpdate.SACInicial = Trabajador.SACInicial;

                if ((ToUpdate.Localizacion == null && Trabajador.Localizacion != null) || (ToUpdate.Localizacion != null && Trabajador.Localizacion != null && ToUpdate.Localizacion.Id != Trabajador.Localizacion.Id))
                {
                    ToUpdate.Localizacion = new Localizacion();
                    ToUpdate.Localizacion.Id = Trabajador.Localizacion.Id;
                }
                else
                {
                    ToUpdate.Localizacion = Trabajador.Localizacion;
                }
                //if ((ToUpdate.Categoria == null && Trabajador.Categoria != null) || (ToUpdate.Categoria != null && Trabajador.Categoria != null && ToUpdate.Categoria.Id != Trabajador.Categoria.Id))
                //{
                //    ToUpdate.Categoria = new ComboItem();
                //    ToUpdate.Categoria.Id = Trabajador.Categoria.Id;
                //}
                //else
                //{
                //    ToUpdate.Categoria = Trabajador.Categoria;
                //}
                //if ((ToUpdate.Sexo == null && Trabajador.Sexo != null) || (ToUpdate.Sexo != null && Trabajador.Sexo != null && ToUpdate.Sexo.Id != Trabajador.Sexo.Id))
                //{
                //    ToUpdate.Sexo = new ComboItem();
                //    ToUpdate.Sexo.Id = Trabajador.Sexo.Id;
                //}
                //else {
                //    ToUpdate.Sexo = Trabajador.Sexo;
                //}
                if ((ToUpdate.EstadoCivil == null && Trabajador.EstadoCivil != null) || (ToUpdate.EstadoCivil != null && Trabajador.EstadoCivil != null && ToUpdate.EstadoCivil.Id != Trabajador.EstadoCivil.Id))
                {
                    ToUpdate.EstadoCivil = new ComboItem();
                    ToUpdate.EstadoCivil.Id = Trabajador.EstadoCivil.Id;
                }
                else
                {
                    ToUpdate.EstadoCivil = Trabajador.EstadoCivil;
                }
                if ((ToUpdate.Sindicato == null && Trabajador.Sindicato != null) || (ToUpdate.Sindicato != null && Trabajador.Sindicato != null && ToUpdate.Sindicato.Id != Trabajador.Sindicato.Id))
                {
                    ToUpdate.Sindicato = new ComboItem();
                    ToUpdate.Sindicato.Id = Trabajador.Sindicato.Id;
                }
                else
                {
                    ToUpdate.Sindicato = Trabajador.Sindicato;
                }
                if ((ToUpdate.Escalafon == null && Trabajador.Escalafon != null) || (ToUpdate.Escalafon != null && Trabajador.Escalafon != null && ToUpdate.Escalafon.Id != Trabajador.Escalafon.Id))
                {
                    ToUpdate.Escalafon = new Escalafon();
                    ToUpdate.Escalafon.Id = Trabajador.Escalafon.Id;
                }
                else
                {
                    ToUpdate.Escalafon = Trabajador.Escalafon;
                }
                //if ((ToUpdate.ObraSocial == null && Trabajador.ObraSocial != null) || (ToUpdate.ObraSocial != null && Trabajador.ObraSocial != null && ToUpdate.ObraSocial.Id != Trabajador.ObraSocial.Id))
                //{
                //    ToUpdate.ObraSocial = new ComboItem();
                //    ToUpdate.ObraSocial.Id = Trabajador.ObraSocial.Id;
                //}
                //else
                //{
                //    ToUpdate.ObraSocial = Trabajador.ObraSocial;
                //}
                //if ((ToUpdate.BancoDeposito == null && Trabajador.BancoDeposito != null) || (ToUpdate.BancoDeposito != null && Trabajador.BancoDeposito != null && ToUpdate.BancoDeposito.Id != Trabajador.BancoDeposito.Id))
                //{
                //    ToUpdate.BancoDeposito = new ComboItem();
                //    ToUpdate.BancoDeposito.Id = Trabajador.BancoDeposito.Id;
                //}
                //else
                //{
                //    ToUpdate.BancoDeposito = Trabajador.BancoDeposito;
                //}
                //if ((ToUpdate.Tarea == null && Trabajador.Tarea != null) || (ToUpdate.Tarea != null && Trabajador.Tarea != null && ToUpdate.Tarea.Id != Trabajador.Tarea.Id))
                //{
                //    ToUpdate.Tarea = new ComboItem();
                //    ToUpdate.Tarea.Id = Trabajador.Tarea.Id;
                //}
                //else
                //{
                //    ToUpdate.Tarea = Trabajador.Tarea;
                //}
                //if ((ToUpdate.Nacionalidad == null && Trabajador.Nacionalidad != null) || ToUpdate.Nacionalidad != null && Trabajador.Nacionalidad != null && (ToUpdate.Nacionalidad.Id != Trabajador.Nacionalidad.Id))
                //{
                //    ToUpdate.Nacionalidad = new ComboItem();
                //    ToUpdate.Nacionalidad.Id = Trabajador.Nacionalidad.Id;
                //}
                //else
                //{
                //    ToUpdate.Nacionalidad = Trabajador.Nacionalidad;
                //}
                this.TrabajadorRepository.Update(ToUpdate);
            });
        }
        public void DeleteTrabajadores(List<int> Ids)
        {
            this.TrabajadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Trabajador Trabajador = this.TrabajadorRepository.Get(Id);
                    Trabajador.Activo = false;
                    this.TrabajadorRepository.Update(Trabajador);
                }
            });
        }

        public SelectCombo GetTrabajadorCombos()
        {
            SelectCombo SelectTrabajadorCombos = new SelectCombo();
            this.TrabajadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectTrabajadorCombos.Items = this.TrabajadorRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + " " + x.Apellido
                                                              }).ToList();
            });
            return SelectTrabajadorCombos;
        }

        public SelectCombo GetAllTrabajadoresByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectTrabajadorCombos = new SelectCombo();
            this.TrabajadorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectTrabajadorCombos.Items = this.TrabajadorRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + " " + x.Apellido + " " + "(" + x.CUIT + ")"
                                                              }).ToList();
            });
            return SelectTrabajadorCombos;
        }

        [Loggable]
        public void ImportTrabajadores(List<Trabajador> Trabajadores)
        {
            this.TrabajadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Trabajador in Trabajadores)
                {
                    this.ImportTrabajador(Trabajador);
                }
            });
        }

        [Loggable]
        public void ImportTrabajador(Trabajador Trabajador)
        {
            this.TrabajadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ImportTrabajadorNT(Trabajador);
            });
        }

        [Loggable]
        public void ImportTrabajadorNT(Trabajador Trabajador)
        {
            if (!ComprobanteHelper.IsCUITValid(Trabajador.CUIT))
                throw new BusinessException("El CUIT es invalido.");

            if (this.ExistCUITNT(Trabajador.CUIT, null))
                throw new BusinessException("El CUIT coincide con uno ya creado");

            //if (Trabajador.Categoria != null)
            //{
            //    Trabajador.Categoria = this.ComboItemRepository.GetByComboAndName(ComboType.CategoriasTrabajadores, Trabajador.Categoria.Data);
            //}
            if (Trabajador.EstadoCivil != null)
            {
                Trabajador.EstadoCivil = this.ComboItemRepository.GetByComboAndName(ComboType.EstadoCivil, Trabajador.EstadoCivil.Data);
            }
            if (Trabajador.Sindicato != null)
            {
                Trabajador.Sindicato = this.ComboItemRepository.GetByComboAndName(ComboType.Sindicato, Trabajador.Sindicato.Data);
            }
            //if (Trabajador.ObraSocial != null)
            //{
            //    Trabajador.ObraSocial = this.ComboItemRepository.GetByComboAndName(ComboType.ObraSocial, Trabajador.ObraSocial.Data);
            //}
            //if (Trabajador.BancoDeposito != null)
            //{
            //    Trabajador.BancoDeposito = this.ComboItemRepository.GetByComboAndName(ComboType.BancoDeposito, Trabajador.BancoDeposito.Data);
            //}
            //if (Trabajador.Sexo != null)
            //{
            //    Trabajador.Sexo = this.ComboItemRepository.GetByComboAndName(ComboType.Sexo, Trabajador.Sexo.Data);            
            //}
            //if (Trabajador.Tarea != null)
            //{
            //    Trabajador.Tarea = this.ComboItemRepository.GetByComboAndName(ComboType.TareasTrabajadores, Trabajador.Tarea.Data);            
            //}
            //if (Trabajador.Nacionalidad != null)
            //{
            //    Trabajador.Nacionalidad = this.ComboItemRepository.GetByComboAndName(ComboType.Paises, Trabajador.Nacionalidad.Data);
            //}
            if (Trabajador.Localizacion != null)
            {
                Trabajador.Localizacion = this.LocalizacionRepository.GetByNombre(Trabajador.Localizacion.Nombre);
            } 

            this.TrabajadorRepository.Add(Trabajador);
        }

        public bool ExistCUITNT(string cuit, int? IdUpdate)
        {
            return this.TrabajadorRepository.ExistCUIT(cuit, IdUpdate);
        }

        public int GetProximoNumeroReferencia()
        {
            int ProximoNumeroReferencia = 1;
            this.TrabajadorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ProximoNumeroReferencia = this.TrabajadorRepository.GetProximoNumeroReferencia();
            });
            return ProximoNumeroReferencia;
        }

        #endregion


        #region Escalafon
        public Escalafon GetEscalafon(int Id)
        {
            Escalafon Escalafon = null;
            this.EscalafonRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Escalafon = this.EscalafonRepository.Get(Id);
            });
            return Escalafon;
        }

        //public Escalafon GetEscalafonCompleto(int Id)
        //{
        //    Escalafon Escalafon = null;
        //    this.EscalafonRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        Escalafon = this.EscalafonRepository.GetCompleto(Id);
        //    });
        //    return Escalafon;
        //}

        public IList<Escalafon> GetAllEscalafones()
        {
            IList<Escalafon> Escalafones = null;
            this.EscalafonRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Escalafones = this.EscalafonRepository.GetAll();
            });
            return Escalafones;
        }

        [Loggable]
        public void AddEscalafon(Escalafon Escalafon)
        {
            this.EscalafonRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.EscalafonRepository.Add(Escalafon);
            });
        }
       
        [Loggable]
        public void UpdateEscalafon(Escalafon Escalafon)
        {
            this.EscalafonRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Escalafon ToUpdate = this.EscalafonRepository.Get(Escalafon.Id);
                ToUpdate.Salario = Escalafon.Salario;
                ToUpdate.Descripcion = Escalafon.Descripcion;
                ToUpdate.VigenciaDesde = Escalafon.VigenciaDesde;
                ToUpdate.VigenciaHasta = Escalafon.VigenciaHasta;
                //ToUpdate.MarcaVigencia = Escalafon.MarcaVigencia;
                //ToUpdate.Resolucion = Escalafon.Resolucion;
                //if ((ToUpdate.Localizacion == null && Escalafon.Localizacion != null) || (ToUpdate.Localizacion != null && Escalafon.Localizacion != null && ToUpdate.Localizacion.Id != Escalafon.Localizacion.Id))
                //{
                //    ToUpdate.Localizacion = new Localizacion();
                //    ToUpdate.Localizacion.Id = Escalafon.Localizacion.Id;
                //}
                //else
                //{
                //    ToUpdate.Localizacion = Escalafon.Localizacion;
                //}
                //if ((ToUpdate.Categoria == null && Escalafon.Categoria != null) || (ToUpdate.Categoria != null && Escalafon.Categoria != null && ToUpdate.Categoria.Id != Escalafon.Categoria.Id))
                //{
                //    ToUpdate.Categoria = new ComboItem();
                //    ToUpdate.Categoria.Id = Escalafon.Categoria.Id;
                //}
                //else
                //{
                //    ToUpdate.Categoria = Escalafon.Categoria;
                //}
                //if ((ToUpdate.Tarea == null && Escalafon.Tarea != null) || (ToUpdate.Tarea != null && Escalafon.Tarea != null && ToUpdate.Tarea.Id != Escalafon.Tarea.Id))
                //{
                //    ToUpdate.Tarea = new ComboItem();
                //    ToUpdate.Tarea.Id = Escalafon.Tarea.Id;
                //}
                //else
                //{
                //    ToUpdate.Tarea = Escalafon.Tarea;
                //}
                this.EscalafonRepository.Update(ToUpdate);
            });
        }
        public void DeleteEscalafones(List<int> Ids)
        {
            this.EscalafonRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Escalafon Escalafon = this.EscalafonRepository.Get(Id);
                    Escalafon.Activo = false;
                    this.EscalafonRepository.Update(Escalafon);
                }
            });
        }

        public SelectCombo GetEscalafonCombos()
        {
            SelectCombo SelectEscalafonCombos = new SelectCombo();
            this.EscalafonRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectEscalafonCombos.Items = this.EscalafonRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion
                                                              }).ToList();
            });
            return SelectEscalafonCombos;
        }

        public SelectCombo GetAllEscalafonesByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectEscalafonCombos = new SelectCombo();
            this.EscalafonRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectEscalafonCombos.Items = this.EscalafonRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion
                                                              }).ToList();
            });
            return SelectEscalafonCombos;
        }

        //[Loggable]
        //public void ImportEscalafones(List<Escalafon> Escalafones)
        //{
        //    this.EscalafonRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        foreach (var Escalafon in Escalafones)
        //        {
        //            this.ImportEscalafon(Escalafon);
        //        }
        //    });
        //}

        //[Loggable]
        //public void ImportEscalafon(Escalafon Escalafon)
        //{
        //    this.EscalafonRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        this.ImportEscalafonNT(Escalafon);
        //    });
        //}

        //[Loggable]
        //public void ImportEscalafonNT(Escalafon Escalafon)
        //{
        //    if (!ComprobanteHelper.IsCUITValid(Escalafon.CUIT))
        //        throw new BusinessException("El CUIT es invalido.");

        //    if (this.ExistCUITNT(Escalafon.CUIT, null))
        //        throw new BusinessException("El CUIT coincide con uno ya creado");

        //    if (Escalafon.Categoria != null)
        //    {
        //        Escalafon.Categoria = this.ComboItemRepository.GetByComboAndName(ComboType.CategoriasEscalafones, Escalafon.Categoria.Data);
        //    }
        //    if (Escalafon.EstadoCivil != null)
        //    {
        //        Escalafon.EstadoCivil = this.ComboItemRepository.GetByComboAndName(ComboType.EstadoCivil, Escalafon.EstadoCivil.Data);
        //    }
        //    if (Escalafon.Sindicato != null)
        //    {
        //        Escalafon.Sindicato = this.ComboItemRepository.GetByComboAndName(ComboType.Sindicato, Escalafon.Sindicato.Data);
        //    }
        //    if (Escalafon.ObraSocial != null)
        //    {
        //        Escalafon.ObraSocial = this.ComboItemRepository.GetByComboAndName(ComboType.ObraSocial, Escalafon.ObraSocial.Data);
        //    }
        //    if (Escalafon.BancoDeposito != null)
        //    {
        //        Escalafon.BancoDeposito = this.ComboItemRepository.GetByComboAndName(ComboType.BancoDeposito, Escalafon.BancoDeposito.Data);
        //    }
        //    if (Escalafon.Sexo != null)
        //    {
        //        Escalafon.Sexo = this.ComboItemRepository.GetByComboAndName(ComboType.Sexo, Escalafon.Sexo.Data);
        //    }
        //    if (Escalafon.Tarea != null)
        //    {
        //        Escalafon.Tarea = this.ComboItemRepository.GetByComboAndName(ComboType.TareasEscalafones, Escalafon.Tarea.Data);
        //    }
        //    if (Escalafon.Nacionalidad != null)
        //    {
        //        Escalafon.Nacionalidad = this.ComboItemRepository.GetByComboAndName(ComboType.Paises, Escalafon.Nacionalidad.Data);
        //    }
        //    if (Escalafon.Localizacion != null)
        //    {
        //        Escalafon.Localizacion = this.LocalizacionRepository.GetByNombre(Escalafon.Localizacion.Nombre);
        //    }

        //    this.EscalafonRepository.Add(Escalafon);
        //}

        #endregion



        //#region Adicional
        //public Adicional GetAdicional(int Id, int IdSindicato, bool OnlyManual)
        //{
        //    Adicional Adicional = null;
        //    this.AdicionalRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        Adicional = this.AdicionalRepository.Get(Id, IdSindicato, OnlyManual);
        //    });
        //    return Adicional;
        //}

        //public IList<Adicional> GetAllAdicionales()
        //{
        //    IList<Adicional> Adicionales = null;
        //    this.AdicionalRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        Adicionales = this.AdicionalRepository.GetAll();
        //    });
        //    return Adicionales;
        //}

        //[Loggable]
        //public void AddAdicional(Adicional Adicional)
        //{
        //    this.AdicionalRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        this.AddAdicionalNT(Adicional);
        //    });
        //}
        //public void AddAdicionalNT(Adicional Adicional)
        //{
        //    this.AdicionalRepository.Add(Adicional);
        //}

        //[Loggable]
        //public void UpdateAdicional(Adicional Adicional, IList<AdicionalAdicionales> AdicionalesAdicional = null)
        //{
        //    this.AdicionalRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        Adicional ToUpdate = this.AdicionalRepository.Get(Adicional.Id);
        //        IList<AdicionalAdicionales> AdicionalesToUpdate = this.AdicionalAdicionalesRepository.GetByAdicional(Adicional.Id, true);

        //        if (AdicionalesToUpdate.Count == 0 && AdicionalesAdicional.Count > 0)
        //        {
        //            foreach (AdicionalAdicionales Adic in AdicionalesAdicional)
        //            {
        //                AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //                RAAS.EsDefault = true;
        //                RAAS.Adicional = new Adicional();
        //                RAAS.Adicional.Id = Adicional.Id;

        //                RAAS.AdicionalSobre = new Adicional();
        //                RAAS.AdicionalSobre.Id = Adic.Id;

        //                this.AddAdicionalAdicionalesNT(RAAS);
        //            }
        //        } else if(AdicionalesToUpdate.Count > 0 && AdicionalesAdicional.Count == 0)
        //        {
        //            foreach(AdicionalAdicionales Adic in AdicionalesToUpdate)
        //            {
        //                this.AdicionalAdicionalesRepository.Delete(Adic);                    
        //            }
        //        }
        //        else if (AdicionalesToUpdate.Count > 0 && AdicionalesAdicional.Count > 0)
        //        {
        //            foreach(AdicionalAdicionales Adic in AdicionalesToUpdate)
        //            {
        //                //int AdicionalAgregar = AdicionalesAdicional.IndexOf(Adic);
        //                //si estaba pero ahora no esta mas, eliminar
        //                AdicionalAdicionales AdicionalAgregar = new AdicionalAdicionales();
        //                //es cn el id porque AdicionalSobre no lo seteamos en la vista
        //                AdicionalAgregar = AdicionalesAdicional.Where(x => x.Id == Adic.AdicionalSobre.Id).SingleOrDefault();
        //                if (AdicionalAgregar == null)
        //                {
        //                    this.AdicionalAdicionalesRepository.Delete(Adic);                    
        //                }
        //            }
        //            foreach (AdicionalAdicionales Adic in AdicionalesAdicional)
        //            {
        //                AdicionalAdicionales AdicionalAgregar = new AdicionalAdicionales();
        //                AdicionalAgregar = AdicionalesToUpdate.Where(x => x.AdicionalSobre.Id == Adic.Id).SingleOrDefault();
        //                //si esta pero antes no estaba, agreagr
        //                //int AdicionalAgregar = AdicionalesToUpdate.IndexOf(Adic);
        //                //if (AdicionalAgregar < 0)
        //                if (AdicionalAgregar == null )
        //                {
        //                    AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //                    RAAS.EsDefault = true;
        //                    RAAS.Adicional = new Adicional();
        //                    RAAS.Adicional.Id = Adicional.Id;

        //                    RAAS.AdicionalSobre = new Adicional();
        //                    RAAS.AdicionalSobre.Id = Adic.Id;

        //                    this.AddAdicionalAdicionalesNT(RAAS);
        //                }
        //            }
        //        }

        //        ToUpdate.Descripcion = Adicional.Descripcion;
        //        ToUpdate.AdditionalDescription = Adicional.AdditionalDescription;
        //        ToUpdate.Porcentaje = Adicional.Porcentaje;
        //        ToUpdate.Valor = Adicional.Valor;
        //        ToUpdate.Suma = Adicional.Suma;
        //        ToUpdate.TipoLiquidacion = Adicional.TipoLiquidacion;
        //        this.AdicionalRepository.Update(ToUpdate);
        //    });
        //}
        //public void DeleteAdicionales(List<int> Ids)
        //{
        //    this.AdicionalRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        foreach (var Id in Ids)
        //        {
        //            Adicional Adicional = this.AdicionalRepository.Get(Id);
        //            IList<AdicionalAdicionales> AdicAdicionales = this.AdicionalAdicionalesRepository.GetByAdicional(Id, false);
        //            foreach(AdicionalAdicionales AA in AdicAdicionales)
        //            {
        //                this.AdicionalAdicionalesRepository.Delete(AA);
        //            }
        //            IList<AdicionalAdicionales> AdicSobreAdicionales = this.AdicionalAdicionalesRepository.GetSobreByAdicional(Id);
        //            foreach (AdicionalAdicionales AA in AdicSobreAdicionales)
        //            {
        //                this.AdicionalAdicionalesRepository.Delete(AA);
        //            }

        //            this.AdicionalRepository.Delete(Adicional);
        //        }
        //    });
        //}

        //public SelectCombo GetAdicionalCombos()
        //{
        //    SelectCombo SelectAdicionalCombos = new SelectCombo();
        //    this.AdicionalRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectAdicionalCombos.Items = this.AdicionalRepository.GetAll()
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                          text = x.Descripcion + x.AdditionalDescription
        //                                                      }).ToList();
        //    });
        //    return SelectAdicionalCombos;
        //}

        //public SelectCombo GetAllAdicionalesByFilterCombo(SelectComboRequest req, int IdSindicato, bool OnlyManual)
        //{
        //    SelectCombo SelectAdicionalCombos = new SelectCombo();
        //    this.AdicionalRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectAdicionalCombos.Items = this.AdicionalRepository.GetAllByFilter(req, IdSindicato, OnlyManual)
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                          text = x.Descripcion + x.AdditionalDescription
        //                                                      }).ToList();
        //    });
        //    return SelectAdicionalCombos;
        //}

        //public void AddAdicionalConAdicionales(Adicional Adicional, IList<Adicional> Adicionales)
        //{
        //    this.AdicionalRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        this.AddAdicionalNT(Adicional);
        //        foreach (Adicional A in Adicionales)
        //        {
        //            AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //            RAAS.EsDefault = true;
        //            RAAS.Adicional = new Adicional();
        //            RAAS.Adicional.Id = Adicional.Id;

        //            RAAS.AdicionalSobre = new Adicional();
        //            RAAS.AdicionalSobre.Id = A.Id;

        //            this.AddAdicionalAdicionalesNT(RAAS);
        //        }
        //    });
        //}


        //#endregion

        //#region AdicionalRecibo
        //public AdicionalRecibo GetAdicionalReciboNT(int Id)
        //{
        //    AdicionalRecibo AdicionalRecibo = null;
        //    AdicionalRecibo = this.AdicionalReciboRepository.Get(Id);
        //    return AdicionalRecibo;
        //}

        //public AdicionalRecibo GetAdicionalRecibo(int Id)
        //{
        //    AdicionalRecibo AdicionalRecibo = null;
        //    this.AdicionalReciboRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        AdicionalRecibo = this.AdicionalReciboRepository.Get(Id);
        //    });
        //    return AdicionalRecibo;
        //}

        ////public IList<AdicionalRecibo> GetAllAdicionalReciboes()
        ////{
        ////    IList<AdicionalRecibo> AdicionalReciboes = null;
        ////    this.AdicionalReciboRepository.GetSessionFactory().SessionInterceptor(() =>
        ////    {
        ////        AdicionalReciboes = this.AdicionalReciboRepository.GetAll();
        ////    });
        ////    return AdicionalReciboes;
        ////}

        //[Loggable]
        //public void AddAdicionalRecibo(AdicionalRecibo AdicionalRecibo)
        //{
        //    this.AdicionalReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        this.AddAdicionalReciboNT(AdicionalRecibo);
        //    });
        //}
        //public void AddAdicionalReciboNT(AdicionalRecibo AdicionalRecibo)
        //{
        //    this.AdicionalReciboRepository.Add(AdicionalRecibo);
        //}

        ////[Loggable]
        ////public void UpdateAdicionalRecibo(AdicionalRecibo AdicionalRecibo, IList<AdicionalAdicionales> AdicionalReciboesAdicionalRecibo = null)
        ////{
        ////    this.AdicionalReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
        ////    {
        ////        AdicionalRecibo ToUpdate = this.AdicionalReciboRepository.Get(AdicionalRecibo.Id);
        ////        IList<AdicionalAdicionales> AdicionalReciboesToUpdate = this.AdicionalAdicionalesRepository.GetByAdicional(AdicionalRecibo.Id, true);

        ////        if (AdicionalReciboesToUpdate.Count == 0 && AdicionalReciboesAdicionalRecibo.Count > 0)
        ////        {
        ////            foreach (AdicionalAdicionales Adic in AdicionalReciboesAdicionalRecibo)
        ////            {
        ////                AdicionalAdicionales RAAS = new AdicionalAdicionales();
        ////                RAAS.EsDefault = true;
        ////                RAAS.Adicional = new Adicional();
        ////                RAAS.Adicional.Id = AdicionalRecibo.Id;

        ////                RAAS.AdicionalSobre = new Adicional();
        ////                RAAS.AdicionalSobre.Id = Adic.Id;

        ////                this.AddAdicionalAdicionalesNT(RAAS);
        ////            }
        ////        }
        ////        else if (AdicionalReciboesToUpdate.Count > 0 && AdicionalReciboesAdicionalRecibo.Count == 0)
        ////        {
        ////            foreach (AdicionalAdicionales Adic in AdicionalReciboesToUpdate)
        ////            {
        ////                this.AdicionalAdicionalesRepository.Delete(Adic);
        ////            }
        ////        }
        ////        else if (AdicionalReciboesToUpdate.Count > 0 && AdicionalReciboesAdicionalRecibo.Count > 0)
        ////        {
        ////            foreach (AdicionalAdicionales Adic in AdicionalReciboesToUpdate)
        ////            {
        ////                //int AdicionalReciboAgregar = AdicionalReciboesAdicionalRecibo.IndexOf(Adic);
        ////                //si estaba pero ahora no esta mas, eliminar
        ////                AdicionalAdicionales AdicionalReciboAgregar = new AdicionalAdicionales();
        ////                //es cn el id porque AdicionalSobre no lo seteamos en la vista
        ////                AdicionalReciboAgregar = AdicionalReciboesAdicionalRecibo.Where(x => x.Id == Adic.AdicionalSobre.Id).SingleOrDefault();
        ////                if (AdicionalReciboAgregar == null)
        ////                {
        ////                    this.AdicionalAdicionalesRepository.Delete(Adic);
        ////                }
        ////            }
        ////            foreach (AdicionalAdicionales Adic in AdicionalReciboesAdicionalRecibo)
        ////            {
        ////                AdicionalAdicionales AdicionalReciboAgregar = new AdicionalAdicionales();
        ////                AdicionalReciboAgregar = AdicionalReciboesToUpdate.Where(x => x.AdicionalSobre.Id == Adic.Id).SingleOrDefault();
        ////                //si esta pero antes no estaba, agreagr
        ////                //int AdicionalReciboAgregar = AdicionalReciboesToUpdate.IndexOf(Adic);
        ////                //if (AdicionalReciboAgregar < 0)
        ////                if (AdicionalReciboAgregar == null)
        ////                {
        ////                    AdicionalAdicionales RAAS = new AdicionalAdicionales();
        ////                    RAAS.EsDefault = true;
        ////                    RAAS.Adicional = new Adicional();
        ////                    RAAS.Adicional.Id = AdicionalRecibo.Id;

        ////                    RAAS.AdicionalSobre = new Adicional();
        ////                    RAAS.AdicionalSobre.Id = Adic.Id;

        ////                    this.AddAdicionalAdicionalesNT(RAAS);
        ////                }
        ////            }
        ////        }

        ////        //ToUpdate.Descripcion = AdicionalRecibo.Descripcion;
        ////        //ToUpdate.Porcentaje = AdicionalRecibo.Porcentaje;
        ////        //ToUpdate.Valor = AdicionalRecibo.Valor;
        ////        //ToUpdate.Suma = AdicionalRecibo.Suma;
        ////        //ToUpdate.TipoLiquidacion = AdicionalRecibo.TipoLiquidacion;
        ////        this.AdicionalReciboRepository.Update(ToUpdate);
        ////    });
        ////}
        //public void DeleteAdicionalRecibos(List<int> Ids)
        //{
        //    this.AdicionalReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        foreach (var Id in Ids)
        //        {
        //            AdicionalRecibo AdicionalRecibo = this.AdicionalReciboRepository.Get(Id);
        //            IList<AdicionalAdicionales> AdicAdicionalReciboes = this.AdicionalAdicionalesRepository.GetByAdicional(Id, false);
        //            foreach (AdicionalAdicionales AA in AdicAdicionalReciboes)
        //            {
        //                this.AdicionalAdicionalesRepository.Delete(AA);
        //            }
        //            IList<AdicionalAdicionales> AdicSobreAdicionalReciboes = this.AdicionalAdicionalesRepository.GetSobreByAdicional(Id);
        //            foreach (AdicionalAdicionales AA in AdicSobreAdicionalReciboes)
        //            {
        //                this.AdicionalAdicionalesRepository.Delete(AA);
        //            }

        //            this.AdicionalReciboRepository.Delete(AdicionalRecibo);
        //        }
        //    });
        //}

        ////public SelectCombo GetAdicionalReciboCombos()
        ////{
        ////    SelectCombo SelectAdicionalReciboCombos = new SelectCombo();
        ////    this.AdicionalReciboRepository.GetSessionFactory().SessionInterceptor(() =>
        ////    {
        ////        SelectAdicionalReciboCombos.Items = this.AdicionalReciboRepository.GetAll()
        ////                                                      .Select(x => new SelectComboItem()
        ////                                                      {
        ////                                                          id = x.Id,
        ////                                                      }).ToList();
        ////    });
        ////    return SelectAdicionalReciboCombos;
        ////}

        ////public SelectCombo GetAllAdicionalReciboesByFilterCombo(SelectComboRequest req)
        ////{
        ////    SelectCombo SelectAdicionalReciboCombos = new SelectCombo();
        ////    this.AdicionalReciboRepository.GetSessionFactory().SessionInterceptor(() =>
        ////    {
        ////        SelectAdicionalReciboCombos.Items = this.AdicionalReciboRepository.GetAllByFilter(req)
        ////                                                      .Select(x => new SelectComboItem()
        ////                                                      {
        ////                                                          id = x.Id,
        ////                                                      }).ToList();
        ////    });
        ////    return SelectAdicionalReciboCombos;
        ////}

        ////public void AddAdicionalReciboConAdicionalReciboes(AdicionalRecibo AdicionalRecibo, IList<AdicionalRecibo> AdicionalReciboes)
        ////{
        ////    this.AdicionalReciboRepository.GetSessionFactory().TransactionalInterceptor(() =>
        ////    {
        ////        this.AddAdicionalReciboNT(AdicionalRecibo);
        ////        foreach (AdicionalRecibo A in AdicionalReciboes)
        ////        {
        ////            AdicionalAdicionales RAAS = new AdicionalAdicionales();
        ////            RAAS.EsDefault = true;
        ////            RAAS.Adicional = new Adicional();
        ////            RAAS.Adicional.Id = AdicionalRecibo.Id;

        ////            RAAS.AdicionalSobre = new Adicional();
        ////            RAAS.AdicionalSobre.Id = A.Id;

        ////            this.AddAdicionalAdicionalesNT(RAAS);
        ////        }
        ////    });
        ////}
        //public IList<AdicionalRecibo> GetAdicionalesDelPeriodoByEscalafon(string Periodo, int IdEscalafon)
        //{
        //    IList<AdicionalRecibo> ARs = null;
        //    this.AdicionalReciboRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        ARs = this.AdicionalReciboRepository.GetAdicionalesDelPeriodoByEscalafon(Periodo, IdEscalafon);
        //    });
        //    return ARs;
        //}

        //#endregion



        //#region AdicionalAdicionales
        //public AdicionalAdicionales GetAdicionalAdicionales(int Id)
        //{
        //    AdicionalAdicionales AdicionalAdicionales = null;
        //    this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        AdicionalAdicionales = this.AdicionalAdicionalesRepository.Get(Id);
        //    });
        //    return AdicionalAdicionales;
        //}

        //public IList<AdicionalAdicionales> GetAdicionalAdicionalesByAdicional(int Id)
        //{
        //    IList<AdicionalAdicionales> AdicionalAdicionales = null;
        //    this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        AdicionalAdicionales = this.AdicionalAdicionalesRepository.GetByAdicional(Id, true);
        //    });
        //    return AdicionalAdicionales;
        //}

        //public IList<AdicionalAdicionales> GetAllAdicionalAdicionaleses()
        //{
        //    IList<AdicionalAdicionales> AdicionalAdicionaleses = null;
        //    this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        AdicionalAdicionaleses = this.AdicionalAdicionalesRepository.GetAll();
        //    });
        //    return AdicionalAdicionaleses;
        //}

        //[Loggable]
        //public void AddAdicionalAdicionales(AdicionalAdicionales AdicionalAdicionales)
        //{
        //    this.AdicionalAdicionalesRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        this.AddAdicionalAdicionalesNT(AdicionalAdicionales);
        //    });
        //}
        //public void AddAdicionalAdicionalesNT(AdicionalAdicionales AdicionalAdicionales)
        //{
        //    this.AdicionalAdicionalesRepository.Add(AdicionalAdicionales);
        //}

        ////[Loggable]
        ////public void UpdateAdicionalAdicionales(AdicionalAdicionales AdicionalAdicionales)
        ////{
        ////    this.AdicionalAdicionalesRepository.GetSessionFactory().TransactionalInterceptor(() =>
        ////    {
        ////        AdicionalAdicionales ToUpdate = this.AdicionalAdicionalesRepository.Get(AdicionalAdicionales.Id);
        ////        ToUpdate.Descripcion = AdicionalAdicionales.Descripcion;
        ////        ToUpdate.Porcentaje = AdicionalAdicionales.Porcentaje;
        ////        ToUpdate.Valor = AdicionalAdicionales.Valor;
        ////        this.AdicionalAdicionalesRepository.Update(ToUpdate);
        ////    });
        ////}
        ////public void DeleteAdicionalAdicionaleses(List<int> Ids)
        ////{
        ////    this.AdicionalAdicionalesRepository.GetSessionFactory().TransactionalInterceptor(() =>
        ////    {
        ////        foreach (var Id in Ids)
        ////        {
        ////            AdicionalAdicionales AdicionalAdicionales = this.AdicionalAdicionalesRepository.Get(Id);
        ////            AdicionalAdicionales.Activo = false;
        ////            this.AdicionalAdicionalesRepository.Update(AdicionalAdicionales);
        ////        }
        ////    });
        ////}

        ////public SelectCombo GetAdicionalAdicionalesCombos()
        ////{
        ////    SelectCombo SelectAdicionalAdicionalesCombos = new SelectCombo();
        ////    this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
        ////    {
        ////        SelectAdicionalAdicionalesCombos.Items = this.AdicionalAdicionalesRepository.GetAll()
        ////                                                      .Select(x => new SelectComboItem()
        ////                                                      {
        ////                                                          id = x.Id,
        ////                                                          text = x.Descripcion
        ////                                                      }).ToList();
        ////    });
        ////    return SelectAdicionalAdicionalesCombos;
        ////}

        ////public SelectCombo GetAllAdicionalAdicionalesesByFilterCombo(SelectComboRequest req)
        ////{
        ////    SelectCombo SelectAdicionalAdicionalesCombos = new SelectCombo();
        ////    this.AdicionalAdicionalesRepository.GetSessionFactory().SessionInterceptor(() =>
        ////    {
        ////        SelectAdicionalAdicionalesCombos.Items = this.AdicionalAdicionalesRepository.GetAllByFilter(req)
        ////                                                      .Select(x => new SelectComboItem()
        ////                                                      {
        ////                                                          id = x.Id,
        ////                                                          text = x.Descripcion
        ////                                                      }).ToList();
        ////    });
        ////    return SelectAdicionalAdicionalesCombos;
        ////}

        //#endregion


        #region Empresa
        public Empresa GetEmpresa(int Id)
        {
            Empresa Empresa = null;
            this.EmpresaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Empresa = this.EmpresaRepository.Get(Id);
            });
            return Empresa;
        }

        //public IList<Empresa> GetAllEmpresas()
        //{
        //    IList<Empresa> Empresas = null;
        //    this.EmpresaRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        Empresas = this.EmpresaRepository.GetAll();
        //    });
        //    return Empresas;
        //}

        [Loggable]
        public void AddEmpresa(Empresa Empresa)
        {
            this.EmpresaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.EmpresaRepository.Add(Empresa);
            });
        }

        /// <summary>
        /// Actualiza la Empresa enviada.
        /// </summary>
        /// <param name="Empresa">Objeto Empresa a actualizar</param>
        /// 
        //[Loggable]
        //public void UpdateEmpresa(Empresa Empresa)
        //{
        //    this.EmpresaRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
                //Empresa ToUpdate = this.EmpresaRepository.Get(Empresa.Id);
                //ToUpdate.CodigoPostal = Empresa.CodigoPostal;
                //ToUpdate.CUIT = Empresa.CUIT;
                //ToUpdate.Domicilio = Empresa.Domicilio;
                //ToUpdate.RazonSocial = Empresa.RazonSocial;
                //ToUpdate.Telefono = Empresa.Telefono;
                //ToUpdate.NombreApoderado = Empresa.NombreApoderado;
                //ToUpdate.ApellidoApoderado = Empresa.ApellidoApoderado;
                //if ((ToUpdate.Localizacion == null && Empresa.Localizacion != null) || (ToUpdate.Localizacion != null && Empresa.Localizacion != null && ToUpdate.Localizacion.Id != Empresa.Localizacion.Id))
                //{
                //    ToUpdate.Localizacion = new Localizacion();
                //    ToUpdate.Localizacion.Id = Empresa.Localizacion.Id;
                //}
                //else
                //{
                //    ToUpdate.Localizacion = Empresa.Localizacion;
                //}
        //        if ((ToUpdate.BancoDeposito == null && Empresa.BancoDeposito != null) || (ToUpdate.BancoDeposito != null && Empresa.BancoDeposito != null && ToUpdate.BancoDeposito.Id != Empresa.BancoDeposito.Id))
        //        {
        //            ToUpdate.BancoDeposito = new ComboItem();
        //            ToUpdate.BancoDeposito.Id = Empresa.BancoDeposito.Id;
        //        }
        //        else
        //        {
        //            ToUpdate.BancoDeposito = Empresa.BancoDeposito;
        //        }
        //        this.EmpresaRepository.Update(ToUpdate);


        //    });
        //}
        //public void DeleteEmpresas(List<int> Ids)
        //{
        //    this.EmpresaRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        foreach (var Id in Ids)
        //        {
        //            Empresa Empresa = this.EmpresaRepository.Get(Id);
        //            Empresa.Activo = false;
        //            this.EmpresaRepository.Update(Empresa);
        //        }
        //    });
        //}
        
        #endregion


        #region ConceptoBolo
        public ConceptoBolo GetConceptoBolo(int Id)
        {
            ConceptoBolo ConceptoBolo = null;
            this.ConceptoBoloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ConceptoBolo = this.ConceptoBoloRepository.Get(Id);
            });
            return ConceptoBolo;
        }

        public IList<ConceptoBolo> GetAllConceptosBolo()
        {
            IList<ConceptoBolo> ConceptosBolo = null;
            this.ConceptoBoloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ConceptosBolo = this.ConceptoBoloRepository.GetAll();
            });
            return ConceptosBolo;
        }

        [Loggable]
        public void AddConceptoBolo(ConceptoBolo ConceptoBolo)
        {
            this.ConceptoBoloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ConceptoBoloRepository.Add(ConceptoBolo);
            });
        }

        /// <summary>
        /// Actualiza la ConceptoBolo enviada.
        /// </summary>
        /// <param name="ConceptoBolo">Objeto ConceptoBolo a actualizar</param>
        /// 
        [Loggable]
        public void UpdateConceptoBolo(ConceptoBolo ConceptoBolo)
        {
            this.ConceptoBoloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ConceptoBolo ToUpdate = this.ConceptoBoloRepository.Get(ConceptoBolo.Id);
                ToUpdate.AdditionalDescription = ConceptoBolo.AdditionalDescription;
                ToUpdate.Descripcion = ConceptoBolo.Descripcion;
                ToUpdate.OnlyAutomatic = ConceptoBolo.OnlyAutomatic;
                ToUpdate.Porcentaje = ConceptoBolo.Porcentaje;
                ToUpdate.Suma = ConceptoBolo.Suma;
                ToUpdate.Valor = ConceptoBolo.Valor;
                this.ConceptoBoloRepository.Update(ToUpdate);
            });
        }
        public void DeleteConceptosBolo(List<int> Ids)
        {
            this.ConceptoBoloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    ConceptoBolo ConceptoBolo = this.ConceptoBoloRepository.Get(Id);
                    ConceptoBolo.Activo = false;
                    this.ConceptoBoloRepository.Update(ConceptoBolo);
                }
            });
        }

        #endregion


        #region TrabajadorBoloEscalafon
        public TrabajadorBoloEscalafon GetTrabajadorBoloEscalafon(int Id)
        {
            TrabajadorBoloEscalafon TrabajadorBoloEscalafon = null;
            this.TrabajadorBoloEscalafonRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                TrabajadorBoloEscalafon = this.TrabajadorBoloEscalafonRepository.Get(Id);
            });
            return TrabajadorBoloEscalafon;
        }

        public IList<TrabajadorBoloEscalafon> GetAllTrabajadoresBoloEscalafon()
        {
            IList<TrabajadorBoloEscalafon> TrabajadoresBoloEscalafon = null;
            this.TrabajadorBoloEscalafonRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                TrabajadoresBoloEscalafon = this.TrabajadorBoloEscalafonRepository.GetAll();
            });
            return TrabajadoresBoloEscalafon;
        }

        [Loggable]
        public void AddTrabajadorBoloEscalafon(TrabajadorBoloEscalafon TrabajadorBoloEscalafon)
        {
            this.TrabajadorBoloEscalafonRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.TrabajadorBoloEscalafonRepository.Add(TrabajadorBoloEscalafon);
            });
        }

        /// <summary>
        /// Actualiza la TrabajadorBoloEscalafon enviada.
        /// </summary>
        /// <param name="TrabajadorBoloEscalafon">Objeto TrabajadorBoloEscalafon a actualizar</param>
        /// 
        [Loggable]
        public void UpdateTrabajadorBoloEscalafon(TrabajadorBoloEscalafon TrabajadorBoloEscalafon)
        {
            this.TrabajadorBoloEscalafonRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                TrabajadorBoloEscalafon ToUpdate = this.TrabajadorBoloEscalafonRepository.Get(TrabajadorBoloEscalafon.Id);
                ToUpdate.FechaDesde = TrabajadorBoloEscalafon.FechaDesde;
                ToUpdate.FechaHasta = TrabajadorBoloEscalafon.FechaHasta;
                if ((ToUpdate.Trabajador == null && TrabajadorBoloEscalafon.Trabajador != null) || (ToUpdate.Trabajador != null && TrabajadorBoloEscalafon.Trabajador != null && ToUpdate.Trabajador.Id != TrabajadorBoloEscalafon.Trabajador.Id))
                {
                    ToUpdate.Trabajador = new Trabajador();
                    ToUpdate.Trabajador.Id = TrabajadorBoloEscalafon.Trabajador.Id;
                }
                else
                {
                    ToUpdate.Trabajador = TrabajadorBoloEscalafon.Trabajador;
                }
                if ((ToUpdate.Escalafon == null && TrabajadorBoloEscalafon.Escalafon != null) || (ToUpdate.Escalafon != null && TrabajadorBoloEscalafon.Escalafon != null && ToUpdate.Escalafon.Id != TrabajadorBoloEscalafon.Escalafon.Id))
                {
                    ToUpdate.Escalafon = new Escalafon();
                    ToUpdate.Escalafon.Id = TrabajadorBoloEscalafon.Escalafon.Id;
                }
                else
                {
                    ToUpdate.Escalafon = TrabajadorBoloEscalafon.Escalafon;
                }
                if ((ToUpdate.Bolo == null && TrabajadorBoloEscalafon.Bolo != null) || (ToUpdate.Bolo != null && TrabajadorBoloEscalafon.Bolo != null && ToUpdate.Bolo.Id != TrabajadorBoloEscalafon.Bolo.Id))
                {
                    ToUpdate.Bolo = new Bolo();
                    ToUpdate.Bolo.Id = TrabajadorBoloEscalafon.Bolo.Id;
                }
                else
                {
                    ToUpdate.Bolo = TrabajadorBoloEscalafon.Bolo;
                }
                this.TrabajadorBoloEscalafonRepository.Update(ToUpdate);
            });
        }
        public void DeleteTrabajadoresBoloEscalafon(List<int> Ids)
        {
            this.TrabajadorBoloEscalafonRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    TrabajadorBoloEscalafon TrabajadorBoloEscalafon = this.TrabajadorBoloEscalafonRepository.Get(Id);
                    TrabajadorBoloEscalafon.Activo = false;
                    this.TrabajadorBoloEscalafonRepository.Update(TrabajadorBoloEscalafon);
                }
            });
        }

        #endregion


        #region Bolo
        public Bolo GetBolo(int Id)
        {
            Bolo Bolo = null;
            this.BoloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Bolo = this.BoloRepository.Get(Id);
            });
            return Bolo;
        }

        public IList<Bolo> GetAllBolos()
        {
            IList<Bolo> Bolos = null;
            this.BoloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Bolos = this.BoloRepository.GetAll();
            });
            return Bolos;
        }

        [Loggable]
        public void AddBolo(Bolo Bolo)
        {
            this.BoloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.BoloRepository.Add(Bolo);
            });
        }

        /// <summary>
        /// Actualiza la Bolo enviada.
        /// </summary>
        /// <param name="Bolo">Objeto Bolo a actualizar</param>
        /// 
        [Loggable]
        public void UpdateBolo(Bolo Bolo)
        {
            this.BoloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Bolo ToUpdate = this.BoloRepository.Get(Bolo.Id);
                ToUpdate.CodigoPostal = Bolo.CodigoPostal;
                ToUpdate.Domicilio = Bolo.Domicilio;
                ToUpdate.Nombre = Bolo.Nombre;
                ToUpdate.DenominacionPelicula = Bolo.DenominacionPelicula;
                ToUpdate.DenominacionProducto = Bolo.DenominacionProducto;
                ToUpdate.FechaLiquidacion = Bolo.FechaLiquidacion;
                ToUpdate.TopeMaximo = Bolo.TopeMaximo;
                ToUpdate.TopeMinimo = Bolo.TopeMinimo;
                ToUpdate.Anunciante = Bolo.Anunciante;
                ToUpdate.Agencia = Bolo.Agencia;

                if ((ToUpdate.Localizacion == null && Bolo.Localizacion != null) || (ToUpdate.Localizacion != null && Bolo.Localizacion != null && ToUpdate.Localizacion.Id != Bolo.Localizacion.Id))
                {
                    ToUpdate.Localizacion = new Localizacion();
                    ToUpdate.Localizacion.Id = Bolo.Localizacion.Id;
                }
                else
                {
                    ToUpdate.Localizacion = Bolo.Localizacion;
                }
                this.BoloRepository.Update(ToUpdate);
            });
        }
        public void DeleteBolos(List<int> Ids)
        {
            this.BoloRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Bolo Bolo = this.BoloRepository.Get(Id);
                    Bolo.Activo = false;
                    this.BoloRepository.Update(Bolo);
                }
            });
        }

        public SelectCombo GetAllBolosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectServicioCombos = new SelectCombo();
            this.BoloRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectServicioCombos.Items = this.BoloRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + '(' + x.DenominacionPelicula + ')'
                                                              }).ToList();
            });
            return SelectServicioCombos;
        }
        

        #endregion



    }
}
