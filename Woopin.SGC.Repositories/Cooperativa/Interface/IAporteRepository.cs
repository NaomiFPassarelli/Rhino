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
    public interface IAporteRepository : IRepository<Aporte>
    {
        IList<Aporte> GetAllAportesByAsociado(int IdAsociado);
        int GetProximoNumeroReferencia();
        DateTime GetProximoPeriodo(int IdAsociado);
        IList<Aporte> GetAll(DateTime _start, DateTime _end);
        //IList<Asociado> GetAllPorVencer();
        //IList<Asociado> GetAllVencidos();
    }
}
