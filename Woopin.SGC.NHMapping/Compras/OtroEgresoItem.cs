using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.NHMapping
{
    public class OtroEgresoItemMap : ClassMap<OtroEgresoItem>
    {
        public OtroEgresoItemMap() 
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.RubroCompra).Not.Nullable().Not.LazyLoad();
        }
    }
}
