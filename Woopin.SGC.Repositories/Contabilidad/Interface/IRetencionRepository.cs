using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Contabilidad
{
    public interface IRetencionRepository : IRepository<Retencion>
    {
        IList<Retencion> GetAllValor();
        IList<Retencion> GetAllByFilter(SelectComboRequest req);
        IList<Retencion> GetAllByFilter(PagingRequest req);
    }
}
