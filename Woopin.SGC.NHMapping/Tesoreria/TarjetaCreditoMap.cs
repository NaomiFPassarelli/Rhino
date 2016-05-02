using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.NHMapping
{
    public class TarjetaCreditoMap : ClassMap<TarjetaCredito>
    {
        public TarjetaCreditoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Numero).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Estado).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CuentaBancaria).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CuentaContable).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
