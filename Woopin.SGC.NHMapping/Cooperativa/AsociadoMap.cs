using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Cooperativa;

namespace Woopin.SGC.NHMapping.Cooperativa
{
    public class AsociadoMap : ClassMap<Asociado>
    {
        public AsociadoMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Apellido).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.CodigoPostal).Nullable().Not.LazyLoad();
            this.Map(c => c.CUIT).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.Departamento).Nullable().Not.LazyLoad();
            //this.Map(c => c.Direccion).Nullable().Not.LazyLoad();
            //this.Map(c => c.Email).Nullable().Not.LazyLoad();
            //this.References(c => c.Localizacion).Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.Numero).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.Piso).Nullable().Not.LazyLoad();
            //this.Map(c => c.Telefono).Nullable().Not.LazyLoad();
            //this.Map(c => c.SueldoBrutoMensual).Nullable().Not.LazyLoad();
            //this.Map(c => c.SueldoBrutoHora).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaIngreso).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaActaIngreso).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaEgreso).Nullable().Not.LazyLoad();
            //this.Map(c => c.FechaAntiguedadReconocida).Nullable().Not.LazyLoad();
            //this.References(c => c.Nacionalidad).Nullable().Not.LazyLoad();
            //this.Map(c => c.FechaNacimiento).Nullable().Not.LazyLoad();
            //this.References(c => c.Categoria).Nullable().Not.LazyLoad();
            //this.References(c => c.EstadoCivil).Nullable().Not.LazyLoad();
            //this.References(c => c.Sindicato).Nullable().Not.LazyLoad();
            //this.References(c => c.ObraSocial).Nullable().Not.LazyLoad();
            //this.References(c => c.BancoDeposito).Nullable().Not.LazyLoad();
            //this.References(c => c.Tarea).Nullable().Not.LazyLoad();
            //this.References(c => c.Sexo).Nullable().Not.LazyLoad();
            this.Map(c => c.DNI).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.BeneficiarioObraSocial).Nullable().Not.LazyLoad();
            this.Map(c => c.ActaAlta).Nullable().Not.LazyLoad();
            this.Map(c => c.ActaBaja).Nullable().Not.LazyLoad();
            this.Map(c => c.ImporteCuota).Nullable().Not.LazyLoad();
            this.Map(c => c.CantidadCuotas).Nullable().Not.LazyLoad();
            this.Map(c => c.CantidadCuotasAbonadas).Not.Nullable().Not.LazyLoad();
        }
    }
}
