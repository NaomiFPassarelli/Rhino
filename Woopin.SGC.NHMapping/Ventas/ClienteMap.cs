using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class ClienteMap : ClassMap<Cliente>
    {
        public ClienteMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.RazonSocial).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CUIT).Not.Nullable().Not.LazyLoad().UniqueKey("UX_CUIT");
            this.Map(c => c.Direccion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Departamento).Nullable().Not.LazyLoad();
            this.Map(c => c.Piso).Nullable().Not.LazyLoad();
            this.Map(c => c.CodigoPostal).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Localidad).Not.Nullable().Not.LazyLoad();  
            this.Map(c => c.Telefono).Nullable().Not.LazyLoad();
            this.Map(c => c.Email).Nullable().Not.LazyLoad();
            this.Map(c => c.CondicionVentaContratada).Nullable().Not.LazyLoad();
            this.Map(c => c.CondicionVentaEstadistica).Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CategoriaIva).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Master).Nullable().Not.LazyLoad();
            this.References(c => c.CondicionVenta).Nullable().Not.LazyLoad();
            this.References(c => c.Localizacion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad().UniqueKey("UX_CUIT");
        }
    }
}
