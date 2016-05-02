using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.NHMapping
{
    public class EjercicioMap : ClassMap<Ejercicio>
    {
        public EjercicioMap()
        {
            this.Id(c => c.Id).Column("Id").GeneratedBy.Identity();
            this.Map(c => c.Inicio).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Final).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Cerrado).Not.Nullable().Not.LazyLoad();
            this.HasMany(c => c.Bloqueos).AsBag().KeyColumn("Ejercicio_id");
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
