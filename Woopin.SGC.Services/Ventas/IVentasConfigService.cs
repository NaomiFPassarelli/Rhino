using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Services
{
    public interface IVentasConfigService
    {
        #region Cliente
        void AddCliente(Cliente Cliente);
        Cliente GetCliente(int Id);
        void UpdateCliente(Cliente Cliente);
        IList<Cliente> GetAllClientes();
        IList<Cliente> GetAllClientesByFilter(PagingRequest req);
 
        void DeleteClientes(List<int> Ids);
        void CambiarActivo(List<int> Ids, bool Estado);
        bool ExistCUITNT(string cuit, int? IdUpdate);

        /// <summary>
        /// Importa desde un listado de clientes
        /// </summary>
        /// <param name="clientes">Listado de clientes</param>
        void ImportClientes(List<Cliente> clientes);

        /// <summary>
        /// Importa un contacto, con solo información, sin Id.
        /// </summary>
        /// <param name="clientes">Listado de clientes</param>
        void ImportCliente(Cliente cliente);
        #endregion

        #region SelectCombo
        SelectCombo GetClienteCombos();
        SelectCombo GetAllClientesByFilterCombo(SelectComboRequest req);
        SelectCombo GetAllGruposByFilterCombo(SelectComboRequest req);
        #endregion

        #region GrupoEconomico
        void AddGrupoEconomico(GrupoEconomico GrupoEconomico);
        GrupoEconomico GetGrupoEconomico(int Id);
        void UpdateGrupoEconomico(GrupoEconomico GrupoEconomico);
        IList<GrupoEconomico> GetAllGrupoEconomicos();
        void DeleteGrupoEconomico(List<int> Ids);
        SelectCombo GetGrupoEconomicoCombos();
        #endregion

        #region Listado de Precios de Ventas
        ListaPreciosItem GetPrecioForArticulo(int IdArticulo, int IdCliente);
        ListaPreciosItem GetListaPreciosItem(int Id);
        IList<ListaPreciosItem> GetAllPreciosById(string Id);
        void SaveListaPrecios(ListaPreciosItem ListaPreciosItem, string IdCliente);
        #endregion

        #region Talonarios
        void AddTalonario(Talonario Talonario);
        Talonario GetTalonario(int Id);
        void UpdateTalonario(Talonario Talonario);
        IList<Talonario> GetAllTalonarios();
        void DeleteTalonario(List<int> Ids);
        #endregion


        
    }
}
