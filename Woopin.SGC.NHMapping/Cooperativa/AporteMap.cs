using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.NHMapping.Cooperativa
{
    public class AporteMap : ClassMap<Aporte>
    {
        public AporteMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.Asociado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.FechaAporte).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
            this.Map(c => c.FechaPeriodo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroAbono).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroReferencia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Observacion).Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
        }
    }
}
