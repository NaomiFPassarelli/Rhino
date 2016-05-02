using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.NHMapping
{
    public class CuentaBancariaMap : ClassMap<CuentaBancaria>
    {
        public CuentaBancariaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Numero).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Fondo).Nullable().Not.LazyLoad();
            this.Map(c => c.EmiteCheques).Nullable().Not.LazyLoad();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Banco).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Moneda).Not.Nullable().Not.LazyLoad();
            this.References(c => c.CuentaContable).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
