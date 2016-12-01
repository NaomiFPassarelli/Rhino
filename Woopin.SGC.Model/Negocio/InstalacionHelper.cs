using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Model.Negocio
{
    public static class InstalacionHelper
    {


        public static List<Cuenta> GetCuentasContables()
        {
            List<Cuenta> cuentas = new List<Cuenta>();

            cuentas.Add(new Cuenta("1.0.0.0", "Activo"));
                cuentas.Add(new Cuenta("1.1.0.0", "Activo Corriente"));
                    cuentas.Add(new Cuenta("1.1.1.0", "Caja y Banco"));
                        cuentas.Add(new Cuenta("1.1.1.002", "Valores A Depositar"));
                    cuentas.Add(new Cuenta("1.1.2.0", "Credito Por Ventas"));
                        cuentas.Add(new Cuenta("1.1.2.001", "Deudores Por Ventas"));
                    cuentas.Add(new Cuenta("1.1.3.0", "Otros Creditos"));
                        cuentas.Add(new Cuenta("1.1.3.001", "IVA Credito Fiscal"));
                cuentas.Add(new Cuenta("1.2.0.0", "Activo No Corriente"));
                    cuentas.Add(new Cuenta("1.2.1.0", "Bienes de Uso"));
                cuentas.Add(new Cuenta("1.3.0.0", "Cuentas Transitorias"));
            
            cuentas.Add(new Cuenta("2.0.0.0", "Pasivo"));
                cuentas.Add(new Cuenta("2.1.0.0", "Pasivo Corriente"));
                    cuentas.Add(new Cuenta("2.1.1.0", "Deudas Comerciales")); 
                        cuentas.Add(new Cuenta("2.1.1.001", "Proveedores"));
                    cuentas.Add(new Cuenta("2.1.2.0", "Deudas Sociales"));
                        cuentas.Add(new Cuenta("2.1.2.001", "Sueldos A Pagar"));
                        cuentas.Add(new Cuenta("2.1.2.002", "Cargas Sociales A Pagar"));
                        cuentas.Add(new Cuenta("2.1.2.003", "Sindicato A Pagar"));
                    cuentas.Add(new Cuenta("2.1.3.0", "Deudas Fiscales"));
                        cuentas.Add(new Cuenta("2.1.3.001", "IVA Debito Fiscal"));
                    cuentas.Add(new Cuenta("2.1.4.0", "Deudas Bancarias"));
                    cuentas.Add(new Cuenta("2.1.5.0", "Otras Deudas"));
                cuentas.Add(new Cuenta("2.2.0.0", "Pasivo No Corriente"));
            
            cuentas.Add(new Cuenta("3.0.0.0", "Patrimonio Neto"));
                cuentas.Add(new Cuenta("3.001", "Capital Social"));

            cuentas.Add(new Cuenta("4.0.0.0", "Ingresos"));
                cuentas.Add(new Cuenta("4.1.0.0", "Ingresos por Ventas"));
                    cuentas.Add(new Cuenta("4.1.001", "Ventas"));
                cuentas.Add(new Cuenta("4.2.0.0", "Otros Ingresos"));

            cuentas.Add(new Cuenta("5.0.0.0", "Egresos"));
                cuentas.Add(new Cuenta("5.1.0.0", "Gastos de Comercialización"));
                cuentas.Add(new Cuenta("5.2.0.0", "Gastos de Administración"));
                cuentas.Add(new Cuenta("5.3.0.0", "Gastos de Financiamiento"));
                cuentas.Add(new Cuenta("5.4.0.0", "Otros Egresos"));

            cuentas.Add(new Cuenta("6.0.0.0", "Costos"));


            return cuentas;
        }

        public static List<Valor> GetValores()
        {
            Moneda ars = new Moneda(){ Id = 1 };
            List<Valor> valores = new List<Valor>();
            valores.Add(new Valor() { Nombre = "Efectivo Pesos", TipoValor = new ComboItem() { Id = 71 } , Activo = true, Moneda = ars });
            valores.Add(new Valor() { Nombre = "Tarjeta de Debito", TipoValor = new ComboItem() { Id = 72 }, Activo = true, Moneda = ars });
            valores.Add(new Valor() { Nombre = "Cheque Propio", TipoValor = new ComboItem() { Id = 73 }, Activo = true, Moneda = ars });
            valores.Add(new Valor() { Nombre = "Cheque Terceros", TipoValor = new ComboItem() { Id = 70 }, Activo = true, Moneda = ars });
            valores.Add(new Valor() { Nombre = "Transferencia", TipoValor = new ComboItem() { Id = 72 }, Activo = true, Moneda = ars });
            valores.Add(new Valor() { Nombre = "Tarjeta de Credito", TipoValor = new ComboItem() { Id = 74 }, Activo = true, Moneda = ars });
            valores.Add(new Valor() { Nombre = "Retención", TipoValor = new ComboItem() { Id = 75 }, Activo = true, Moneda = ars }); 
            
            return valores;
        }
    }
}
