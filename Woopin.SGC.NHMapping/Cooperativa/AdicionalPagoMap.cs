using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.NHMapping.Cooperativa
{
    public class AdicionalPagoMap : ClassMap<AdicionalPago>
    {
        public AdicionalPagoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Concepto).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Unidades).Nullable().Not.LazyLoad();
            this.References(c => c.Pago).Not.Nullable().Not.LazyLoad();//.Column("Recibo_Id").LazyLoad();
        }
    }
}
