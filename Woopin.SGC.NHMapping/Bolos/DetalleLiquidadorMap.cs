using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.NHMapping.Bolos
{
    public class DetalleLiquidadorMap : ClassMap<DetalleLiquidador>
    {
        public DetalleLiquidadorMap() 
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.IVA).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Liquidador).Column("Liquidador_Id").LazyLoad();
            //this.References(c => c.TipoIva).Not.Nullable().Not.LazyLoad();
            this.References(c => c.ConceptoBolo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
        }
    }
}
