using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.NombreCompleto).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Username).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.LastLogin).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Email).Nullable().Not.LazyLoad();
            this.References(c => c.OrganizacionActual).Nullable().LazyLoad();
        }
    }
}
