using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.NHMapping
{
    public class ImputacionCompraMap : ClassMap<ImputacionCompra>
    {
        public ImputacionCompraMap()
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Fecha).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Importe).Nullable().Not.LazyLoad();
            this.References(c => c.Usuario).Nullable().Not.LazyLoad();
            this.References(c => c.NotaCredito).Not.Nullable().Not.LazyLoad();
            this.References(c => c.ComprobanteADescontar).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Organizacion).Not.Nullable().LazyLoad();
        }
    }
}
