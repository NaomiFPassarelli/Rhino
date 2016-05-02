using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public interface ICuentaRepository : IRepository<Cuenta>
    {
        void Create(Cuenta c);
        IList<Cuenta> GetRubros();
        IList<Cuenta> GetCorriente(int Rubro);
        IList<Cuenta> GetSubRubros(int Rubro, int Corriente);
        IList<Cuenta> GetCuentasdeIngresos();
        IList<Cuenta> GetCuentasdeEgresos();
        IList<Cuenta> GetSubRubrosEgresos();
        Cuenta GetCuentaByCodigo(string Codigo);
        IList<Cuenta> GetAllByFilter(SelectComboRequest req);
    }
}
