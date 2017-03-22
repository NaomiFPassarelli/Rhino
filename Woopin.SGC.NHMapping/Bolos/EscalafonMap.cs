using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.NHMapping.Bolos
{
    public class EscalafonMap : ClassMap<Escalafon>
    {
        public EscalafonMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.NumeroReferencia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            //this.References(c => c.Categoria).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Salario).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.Resolucion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.VigenciaDesde).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.VigenciaHasta).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.MarcaVigencia).Not.Nullable().Not.LazyLoad();
        }
    }
}
