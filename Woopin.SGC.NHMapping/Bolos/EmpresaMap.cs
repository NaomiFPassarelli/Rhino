using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Bolos;

namespace Woopin.SGC.NHMapping.Bolos
{
    public class EmpresaMap : ClassMap<Empresa>
    {
        public EmpresaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            //this.Map(c => c.RazonSocial).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.CUIT).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.Telefono).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.Domicilio).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.CodigoPostal).Not.Nullable().Not.LazyLoad();
            //this.Map(c => c.Activo).Not.Nullable().Not.LazyLoad();
            //this.References(c => c.Localizacion).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.NombreApoderado).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.ApellidoApoderado).Not.Nullable().Not.LazyLoad();
            this.References(c => c.BancoDeposito).Not.Nullable().Not.LazyLoad();
        }
    }
}
