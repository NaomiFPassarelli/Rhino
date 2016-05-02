using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Negocio
{
    public class AsientosHelper
    {
        public string Ventas = "4.1.001";
        public string IvaDebito = "2.1.3.001";
        public string IvaCredito = "1.1.3.001";
        public string ValoresADepositar = "1.1.1.002";
        public string DeudoresXVenta = "1.1.2.001";
        public string Proveedores = "2.1.1.001";

        public Asiento AsientoComprobanteVenta(ComprobanteVenta comprobante)
        {
            Asiento asiento = new Asiento();
            Cliente cliente = comprobante.Cliente;
            asiento.Fecha = comprobante.Fecha;
            asiento.FechaCreacion = DateTime.Now;
            asiento.Leyenda = "Venta " + comprobante.Tipo.Data + " " + comprobante.GetLetraNumero() + " (Cli.: " + cliente.RazonSocial + ")";
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();
            asiento.TipoOperacion = TipoOperacion.FV;

            decimal SubTotal = decimal.Round( comprobante.Cotizacion > 0 ? comprobante.Subtotal * comprobante.Cotizacion : comprobante.Subtotal, 2, MidpointRounding.AwayFromZero);
            decimal IVA = decimal.Round( comprobante.Cotizacion > 0 ? comprobante.IVA * comprobante.Cotizacion : comprobante.IVA, 2, MidpointRounding.AwayFromZero);
            decimal Total = decimal.Round( comprobante.Cotizacion > 0 ? comprobante.Total * comprobante.Cotizacion : comprobante.Total, 2, MidpointRounding.AwayFromZero);

            //menor a 1 es por ejemplo nota de credito
            if (Convert.ToInt32(comprobante.Tipo.AdditionalData) >= 1 )
            {
                AsientoItem itemVentas = new AsientoItem() { Asiento = asiento, Haber = SubTotal, Cuenta = new Cuenta() { Codigo = this.Ventas } };
                asiento.Items.Add(itemVentas);

                if (IVA > 0)
                {
                    AsientoItem itemIVA = new AsientoItem() { Asiento = asiento, Haber = IVA, Cuenta = new Cuenta() { Codigo = this.IvaDebito } };
                    asiento.Items.Add(itemIVA);
                }


                AsientoItem itemCliente = new AsientoItem() { Asiento = asiento, Debe = Total, Cuenta = new Cuenta() { Codigo = this.DeudoresXVenta } };
                asiento.Items.Add(itemCliente);
                
            }
            else
            {
                AsientoItem itemVentas = new AsientoItem() { Asiento = asiento, Debe = SubTotal, Cuenta = new Cuenta() { Codigo = this.Ventas } };
                asiento.Items.Add(itemVentas);
                
                if (IVA > 0)
                {
                    AsientoItem itemIVA = new AsientoItem() { Asiento = asiento, Debe = IVA, Cuenta = new Cuenta() { Codigo = this.IvaDebito } };
                    asiento.Items.Add(itemIVA);
                }

                AsientoItem itemCliente = new AsientoItem() { Asiento = asiento, Haber = Total, Cuenta = new Cuenta() { Codigo = this.DeudoresXVenta } };
                asiento.Items.Add(itemCliente);
                
            }

            return asiento;
        }
        public Asiento AsientoComprobanteCompra(ComprobanteCompra Comprobante, Proveedor proveedor)
        {
            Asiento asiento = new Asiento();
            asiento.Fecha = Comprobante.FechaContable;
            asiento.FechaCreacion = DateTime.Now;
            asiento.Leyenda = "Compra " + Comprobante.Tipo.Data + " " + Comprobante.GetLetraNumero() + " (Prov.: " + proveedor.RazonSocial + ")";
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();
            asiento.TipoOperacion = TipoOperacion.OC;


            decimal SubTotal = decimal.Round(Comprobante.Subtotal, 2, MidpointRounding.AwayFromZero);
            decimal IVA = decimal.Round(Comprobante.IVA, 2, MidpointRounding.AwayFromZero);
            decimal Total = decimal.Round(Comprobante.Total, 2, MidpointRounding.AwayFromZero);


            if (Convert.ToInt32(Comprobante.Tipo.AdditionalData) >= 1)
            {
                if (Comprobante.Letra != "C" && Comprobante.IVA > 0)
                {
                    AsientoItem itemIVA = new AsientoItem() { Asiento = asiento, Debe = IVA, Cuenta = new Cuenta() { Codigo = this.IvaCredito } };
                    asiento.Items.Add(itemIVA);
                }

                foreach(var itemDetalle in Comprobante.Detalle)
                {
                    AsientoItem item = new AsientoItem() { Asiento = asiento, Debe = itemDetalle.Total, Cuenta = new Cuenta() { Id = itemDetalle.RubroCompra.Cuenta.Id } };
                    asiento.Items.Add(item);
                }
                AsientoItem itemProveedor = new AsientoItem() { Asiento = asiento, Haber = Total, Cuenta = new Cuenta() { Codigo = this.Proveedores } };
                asiento.Items.Add(itemProveedor);
                
            }
            else
            {
                if (Comprobante.Letra != "C" && Comprobante.IVA > 0)
                {
                    AsientoItem itemIVA = new AsientoItem() { Asiento = asiento, Haber = IVA, Cuenta = new Cuenta() { Codigo = this.IvaCredito } };
                    asiento.Items.Add(itemIVA);
                }

                foreach (var itemDetalle in Comprobante.Detalle)
                {
                    AsientoItem item = new AsientoItem() { Asiento = asiento, Haber = itemDetalle.Total, Cuenta = new Cuenta() { Id = itemDetalle.RubroCompra.Cuenta.Id } };
                    asiento.Items.Add(item);
                }
                AsientoItem itemProveedor = new AsientoItem() { Asiento = asiento, Debe = Total, Cuenta = new Cuenta() { Codigo = this.Proveedores } };
                asiento.Items.Add(itemProveedor);
                
            }
            return asiento;
        }
        public Asiento AsientoMovimientoFondos(Tesoreria.MovimientoFondo movimiento)
        {
            Asiento asiento = new Asiento();
            asiento.Fecha = movimiento.Fecha;
            asiento.FechaCreacion = DateTime.Now;
            asiento.Leyenda = "Movimiento de Fondos " + movimiento.Movimiento.Data + "(" + movimiento.Concepto + ")";
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();
            asiento.TipoOperacion = TipoOperacion.MOVF;

            AsientoItem itemCaja, itemCuenta, itemCuentaDestino;
            switch(movimiento.Movimiento.Id)
            {
                case ValoresHelper.Deposito:
                    itemCaja = new AsientoItem() { Asiento = asiento, Haber = movimiento.Importe, Cuenta = new Cuenta() { Id = movimiento.Caja.CuentaContable.Id } };
                        asiento.Items.Add(itemCaja);
                        itemCuenta = new AsientoItem() { Asiento = asiento, Debe = movimiento.Importe, Cuenta = new Cuenta() { Id = movimiento.CuentaBancaria.CuentaContable.Id } };
                        asiento.Items.Add(itemCuenta);
                    break;

                case ValoresHelper.Extraccion:
                    itemCaja = new AsientoItem() { Asiento = asiento, Debe = movimiento.Importe, Cuenta = new Cuenta() { Id = movimiento.Caja.CuentaContable.Id } };
                        asiento.Items.Add(itemCaja);
                        itemCuenta = new AsientoItem() { Asiento = asiento, Haber = movimiento.Importe, Cuenta = new Cuenta() { Id = movimiento.CuentaBancaria.CuentaContable.Id } };
                        asiento.Items.Add(itemCuenta);
                    break;

                case ValoresHelper.Transferencia:
                    itemCuentaDestino = new AsientoItem() { Asiento = asiento, Debe = movimiento.Importe, Cuenta = new Cuenta() { Id = movimiento.CuentaDestino.CuentaContable.Id } };
                    asiento.Items.Add(itemCuentaDestino);
                    itemCuenta = new AsientoItem() { Asiento = asiento, Haber = movimiento.Importe, Cuenta = new Cuenta() { Id = movimiento.CuentaBancaria.CuentaContable.Id } };
                    asiento.Items.Add(itemCuenta);
                    break;

                default:
                    throw new BusinessException("Movimiento no implementado, consultar al administrador!");
            }

            return asiento;
        }
        public Asiento AsientoOrdenPago(OrdenPago op)
        {
            Asiento asiento = new Asiento();
            asiento.Fecha = op.Fecha;
            asiento.FechaCreacion = DateTime.Now;
            asiento.Leyenda = op.Tipo.Data + " " + op.Numero + " a " + op.Proveedor.RazonSocial;
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();
            asiento.TipoOperacion = TipoOperacion.OP;

            // Descuento lo cancelado del proveedor
            AsientoItem itemProveedor = new AsientoItem() { Asiento = asiento, Debe = op.Comprobantes.Sum(x => x.Importe), Cuenta = new Cuenta() { Codigo = this.Proveedores } };
            asiento.Items.Add(itemProveedor);

            // Descuento lo cancelado de los valores.
            foreach(var Valor in op.Pagos.Select(x => x.Valor))
            {
                AsientoItem itemValor = new AsientoItem() { Asiento = asiento, Haber = Valor.Importe, Cuenta = Valor.CuentaContable };
                asiento.Items.Add(itemValor);

                //[GetCuentaContableForValor]
                if(Valor.Valor.TipoValor.Data == TipoValor.ChequePropio)
                {
                    itemValor.ChequePropio = new ChequePropio() { Id = Valor.NumeroReferencia };
                } 
            }
            return asiento;
        }
        public Asiento AsientoOtroEgreso(OtroEgreso oe)
        {
            Asiento asiento = new Asiento();
            asiento.Fecha = oe.FechaContable;
            asiento.FechaCreacion = DateTime.Now;
            asiento.Leyenda = "Otro Egreso  " + oe.NumeroReferencia.ToString() + " a " + oe.Proveedor.RazonSocial;
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();
            asiento.TipoOperacion = TipoOperacion.OE;

            // Descuento lo cancelado de los rubros
            foreach (var itemDetalle in oe.Detalle)
            {
                AsientoItem item = new AsientoItem() { Asiento = asiento, Debe = itemDetalle.Total, Cuenta = new Cuenta() { Id = itemDetalle.RubroCompra.Cuenta.Id } };
                asiento.Items.Add(item);
            }

            // Descuento lo cancelado de los valores.
            foreach (var Valor in oe.Pagos.Select(x => x.Valor))
            {
                AsientoItem itemValor = new AsientoItem() { Asiento = asiento, Haber = Valor.Importe, Cuenta = Valor.CuentaContable };
                asiento.Items.Add(itemValor);

                //[GetCuentaContableForValor]
                if (Valor.Valor.TipoValor.Data == TipoValor.ChequePropio)
                {
                    itemValor.ChequePropio = new ChequePropio() { Id = Valor.NumeroReferencia };
                } 
            }
            return asiento;
        }

        public Asiento AsientoCancelacionTarjeta(Tesoreria.CancelacionTarjeta cancelacion)
        {
            Asiento asiento = new Asiento();
            asiento.Fecha = cancelacion.Fecha;
            asiento.FechaCreacion = DateTime.Now;
            asiento.Leyenda = "Cancelación tarjeta de credito  " + cancelacion.Pago.Tarjeta.Numero;
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();
            asiento.TipoOperacion = TipoOperacion.CTC;

            AsientoItem item = null;
            item = new AsientoItem() { Asiento = asiento, Debe = cancelacion.Importe, Cuenta = new Cuenta() { Id = cancelacion.Pago.Tarjeta.CuentaContable.Id } };
            asiento.Items.Add(item);
            item = new AsientoItem() { Asiento = asiento, Haber = cancelacion.Importe, Cuenta = new Cuenta() { Id = cancelacion.Pago.Tarjeta.CuentaBancaria.CuentaContable.Id } };
            asiento.Items.Add(item);


            return asiento;
        }

        public Asiento AsientoCobranza(Cobranza cobranza)
        {
            Asiento asiento = new Asiento();
            asiento.Fecha = cobranza.Fecha;
            asiento.FechaCreacion = DateTime.Now;
            asiento.TipoOperacion = TipoOperacion.CZ;
            asiento.Leyenda = cobranza.Tipo.Data + " " + cobranza.Numero + " a " + cobranza.Cliente.RazonSocial;
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();

            // Descuento lo cancelado del cliente
            Cuenta cta = new Cuenta() { Codigo = this.DeudoresXVenta };

            AsientoItem itemCliente = new AsientoItem() { Asiento = asiento, Haber = cobranza.Comprobantes.Sum(x => x.Importe), Cuenta =  cta  };
            asiento.Items.Add(itemCliente);

            // Descuento lo cancelado de los valores.
            foreach (var Valor in cobranza.Valores.Select(x => x.Valor))
            {
                AsientoItem itemValor = new AsientoItem() { Asiento = asiento, Debe = Valor.Importe, Cuenta = Valor.CuentaContable };
                asiento.Items.Add(itemValor);
            }
            return asiento;
        }

        public Asiento AsientoDeposito(Deposito deposito)
        {
            Asiento asiento = new Asiento();
            asiento.Fecha = deposito.FechaAcreditacion;
            asiento.FechaCreacion = DateTime.Now;
            asiento.Leyenda = "Deposito de Cheques ";
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();
            asiento.TipoOperacion = TipoOperacion.DEP;

            decimal Importe = 0;
            foreach(var itemCheque in deposito.Cheques)
            {
                Importe += itemCheque.Cheque.Importe;
            }

            AsientoItem itemValorADepositar = new AsientoItem() { Asiento = asiento, Haber = Importe, Cuenta = new Cuenta() { Codigo = this.ValoresADepositar } };
            asiento.Items.Add(itemValorADepositar);

            AsientoItem itemBanco = new AsientoItem() { Asiento = asiento, Debe = Importe, Cuenta = new Cuenta() { Id = deposito.Cuenta.CuentaContable.Id } };
            asiento.Items.Add(itemBanco);

            return asiento;
        }

        public Asiento AsientoCanjeCheque(ChequePropio chequeCanje, ChequePropio chequeAnula )
        {
            Asiento asiento = new Asiento();
            asiento.Fecha = chequeCanje.Fecha;
            asiento.FechaCreacion = DateTime.Now;
            asiento.Leyenda = "Canje de cheques anulado " + chequeAnula.Numero + " entrega " + chequeCanje.Numero;
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();

            AsientoItem itemBancoCanje = new AsientoItem() { Asiento = asiento, Haber = chequeAnula.Importe, Cuenta = chequeCanje.CuentaBancaria.CuentaContable };
            asiento.Items.Add(itemBancoCanje);

            AsientoItem itemBancoAnula = new AsientoItem() { Asiento = asiento, Debe = chequeAnula.Importe, Cuenta = chequeAnula.CuentaBancaria.CuentaContable };
            asiento.Items.Add(itemBancoAnula);

            return asiento;
        }
    }
}
