using System;
using FluentNHibernate.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.NHMapping
{
    public class RubroCompraMap : ClassMap<RubroCompra>
    {
        public RubroCompraMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.PercepcionIIBB).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.PercepcionIVA).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Cuenta).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
