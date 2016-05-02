using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Web.Tests.Contabilidad
{
    [TestClass]
    public class CuentaTest
    {
        [TestMethod]
        public void ProximoNro()
        {
            Cuenta c = new Cuenta()
            {
                Corriente = 3,
                Numero = 0,
                SubRubro = 0,
                Rubro = 1,
                Nombre = "Cuentas Transitorias",
                Codigo = "1.003"
            };

            string codigo = c.CodigoProxima();

            Assert.AreEqual("1.004", codigo);
        }
    }
}
