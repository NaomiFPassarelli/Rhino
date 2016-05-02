using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class CobranzaComprobanteItemMap : ClassMap<CobranzaComprobanteItem>
    {
        public CobranzaComprobanteItemMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Importe).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Cobrado).Not.Nullable().Not.LazyLoad();
            this.References(c => c.ComprobanteVenta).Not.Nullable().Not.LazyLoad();
        }
    }
}
