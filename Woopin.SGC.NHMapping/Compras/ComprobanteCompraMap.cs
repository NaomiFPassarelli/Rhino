using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.NHMapping
{
    public class ComprobanteCompraMap : ClassMap<ComprobanteCompra>
    {
        public ComprobanteCompraMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaContable).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaVencimiento).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Letra).Not.Nullable().Not.LazyLoad().Index("IDX_LetraNumero");
            this.Map(c => c.Numero).Not.Nullable().Not.LazyLoad().Index("IDX_LetraNumero");
            this.Map(c => c.Estado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA105).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA21).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA27).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ImporteExento).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ImporteNoGravado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Observacion).Nullable().Not.LazyLoad();
            this.Map(c => c.Subtotal).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TotalPagado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.PercepcionesIIBB).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.PercepcionesIVA).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Proveedor).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Tipo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CondicionCompra).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Asiento).Nullable().LazyLoad();
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad().Index("IDX_LetraNumero");
            this.HasMany(c => c.Detalle).AsBag().KeyColumn("ComprobanteComra_Id").Cascade.AllDeleteOrphan();
        }
    }
}
