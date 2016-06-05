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
    public interface IAdicionalReciboRepository : IRepository<AdicionalRecibo>
    {
        IList<AdicionalRecibo> GetAllByFilter(SelectComboRequest req);
        IList<AdicionalRecibo> GetAllByFilter(PagingRequest req);
    }
}
