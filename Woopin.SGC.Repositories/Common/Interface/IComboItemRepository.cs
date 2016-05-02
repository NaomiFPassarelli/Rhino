using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Repositories.Common
{
    public interface IComboItemRepository : IRepository<ComboItem>
    {
        IList<ComboItem> GetItemsByComboId(int ComboId);

        ComboItem GetByComboAndName(ComboType comboType, string name);
    }
}
