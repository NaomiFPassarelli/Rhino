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
    public class PagoTarjetaMap : ClassMap<PagoTarjeta>
    {
        public PagoTarjetaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCancelado).Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Nullable().Not.LazyLoad();
            this.Map(c => c.TotalCancelado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Estado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Cuotas).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CuotasCanceladas).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Detalle).Nullable().Not.LazyLoad();
            this.References(c => c.Tarjeta).Not.Nullable().Not.LazyLoad();
            this.HasMany(c => c.Cancelaciones).AsBag().KeyColumn("Pago_id");
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
