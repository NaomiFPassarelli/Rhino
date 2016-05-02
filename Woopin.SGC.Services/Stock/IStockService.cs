using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.Services
{
    public interface IStockService
    {
        #region IngresoStock

        void AddIngresoStock(IngresoStock IngresoStock);
        //void AddIngresoStockNT(IngresoStock IngresoStock);
        IngresoStock GetIngresoStock(int Id);
        IList<IngresoStock> GetAllIngresosStock(int Id, DateTime? start, DateTime? end);
        //SelectCombo GetAllIngresosStockByFilterCombo(SelectComboRequest req);
        
        #endregion

        #region EgresoStock

        void AddEgresoStock(EgresoStock EgresoStock);
        //void AddEgresoStockNT(EgresoStock EgresoStock);
        EgresoStock GetEgresoStock(int Id);
        IList<EgresoStock> GetAllEgresosStock(int Id, DateTime? start, DateTime? end);
        //SelectCombo GetAllEgresosStockByFilterCombo(SelectComboRequest req);
        
        #endregion
        
    }
}
