using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.NHMapping
{
    public class OrdenPagoComprobanteItemMap : ClassMap<OrdenPagoComprobanteItem>
    {
        public OrdenPagoComprobanteItemMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Importe).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Pagado).Not.Nullable().Not.LazyLoad();
            this.References(c => c.ComprobanteCompra).Not.Nullable().Not.LazyLoad();
        }
    }
}
