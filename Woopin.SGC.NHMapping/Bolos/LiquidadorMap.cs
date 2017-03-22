using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.NHMapping.Bolos
{
    public class LiquidadorMap : ClassMap<Liquidador>
    {
        public LiquidadorMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            //this.References(c => c.Adicionales).Nullable().Not.LazyLoad();
            this.References(c => c.Trabajador).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Bolo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaDesde).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaHasta).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Periodo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Subtotal).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Observacion).Nullable().Not.LazyLoad();
            //this.Map(c => c.NumeroReferencia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.DiasNormales).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.HasMany(c => c.Detalle).AsBag().KeyColumn("Liquidador_Id").Cascade.AllDeleteOrphan();
            this.Map(c => c.FechaUltimoDeposito).Not.Nullable().Not.LazyLoad();
        }
    }
}
