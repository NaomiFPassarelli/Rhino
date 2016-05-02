using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.NHMapping
{
    public class ValorIngresadoMap : ClassMap<ValorIngresado>
    {
        public ValorIngresadoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroOperacion).Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroReferencia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Importe).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TipoIngreso).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IdEntidadExt).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CuentaContable).Not.Nullable().LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
            this.References(c => c.Valor).Not.Nullable().Not.LazyLoad();
        }
    }
}
