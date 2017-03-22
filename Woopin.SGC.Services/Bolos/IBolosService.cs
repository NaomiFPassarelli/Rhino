using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.Services
{
    public interface IBolosService
    {
        #region Liquidador
        void AddLiquidador(Liquidador Liquidador);
        IList<Liquidador> GetLiquidadores(IList<int> Ids);

        void AddLiquidadorNT(Liquidador Liquidador);
        Liquidador GetLiquidador(int Id);
        Liquidador GetLiquidadorCompleto(int Id);
        //void UpdateLiquidador(Liquidador Liquidador);
        //IList<Liquidador> GetAllLiquidadores(DateTime? start, DateTime? end);
        IList<Liquidador> GetAllLiquidadores();
        void DeleteLiquidadores(List<int> Ids);
        SelectCombo GetAllLiquidadoresByFilterCombo(SelectComboRequest req);
        SelectCombo GetLiquidadorCombos();
        int GetProximoNumeroReferencia();
        //Liquidador GetLiquidadorAnterior(int IdEmpleado);
        Liquidador GetLiquidadorAnterior();
        //decimal GetMejorRemuneracion(int IdEmpleado);
        //decimal[] GetPromedioRemunerativo(int IdEmpleado);

        void AnularLiquidador(int IdLiquidador);
        void EliminarLiquidador(int IdLiquidador);

        //IList<Liquidador> GetAllLiquidadoresByBolo(int Id, DateTime? start, DateTime? end, DateTime? startvenc, DateTime? endvenc, Model.Common.CuentaCorrienteFilter filter);
        IList<Liquidador> GetAllLiquidadoresByBolo(int Id, DateTime? start, DateTime? end);
        //IList<Liquidador> GetAllByBolo(int IdBolo, DateTime _start, DateTime _end, DateTime _startvenc, DateTime _endvenc, Model.Common.CuentaCorrienteFilter filter);


        #endregion
    }
}
