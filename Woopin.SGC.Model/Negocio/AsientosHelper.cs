using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Contabilidad;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Sueldos;
using Woopin.SGC.Model.Tesoreria;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Negocio
{
    public class AsientosHelper
    {
        public string Ventas = "4.1.0.001";
        public string IvaDebito = "2.1.3.001";
        public string IvaCredito = "1.1.3.001";
        public string ValoresADepositar = "1.1.1.002";
        public string DeudoresXVenta = "1.1.2.001";
        public string Proveedores = "2.1.1.001";
        public string SueldosAPagar = "2.1.2.001";
        public string CargasSocialesAPagar = "2.1.2.002";
        public string Sindicato = "2.1.2.003";

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

        public Asiento AsientoReciboSueldo(Recibo Recibo)
        {
            Asiento asiento = new Asiento();
            Empleado empleado = Recibo.Empleado;
            asiento.Fecha = Recibo.FechaPago;
            asiento.FechaCreacion = DateTime.Now;
            asiento.Leyenda = "Recibo Sueldo " + Recibo.NumeroReferencia + " (Emple.: " + empleado.Apellido + " " + empleado.Nombre + ")";
            asiento.Modulo = ModulosSistema.SISTEMA_GESTION;
            asiento.Items = new List<AsientoItem>();
            asiento.TipoOperacion = TipoOperacion.RS;

            //Activo
            //Caja - Bancos  - Asignaciones familiares a cobrar  - Anticipos de sueldos  - Anticipo de vacaciones
            
            //Pasivo
            //Sueldos a pagar - Cargas Sociales a pagar - Vacaciones a pagar - S.A.C. a pagar - 
            //Indemnizaciones a pagar - Provisión S.A.C. - Provisión Vacaciones
            
            //Patrimonio Neto
            //dentro de Estado de Resultados
            //Sueldos - Cargas Sociales o Contribuciones - Seguro de Vida - ART - Indemnizaciones
            
            
            //Pago de Cargas Sociales
            //Cargas Sociales a Pagar
            //a Caja o Banco

            //Pago de Sindicato
            //Sindicato a Pagar
            //a Caja o Banco

            //Otorga Anticipo de Sueldo
            //Anticipo de Sueldo
            //a Caja o Banco

            //Anticipo de Vacaciones hay dos alternativas

            //Pago de Sueldos
            //Sueldo 
            //a Caja o Banco


            //if (Recibo.TipoRecibo == TypeRecibo.Sueldo) 
            //{
            //    TODO habria revisar que no se haya agregado SAC ni Vacaciones ni nada que no sea propiamente del sueldo, no?
            //}

            decimal Sueldo = 0, Banco = 0;
            decimal Sindicato = 0;
            decimal CargasSociales = 0;
            //decimal SubTotal = decimal.Round(Recibo.Cotizacion > 0 ? Recibo.Subtotal * Recibo.Cotizacion : Recibo.Subtotal, 2, MidpointRounding.AwayFromZero);
            //decimal IVA = decimal.Round(Recibo.Cotizacion > 0 ? Recibo.IVA * Recibo.Cotizacion : Recibo.IVA, 2, MidpointRounding.AwayFromZero);
            decimal Total = decimal.Round(Recibo.Total, 2, MidpointRounding.AwayFromZero);

            foreach (var itemDetalle in Recibo.AdicionalesRecibo)
            {
                switch (itemDetalle.Adicional.Id)
                {
                    //AsientoItem item = new AsientoItem() { Asiento = asiento, Debe = itemDetalle.Total, Cuenta = new Cuenta() { Id = itemDetalle.Adicional.Cuenta.Id } };
                    //asiento.Items.Add(item);
                    case 1://sueldo
                    case 2://suelo
                    case 3://falta justi
                    case 6://antig
                    case 7://antig
                    case 8://antig
                    case 9://antig
                    case 10://antig
                    case 11://antig
                    case 12://antig
                    case 13://antig
                    case 14://antig
                    case 15://antig
                    case 16://antig
                    case 17://antig
                    case 18://antig
                    case 1004://premio
                    case 1005://premio
                    case 1006://refri
                    case 4007: //hs extras
                    case 4008: //premio
                    case 4009: //premio
                    case 5012: //refri
                    case 5013: //refri
                    case 5014: //viatic
                    case 5015: //viatic
                    case 5016: //viatic
                    case 6013: //falta justif
                    case 9011: //dia cortador
                    //Sueldo
                        Sueldo += itemDetalle.Total;
                        break;
                    case 4://falta injus
                    case 5://descu de hs
                        Sueldo -= itemDetalle.Total;
                        break;
                    case 1007: //os
                    case 1008: //jub
                    case 1009: //ley
                        CargasSociales += itemDetalle.Total;
                        break;
                    case 1010: //sindic
                        Sindicato += itemDetalle.Total;
                        break;
                    case 2006: // Anticipo sueldo
                        //TODO
                        break;
                    case 2008: //redondeo
                        break;
                    case 3006: //suma no remun
                        break;
                    case 3007: //diferen
                    case 3008: //diferen
                    case 3009: //diferen
                    case 3010: //diferen
                    case 3011: //diferen
                    case 3012: //diferen
                        break;
                    case 4010: //SAC
                        break;
                    case 5011: //Benef os
                        break;
                    case 6011: //Suspen
                        break;
                    case 8011: //Vacaciones
                        break;
                    case 6012: //ART
                        break;
                }


                if (Sueldo != 0)
                {
                    AsientoItem itemSueldo = new AsientoItem() { Asiento = asiento, Debe = Sueldo, Cuenta = new Cuenta() { Codigo = this.SueldosAPagar } };
                    asiento.Items.Add(itemSueldo);
                }
                if (CargasSociales != 0)
                {
                    AsientoItem itemCargasSociales = new AsientoItem() { Asiento = asiento, Debe = CargasSociales, Cuenta = new Cuenta() { Codigo = this.CargasSocialesAPagar } };
                    asiento.Items.Add(itemCargasSociales);
                }
                if (Sindicato != 0)
                {
                    AsientoItem itemSindicato = new AsientoItem() { Asiento = asiento, Debe = Sindicato, Cuenta = new Cuenta() { Codigo = this.Sindicato } };
                    asiento.Items.Add(itemSindicato);
                }
                if (Recibo.Empleado.BancoDeposito != null)
                {
                    Banco += Sueldo;
                    Banco += CargasSociales;
                    Banco += Sindicato;
                    //TODO
                    //AsientoItem itemBanco = new AsientoItem() { Asiento = asiento, Debe = Banco, Cuenta = new Cuenta() { Codigo = this.Banco } };
                    //asiento.Items.Add(itemBanco);
                    //AsientoItem itemBanco = new AsientoItem() { Asiento = asiento, Debe = Banco, Cuenta = new Cuenta() { Id = Recibo.Empleado.BancoDeposito } };
                    //asiento.Items.Add(itemBanco);
                }
                else 
                {
                    //AsientoItem itemCaja = new AsientoItem() { Asiento = asiento, Debe = Banco, Cuenta = new Cuenta() { Id = Recibo.Caja.CuentaContable.Id } };
                    //asiento.Items.Add(itemCaja);  
                }
                
            }

            //AsientoItem itemEmpleado = new AsientoItem() { Asiento = asiento, Haber = Total, Cuenta = new Cuenta() { Codigo = this.Proveedores } };
            //asiento.Items.Add(itemEmpleado);
            
            return asiento;
        }
        
    }
}
