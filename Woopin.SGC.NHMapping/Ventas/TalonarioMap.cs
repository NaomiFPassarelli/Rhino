using System;
using FluentNHibernate.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class TalonarioMap : ClassMap<Talonario>
    {
        public TalonarioMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Prefijo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.InicioActividad).Nullable().Not.LazyLoad();
            this.Map(c => c.PuntoVenta).Nullable().Not.LazyLoad();
            this.Map(c => c.CertificadoPath).Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
