using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Repositories.Common;

namespace Woopin.SGC.Services
{
    public class CommonConfigService : ICommonConfigService
    {
        #region VariablesyConstructor
        private readonly ISucursalRepository sucursalRepository;
        private readonly ILocalizacionRepository localizacionRepository;
        private readonly ILocalidadRepository LocalidadRepository;
        private readonly IMonedaRepository monedaRepository;
        private readonly ICategoriaIVARepository categoriaIVARepository;
        private readonly IDireccionRepository DireccionRepository;
        private readonly IComboRepository comboRepository;
        private readonly IComboItemRepository comboItemRepository;
        public CommonConfigService(ISucursalRepository sucursalRepository, ILocalizacionRepository localizacionRepository, 
            IMonedaRepository monedaRepository, ICategoriaIVARepository categoriaIVARepository,
            IComboRepository comboRepository, IComboItemRepository comboItemRepository,
            ILocalidadRepository localidadRepository, IDireccionRepository DireccionRepository)
        {
            this.sucursalRepository = sucursalRepository;
            this.localizacionRepository = localizacionRepository;
            this.monedaRepository = monedaRepository;
            this.categoriaIVARepository = categoriaIVARepository;
            this.comboRepository = comboRepository;
            this.LocalidadRepository = localidadRepository;
            this.comboItemRepository = comboItemRepository;
            this.DireccionRepository = DireccionRepository;
        }
        #endregion

        #region Sucursal
        public Sucursal GetSucursal(int Id)
        {
            Sucursal Sucursal = null;
            this.sucursalRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Sucursal = this.sucursalRepository.Get(Id);
            });
            return Sucursal;
        }

        public IList<Sucursal> GetAllSucursales()
        {
            IList<Sucursal> Sucursales = null;
            this.sucursalRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Sucursales = this.sucursalRepository.GetAll();
            });
            return Sucursales;
        }
        public void AddSucursal(Sucursal Sucursal)
        {
            this.sucursalRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.sucursalRepository.Add(Sucursal);
            });
        }
        public void UpdateSucursal(Sucursal Sucursal)
        {
            this.sucursalRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                Sucursal sucursalToUpdate = this.sucursalRepository.Get(Sucursal.Id);
                sucursalToUpdate.CodigoPostal = Sucursal.CodigoPostal;
                sucursalToUpdate.Direccion = Sucursal.Direccion;
                sucursalToUpdate.Email = Sucursal.Email;
                sucursalToUpdate.Localidad = Sucursal.Localidad;
                sucursalToUpdate.Lugar = Sucursal.Lugar;
                sucursalToUpdate.Nombre = Sucursal.Nombre;
                sucursalToUpdate.Telefono1 = Sucursal.Telefono1;
                sucursalToUpdate.Telefono2 = Sucursal.Telefono2;
                sucursalToUpdate.Telefono3 = Sucursal.Telefono3;
                this.sucursalRepository.Update(sucursalToUpdate);
            });
        }
        public void SetDefaultSucursal(int Id)
        {
            this.sucursalRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.sucursalRepository.SetDefault(Id);
            });
        }
        public void DeleteSucursales(List<int> Ids)
        {
            this.sucursalRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Sucursal sucursal = this.sucursalRepository.Get(Id);
                    sucursal.Activo = false;
                    this.sucursalRepository.Update(sucursal);
                }
            });
        }
        #endregion

        #region Localizacion
        public Localizacion GetLocalizacion(int Id)
        {
            Localizacion Localizacion = null;
            this.localizacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Localizacion = this.localizacionRepository.Get(Id);
            });
            return Localizacion;
        }
        public void DeleteLocalizaciones(List<int> Ids)
        {
            this.localizacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Localizacion localizacion = this.localizacionRepository.Get(Id);
                    localizacion.Activo = false;
                    this.localizacionRepository.Update(localizacion);
                }
            });
        }
        public IList<Localizacion> GetAllLocalizaciones()
        {
            IList<Localizacion> Localizaciones = null;
            this.localizacionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Localizaciones = this.localizacionRepository.GetAll();
            });
            return Localizaciones;
        }
        public void AddLocalizacion(Localizacion Localizacion)
        {
            this.localizacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.localizacionRepository.Add(Localizacion);
            });
        }
        public Localizacion UpdateLocalizacion(Localizacion Localizacion)
        {
            Localizacion ToUpdate = new Localizacion();
            this.localizacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ToUpdate = this.localizacionRepository.Get(Localizacion.Id);
                ToUpdate.Nombre = Localizacion.Nombre;
                ToUpdate.Provincia = Localizacion.Provincia;
                ToUpdate.Pais = Localizacion.Pais;
                this.localizacionRepository.Update(ToUpdate);
            });
            return ToUpdate;
        }
        public void SetDefaultLocalizacion(int Id)
        {
            this.localizacionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.localizacionRepository.SetDefault(Id);
            });
        }
        #endregion

        #region Localidad
        public Localidad GetLocalidad(int Id)
        {
            Localidad Localidad = null;
            this.LocalidadRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Localidad = this.LocalidadRepository.Get(Id);
            });
            return Localidad;
        }
        public void DeleteLocalidades(List<int> Ids)
        {
            this.LocalidadRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Localidad Localidad = this.LocalidadRepository.Get(Id);
                    Localidad.Activo = false;
                    this.LocalidadRepository.Update(Localidad);
                }
            });
        }
        public IList<Localidad> GetAllLocalidades()
        {
            IList<Localidad> Localidades = null;
            this.LocalidadRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Localidades = this.LocalidadRepository.GetAll();
            });
            return Localidades;
        }
        public void AddLocalidad(Localidad Localidad)
        {
            this.LocalidadRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.LocalidadRepository.Add(Localidad);
            });
        }
        public Localidad UpdateLocalidad(Localidad Localidad)
        {
            Localidad ToUpdate = new Localidad();
            this.LocalidadRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                ToUpdate = this.LocalidadRepository.Get(Localidad.Id);
                ToUpdate.Nombre = Localidad.Nombre;
                ToUpdate.Provincia = Localidad.Provincia;
                this.LocalidadRepository.Update(ToUpdate);
            });
            return ToUpdate;
        }
        public void SetDefaultLocalidad(int Id)
        {
            this.LocalidadRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.LocalidadRepository.SetDefault(Id);
            });
        }
        #endregion


        #region Moneda
        public Moneda GetMoneda(int Id)
        {
            Moneda Moneda = null;
            this.monedaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Moneda = this.monedaRepository.Get(Id);
            });
            return Moneda;
        }
        public IList<Moneda> GetAllMonedas()
        {
            IList<Moneda> Monedaes = null;
            this.monedaRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Monedaes = this.monedaRepository.GetAll();
            });
            return Monedaes;
        }
        public void AddMoneda(Moneda Moneda)
        {
            this.monedaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.monedaRepository.Add(Moneda);
            });
        }
        public void UpdateMoneda(Moneda Moneda)
        {
            this.monedaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.monedaRepository.Update(Moneda);
            });
        }
        public void DeleteMonedas(List<int> Ids)
        {
            this.monedaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Moneda moneda = this.monedaRepository.Get(Id);
                    moneda.Activo = false;
                    this.monedaRepository.Update(moneda);
                }
            });
        }
        public void SetDefaultMoneda(int Id)
        {
            this.monedaRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.monedaRepository.SetDefaultMoneda(Id);
            });
        }

        #endregion

        #region Categorias de IVA
        public CategoriaIVA GetCategoriaIVA(int Id)
        {
            CategoriaIVA CategoriaIVA = null;
            this.categoriaIVARepository.GetSessionFactory().SessionInterceptor(() =>
            {
                CategoriaIVA = this.categoriaIVARepository.Get(Id);
            });
            return CategoriaIVA;
        }

        public IList<CategoriaIVA> GetAllCategoriaIVAs()
        {
            IList<CategoriaIVA> CategoriaIVAs = null;
            this.categoriaIVARepository.GetSessionFactory().SessionInterceptor(() =>
            {
                CategoriaIVAs = this.categoriaIVARepository.GetAll();
            });
            return CategoriaIVAs;
        }
        public void AddCategoriaIVA(CategoriaIVA CategoriaIVA)
        {
            this.categoriaIVARepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.categoriaIVARepository.Add(CategoriaIVA);
            });
        }
        public void UpdateCategoriaIVA(CategoriaIVA CategoriaIVA)
        {
            this.categoriaIVARepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.categoriaIVARepository.Update(CategoriaIVA);
            });
        }
        public void SetDefaultCategoriaIVA(int Id)
        {
            this.categoriaIVARepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.categoriaIVARepository.SetDefault(Id);
            });
        }
        public void DeleteCategoriaIVAs(List<int> Ids)
        {
            this.categoriaIVARepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    CategoriaIVA CategoriaIVA = this.categoriaIVARepository.Get(Id);
                    CategoriaIVA.Activo = false;
                    this.categoriaIVARepository.Update(CategoriaIVA);
                }
            });
        }
        #endregion


        #region Direccion
        public Direccion GetDireccion(int Id)
        {
            Direccion Direccion = null;
            this.DireccionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Direccion = this.DireccionRepository.Get(Id);
            });
            return Direccion;
        }

        public IList<Direccion> GetAllDirecciones()
        {
            IList<Direccion> Direcciones = null;
            this.DireccionRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Direcciones = this.DireccionRepository.GetAll();
            });
            return Direcciones;
        }
        public void AddDireccion(Direccion Direccion)
        {
            this.DireccionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.DireccionRepository.Add(Direccion);
            });
        }
        public void UpdateDireccion(Direccion Direccion)
        {
            this.DireccionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.DireccionRepository.Update(Direccion);
            });
        }
        public void SetDefaultDireccion(int Id)
        {
            this.DireccionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.DireccionRepository.SetDefault(Id);
            });
        }
        public void DeleteDirecciones(List<int> Ids)
        {
            this.DireccionRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    Direccion Direccion = this.DireccionRepository.Get(Id);
                    this.DireccionRepository.Update(Direccion);
                }
            });
        }
        #endregion




        #region Combo
        public Combo GetCombo(int Id)
        {
            Combo Combo = null;
            this.comboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Combo = this.comboRepository.Get(Id);
            });
            return Combo;
        }

        public IList<Combo> GetAllCombos()
        {
            IList<Combo> Combos = null;
            this.comboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                Combos = this.comboRepository.GetAll();
            });
            return Combos;
        }
        
        #endregion

        #region Combos Items
        public ComboItem GetComboItem(int Id)
        {
            ComboItem ComboItem = null;
            this.comboItemRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                ComboItem = this.comboItemRepository.Get(Id);
            });
            return ComboItem;
        }

        public IList<ComboItem> GetAllCombosItems()
        {
            IList<ComboItem> CombosItems = null;
            this.comboItemRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                CombosItems = this.comboItemRepository.GetAll();
            });
            return CombosItems;
        }
        public void AddComboItem(ComboItem ComboItem)
        {
            this.comboItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.comboItemRepository.Add(ComboItem);
            });
        }
        public void UpdateComboItem(ComboItem ComboItem)
        {
            this.comboItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                this.comboItemRepository.Update(ComboItem);
            });
        }
        public void DeleteCombosItems(List<int> Ids)
        {
            this.comboItemRepository.GetSessionFactory().TransactionalInterceptor(() =>
            {
                foreach (var Id in Ids)
                {
                    ComboItem ComboItem = this.comboItemRepository.Get(Id);
                    ComboItem.Activo = false;
                    this.comboItemRepository.Update(ComboItem);
                }
            });
        }

        public IList<ComboItem> GetItemsByCombo(ComboType type)
        {
            IList<ComboItem> CombosItems = null;
            this.comboRepository.GetSessionFactory().SessionInterceptor(() =>
            {
                CombosItems = this.comboItemRepository.GetItemsByComboId((int)type);
            });
            return CombosItems;
        }

        public SelectCombo GetSelectItemsByComboId(ComboType type)
        {
            SelectCombo SelectCombo = new SelectCombo();
            SelectCombo.Items = new List<SelectComboItem>();
            this.comboItemRepository.GetSessionFactory().SessionInterceptor(() =>
               {
                   IList<ComboItem> CombosItems = this.comboItemRepository.GetItemsByComboId((int)type);
                   foreach (var ComboItem in CombosItems)
                   {
                       SelectComboItem SelectItem = new SelectComboItem();
                       SelectItem.id = ComboItem.Id;
                       SelectItem.text = ComboItem.Data;
                       SelectItem.additionalData = ComboItem.AdditionalData;
                       SelectCombo.Items.Add(SelectItem);
                   }
               });
            
            return SelectCombo;
        }

        #endregion
    }
}
