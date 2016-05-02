using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class CobranzaValorItemMap : ClassMap<CobranzaValorItem>
    {
        public CobranzaValorItemMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.Valor).Not.Nullable().Not.LazyLoad().Cascade.SaveUpdate();  
        }
    }
}
