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
    public class CancelacionTarjetaMap : ClassMap<CancelacionTarjeta>
    {
        public CancelacionTarjetaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Importe).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Cuotas).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Pago).Not.Nullable().LazyLoad();
            this.References(c => c.Valor).Not.Nullable().Not.LazyLoad().Cascade.SaveUpdate();
            this.References(c => c.Asiento).Not.Nullable().LazyLoad();
            this.References(c => c.Usuario).Not.Nullable().LazyLoad();
            // TODO -- Faltaria la organizacion? o como es hijo no importaria?
        }
    }
}
