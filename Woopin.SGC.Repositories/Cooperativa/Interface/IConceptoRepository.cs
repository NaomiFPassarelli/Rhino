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
    public interface IConceptoRepository : IRepository<Concepto>
    {
        IList<Concepto> GetAllByFilter(SelectComboRequest req);
        IList<Concepto> GetAllByFilter(PagingRequest req);
        Concepto Get(int IdConcepto);

    }
}
