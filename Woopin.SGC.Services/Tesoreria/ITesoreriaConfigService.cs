using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Services
{
    public interface ITesoreriaConfigService
    {
        #region Banco
        void AddBanco(Banco Banco);
        Banco GetBanco(int Id);
        void UpdateBanco(Banco Banco);
        IList<Banco> GetAllBancos();
        void DeleteBancos(List<int> Ids);
        void CambiarActivo(List<int> Ids, bool Estado);
        #endregion

        #region CuentaBancaria
        void AddCuentaBancaria(CuentaBancaria CuentaBancaria);
        CuentaBancaria GetCuentaBancaria(int Id);
        void UpdateCuentaBancaria(CuentaBancaria CuentaBancaria);
        IList<CuentaBancaria> GetAllCuentasBancarias();
        IList<CuentaBancaria> GetAllEmiteCheque();
        void DeleteCuentasBancarias(List<int> Ids);
        void CambiarActivoCuentaBancaria(List<int> Ids, bool Estado);

        SelectCombo GetCuentaBancariaCombos();

        #endregion

        #region TarjetaCredito
        void AddTarjetaCredito(TarjetaCredito TarjetaCredito);
        TarjetaCredito GetTarjetaCredito(int Id);
        void UpdateTarjetaCredito(TarjetaCredito TarjetaCredito);
        IList<TarjetaCredito> GetAllTarjetaCreditos();
        void DeleteTarjetaCreditos(List<int> Ids);

        SelectCombo GetTarjetaCreditoCombos();
        #endregion

        #region Caja
        void AddCaja(Caja Caja);
        Caja GetCaja(int Id);
        void UpdateCaja(Caja Caja);
        IList<Caja> GetAllCajas();
        void DeleteCajas(List<int> Ids);
        #endregion

        #region Chequera
        void AddChequera(Chequera Chequera);
        Chequera GetChequera(int Id);
        void UpdateChequera(Chequera Chequera);
        IList<Chequera> GetAllChequeras();
        void DeleteChequeras(List<int> Ids);
        IList<ChequePropio> GetAllChequesInChequera(int IdChequera);

        void ControlChequePropioChequera(int IdCuentaBancaria, int Numero);
        #endregion 

        #region Valor
        void AddValor(Valor Valor);
        void AddValorNT(Valor Valor);
        Valor GetValor(int Id);
        void UpdateValor(Valor Valor);
        IList<Valor> GetAllValores();
        void DeleteValores(List<int> Ids);
        
        #endregion
    }
}
