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
    public class ComprobanteRetencionMap : ClassMap<ComprobanteRetencion>
    {
        public ComprobanteRetencionMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Nullable().Not.LazyLoad();
            this.Map(c => c.Estado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroRetencion).Nullable().Not.LazyLoad();
            this.References(c => c.Usuario).Nullable().LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
            this.References(c => c.Retencion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Cliente).Nullable().Not.LazyLoad();
            this.References(c => c.Proveedor).Nullable().Not.LazyLoad();
        }
    }
}
