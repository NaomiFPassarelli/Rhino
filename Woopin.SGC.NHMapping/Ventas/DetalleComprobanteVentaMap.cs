using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.NHMapping
{
    public class DetalleComprobanteVentaMap : ClassMap<DetalleComprobanteVenta>
    {
        public DetalleComprobanteVentaMap() 
        {
            this.Id(c => c.Id).GeneratedBy.Identity();
            this.Map(c => c.Cantidad).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Descripcion).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.PrecioUnitario).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Total).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.TotalConIVA).Not.Nullable().Not.LazyLoad();
            this.Map(c => c.Descuento).Nullable().Not.LazyLoad();
            this.References(c => c.Articulo).Not.Nullable().Not.LazyLoad();
            this.References(c => c.TipoIva).Not.Nullable().Not.LazyLoad();
            this.References(c => c.Comprobante).Column("ComprobanteVenta_Id").LazyLoad();
        }
    }
}
