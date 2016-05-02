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
    public class ChequeraMap : ClassMap<Chequera>
    {
        public ChequeraMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.CuentaBancaria).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroDesde).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroHasta).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
