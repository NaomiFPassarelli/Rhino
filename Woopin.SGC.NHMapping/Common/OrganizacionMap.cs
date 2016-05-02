using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class OrganizacionMap : ClassMap<Organizacion>
    {
        public OrganizacionMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.RazonSocial).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NombreFantasia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CUIT).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IngresosBrutos).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Email).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Telefono).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Domicilio).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CodigoPostal).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Provincia).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Actividad).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Categoria).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Administrador).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ImagePath).Nullable().Not.LazyLoad();
        }
    }
}
