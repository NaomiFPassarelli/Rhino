using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class ComboItemOrganizacionMap : ClassMap<ComboItemOrganizacion>
    {
        public ComboItemOrganizacionMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Data).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.AdditionalData).Nullable().Not.LazyLoad();
            this.Map(c => c.AfipData).Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Combo).Not.Nullable().Not.LazyLoad();
        }
    }
}
