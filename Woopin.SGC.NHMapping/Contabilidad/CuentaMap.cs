using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.NHMapping
{
    public class CuentaMap : ClassMap<Cuenta>
    {
        public CuentaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Codigo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Rubro).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Corriente).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.SubRubro).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
