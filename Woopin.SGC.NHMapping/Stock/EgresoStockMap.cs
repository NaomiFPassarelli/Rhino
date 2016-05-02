using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.NHMapping.Stock
{
    public class EgresoStockMap : ClassMap<EgresoStock>
    {
        public EgresoStockMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Cantidad).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Articulo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
            this.Map(c => c.Observacion).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
        }
    }
}
