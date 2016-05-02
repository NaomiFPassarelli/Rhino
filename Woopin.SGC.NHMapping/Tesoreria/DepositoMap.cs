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
    public class DepositoMap : ClassMap<Deposito>
    {
        public DepositoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Concepto).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaAcreditacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaDeposito).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroBoleta).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Cuenta).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Asiento).Not.Nullable().LazyLoad();
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
            this.HasMany(c => c.Cheques).AsBag().KeyColumn("Deposito_Id").Cascade.AllDeleteOrphan();
        }
    }
}
