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
    public interface IPagoRepository : IRepository<Pago>
    {
        IList<Pago> GetAllPagosByAsociado(int IdAsociado);
        int GetProximoNumeroReferencia();
        IList<Pago> GetAll(DateTime _start, DateTime _end);
        IList<Pago> GetPagos(IList<int> Ids);
        IList<Asociado> GetAllPorVencer();
        IList<Asociado> GetAllVencidos();
    }
}
