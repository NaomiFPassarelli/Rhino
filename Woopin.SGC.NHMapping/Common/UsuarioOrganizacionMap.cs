using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class UsuarioOrganizacionMap : ClassMap<UsuarioOrganizacion>
    {
        public UsuarioOrganizacionMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.Usuario).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Nullable().Not.LazyLoad();
        }
    }
}
