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
    public interface IBoloRepository : IRepository<Bolo>
    {
        IList<Bolo> GetAllByFilter(SelectComboRequest req);
        //IList<Bolo> GetAllByFilter(PagingRequest req);
        //Bolo GetCompleto(int IdBolo);
    }
}
