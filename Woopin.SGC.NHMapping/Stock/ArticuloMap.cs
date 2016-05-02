using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Stock;

namespace Woopin.SGC.NHMapping.Stock
{
    public class ArticuloMap : ClassMap<Articulo>
    {
        public ArticuloMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CodigoBarras).Nullable().Not.LazyLoad();
            this.Map(c => c.Tipo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.AlicuotaIVA).Not.Nullable().Not.LazyLoad();
            this.References(c => c.UnidadMedida).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Rubro).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
            this.Map(c => c.Estado).Nullable().Not.LazyLoad();
            this.Map(c => c.Inventario).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Stock).Nullable().Not.LazyLoad();
        }
    }
}
