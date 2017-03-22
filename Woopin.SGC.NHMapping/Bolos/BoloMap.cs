using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.NHMapping.Bolos
{
    public class BoloMap : ClassMap<Bolo>
    {
        public BoloMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            //this.Map(c => c.NombreFantasia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Domicilio).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CodigoPostal).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Localizacion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.DenominacionProducto).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.DenominacionPelicula).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaLiquidacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TopeMinimo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TopeMaximo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Agencia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Anunciante).Not.Nullable().Not.LazyLoad();            
        }
    }
}
