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
            this.Map(c => c.CodigoPostal).Nullable().Not.LazyLoad();
            this.Map(c => c.CUIT).Nullable().Not.LazyLoad();
            this.Map(c => c.Departamento).Nullable().Not.LazyLoad();
            this.Map(c => c.Direccion).Nullable().Not.LazyLoad();
            this.References(c => c.Localizacion).Nullable().Not.LazyLoad();
            this.Map(c => c.Nombre).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Numero).Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Piso).Nullable().Not.LazyLoad();
            this.Map(c => c.Telefono).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaIngreso).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.FechaActaIngreso).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaEgreso).Nullable().Not.LazyLoad();
            this.References(c => c.Nacionalidad).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaNacimiento).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaNotificacion).Nullable().Not.LazyLoad();
            this.Map(c => c.FechaCreacion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.EstadoCivil).Nullable().Not.LazyLoad();
            this.Map(c => c.DNI).Nullable().Not.LazyLoad();
            this.Map(c => c.CI).Nullable().Not.LazyLoad();
            this.Map(c => c.LC).Nullable().Not.LazyLoad();
            this.Map(c => c.LE).Nullable().Not.LazyLoad();         
            this.References(c => c.ActaAlta).Nullable().Column("ActaAlta_Id").LazyLoad();
            this.References(c => c.ActaBaja).Nullable().Column("ActaBaja_Id").LazyLoad();
            this.Map(c => c.ImportePago).Nullable().Not.LazyLoad();
            //this.Map(c => c.CantidadCuotas).Nullable().Not.LazyLoad();
            this.Map(c => c.CantidadPagosAbonados).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.RecomendadoPor).Nullable().Not.LazyLoad();
            this.Map(c => c.NumeroReferencia).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NroCarnetConductor).Nullable().Not.LazyLoad();
            this.Map(c => c.CategoriaConductor).Nullable().Not.LazyLoad();
            this.Map(c => c.MarcaVehiculo).Nullable().Not.LazyLoad();
            this.Map(c => c.ModeloVehiculo).Nullable().Not.LazyLoad();
            this.Map(c => c.NroChapaVehiculo).Nullable().Not.LazyLoad();
            //this.References(c => c.LugarNacimiento).Nullable().Not.LazyLoad();
            this.Map(c => c.Cargo).Nullable().Not.LazyLoad();
            //this.Map(c => c.AbonoTotalmente).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Padre).Nullable().Not.LazyLoad();
            this.Map(c => c.Madre).Nullable().Not.LazyLoad();
            this.Map(c => c.CantidadAbonos).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ImporteAbono).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.AbonoFinalizado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.CantidadAbonosEfectivos).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ImagePath).Nullable().Not.LazyLoad();
        }
    }
}
