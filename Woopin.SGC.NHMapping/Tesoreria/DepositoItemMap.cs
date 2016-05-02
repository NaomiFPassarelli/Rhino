using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.NHMapping
{
    public class DepositoItemMap : ClassMap<DepositoItem>
    {
        public DepositoItemMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.Cheque).Not.Nullable().Not.LazyLoad();
        }
    }
}
