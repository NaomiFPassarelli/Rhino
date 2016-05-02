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
    public interface IEmpleadoRepository : IRepository<Empleado>
    {
        IList<Empleado> GetAllByFilter(SelectComboRequest req);
        IList<Empleado> GetAllByFilter(PagingRequest req);
        bool ExistCUIT(string cuit, int? IdUpdate);
    }
}
