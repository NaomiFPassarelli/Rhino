using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.Services
{
    public interface ICooperativaConfigService
    {
        #region Asociado
        void AddAsociado(Asociado Asociado);
        void AddAsociadoNT(Asociado Asociado);
        Asociado GetAsociado(int Id);
        Asociado GetAsociadoCompleto(int Id);
        void UpdateAsociado(Asociado Asociado);
        IList<Asociado> GetAllAsociados();
        void DeleteAsociados(List<int> Ids);
        SelectCombo GetAllAsociadosByFilterCombo(SelectComboRequest req);
        SelectCombo GetAsociadoCombos();
        bool ExistCUITNT(string cuit, int? IdUpdate);
        void ImportAsociado(Asociado c);
        void ImportAsociadoNT(Asociado c);
        void ImportAsociados(List<Asociado> Asociados);
        IList<Asociado> GetAsociadosMes(int Mes, int Año);
        #endregion


    }
}
