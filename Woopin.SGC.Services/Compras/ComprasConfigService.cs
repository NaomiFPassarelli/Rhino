using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.App.Logging;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Repositories.Common;
using Woopin.SGC.Repositories.Compras;
using Woopin.SGC.Repositories.Contabilidad;

namespace Woopin.SGC.Services
{
    public class ComprasConfigService : IComprasConfigService
    {
        #region VariablesyConstructor

        private readonly IProveedorRepository ProveedorRepository;
        private readonly IRubroCompraRepository RubroCompraRepository;
        private readonly IComprobanteCompraRepository ComprobanteCompraRepository;
        private readonly ICuentaRepository CuentaRepository;
        private readonly IOrganizacionRepository OrganizacionRepository;
        private readonly ICategoriaIVARepository CategoriaIVARepository;
        private readonly ILocalizacionRepository LocalizacionRepository;
        private readonly ILocalidadRepository LocalidadRepository;
        private readonly IComboItemRepository ComboItemRepository;

        public ComprasConfigService(IProveedorRepository ProveedorRepository, IRubroCompraRepository RubroCompraRepository, IComprobanteCompraRepository ComprobanteCompraRepository,
            ICuentaRepository CuentaRepository, IOrganizacionRepository OrganizacionRepository, ICategoriaIVARepository CategoriaIVARepository, ILocalizacionRepository LocalizacionRepository,
            IComboItemRepository ComboItemRepository, ILocalidadRepository LocalidadRepository)
        {
            this.ProveedorRepository = ProveedorRepository;
            this.RubroCompraRepository = RubroCompraRepository;
            this.ComprobanteCompraRepository = ComprobanteCompraRepository;
            this.CuentaRepository = CuentaRepository;
            this.OrganizacionRepository = OrganizacionRepository;
            this.ComboItemRepository = ComboItemRepository;
            this.CategoriaIVARepository = CategoriaIVARepository;
            this.LocalizacionRepository = LocalizacionRepository;
            this.LocalidadRepository = LocalidadRepository;
        }

        #endregion


        #region Proveedor
        public Proveedor GetProveedor(int Id)
        {
            Proveedor Proveedor = null;
            this.ProveedorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Proveedor = this.ProveedorRepository.Get(Id);
            });
            return Proveedor;
        }

        public IList<Proveedor> GetAllProveedores()
        {
            IList<Proveedor> Proveedores = null;
            this.ProveedorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Proveedores = this.ProveedorRepository.GetAll();
            });
            return Proveedores;
        }
        public void AddProveedor(Proveedor Proveedor)
        {
            this.ProveedorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if(!ComprobanteHelper.IsCUITValid(Proveedor.CUIT))
                    throw new BusinessException("El CUIT no es valido");
                
                if(this.ExistCUITNT(Proveedor.CUIT, null))
                    throw new BusinessException("El CUIT ya existe");

                this.ProveedorRepository.Add(Proveedor);
            });
        }
        public void UpdateProveedor(Proveedor Proveedor)
        {
            this.ProveedorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Proveedor ToUpdate = this.ProveedorRepository.Get(Proveedor.Id);

                if (!ComprobanteHelper.IsCUITValid(Proveedor.CUIT))
                    throw new BusinessException("El CUIT es invalido.");

                if (this.ExistCUITNT(Proveedor.CUIT, Proveedor.Id))
                    throw new BusinessException("El CUIT coincide con uno ya creado");

                ToUpdate.RazonSocial = Proveedor.RazonSocial;
                ToUpdate.CUIT = Proveedor.CUIT;
                ToUpdate.CategoriaIva = new CategoriaIVA();
                ToUpdate.CategoriaIva.Id = Proveedor.CategoriaIva.Id;
                ToUpdate.Direccion = Proveedor.Direccion;
                ToUpdate.Numero = Proveedor.Numero;
                ToUpdate.Departamento = Proveedor.Departamento;
                ToUpdate.Piso = Proveedor.Piso;
                ToUpdate.Localidad = Proveedor.Localidad;
                ToUpdate.CodigoPostal = Proveedor.CodigoPostal;
                ToUpdate.Localizacion = new Localizacion();
                ToUpdate.Localizacion.Id = Proveedor.Localizacion.Id;
                ToUpdate.Telefono = Proveedor.Telefono;
                ToUpdate.Email = Proveedor.Email;
                ToUpdate.CondicionCompra = new ComboItem();
                ToUpdate.CondicionCompra.Id = Proveedor.CondicionCompra.Id;
                ToUpdate.Interno = Proveedor.Interno;

                this.ProveedorRepository.Update(ToUpdate);
            });
        }
        public void DeleteProveedores(List<int> Ids)
        {
            this.ProveedorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Proveedor Proveedor = this.ProveedorRepository.Get(Id);
                    Proveedor.Activo = false;
                    this.ProveedorRepository.Update(Proveedor);
                }
            });
        }
        public void CambiarActivo(List<int> Ids, bool Activo)
        {
            this.ProveedorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach(var Id in Ids)
                {
                    Proveedor Proveedor = this.ProveedorRepository.Get(Id);
                    Proveedor.Activo = Activo;
                    this.ProveedorRepository.Update(Proveedor);
                }
            });
        }

        public bool ExistCUITNT(string cuit, int? IdUpdate)
        {
            return this.ProveedorRepository.ExistCUIT(cuit, IdUpdate);
        }

        [Loggable]
        public void ImportProveedores(List<Proveedor> proveedores)
        {
            this.ProveedorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var proveedor in proveedores)
                {
                    this.ImportProveedor(proveedor);
                }
            });
        }

        [Loggable]
        public void ImportProveedor(Proveedor Proveedor)
        {
            this.ProveedorRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.ImportProveedorNT(Proveedor);
            });
        }

        [Loggable]
        public void ImportProveedorNT(Proveedor Proveedor)
        {
            if (!ComprobanteHelper.IsCUITValid(Proveedor.CUIT))
                throw new BusinessException("El CUIT es invalido.");

            if (this.ExistCUITNT(Proveedor.CUIT, null))
                throw new BusinessException("El CUIT coincide con uno ya creado");

            
            if (Proveedor.CondicionCompra != null)
            {
                Proveedor.CondicionCompra = this.ComboItemRepository.GetByComboAndName(ComboType.CondicionCompraVenta, Proveedor.CondicionCompra.Data);
            }
            if (Proveedor.CategoriaIva != null)
            {
                Proveedor.CategoriaIva = this.CategoriaIVARepository.GetByNombre(Proveedor.CategoriaIva.Nombre);
            }
            if (Proveedor.Localizacion != null)
            {
                Proveedor.Localizacion = this.LocalizacionRepository.GetByNombre(Proveedor.Localizacion.Nombre);
            }
            if (Proveedor.Localidad != null)
            {
                Proveedor.Localidad = this.LocalidadRepository.GetByNombre(Proveedor.Localidad.Nombre);
            }
            if (Proveedor.Pais != null)
            {
                Proveedor.Pais = this.ComboItemRepository.GetByComboAndName(ComboType.Paises, Proveedor.Pais.Data);
            }
            
            this.ProveedorRepository.Add(Proveedor);
        }
        #endregion

        #region Rubro Compra
        public RubroCompra GetRubro(int Id)
        {
            RubroCompra Rubro = null;
            this.RubroCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Rubro = this.RubroCompraRepository.Get(Id);
                if (Rubro != null) {
                    Rubro.Organizacion = null;
                }
            });
            return Rubro;
        }

        public IList<RubroCompra> GetAllRubros()
        {
            IList<RubroCompra> Rubros = null;
            this.RubroCompraRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Rubros = this.RubroCompraRepository.GetAll();
            });
            return Rubros;
        }
        public void AddRubro(RubroCompra Rubro)
        {
            this.RubroCompraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                if (Rubro.PercepcionIIBB || Rubro.PercepcionIVA)
                {
                    Rubro.Cuenta = CuentaContableHelper.GetCuentaPercepcion();
                }
                else
                {
                    Rubro.Cuenta = CuentaContableHelper.GetCuentaRubroBasica(Rubro.CodigoPadre);
                }
                

                Rubro.Cuenta.Nombre = Rubro.Descripcion;
                this.CuentaRepository.Create(Rubro.Cuenta);

                this.RubroCompraRepository.Add(Rubro);
            });
        }
        public void UpdateRubro(RubroCompra Rubro)
        {
            this.RubroCompraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                RubroCompra ToUpdate = this.RubroCompraRepository.Get(Rubro.Id);
                if (ToUpdate.Descripcion != Rubro.Descripcion)
                {
                    ToUpdate.Cuenta = this.CuentaRepository.Get(ToUpdate.Cuenta.Id);
                    ToUpdate.Cuenta.Nombre = Rubro.Descripcion;
                    this.CuentaRepository.Update(ToUpdate.Cuenta);
                }
                ToUpdate.Descripcion = Rubro.Descripcion;

                this.RubroCompraRepository.Update(ToUpdate);
            });
        }
        public void DeleteRubros(List<int> Ids)
        {
            this.RubroCompraRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    RubroCompra Rubro = this.RubroCompraRepository.Get(Id);
                    Rubro.Activo = false;
                    this.RubroCompraRepository.Update(Rubro);
                }
            });
        }
        #endregion

        #region SelectCombos

        public SelectCombo GetRubrosCombo()
        {
            SelectCombo combo = new SelectCombo();
            combo.Items = new List<SelectComboItem>();
            this.ProveedorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                IList<RubroCompra> Rubros = this.RubroCompraRepository.GetAll();
                foreach (var Rubro in Rubros)
                {
                    SelectComboItem item = new SelectComboItem();
                    item.id = Rubro.Id;
                    item.text = Rubro.Descripcion;
                    item.selected = false;
                    combo.Items.Add(item);
                }
            });
            return combo;
        }
        public SelectCombo GetProveedorCombos()
        {
            SelectCombo SelectProveedorCombos = new SelectCombo();
            SelectProveedorCombos.Items = new List<SelectComboItem>();
            this.ProveedorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                IList<Proveedor> Proveedores = this.ProveedorRepository.GetAll();
                foreach (var Proveedor in Proveedores)
                {
                    SelectComboItem ProveedorItem = new SelectComboItem();
                    ProveedorItem.id = Proveedor.Id;
                    ProveedorItem.text = Proveedor.RazonSocial + '(' + Proveedor.CUIT + ')';
                    ProveedorItem.selected = false;
                    SelectProveedorCombos.Items.Add(ProveedorItem);
                }
            });
            return SelectProveedorCombos;
        }

        public SelectCombo GetAllProveedoresByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectServicioCombos = new SelectCombo();
            this.ProveedorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectServicioCombos.Items = this.ProveedorRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.RazonSocial + '(' + x.CUIT + ')'
                                                              }).ToList();
            });
            return SelectServicioCombos;
        }
        public SelectCombo GetAllRubrosByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectServicioCombos = new SelectCombo();
            this.ProveedorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectServicioCombos.Items = this.RubroCompraRepository.GetAllByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion
                                                              }).ToList();
            });
            return SelectServicioCombos;
        }

        public SelectCombo GetAllRubrosSinPerceByFilterCombo(SelectComboRequest req)
        {
            SelectCombo SelectServicioCombos = new SelectCombo();
            this.ProveedorRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                SelectServicioCombos.Items = this.RubroCompraRepository.GetAllSinPerceByFilter(req)
                                                              .Select(x => new SelectComboItem()
                                                              {
                                                                  id = x.Id,
                                                                  text = x.Descripcion
                                                              }).ToList();
            });
            return SelectServicioCombos;
        }
        #endregion


    }
}
