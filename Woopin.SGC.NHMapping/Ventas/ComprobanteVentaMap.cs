using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class ComprobanteVentaMap : ClassMap<ComprobanteVenta>
    {
        public ComprobanteVentaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaVencimiento).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.MesPrestacion).Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Not.Nullable().Not.LazyLoad().UniqueKey("IDX_LetraNumero");
            this.Map(c => c.Cotizacion).Nullable().Not.LazyLoad();
            this.Map(c => c.MailCobro).Length(500).Nullable().Not.LazyLoad();
            this.Map(c => c.NombreCobro).Nullable().Not.LazyLoad();
            this.Map(c => c.Letra).Not.Nullable().Not.LazyLoad().UniqueKey("IDX_LetraNumero");
            this.Map(c => c.Estado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Subtotal).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA105).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA21).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA27).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ImporteExento).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ImporteNoGravado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TotalCobrado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Descuento).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Observacion).Nullable().Not.LazyLoad();
            this.Map(c => c.CAE).Nullable().Not.LazyLoad();
            this.Map(c => c.VencimientoCAE).Nullable().Not.LazyLoad();
            this.Map(c => c.EnviadoMail).Nullable().Not.LazyLoad();
            this.References(c => c.Cliente).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Tipo).Not.Nullable().Not.LazyLoad().UniqueKey("IDX_LetraNumero");
            this.References(c => c.CondicionVenta).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Asiento).Nullable().LazyLoad();
            this.References(c => c.Moneda).Nullable().Not.LazyLoad();
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            this.References(c => c.Talonario).Not.Nullable().LazyLoad();
            this.HasMany(c => c.Detalle).AsBag().KeyColumn("ComprobanteVenta_Id").Cascade.AllDeleteOrphan();
            this.HasMany(c => c.Observaciones).AsBag().KeyColumn("ComprobanteVenta_Id").Cascade.AllDeleteOrphan();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad().UniqueKey("IDX_LetraNumero");
        }
    }
}
