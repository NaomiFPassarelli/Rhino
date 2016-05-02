using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class CobranzaMap : ClassMap<Cobranza>
    {
        public CobranzaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Numero).Not.Nullable().Not.LazyLoad().UniqueKey("UX_Numero");
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Detalle).Nullable().Not.LazyLoad();
            this.Map(c => c.Estado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Asiento).Nullable().LazyLoad();
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad().UniqueKey("UX_Numero");
            this.References(c => c.Tipo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Cliente).Not.Nullable().Not.LazyLoad();
            this.HasMany(c => c.Comprobantes).AsBag().KeyColumn("Cobranza_id").Cascade.AllDeleteOrphan();
            this.HasMany(c => c.Valores).AsBag().KeyColumn("Cobranza_id").Cascade.AllDeleteOrphan();
        }
    }
}
