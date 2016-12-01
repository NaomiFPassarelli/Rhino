using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.Repositories.Cooperativa
{
    public interface IAdicionalPagoRepository : IRepository<AdicionalPago>
    {
        IList<AdicionalPago> GetAllByFilter(SelectComboRequest req);
        IList<AdicionalPago> GetAllByFilter(PagingRequest req);
        //IList<AdicionalPago> GetAdicionalesDelPeriodoByEmpleado(string Periodo, int IdEmpleado);

        IList<AdicionalPago> GetAllAdicionalesByPago(int PagoId);
    }
}
