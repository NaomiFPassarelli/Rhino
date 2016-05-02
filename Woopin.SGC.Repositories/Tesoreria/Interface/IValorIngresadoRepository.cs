using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Repositories.Tesoreria
{
    public interface IValorIngresadoRepository : IRepository<ValorIngresado>
    {
        IList<ReporteTesoreria> GetIngresosByDates(DateTime start, DateTime end);
        IList<ReporteTesoreria> GetEgresosByDates(DateTime start, DateTime end);
        ValorIngresado GetByTipo(int NroReferencia, string TipoValor);
    }
}
