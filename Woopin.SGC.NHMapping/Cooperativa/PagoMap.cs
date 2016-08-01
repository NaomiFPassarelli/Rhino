using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.NHMapping.Cooperativa
{
    public class PagoMap : ClassMap<Pago>
    {
        public PagoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.Asociado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaPago).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad(); 
        }
    }
}
