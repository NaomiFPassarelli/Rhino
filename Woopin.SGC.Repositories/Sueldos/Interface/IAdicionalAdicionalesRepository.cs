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
    public interface IAdicionalAdicionalesRepository : IRepository<AdicionalAdicionales>
    {
        //IList<AdicionalAdicionales> GetAllByFilter(SelectComboRequest req);
        //IList<AdicionalAdicionales> GetAllByFilter(PagingRequest req);
        IList<AdicionalAdicionales> GetByAdicional(int IdAdicional, bool ImportaSobreDefault);
        IList<AdicionalAdicionales> GetSobreByAdicional(int IdAdicional);

        void Delete(AdicionalAdicionales Adic);
    }
}