using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.NHMapping.Sueldos
{
    public class AdicionalReciboMap : ClassMap<AdicionalRecibo>
    {
        public AdicionalReciboMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Adicional).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Unidades).Nullable().Not.LazyLoad();
            this.References(c => c.Recibo).Not.Nullable().Not.LazyLoad();//.Column("Recibo_Id").LazyLoad();
        }
    }
}
