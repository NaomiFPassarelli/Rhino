using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.NHMapping.Tesoreria
{
    public class MovimientoFondoMap : ClassMap<MovimientoFondo>
    {
        public MovimientoFondoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.References(c => c.CuentaBancaria).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CuentaDestino).Nullable().Not.LazyLoad();
            this.References(c => c.Movimiento).Not.Nullable();
            this.References(c => c.Caja).Nullable();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaAcredita).Nullable().Not.LazyLoad();
            this.Map(c => c.Concepto).Nullable().Not.LazyLoad();
            this.Map(c => c.Importe).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Asiento).Not.Nullable();
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
