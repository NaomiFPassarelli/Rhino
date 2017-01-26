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
    
    public class CooperativaConfigService : ICooperativaConfigService
    {

        #region VariablesyConstructor

        private readonly IAsociadoRepository AsociadoRepository;
        private readonly IComboItemRepository ComboItemRepository;
        private readonly IConceptoRepository ConceptoRepository;
        private readonly IAdicionalPagoRepository AdicionalPagoRepository;

        public CooperativaConfigService(IAsociadoRepository AsociadoRepository, IComboItemRepository ComboItemRepository,
                                IConceptoRepository ConceptoRepository, IAdicionalPagoRepository AdicionalPagoRepository)
        {
            this.AsociadoRepository = AsociadoRepository;
            this.ComboItemRepository = ComboItemRepository;
            this.ConceptoRepository = ConceptoRepository;
            this.AdicionalPagoRepository = AdicionalPagoRepository;
        }

        #endregion

        #region Asociado
        public Asociado GetAsociado(int Id)
        {
            Asociado Asociado = null;
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociado = this.AsociadoRepository.Get(Id);
            });
            return Asociado;
        }

        public Asociado GetAsociadoCompleto(int Id)
        {
            Asociado Asociado = null;
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociado = this.AsociadoRepository.GetCompleto(Id);
            });
            return Asociado;
        }

        public IList<Asociado> GetAsociados(IList<int> Ids)
        {
            IList<Asociado> Asociados = null;
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociados = this.AsociadoRepository.GetAsociados(Ids);
            });
            return Asociados;
        }


        public IList<Asociado> GetAllAsociados()
        {
            IList<Asociado> Asociados = null;
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociados = this.AsociadoRepository.GetAll();
            });
            return Asociados;
        }

        [Loggable]
        public void AddAsociado(Asociado Asociado)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (Asociado.CUIT != null && !ComprobanteHelper.IsCUITValid(Asociado.CUIT))
                    throw new BusinessException("El CUIT no es valido");

                if (Asociado.CUIT != null && this.ExistCUITNT(Asociado.CUIT, null))
                    throw new BusinessException("El CUIT ya existe");

                Asociado.CantidadPagosAbonados = 0;
                
                this.AddAsociadoNT(Asociado); 
            });
        }
        public void AddAsociadoNT(Asociado Asociado)
        {
            this.AsociadoRepository.Add(Asociado);
        }

        [Loggable]
        public void UpdateAsociado(Asociado Asociado)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (Asociado.CUIT != null && !ComprobanteHelper.IsCUITValid(Asociado.CUIT))
                    throw new BusinessException("El CUIT no es valido");

                if (Asociado.CUIT != null && this.ExistCUITNT(Asociado.CUIT, Asociado.Id))
                    throw new BusinessException("El CUIT ya existe");

                Asociado ToUpdate = this.AsociadoRepository.Get(Asociado.Id);
                ToUpdate.Apellido = Asociado.Apellido;
                ToUpdate.Nombre = Asociado.Nombre;
                ToUpdate.CUIT = Asociado.CUIT;
                ToUpdate.DNI = Asociado.DNI;
                ToUpdate.CI = Asociado.CI;
                ToUpdate.LC = Asociado.LC;
                ToUpdate.LE = Asociado.LE;
                ToUpdate.Direccion = Asociado.Direccion;
                ToUpdate.Numero = Asociado.Numero;
                ToUpdate.Departamento = Asociado.Departamento;
                ToUpdate.Piso = Asociado.Piso;
                ToUpdate.CodigoPostal = Asociado.CodigoPostal;
                ToUpdate.Localizacion = Asociado.Localizacion;
                ToUpdate.Nacionalidad = Asociado.Nacionalidad;
                ToUpdate.FechaNacimiento = Asociado.FechaNacimiento;
                ToUpdate.Padre = Asociado.Padre;
                ToUpdate.Madre = Asociado.Madre;
                ToUpdate.Telefono = Asociado.Telefono;
                ToUpdate.EstadoCivil = Asociado.EstadoCivil;
                ToUpdate.NroCarnetConductor = Asociado.NroCarnetConductor;
                ToUpdate.CategoriaConductor = Asociado.CategoriaConductor;
                ToUpdate.MarcaVehiculo = Asociado.MarcaVehiculo;
                ToUpdate.ModeloVehiculo = Asociado.ModeloVehiculo;
                ToUpdate.NroChapaVehiculo = Asociado.NroChapaVehiculo;
                ToUpdate.FechaNotificacion = Asociado.FechaNotificacion;
                ToUpdate.ImagePath = Asociado.ImagePath;

                if (ToUpdate.ActaAlta == null)
                {
                    ToUpdate.FechaIngreso = Asociado.FechaIngreso;
                    ToUpdate.FechaActaIngreso = Asociado.FechaActaIngreso;
                }

                if (ToUpdate.ActaBaja == null)
                {
                    ToUpdate.FechaEgreso = Asociado.FechaEgreso;
                }
                ToUpdate.ActaAlta = Asociado.ActaAlta;
                ToUpdate.ActaBaja = Asociado.ActaBaja;
                ToUpdate.CantidadAbonos = Asociado.CantidadAbonos;
                ToUpdate.ImportePago = Asociado.ImportePago;
                ToUpdate.RecomendadoPor = Asociado.RecomendadoPor;
                ToUpdate.Cargo = Asociado.Cargo;
                this.AsociadoRepository.Update(ToUpdate);
            });
        }
        public void DeleteAsociados(List<int> Ids)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Asociado Asociado = this.AsociadoRepository.Get(Id);
                    Asociado.Activo = false;
                    this.AsociadoRepository.Update(Asociado);
                }
            });
        }

        public SelectCombo GetAsociadoCombos()
        {
            SelectCombo SelectAsociadoCombos = new SelectCombo();
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectAsociadoCombos.Items = this.AsociadoRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + " " + x.Apellido
                                                              }).ToList();
            });
            return SelectAsociadoCombos;
        }

        public SelectCombo GetAllAsociadosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectAsociadoCombos = new SelectCombo();
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectAsociadoCombos.Items = this.AsociadoRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre + " " + x.Apellido + " " + "(" + x.CUIT + ")"
                                                              }).ToList();
            });
            return SelectAsociadoCombos;
        }

        public int GetProximoNumeroReferencia()
        {
            int ProximoNumeroReferencia = 1;
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ProximoNumeroReferencia = this.AsociadoRepository.GetProximoNumeroReferencia();
            });
            return ProximoNumeroReferencia;
        }

        [Loggable]
        public void ImportAsociados(List<Asociado> Asociados)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Asociado in Asociados)
                {
                    this.ImportAsociado(Asociado);
                }
            });
        }

        [Loggable]
        public void ImportAsociado(Asociado Asociado)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ImportAsociadoNT(Asociado);
            });
        }

        [Loggable]
        public void ImportAsociadoNT(Asociado Asociado)
        {
            if (!ComprobanteHelper.IsCUITValid(Asociado.CUIT))
                throw new BusinessException("El CUIT es invalido.");

            if (this.ExistCUITNT(Asociado.CUIT, null))
                throw new BusinessException("El CUIT coincide con uno ya creado");

            this.AsociadoRepository.Add(Asociado);
        }

        public bool ExistCUITNT(string cuit, int? IdUpdate)
        {
            return this.AsociadoRepository.ExistCUIT(cuit, IdUpdate);
        }

        public IList<Asociado> GetAsociadosMes(int Mes, int Año)
        {
            IList<Asociado> Asociados = null;
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociados = this.AsociadoRepository.GetAsociadosMes(Mes, Año);
            });
            return Asociados;
        }

        public IList<Asociado> GetAsociadosMesEgreso(int Mes, int Año)
        {
            IList<Asociado> Asociados = null;
            this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Asociados = this.AsociadoRepository.GetAsociadosMesEgreso(Mes, Año);
            });
            return Asociados;
        }

        public void BajarAsociado(Asociado Asociado) 
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Asociado ToUpdate = this.AsociadoRepository.Get(Asociado.Id);
                ToUpdate.FechaEgreso = Asociado.FechaEgreso;
                this.AsociadoRepository.Update(ToUpdate);
            });
        }


        public void ActualizarAltaAsociados(Asociado Asociado, int Mes, int Año)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                IList<Asociado> ToUpdates = this.AsociadoRepository.GetAsociadosMes(Mes, Año);
                foreach (Asociado ToUpdateIncompleto in ToUpdates)
                {
                    Asociado ToUpdate = this.AsociadoRepository.Get(ToUpdateIncompleto.Id);
                    //ToUpdate.FechaFinalizacionAlta = Asociado.FechaFinalizacionAlta;
                    //ToUpdate.ActaAlta = Asociado.ActaAlta;
                    //ToUpdate.FechaActaIngreso = Asociado.FechaActaIngreso;
                    //ToUpdate.Presidente = Asociado.Presidente;
                    //ToUpdate.Tesorero = Asociado.Tesorero;
                    //ToUpdate.Secretario = Asociado.Secretario;
                    //ToUpdate.OtroPresente = Asociado.OtroPresente;
                    this.AsociadoRepository.Update(ToUpdate);           
                }
            });
        }

        public void ActualizarBajaAsociados(Asociado Asociado, int Mes, int Año)
        {
            this.AsociadoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                IList<Asociado> ToUpdates = this.AsociadoRepository.GetAsociadosMesEgreso(Mes, Año);
                foreach (Asociado ToUpdateIncompleto in ToUpdates)
                {
                    Asociado ToUpdate = this.AsociadoRepository.Get(ToUpdateIncompleto.Id);
                    ToUpdate.ActaBaja = Asociado.ActaBaja;
                    this.AsociadoRepository.Update(ToUpdate);
                }
            });
        }

        //public Asociado LoadHeader()
        //{
        //    Asociado Header = null;
        //    this.AsociadoRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        Header = this.AsociadoRepository.LoadHeader();

        //    });
        //    return Header;
        //}
        

        #endregion




        #region Adicional
        public Concepto GetConcepto(int Id)
        {
            Concepto Concepto = null;
            this.ConceptoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Concepto = this.ConceptoRepository.Get(Id);
            });
            return Concepto;
        }

        public IList<Concepto> GetAllConceptos()
        {
            IList<Concepto> Conceptos = null;
            this.ConceptoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Conceptos = this.ConceptoRepository.GetAll();
            });
            return Conceptos;
        }

        [Loggable]
        public void AddConcepto(Concepto Concepto)
        {
            this.ConceptoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddConceptoNT(Concepto);
            });
        }
        public void AddConceptoNT(Concepto Concepto)
        {
            this.ConceptoRepository.Add(Concepto);
        }

        [Loggable]
        public void UpdateConcepto(Concepto Concepto)
        {
            this.ConceptoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Concepto ToUpdate = this.ConceptoRepository.Get(Concepto.Id);
                ToUpdate.Descripcion = Concepto.Descripcion;
                ToUpdate.AdditionalDescription = Concepto.AdditionalDescription;
                //ToUpdate.Porcentaje = Concepto.Porcentaje;
                ToUpdate.Valor = Concepto.Valor;
                ToUpdate.Suma = Concepto.Suma;
                ToUpdate.TipoConcepto = Concepto.TipoConcepto;
                this.ConceptoRepository.Update(ToUpdate);
            });
        }
        public void DeleteConceptos(List<int> Ids)
        {
            this.ConceptoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Concepto Concepto = this.ConceptoRepository.Get(Id);
                    this.ConceptoRepository.Delete(Concepto);
                }
            });
        }

        public SelectCombo GetConceptoCombos()
        {
            SelectCombo SelectConceptoCombos = new SelectCombo();
            this.ConceptoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectConceptoCombos.Items = this.ConceptoRepository.GetAll()
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion + x.AdditionalDescription
                                                              }).ToList();
            });
            return SelectConceptoCombos;
        }

        public SelectCombo GetAllConceptosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectConceptoCombos = new SelectCombo();
            this.ConceptoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectConceptoCombos.Items = this.ConceptoRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion + x.AdditionalDescription
                                                              }).ToList();
            });
            return SelectConceptoCombos;
        }

        public Concepto GetConceptoByFilterCombo(string DescripcionConcepto)
        {
            Concepto c = null;
            this.ConceptoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                c = this.ConceptoRepository.GetByFilter(DescripcionConcepto);
                                                            //.Select(x => new SelectComboItem()
                                                            //  {
                                                            //      id = x.Id,
                                                            //      text = x.Descripcion + x.AdditionalDescription
                                                            //  });
                
            });
            return c;
        }


        //public void AddAdicionalConAdicionales(Concepto Concepto, IList<Concepto> Adicionales)
        //{
        //    this.ConceptoRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        this.AddAdicionalNT(Adicional);
        //        foreach (Adicional A in Adicionales)
        //        {
        //            AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //            RAAS.EsDefault = true;
        //            RAAS.Adicional = new Concepto();
        //            RAAS.Concepto.Id = Concepto.Id;

        //            RAAS.AdicionalSobre = new Concepto();
        //            RAAS.AdicionalSobre.Id = A.Id;

        //            this.AddAdicionalAdicionalesNT(RAAS);
        //        }
        //    });
        //}


        #endregion

        #region AdicionalPago
        public AdicionalPago GetAdicionalPagoNT(int Id)
        {
            AdicionalPago AdicionalPago = null;
            AdicionalPago = this.AdicionalPagoRepository.Get(Id);
            return AdicionalPago;
        }

        public AdicionalPago GetAdicionalPago(int Id)
        {
            AdicionalPago AdicionalPago = null;
            this.AdicionalPagoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                AdicionalPago = this.AdicionalPagoRepository.Get(Id);
            });
            return AdicionalPago;
        }

        //public IList<AdicionalPago> GetAllAdicionalPagoes()
        //{
        //    IList<AdicionalPago> AdicionalPagoes = null;
        //    this.AdicionalPagoRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        AdicionalPagoes = this.AdicionalPagoRepository.GetAll();
        //    });
        //    return AdicionalPagoes;
        //}

        [Loggable]
        public void AddAdicionalPago(AdicionalPago AdicionalPago)
        {
            this.AdicionalPagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.AddAdicionalPagoNT(AdicionalPago);
            });
        }
        public void AddAdicionalPagoNT(AdicionalPago AdicionalPago)
        {
            this.AdicionalPagoRepository.Add(AdicionalPago);
        }

        //[Loggable]
        //public void UpdateAdicionalPago(AdicionalPago AdicionalPago, IList<AdicionalAdicionales> AdicionalPagoesAdicionalPago = null)
        //{
        //    this.AdicionalPagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        AdicionalPago ToUpdate = this.AdicionalPagoRepository.Get(AdicionalPago.Id);
        //        IList<AdicionalAdicionales> AdicionalPagoesToUpdate = this.AdicionalAdicionalesRepository.GetByAdicional(AdicionalPago.Id, true);

        //        if (AdicionalPagoesToUpdate.Count == 0 && AdicionalPagoesAdicionalPago.Count > 0)
        //        {
        //            foreach (AdicionalAdicionales Adic in AdicionalPagoesAdicionalPago)
        //            {
        //                AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //                RAAS.EsDefault = true;
        //                RAAS.Adicional = new Concepto();
        //                RAAS.Concepto.Id = AdicionalPago.Id;

        //                RAAS.AdicionalSobre = new Concepto();
        //                RAAS.AdicionalSobre.Id = Adic.Id;

        //                this.AddAdicionalAdicionalesNT(RAAS);
        //            }
        //        }
        //        else if (AdicionalPagoesToUpdate.Count > 0 && AdicionalPagoesAdicionalPago.Count == 0)
        //        {
        //            foreach (AdicionalAdicionales Adic in AdicionalPagoesToUpdate)
        //            {
        //                this.AdicionalAdicionalesRepository.Delete(Adic);
        //            }
        //        }
        //        else if (AdicionalPagoesToUpdate.Count > 0 && AdicionalPagoesAdicionalPago.Count > 0)
        //        {
        //            foreach (AdicionalAdicionales Adic in AdicionalPagoesToUpdate)
        //            {
        //                //int AdicionalPagoAgregar = AdicionalPagoesAdicionalPago.IndexOf(Adic);
        //                //si estaba pero ahora no esta mas, eliminar
        //                AdicionalAdicionales AdicionalPagoAgregar = new AdicionalAdicionales();
        //                //es cn el id porque AdicionalSobre no lo seteamos en la vista
        //                AdicionalPagoAgregar = AdicionalPagoesAdicionalPago.Where(x => x.Id == Adic.AdicionalSobre.Id).SingleOrDefault();
        //                if (AdicionalPagoAgregar == null)
        //                {
        //                    this.AdicionalAdicionalesRepository.Delete(Adic);
        //                }
        //            }
        //            foreach (AdicionalAdicionales Adic in AdicionalPagoesAdicionalPago)
        //            {
        //                AdicionalAdicionales AdicionalPagoAgregar = new AdicionalAdicionales();
        //                AdicionalPagoAgregar = AdicionalPagoesToUpdate.Where(x => x.AdicionalSobre.Id == Adic.Id).SingleOrDefault();
        //                //si esta pero antes no estaba, agreagr
        //                //int AdicionalPagoAgregar = AdicionalPagoesToUpdate.IndexOf(Adic);
        //                //if (AdicionalPagoAgregar < 0)
        //                if (AdicionalPagoAgregar == null)
        //                {
        //                    AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //                    RAAS.EsDefault = true;
        //                    RAAS.Adicional = new Concepto();
        //                    RAAS.Concepto.Id = AdicionalPago.Id;

        //                    RAAS.AdicionalSobre = new Concepto();
        //                    RAAS.AdicionalSobre.Id = Adic.Id;

        //                    this.AddAdicionalAdicionalesNT(RAAS);
        //                }
        //            }
        //        }

        //        //ToUpdate.Descripcion = AdicionalPago.Descripcion;
        //        //ToUpdate.Porcentaje = AdicionalPago.Porcentaje;
        //        //ToUpdate.Valor = AdicionalPago.Valor;
        //        //ToUpdate.Suma = AdicionalPago.Suma;
        //        //ToUpdate.TipoLiquidacion = AdicionalPago.TipoLiquidacion;
        //        this.AdicionalPagoRepository.Update(ToUpdate);
        //    });
        //}
        public void DeleteAdicionalPagos(List<int> Ids)
        {
            this.AdicionalPagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    AdicionalPago AdicionalPago = this.AdicionalPagoRepository.Get(Id);
                    this.AdicionalPagoRepository.Delete(AdicionalPago);
                }
            });
        }

        //public SelectCombo GetAdicionalPagoCombos()
        //{
        //    SelectCombo SelectAdicionalPagoCombos = new SelectCombo();
        //    this.AdicionalPagoRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectAdicionalPagoCombos.Items = this.AdicionalPagoRepository.GetAll()
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                      }).ToList();
        //    });
        //    return SelectAdicionalPagoCombos;
        //}

        //public SelectCombo GetAllAdicionalPagoesByFilterCombo(SelectComboRequest req)
        //{
        //    SelectCombo SelectAdicionalPagoCombos = new SelectCombo();
        //    this.AdicionalPagoRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        SelectAdicionalPagoCombos.Items = this.AdicionalPagoRepository.GetAllByFilter(req)
        //                                                      .Select(x => new SelectComboItem()
        //                                                      {
        //                                                          id = x.Id,
        //                                                      }).ToList();
        //    });
        //    return SelectAdicionalPagoCombos;
        //}

        //public void AddAdicionalPagoConAdicionalPagoes(AdicionalPago AdicionalPago, IList<AdicionalPago> AdicionalPagoes)
        //{
        //    this.AdicionalPagoRepository.GetSessionFactory().TransactionalInterceptor(() =>
        //    {
        //        this.AddAdicionalPagoNT(AdicionalPago);
        //        foreach (AdicionalPago A in AdicionalPagoes)
        //        {
        //            AdicionalAdicionales RAAS = new AdicionalAdicionales();
        //            RAAS.EsDefault = true;
        //            RAAS.Adicional = new Concepto();
        //            RAAS.Concepto.Id = AdicionalPago.Id;

        //            RAAS.AdicionalSobre = new Concepto();
        //            RAAS.AdicionalSobre.Id = A.Id;

        //            this.AddAdicionalAdicionalesNT(RAAS);
        //        }
        //    });
        //}
        //public IList<AdicionalPago> GetAdicionalesDelPeriodoByEmpleado(string Periodo, int IdEmpleado)
        //{
        //    IList<AdicionalPago> ARs = null;
        //    this.AdicionalPagoRepository.GetSessionFactory().SessionInterceptor(() =>
        //    {
        //        ARs = this.AdicionalPagoRepository.GetAdicionalesDelPeriodoByEmpleado(Periodo, IdEmpleado);
        //    });
        //    return ARs;
        //}

        #endregion


    }
}
