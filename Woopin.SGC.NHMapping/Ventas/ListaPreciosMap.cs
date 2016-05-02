using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class ListaPreciosItemMap : ClassMap<ListaPreciosItem>
    {
        public ListaPreciosItemMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Precio).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Cliente).Nullable().Not.LazyLoad();
            this.References(c => c.Grupo).Nullable().Not.LazyLoad();
            this.References(c => c.Articulo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
