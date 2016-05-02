using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.NHMapping.Sueldos
{
    public class AdicionalAdicionalesMap : ClassMap<AdicionalAdicionales>
    {
        public AdicionalAdicionalesMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            //this.References(c => c.Recibo).Nullable().Column("Recibo_Id").LazyLoad();
            //this.References(c => c.Adicional).Nullable().Column("Adicional_Id").LazyLoad();
            this.References(c => c.Recibo).Nullable().Not.LazyLoad();
            this.References(c => c.Adicional).Not.Nullable().Not.LazyLoad();
            this.References(c => c.AdicionalSobre).Not.Nullable().Column("AdicionalSobre_Id").LazyLoad();
            //this.HasMany(c => c.Adicionales).AsBag().KeyColumn("AdicionalAdicionales_Id").Cascade.AllDeleteOrphan();
            this.Map(c => c.EsDefault).Not.Nullable().Not.LazyLoad();
        }
    }
}
