using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Model.Negocio;
using Woopin.SGC.Common.Helpers;

namespace Woopin.SGC.Web.Tests.Ventas
{
    [TestClass]
    public class ComprobanteVentaTest
    {
        protected ComprobanteVenta cv = new ComprobanteVenta()
        {
            CAE = "11321231232",
            Cliente = new Cliente() { Id = 1 },
            CondicionVenta = new Model.Common.ComboItem() { Id = 2 },
            Total = 121,
            TotalCobrado = 0,
            Tipo = new Model.Common.ComboItem() { Id = ComprobanteVentaHelper.Factura },
            Asiento = new Model.Contabilidad.Asiento() { Id = 3 },
            IVA = 21,
            IVA21 = 21,
            Letra = "A",
            Numero = "0001-00000001",
            FechaVencimiento = DateTime.Now.AddDays(30),
            Fecha = DateTime.Now,
            FechaCreacion = DateTime.Now,
            Estado = Model.Common.EstadoComprobante.Creada,
            MailCobro = "mail@gmail.com",
            Cotizacion = 1,
            MesPrestacion = "01-2015",
            Observacion = "Prueba",
            Subtotal = 100,
            Usuario = new Model.Common.Usuario() { Id = 1 },
            Moneda = new Model.Common.Moneda() { Id = 1 }
        };


        [TestMethod]
        public void Anulaciones()
        {
            Assert.IsFalse(cv.CanAnular());
            cv.CAE = null;
            Assert.IsTrue(cv.CanAnular());
            cv.TotalCobrado = 1;
            Assert.IsFalse(cv.CanAnular());
            cv.TotalCobrado = 0;
            Assert.IsTrue(cv.CanAnular());
            cv.Estado = Model.Common.EstadoComprobante.Anulada;
            Assert.IsFalse(cv.CanAnular());
            cv.Estado = Model.Common.EstadoComprobante.Pendiente_Afip;
            Assert.IsFalse(cv.CanAnular());
            cv.Estado = Model.Common.EstadoComprobante.Creada;
            Assert.IsTrue(cv.CanAnular());
            cv.Tipo = new Model.Common.ComboItem() { Id = ComprobanteVentaHelper.NotaCredito };
            Assert.IsTrue(cv.CanAnular());
            cv.Tipo = new Model.Common.ComboItem() { Id = ComprobanteVentaHelper.Factura };
            Assert.IsTrue(cv.CanAnular());

             
            // Lo dejo en el estado original.
            cv.CAE = "123123123";
            cv.TotalCobrado = 0;
            cv.Estado = Model.Common.EstadoComprobante.Creada;
            cv.Tipo = new Model.Common.ComboItem() { Id = ComprobanteVentaHelper.Factura };
        }

        [TestMethod]
        public void DecimalToLetras()
        {
            decimal nro = 220.5M;
            string text = CurrencyHelper.ToLetras(nro);
            Assert.IsTrue("DOSCIENTOS VEINTE CON 50/100" == text);
        }
    }
}
