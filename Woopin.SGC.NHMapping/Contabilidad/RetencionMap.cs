using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.NHMapping
{
    public class RetencionMap : ClassMap<Retencion>
    {
        public RetencionMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Abreviatura).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.EsValor).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CuentaContable).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Juridiccion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CuentaADepositar).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
