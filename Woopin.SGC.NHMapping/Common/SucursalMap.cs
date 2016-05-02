using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class SucursalMap : ClassMap<Sucursal>
    {
        public SucursalMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Direccion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Email).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Telefono1).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Telefono2).Nullable().Not.LazyLoad();
            this.Map(c => c.Telefono3).Nullable().Not.LazyLoad();
            this.Map(c => c.Predeterminado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Localidad).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CodigoPostal).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
            this.References(c => c.Lugar).Not.Nullable().Not.LazyLoad();
        }
    }
}
