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
    public class ChequeMap : ClassMap<Cheque>
    {
        public ChequeMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaVencimiento).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaEfectivizado).Nullable().Not.LazyLoad();
            this.Map(c => c.Importe).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Estado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroCuenta).Nullable().Not.LazyLoad();
            this.Map(c => c.Propio).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Banco).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Cliente).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
