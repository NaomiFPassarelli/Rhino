using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.NHMapping.Bolos
{
    public class TrabajadorMap : ClassMap<Trabajador>
    {
        public TrabajadorMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Apellido).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CodigoPostal).Nullable().Not.LazyLoad();
            this.References(c => c.Localizacion).Nullable().Not.LazyLoad();
            this.Map(c => c.CUIT).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Departamento).Nullable().Not.LazyLoad();
            this.Map(c => c.Direccion).Nullable().Not.LazyLoad();
            this.Map(c => c.Email).Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Piso).Nullable().Not.LazyLoad();
            this.Map(c => c.Telefono).Nullable().Not.LazyLoad();
            this.Map(c => c.SalarioEspecial).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaNacimiento).Not.Nullable().Not.LazyLoad();
            //this.References(c => c.Escalafon).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Escalafon).Nullable().Not.LazyLoad();
            //this.References(c => c.Sindicato).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Sindicato).Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroReferencia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Hijos).Nullable().Not.LazyLoad();
            this.HasMany(c => c.TrabajadorBoloEscalafon).AsBag().KeyColumn("Trabajador_Id").Cascade.AllDeleteOrphan();
            this.References(c => c.EstadoCivil).Not.Nullable().Not.LazyLoad();
        }
    }
}
