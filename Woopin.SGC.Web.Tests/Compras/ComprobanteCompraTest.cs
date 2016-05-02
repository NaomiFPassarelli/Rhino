using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Model.Compras;

namespace Woopin.SGC.Web.Tests.Compras
{
    [TestClass]
    public class ComprobanteCompraTest
    {
        protected ComprobanteCompra cc = new ComprobanteCompra()
        {
            Proveedor = new Proveedor() { Id = 1 },
            CondicionCompra = new Model.Common.ComboItem() { Id = 2 },
            Total = 121,
            TotalPagado = 0,
            Tipo = new Model.Common.ComboItem() { Id = ComprobanteCompraHelper.Factura },
            Asiento = new Model.Contabilidad.Asiento() { Id = 3 },
            IVA = 21,
            IVA21 = 21,
            Letra = "A",
            Numero = "0001-00000001",
            FechaVencimiento = DateTime.Now.AddDays(30),
            Fecha = DateTime.Now,
            FechaCreacion = DateTime.Now,
            Estado = Model.Common.EstadoComprobante.Creada,
            Observacion = "Prueba",
            Subtotal = 100,
            Usuario = new Model.Common.Usuario() { Id = 1 }
        };


        [TestMethod]
        public void Anulaciones()
        {
            Assert.IsFalse(cc.CanAnular());
            cc.TotalPagado = 1;
            Assert.IsFalse(cc.CanAnular());
            cc.TotalPagado = 0;
            Assert.IsTrue(cc.CanAnular());
            cc.Estado = Model.Common.EstadoComprobante.Anulada;
            Assert.IsFalse(cc.CanAnular());
            cc.Estado = Model.Common.EstadoComprobante.Pagada;
            Assert.IsFalse(cc.CanAnular());
            cc.Estado = Model.Common.EstadoComprobante.Creada;
            Assert.IsTrue(cc.CanAnular());

             
            // Lo dejo en el estado original.
            cc.TotalPagado = 0;
            cc.Estado = Model.Common.EstadoComprobante.Creada;
        }
    }
}
