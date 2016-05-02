using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.NHMapping
{
    public class DetalleComprobanteCompraMap : ClassMap<DetalleComprobanteCompra>
    {
        public DetalleComprobanteCompraMap() 
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.IVA).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Comprobante).Column("Comprobante_Id").LazyLoad();
            this.References(c => c.TipoIva).Not.Nullable().Not.LazyLoad();
            this.References(c => c.RubroCompra).Not.Nullable().Not.LazyLoad();
        }
    }
}
