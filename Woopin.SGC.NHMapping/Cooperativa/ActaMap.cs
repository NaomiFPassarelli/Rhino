using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.NHMapping.Cooperativa
{
    public class ActaMap : ClassMap<Acta>
    {
        public ActaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.FechaActa).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.HasMany(c => c.AsociadosEgreso).AsBag().KeyColumn("ActaEgreso_Id").Cascade.AllDeleteOrphan();
            this.HasMany(c => c.AsociadosIngreso).AsBag().KeyColumn("ActaIngreso_Id").Cascade.AllDeleteOrphan();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
            this.HasMany(c => c.OtrosPuntos).AsBag().KeyColumn("ActaOtrosPuntos_Id").Cascade.AllDeleteOrphan();
            this.Map(c => c.Presidente).Nullable().Not.LazyLoad();
            this.Map(c => c.Tesorero).Nullable().Not.LazyLoad();
            this.Map(c => c.Secretario).Nullable().Not.LazyLoad();
            this.Map(c => c.OtroPresente).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaFinalizacionActa).Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroActa).Not.Nullable().Not.LazyLoad();
        }
    }
}