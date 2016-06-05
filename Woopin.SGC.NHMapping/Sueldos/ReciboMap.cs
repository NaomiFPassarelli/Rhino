using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.NHMapping.Sueldos
{
    public class ReciboMap : ClassMap<Recibo>
    {
        public ReciboMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            //this.References(c => c.Adicionales).Nullable().Not.LazyLoad();
            this.References(c => c.Empleado).Not.Nullable().Not.LazyLoad();
            this.Map(x => x.Sindicato).Nullable().Not.LazyLoad();
            this.Map(x => x.ObraSocial).Nullable().Not.LazyLoad();
            this.Map(x => x.BancoDeposito).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaFin).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaInicio).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Periodo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TotalRemunerativo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TotalNoRemunerativo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TotalDescuento).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Observacion).Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroReferencia).Not.Nullable().Not.LazyLoad();
            this.References(c => c.TipoRecibo).Not.Nullable().Not.LazyLoad();
            //this.HasMany(c => c.AdicionalXAdicionales).AsBag().KeyColumn("Recibo_Id").Cascade.AllDeleteOrphan();
            this.HasMany(c => c.AdicionalesRecibo).AsBag().KeyColumn("Recibo_Id").Cascade.AllDeleteOrphan();
            this.Map(c => c.DomicilioEmpresa).Not.Nullable().Not.LazyLoad();
        }
    }
}
