using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.NHMapping
{
    public class BloqueoContableMap : ClassMap<BloqueoContable>
    {
        public BloqueoContableMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Inicio).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Final).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Ejercicio).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
