using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping.Ventas
{

    public class ImputacionVentaMap : ClassMap<ImputacionVenta>
    {
        public ImputacionVentaMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Importe).Nullable().Not.LazyLoad();
            this.References(c => c.Usuario).Not.Nullable().Not.LazyLoad();
            this.References(c => c.NotaCredito).Not.Nullable().Not.LazyLoad();
            this.References(c => c.ComprobanteADescontar).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
