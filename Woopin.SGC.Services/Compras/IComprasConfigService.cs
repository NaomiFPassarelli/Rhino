using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Services
{
    public interface IComprasConfigService
    {
        #region Proveedor
        void AddProveedor(Proveedor Proveedor);
        Proveedor GetProveedor(int Id);
        void UpdateProveedor(Proveedor Proveedor);
        IList<Proveedor> GetAllProveedores();
        void DeleteProveedores(List<int> Ids);
        void CambiarActivo(List<int> Ids, bool Estado);
        bool ExistCUITNT(string cuit, int? IdUpdate);
        void ImportProveedor(Proveedor c);
        void ImportProveedorNT(Proveedor c);
        void ImportProveedores(List<Proveedor> proveedores);
        #endregion

        #region Rubro Compra
        void AddRubro(RubroCompra Rubro);
        RubroCompra GetRubro(int Id);
        void UpdateRubro(RubroCompra Rubro);
        IList<RubroCompra> GetAllRubros();
        void DeleteRubros(List<int> Ids);

        #endregion

        #region SelectCombo
        SelectCombo GetRubrosCombo();
        SelectCombo GetProveedorCombos();
        SelectCombo GetAllProveedoresByFilterCombo(SelectComboRequest req);
        SelectCombo GetAllRubrosByFilterCombo(SelectComboRequest req);
        SelectCombo GetAllRubrosSinPerceByFilterCombo(SelectComboRequest req);
        #endregion


    }
}
