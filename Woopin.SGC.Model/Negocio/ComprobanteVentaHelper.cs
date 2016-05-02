using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Helpers;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Negocio
{
    public static class ComprobanteVentaHelper
    {
        public const int Factura = 51;
        public const int NotaCredito = 52;
        public const int NotaDebito = 53;
        public const int FEFacturaA = 54;
        public const int FEFacturaB = 55;
        public const int FEFacturaC = 56;
        public const int FENotaCreditoA = 57;
        public const int FENotaDebitoA = 60;
        public const int FENotaCreditoB = 58;
        public const int FENotaDebitoB = 61;
        public const int FENotaCreditoC = 59;
        public const int FENotaDebitoC = 62;
        public static ComprobanteVenta ReMap(ComprobanteVenta comprobante)
        {
            decimal subtotal = 0;
            decimal iva105 = 0;
            decimal iva21 = 0;
            decimal iva27 = 0;
            decimal ImporteExento = 0;
            decimal ImporteNoGravado = 0;
            decimal descuento = 0;

            foreach(var detalle in comprobante.Detalle)
            {
                decimal precioTotal = decimal.Round(detalle.Cantidad * detalle.PrecioUnitario, 2, MidpointRounding.AwayFromZero);
                if (detalle.Descuento.HasValue)
                {
                    decimal dtoItem = decimal.Round((Convert.ToDecimal(detalle.Descuento.Value) / 100) * precioTotal, 2, MidpointRounding.AwayFromZero);
                    precioTotal -= dtoItem;
                    descuento += dtoItem;
                }

                detalle.Total = precioTotal;
                detalle.TotalConIVA = detalle.Total * Convert.ToDecimal(detalle.TipoIva.AdditionalData) / 100;

                subtotal += detalle.Total;
                detalle.Comprobante = comprobante; // Importante! Mantiene la relacion bidireccional.

                switch (detalle.TipoIva.AdditionalData)
                {
                    case "10.5":
                        iva105 += detalle.Total * Convert.ToDecimal(detalle.TipoIva.AdditionalData) / 100;
                        break;
                    case "21":
                        iva21 += detalle.Total * Convert.ToDecimal(detalle.TipoIva.AdditionalData) / 100;
                        break;
                    case "27":
                        iva27 += detalle.Total * Convert.ToDecimal(detalle.TipoIva.AdditionalData) / 100;
                        break;
                    case "0":
                        ImporteExento += detalle.Total;
                        break;
                    case "-1":
                        ImporteNoGravado += detalle.Total;
                        break;
                    default:
                        throw new BusinessException("Alicuota de IVA no soportada.");
                }

            }

            comprobante.Descuento = descuento;
            comprobante.ImporteExento = decimal.Round(ImporteExento, 2, MidpointRounding.AwayFromZero);
            comprobante.ImporteNoGravado = decimal.Round(ImporteNoGravado, 2, MidpointRounding.AwayFromZero);
            comprobante.Subtotal = decimal.Round(subtotal, 2, MidpointRounding.AwayFromZero);
            comprobante.IVA105 = decimal.Round(iva105, 2, MidpointRounding.AwayFromZero);
            comprobante.IVA21 = decimal.Round(iva21, 2, MidpointRounding.AwayFromZero);
            comprobante.IVA27 = decimal.Round(iva27, 2, MidpointRounding.AwayFromZero);
            decimal iva = comprobante.IVA105 + comprobante.IVA21 + comprobante.IVA27;
            comprobante.IVA = decimal.Round(iva, 2, MidpointRounding.AwayFromZero);
            comprobante.Total = decimal.Round(comprobante.Subtotal + comprobante.IVA, 2, MidpointRounding.AwayFromZero);

            // Validaciones correspondientes
            if (comprobante.IVA > 0 && (comprobante.Letra == "E" || comprobante.Letra == "C"))
                throw new BusinessException("Los comprobantes E y C, no discriminan IVA.");

            // Verificar que si esta usando la moneda predeterminada del sistema la cotizacion sea 1.
            if (comprobante.Moneda.Predeterminado)
            {
                comprobante.Cotizacion = 1;
            }

            if (comprobante.Cliente.CondicionVentaContratada != null && comprobante.Cliente.CondicionVentaContratada > 0)
            {
                comprobante.FechaVencimiento = comprobante.Fecha.AddDays((int)comprobante.Cliente.CondicionVentaContratada);
            }
            else
            {
                comprobante.FechaVencimiento = comprobante.Fecha.AddDays(Convert.ToInt32(comprobante.CondicionVenta.AdditionalData));
            }

            comprobante.FechaCreacion = DateTime.Now;
            return comprobante;
        }

        public static string GetSemanaEstipuladaCobro(ComprobanteVenta comprobante)
        {
            DateTime fechaCobro = comprobante.FechaVencimiento;
            if (comprobante.Cliente.CondicionVentaEstadistica != null && comprobante.Cliente.CondicionVentaEstadistica > 0)
            {
                fechaCobro = comprobante.Fecha.AddDays((int)comprobante.Cliente.CondicionVentaEstadistica);
            }

            if (fechaCobro < DateHelper.GetWeekSatToFri(DateTime.Now).Start)
            {
                return "Antigüos";
            }
            else
            {
                return DateHelper.GetWeekSatToFri(fechaCobro).ToString("dd/MM/yyyy", "al");
            }                                
        }
    }
}
