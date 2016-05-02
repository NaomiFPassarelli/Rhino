using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.NHMapping
{
    public class AsientoMap : ClassMap<Asiento>
    {
        public AsientoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Leyenda).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Modulo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Manualizado).Nullable().Not.LazyLoad();
            this.Map(c => c.TipoOperacion).Nullable().Not.LazyLoad();
            this.Map(c => c.ComprobanteAsociado).Nullable().Not.LazyLoad(); 
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            this.References(c => c.Ejercicio).Not.Nullable().Not.LazyLoad();
            this.HasMany(c => c.Items).AsBag().KeyColumn("Asiento_id").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
