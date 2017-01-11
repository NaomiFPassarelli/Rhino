using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class OrganizacionModuloMap : ClassMap<OrganizacionModulo>
    {
        public OrganizacionModuloMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.ModulosSistemaGestion).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Nullable().Not.LazyLoad();
        }
    }
}
