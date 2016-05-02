using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.Services
{
    public interface ISueldosService
    {
        #region Recibo
        void AddRecibo(Recibo Recibo);
        void AddReciboNT(Recibo Recibo);
        Recibo GetRecibo(int Id);
        //void UpdateRecibo(Recibo Recibo);
        IList<Recibo> GetAllRecibos();
        //void DeleteRecibos(List<int> Ids);
        SelectCombo GetAllRecibosByFilterCombo(SelectComboRequest req);
        SelectCombo GetReciboCombos();
        int GetProximoNumeroReferencia();
        #endregion
    }
}
