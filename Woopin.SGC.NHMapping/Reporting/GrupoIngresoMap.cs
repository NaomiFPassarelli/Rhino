using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Reporting;

namespace Woopin.SGC.NHMapping
{
    public class GrupoIngresoMap : ClassMap<GrupoIngreso>
    {
        public GrupoIngresoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Raiz).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Level).Not.Nullable().Not.LazyLoad();
            this.References(c => c.NodoPadre).Nullable().Not.LazyLoad();
            this.References(c => c.Articulo).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
