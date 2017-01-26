using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.App.Logging;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.CommonApp.Session;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Contabilidad;
using Woopin.SGC.Repositories.Ventas;

namespace Woopin.SGC.Services
{
    public class VentasConfigService : IVentasConfigService
    {
        #region VariablesyConstructor
        private readonly ICategoriaIVARepository CategoriaIVARepository;
        private readonly ILocalizacionRepository LocalizacionRepository;
        private readonly IComboItemRepository ComboItemRepository;
        private readonly IClienteRepository ClienteRepository;
        private readonly IComprobanteVentaRepository ComprobanteVentaRepository;
        private readonly IGrupoEconomicoRepository GrupoEconomicoRepository;
        private readonly ICuentaRepository CuentaRepository;
        private readonly IListaPreciosRepository ListaPreciosRepository;
        private readonly ITalonarioRepository TalonarioRepoitory;
        private readonly ILocalidadRepository LocalidadRepository;
        private readonly IOrganizacionRepository OrganizacionRepository;
        private readonly IDireccionRepository DireccionRepository;
        public VentasConfigService(IClienteRepository ClienteRepository, IComprobanteVentaRepository ComprobanteVentaRepository, IGrupoEconomicoRepository GrupoEconomicoRepository,
                                ICuentaRepository CuentaRepository, IListaPreciosRepository ListaPreciosRepository, ITalonarioRepository TalonarioRepoitory,
                                IOrganizacionRepository OrganizacionRepository, IComboItemRepository ComboItemRepository, ILocalizacionRepository LocalizacionRepository,
                                ICategoriaIVARepository CategoriaIVARepository, ILocalidadRepository LocalidadRepository, IDireccionRepository DireccionRepository)
        {
            this.ClienteRepository = ClienteRepository;
            this.ComprobanteVentaRepository = ComprobanteVentaRepository;
            this.GrupoEconomicoRepository = GrupoEconomicoRepository;
            this.CuentaRepository = CuentaRepository;
            this.ListaPreciosRepository = ListaPreciosRepository;
            this.TalonarioRepoitory = TalonarioRepoitory;
            this.OrganizacionRepository = OrganizacionRepository;
            this.ComboItemRepository = ComboItemRepository;
            this.CategoriaIVARepository = CategoriaIVARepository;
            this.LocalizacionRepository = LocalizacionRepository;
            this.LocalidadRepository = LocalidadRepository;
            this.DireccionRepository = DireccionRepository;
        }

        #endregion


        #region Cliente
        public Cliente GetCliente(int Id)
        {
            Cliente Cliente = null;
            this.ClienteRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                //Cliente = this.ClienteRepository.Get(Id);
                Cliente = this.ClienteRepository.GetCompleto(Id);
            });
            return Cliente;
        }

        public IList<Cliente> GetAllClientes()
        {
            IList<Cliente> Clientes = null;
            this.ClienteRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Clientes = this.ClienteRepository.GetAll();
                foreach (Cliente c in Clientes)
                {
                    c.DireccionesEntrega = null;
                }
            });
            return Clientes;
        }

        public IList<Cliente> GetAllClientesByFilter(PagingRequest paging)
        {
            IList<Cliente> Clientes = null;
            this.ClienteRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Clientes = this.ClienteRepository.GetAllByFilter(paging);
            });
            return Clientes;
        }

        [Loggable]
        public void AddCliente(Cliente Cliente)
        {
            this.ClienteRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (!ComprobanteHelper.IsCUITValid(Cliente.CUIT))
                    throw new BusinessException("El CUIT no es valido");

                if (this.ExistCUITNT(Cliente.CUIT, null))
                    throw new BusinessException("El CUIT ya existe");

                //TODO ver si se agregan las direcciones de entrega
                if (Cliente.DireccionesEntrega == null || Cliente.DireccionesEntrega.Count == 0)
                {
                    Cliente.DireccionesEntrega = null;
                }
                else {
                    foreach (Direccion Dir in Cliente.DireccionesEntrega)
                    {
                        this.DireccionRepository.Add(Dir);
                    }
                
                }
                this.ClienteRepository.Add(Cliente);
            });
        }

        [Loggable]
        public void ImportClientes(List<Cliente> clientes)
        {
            this.ClienteRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach(var Cliente in clientes)
                {
                    this.ImportClienteNT(Cliente);
                }                
            });
        }

        [Loggable]
        public void ImportCliente(Cliente Cliente)
        {
            this.ClienteRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ImportClienteNT(Cliente);
            });
        }

        [Loggable]
        public void ImportClienteNT(Cliente Cliente)
        {
            if (!ComprobanteHelper.IsCUITValid(Cliente.CUIT))
                throw new BusinessException("El CUIT es invalido.");

            if (this.ExistCUITNT(Cliente.CUIT, null))
                throw new BusinessException("El CUIT coincide con uno ya creado");

            if (Cliente.CondicionVenta != null)
            {
                Cliente.CondicionVenta = this.ComboItemRepository.GetByComboAndName(ComboType.CondicionCompraVenta, Cliente.CondicionVenta.Data);
            }
            if (Cliente.CategoriaIva != null)
            {
                Cliente.CategoriaIva = this.CategoriaIVARepository.GetByNombre(Cliente.CategoriaIva.Nombre);
            }
            if (Cliente.Localizacion != null)
            {
                Cliente.Localizacion = this.LocalizacionRepository.GetByNombre(Cliente.Localizacion.Nombre);
            }
            if (Cliente.Localidad != null)
            {
                Cliente.Localidad = this.LocalidadRepository.GetByNombre(Cliente.Localidad.Nombre);
            }
            if (Cliente.Pais != null)
            {
                Cliente.Pais = this.ComboItemRepository.GetByComboAndName(ComboType.Paises, Cliente.Pais.Data);
            }
            
            this.ClienteRepository.Add(Cliente);
        }

        [Loggable]
        public void UpdateCliente(Cliente Cliente)
        {
            this.ClienteRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Cliente ToUpdate = this.ClienteRepository.Get(Cliente.Id);

                if (!ComprobanteHelper.IsCUITValid(Cliente.CUIT))
                    throw new BusinessException("El CUIT es invalido.");

                if (this.ExistCUITNT(Cliente.CUIT, Cliente.Id))
                    throw new BusinessException("El CUIT coincide con uno ya creado");


                ToUpdate.RazonSocial = Cliente.RazonSocial;
                ToUpdate.CUIT = Cliente.CUIT;
                ToUpdate.CategoriaIva = new CategoriaIVA();
                ToUpdate.CategoriaIva.Id = Cliente.CategoriaIva.Id;
                ToUpdate.Direccion = Cliente.Direccion;
                ToUpdate.Numero = Cliente.Numero;
                ToUpdate.Departamento = Cliente.Departamento;
                ToUpdate.Piso = Cliente.Piso;
                ToUpdate.Localidad = Cliente.Localidad;
                ToUpdate.CodigoPostal = Cliente.CodigoPostal;
                ToUpdate.Localizacion = new Localizacion();
                ToUpdate.Localizacion.Id = Cliente.Localizacion.Id;
                ToUpdate.Telefono = Cliente.Telefono;
                ToUpdate.Email = Cliente.Email;
                ToUpdate.CondicionVenta = new ComboItem();
                ToUpdate.CondicionVenta.Id = Cliente.CondicionVenta.Id;
                ToUpdate.CondicionVentaEstadistica = Cliente.CondicionVentaEstadistica;
                ToUpdate.CondicionVentaContratada = Cliente.CondicionVentaContratada;

                // Porque no es requiered
                if (Cliente.Master != ToUpdate.Master && Cliente.Master.Id > 0)
                {
                    if (Cliente.Master != null)
                    {
                        ToUpdate.Master = new GrupoEconomico() { Id = Cliente.Master.Id };
                    }
                    else {
                        ToUpdate.Master = Cliente.Master;
                    }
                }
                //si antes era null y ahora != null, pones directamente las nuevas
                //si antes era != null y ahora null, eliminas todo
                //si antes y ahora es != null entonces 
                //Cliente es lo nuevo ToUpdate lo viejo
                if (ToUpdate.DireccionesEntrega == null && Cliente.DireccionesEntrega != null)
                {
                    ToUpdate.DireccionesEntrega = new List<Direccion>();
                    ToUpdate.DireccionesEntrega = Cliente.DireccionesEntrega;
                    foreach(Direccion Dir in ToUpdate.DireccionesEntrega)
                    {
                        this.DireccionRepository.Add(Dir);                    
                    }
                } else if(ToUpdate.DireccionesEntrega != null && Cliente.DireccionesEntrega == null ){
                    foreach(Direccion Dir in ToUpdate.DireccionesEntrega)
                    {
                        this.DireccionRepository.Delete(Dir);
                    }
                    ToUpdate.DireccionesEntrega = null;
                }
                else
                {
                    //TODO revisar de los nuevos si vienen con id=0, de ser asi a esos habria que agregarlos
                    //los que siguen existiendo dejarlos
                    //y los que se eliminaron sacarlos
                    //capaz con esto funciona, capaz no
                    foreach(Direccion direcToUpdate in Cliente.DireccionesEntrega)
                    {
                        if (direcToUpdate.Id > 0 && direcToUpdate.Id != null)
                        {
                            //existe, actualizar
                            this.DireccionRepository.Update(direcToUpdate);
                        }
                        else { 
                            //no existe agregar
                            this.DireccionRepository.Add(direcToUpdate);
                        }
                    }
                    ToUpdate.DireccionesEntrega = Cliente.DireccionesEntrega;

                }
                
                this.ClienteRepository.Update(ToUpdate);

            });
        }
        public void DeleteClientes(List<int> Ids)
        {
            this.ClienteRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Cliente Cliente = this.ClienteRepository.Get(Id);
                    Cliente.Activo = false;


                    IList<Direccion> DireccionesEntrega = Cliente.DireccionesEntrega;
                    Cliente.DireccionesEntrega = null;
                    this.ClienteRepository.Update(Cliente);

                    foreach (Direccion Dir in DireccionesEntrega)
                    {
                        Direccion D = this.DireccionRepository.Get(Dir.Id);
                        this.DireccionRepository.Delete(D);
                    }

                    //for(int cantDire = 0; cantDire < Cliente.DireccionesEntrega.Count; cantDire++)
                    //{
                    //    this.DireccionRepository.Delete(Cliente.DireccionesEntrega[cantDire]);
                    //}

                }
            });
        }
        public void CambiarActivo(List<int> Ids, bool Activo)
        {
            this.ClienteRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach(var Id in Ids)
                { 
                    Cliente Cliente = this.ClienteRepository.Get(Id);
                    Cliente.Activo = Activo;
                    this.ClienteRepository.Update(Cliente);
                }
            });
        }

        public bool ExistCUITNT(string cuit, int? IdUpdate)
        {
            return this.ClienteRepository.ExistCUIT(cuit, IdUpdate);
        }

        #endregion

        #region SelectCombo
        public SelectCombo GetClienteCombos()
        {
            SelectCombo SelectClienteCombos = new SelectCombo();
            this.ClienteRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectClienteCombos.Items = this.ClienteRepository.GetAll()
                                                            .Select(x => new SelectComboItem(){
                                                                    id = x.Id,
                                                                    text = x.RazonSocial + '(' + x.CUIT + ')'
                                                            }).ToList();
            });
            return SelectClienteCombos;
        }

        public SelectCombo GetAllClientesByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectServicioCombos = new SelectCombo();
            this.ClienteRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectServicioCombos.Items = this.ClienteRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.RazonSocial + '(' + x.CUIT + ')'
                                                              }).ToList();
            });
            return SelectServicioCombos;
        }

        public SelectCombo GetAllGruposByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectServicioCombos = new SelectCombo();
            this.GrupoEconomicoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectServicioCombos.Items = this.GrupoEconomicoRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Nombre
                                                              }).ToList();
            });
            return SelectServicioCombos;
        }
        

        #endregion

        #region GrupoEconomico
        public GrupoEconomico GetGrupoEconomico(int Id)
        {
            GrupoEconomico GrupoEconomico = null;
            this.GrupoEconomicoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                GrupoEconomico = this.GrupoEconomicoRepository.Get(Id);
            });
            return GrupoEconomico;
        }

        public IList<GrupoEconomico> GetAllGrupoEconomicos()
        {
            IList<GrupoEconomico> GrupoEconomicos = null;
            this.GrupoEconomicoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                GrupoEconomicos = this.GrupoEconomicoRepository.GetAll();
            });
            return GrupoEconomicos;
        }
        public void AddGrupoEconomico(GrupoEconomico GrupoEconomico)
        {
            this.GrupoEconomicoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.GrupoEconomicoRepository.Add(GrupoEconomico);
            });
        }
        public void UpdateGrupoEconomico(GrupoEconomico GrupoEconomico)
        {
            this.GrupoEconomicoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                GrupoEconomico ToUpdate = this.GrupoEconomicoRepository.Get(GrupoEconomico.Id);
                ToUpdate.Nombre = GrupoEconomico.Nombre;
                this.GrupoEconomicoRepository.Update(ToUpdate);
            });
        }
        public void DeleteGrupoEconomico(List<int> Ids)
        {
            this.GrupoEconomicoRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    GrupoEconomico GrupoEconomico = this.GrupoEconomicoRepository.Get(Id);
                    GrupoEconomico.Activo = false;
                    this.GrupoEconomicoRepository.Update(GrupoEconomico);
                }
            });
        }

        public SelectCombo GetGrupoEconomicoCombos()
        {
            SelectCombo SelectClienteCombos = new SelectCombo();
            this.GrupoEconomicoRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectClienteCombos.Items = this.GrupoEconomicoRepository.GetAll()
                                            .Select(x => new SelectComboItem()
                                            {
                                                text = x.Nombre,
                                                id = x.Id,
                                                additionalData = x.Activo.ToString()
                                            }).ToList();
            });
            return SelectClienteCombos;
        }
        #endregion

        #region Listado de Precios de Ventas
        public ListaPreciosItem GetPrecioForArticulo(int IdArticulo, int IdCliente)
        {
            ListaPreciosItem ListaPreciosItem = null;
            this.ListaPreciosRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ListaPreciosItem = this.ListaPreciosRepository.GetForVentas(IdArticulo, IdCliente);
            });
            return ListaPreciosItem;
        }
        public ListaPreciosItem GetListaPreciosItem(int Id)
        {
            ListaPreciosItem ListaPreciosItem = null;
            this.ListaPreciosRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ListaPreciosItem = this.ListaPreciosRepository.Get(Id);
            });
            return ListaPreciosItem;
        }
        public IList<ListaPreciosItem> GetAllPreciosById(string Id)
        {
            IList<ListaPreciosItem> lista = null;
            if (Id == "") return new List<ListaPreciosItem>();
            this.ListaPreciosRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                if (Id.Contains(TipoListaPrecios.Default))
                {
                    lista = this.ListaPreciosRepository.GetAllDefault();
                }
                else if (Id.Contains(TipoListaPrecios.Cliente))
                {
                    int IdCliente = Convert.ToInt32(Id.Replace(TipoListaPrecios.Cliente, ""));
                    lista = this.ListaPreciosRepository.GetAllByCliente(IdCliente);
                }
                else
                {
                    int IdCliente = Convert.ToInt32(Id.Replace(TipoListaPrecios.Grupo, ""));
                    lista = this.ListaPreciosRepository.GetAllByGrupo(IdCliente);
                }

            });
            return lista;
        }

        [Loggable]
        public void SaveListaPrecios(ListaPreciosItem ListaPreciosItem, string IdListado)
        {
            this.ListaPreciosRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (IdListado.Contains(TipoListaPrecios.Cliente))
                {
                    int Id = Convert.ToInt32(IdListado.Replace(TipoListaPrecios.Cliente, ""));
                    this.ListaPreciosRepository.SaveByCliente(ListaPreciosItem, Id);
                }
                else if (IdListado.Contains(TipoListaPrecios.Default))
                {
                    this.ListaPreciosRepository.SaveByDefault(ListaPreciosItem);
                }
                else
                {
                    int Id = Convert.ToInt32(IdListado.Replace(TipoListaPrecios.Grupo, ""));
                    this.ListaPreciosRepository.SaveByGrupo(ListaPreciosItem, Id);
                }

            });
        }
        #endregion

        #region Talonarios
        public Talonario GetTalonario(int Id)
        {
            Talonario Talonario = null;
            this.TalonarioRepoitory.GetSessionFactory().SessionInterceptor(() =>
            {
                Talonario = this.TalonarioRepoitory.Get(Id);
            });
            return Talonario;
        }
        public IList<Talonario> GetAllTalonarios()
        {
            IList<Talonario> Talonarios = null;
            this.TalonarioRepoitory.GetSessionFactory().SessionInterceptor(() =>
            {
                Talonarios = this.TalonarioRepoitory.GetAll();
            });
            return Talonarios;
        }
        public void AddTalonario(Talonario Talonario)
        {
            this.TalonarioRepoitory.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (this.TalonarioRepoitory.GetByPrefijo(Talonario.Prefijo) != null)
                {
                    throw new ValidationException("Ya se encuentra un talonario creado con ese prefijo.");
                }

                this.TalonarioRepoitory.Add(Talonario);
            });
        }
        public void UpdateTalonario(Talonario Talonario)
        {
            this.TalonarioRepoitory.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Talonario talonarioPrefijo = this.TalonarioRepoitory.GetByPrefijo(Talonario.Prefijo);
                if (talonarioPrefijo != null && talonarioPrefijo.Id != Talonario.Id)
                {
                    throw new ValidationException("Ya se encuentra un talonario creado con ese prefijo.");
                }

                Talonario talonarioToUpdate = this.TalonarioRepoitory.Get(Talonario.Id);

                talonarioToUpdate.Prefijo = Talonario.Prefijo;
                talonarioToUpdate.PuntoVenta = Talonario.PuntoVenta;
                talonarioToUpdate.InicioActividad = Talonario.InicioActividad;
                talonarioToUpdate.Descripcion = Talonario.Descripcion;
                talonarioToUpdate.CertificadoPath = Talonario.CertificadoPath;

                this.TalonarioRepoitory.Update(talonarioToUpdate);
            });
        }
        public void DeleteTalonario(List<int> Ids)
        {
            this.TalonarioRepoitory.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Talonario Talonario = this.TalonarioRepoitory.Get(Id);
                    Talonario.Activo = false;
                    this.TalonarioRepoitory.Update(Talonario);
                }
            });
        }
        #endregion
    }
}
