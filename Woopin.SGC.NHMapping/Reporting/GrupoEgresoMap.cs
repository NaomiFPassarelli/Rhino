using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Reporting;

namespace Woopin.SGC.NHMapping
{
    public class GrupoEgresoMap : ClassMap<GrupoEgreso>
    {
        public GrupoEgresoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Raiz).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Level).Not.Nullable().Not.LazyLoad();
            this.References(c => c.NodoPadre).Nullable().Not.LazyLoad();
            this.References(c => c.Rubro).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Nullable().LazyLoad();
        }
    }
}
