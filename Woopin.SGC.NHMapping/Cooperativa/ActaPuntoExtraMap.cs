using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.NHMapping.Cooperativa
{
    public class ActaPuntoExtraMap : ClassMap<ActaPuntoExtra>
    {
        public ActaPuntoExtraMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Encabezado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Detalle).Nullable().Not.LazyLoad();
            this.References(c => c.Acta).Column("Acta_Id").LazyLoad();
        }
    }
}