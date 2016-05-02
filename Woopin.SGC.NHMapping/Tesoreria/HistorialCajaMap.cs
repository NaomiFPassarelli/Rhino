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
    public class HistorialCajaMap : ClassMap<HistorialCaja>
    {
        public HistorialCajaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.Caja).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Concepto).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Importe).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Saldo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
