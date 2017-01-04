using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Services
{
    public interface ICommonConfigService
    {
        #region Moneda
        void AddMoneda(Moneda moneda);
        Moneda GetMoneda(int Id);
        void UpdateMoneda(Moneda moneda);
        IList<Moneda> GetAllMonedas();
        void DeleteMonedas(List<int> Ids);
        void SetDefaultMoneda(int Id);
        #endregion

        #region Localizacion
        void AddLocalizacion(Localizacion Localizacion);
        Localizacion GetLocalizacion(int Id);
        Localizacion UpdateLocalizacion(Localizacion Localizacion);
        IList<Localizacion> GetAllLocalizaciones();
        IList<Localidad> GetAllLocalidades();
        void DeleteLocalizaciones(List<int> Ids);
        void SetDefaultLocalizacion(int Id);
        #endregion

        #region Sucursal
        void AddSucursal(Sucursal Sucursal);
        void DeleteSucursales(List<int> Ids);
        Sucursal GetSucursal(int Id);
        void UpdateSucursal(Sucursal Sucursal);
        IList<Sucursal> GetAllSucursales();
        void SetDefaultSucursal(int Id);
        #endregion

        #region CategoriaIVA
        void AddCategoriaIVA(CategoriaIVA CategoriaIVA);
        void DeleteCategoriaIVAs(List<int> Ids);
        CategoriaIVA GetCategoriaIVA(int Id);
        void UpdateCategoriaIVA(CategoriaIVA CategoriaIVA);
        IList<CategoriaIVA> GetAllCategoriaIVAs();
        void SetDefaultCategoriaIVA(int Id);
        #endregion

        #region Direccion
        void AddDireccion(Direccion Direccion);
        void DeleteDirecciones(List<int> Ids);
        Direccion GetDireccion(int Id);
        void UpdateDireccion(Direccion Direccion);
        IList<Direccion> GetAllDirecciones();
        void SetDefaultDireccion(int Id);
        #endregion



        #region Combo
        Combo GetCombo(int Id);
        IList<Combo> GetAllCombos();

        #endregion

        #region ComboItem
        void AddComboItem(ComboItem ComboItem);
        void DeleteCombosItems(List<int> Ids);
        ComboItem GetComboItem(int Id);
        void UpdateComboItem(ComboItem ComboItem);
        IList<ComboItem> GetAllCombosItems();
        IList<ComboItem> GetItemsByCombo(ComboType type);
        SelectCombo GetSelectItemsByComboId(ComboType ComboId);
        #endregion
    }
}
