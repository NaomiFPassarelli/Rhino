using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.NHMapping
{
    public class AsientoItemMap : ClassMap<AsientoItem>
    {
        public AsientoItemMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Debe).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Haber).Not.Nullable().Not.LazyLoad();
            this.References(c => c.ChequePropio).Nullable().Not.LazyLoad();
            this.References(c => c.Asiento).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Cuenta).Not.Nullable().Not.LazyLoad();
        }
    }
}
