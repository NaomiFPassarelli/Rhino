using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.NHMapping
{
    public class DireccionMap : ClassMap<Direccion>
    {
        public DireccionMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Calle).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CodigoPostal).Nullable().Not.LazyLoad();
            this.Map(c => c.Departamento).Nullable().Not.LazyLoad();
            this.Map(c => c.Email).Nullable().Not.LazyLoad();
            //this.References(c => c.Localidad).Nullable().Not.LazyLoad();
            //this.References(c => c.Localizacion).Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Nullable().Not.LazyLoad();
            //this.References(c => c.Pais).Nullable().Not.LazyLoad();
            this.Map(c => c.Piso).Nullable().Not.LazyLoad();
            this.Map(c => c.Telefono).Nullable().Not.LazyLoad();
            this.Map(c => c.Predeterminado).Not.Nullable().Not.LazyLoad();
        }
    }
}
