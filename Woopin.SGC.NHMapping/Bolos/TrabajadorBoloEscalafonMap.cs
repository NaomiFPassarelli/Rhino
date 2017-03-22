using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.NHMapping.Bolos
{
    public class TrabajadorBoloEscalafonMap : ClassMap<TrabajadorBoloEscalafon>
    {
        public TrabajadorBoloEscalafonMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.NumeroReferencia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaDesde).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaHasta).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Trabajador).Not.Nullable().Not.LazyLoad();
            //this.References(c => c.Trabajador).Column("Trabajador_Id").LazyLoad();
            this.References(c => c.Escalafon).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Bolo).Not.Nullable().Not.LazyLoad();
        }
    }
}
