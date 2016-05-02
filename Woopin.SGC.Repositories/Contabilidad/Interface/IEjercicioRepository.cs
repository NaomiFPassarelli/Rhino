using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public interface IEjercicioRepository : IRepository<Ejercicio>
    {
        void ControlarIngreso(DateTime fechaContable);
        Ejercicio GetCompleto(int Id);
        IList<Ejercicio> GetAllAvailable();
        Ejercicio GetByDate(DateTime dateTime);
    }
}
