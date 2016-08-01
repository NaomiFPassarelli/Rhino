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
    public interface ICooperativaService
    {
        #region Pago
        void AddPagos(Pago Pago, int CantidadCuotasAPagar);
        void AddPago(Pago Pago);
        //void AddPagoNT(Pago Pago);
        Pago GetPago(int Id);
        IList<Pago> GetAllPagos();
        //void DeletePagos(List<int> Ids);
        //SelectCombo GetAllPagosByFilterCombo(SelectComboRequest req);
        //SelectCombo GetPagoCombos();
        IList<Pago> GetAllPagosByAsociado(int IdAsociado);

        #endregion

    }
}
