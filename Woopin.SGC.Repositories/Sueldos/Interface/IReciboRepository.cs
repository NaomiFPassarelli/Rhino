using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.Repositories.Sueldos
{
    public interface IReciboRepository : IRepository<Recibo>
    {
        IList<Recibo> GetAllByFilter(SelectComboRequest req);
        int GetProximoNumeroReferencia();
        IList<Recibo> GetAllByFilter(PagingRequest req);
        Recibo GetCompleto(int Id);
        Recibo GetReciboAnterior(int IdEmpleado);
        IList<Recibo> GetRecibos(IList<int> Ids);

        decimal GetMejorRemuneracion(int IdEmpleado);
        decimal[] GetPromedioRemunerativo(int IdEmpleado);
    }
}
