using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class CategoriaIVAMap : ClassMap<CategoriaIVA>
    {
        public CategoriaIVAMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Abreviatura).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Discrimina).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.LiquidaInternos).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ExentoIva).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ResponsabilidadAfip).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Predeterminado).Not.Nullable().Not.LazyLoad();
            this.References(c => c.LetraCompras).Not.Nullable().Not.LazyLoad();
            this.References(c => c.LetraVentas).Not.Nullable().Not.LazyLoad();
        }
    }
}
