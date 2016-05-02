using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class ObservComprobanteVentaMap : ClassMap<ObservComprobanteVenta>
    {
        public ObservComprobanteVentaMap() 
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
        }
    }
}
