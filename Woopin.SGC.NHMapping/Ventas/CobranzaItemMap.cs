using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class CobranzaItemMap : ClassMap<CobranzaItem>
    {
        public CobranzaItemMap()
        {
            this.Id(c => c.Id).Column("IdCobranzaItem").GeneratedBy.Identity();
            this.References(c => c.Comprobante).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CuentaBancaria).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Importe).Not.Nullable().Not.LazyLoad();
        }
    }
}
