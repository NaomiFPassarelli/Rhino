using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.Repositories.Bolos
{
    public interface ITrabajadorRepository : IRepository<Trabajador>
    {
        IList<Trabajador> GetAllByFilter(SelectComboRequest req);
        int GetProximoNumeroReferencia();
        IList<Trabajador> GetAllByFilter(PagingRequest req);
        bool ExistCUIT(string cuit, int? IdUpdate);
        Trabajador GetCompleto(int IdTrabajador);
    }
}
