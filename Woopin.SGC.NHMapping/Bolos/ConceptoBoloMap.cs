using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.NHMapping.Bolos
{
    public class ConceptoBoloMap : ClassMap<ConceptoBolo>
    {
        public ConceptoBoloMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.AdditionalDescription).Nullable().Not.LazyLoad();
            this.Map(c => c.Porcentaje).Nullable().Not.LazyLoad();
            this.Map(c => c.Valor).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Suma).Not.Nullable().Not.LazyLoad();
            //this.References(c => c.Cuenta).Not.Nullable().Not.LazyLoad();//TODO
            this.Map(c => c.OnlyAutomatic).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
        }
    }
}
