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
        IList<Recibo> GetRecibos(IList<int> Ids);

        void AddReciboNT(Recibo Recibo);
        Recibo GetRecibo(int Id);
        Recibo GetReciboCompleto(int Id);
        //void UpdateRecibo(Recibo Recibo);
        IList<Recibo> GetAllRecibos(DateTime? start, DateTime? end);
        void DeleteRecibos(List<int> Ids);
        SelectCombo GetAllRecibosByFilterCombo(SelectComboRequest req);
        SelectCombo GetReciboCombos();
        int GetProximoNumeroReferencia();
        Recibo GetReciboAnterior(int IdEmpleado);
        decimal GetMejorRemuneracion(int IdEmpleado);
        decimal[] GetPromedioRemunerativo(int IdEmpleado);
        #endregion
    }
}
