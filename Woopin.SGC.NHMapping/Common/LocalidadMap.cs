using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class LocalidadMap : ClassMap<Localidad>
    {
        public LocalidadMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Provincia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Predeterminado).Not.Nullable().Not.LazyLoad();
        }
    }
}
