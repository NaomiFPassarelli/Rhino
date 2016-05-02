using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.LinqHelpers;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Compras;
using Woopin.SGC.Model.Exceptions;

namespace Woopin.SGC.Model.Negocio
{
    public static class ComprobanteCompraHelper
    {
        public const int SinComprobante = 80;
        public const int Factura = 81;
        public const int NotaDebito = 82;
        public const int NotaCredito = 83;
        public const int Ticket = 84;
        public const int TicketFactura = 85;
        public static ComprobanteCompra ReMap(ComprobanteCompra comprobante)
        {
            decimal subtotal = 0;
            decimal iva105 = 0;
            decimal iva21 = 0;
            decimal iva27 = 0;
            decimal ImporteExento = 0;
            decimal percepcionesIVA = 0;
            decimal percepcionesIIBB = 0;
            decimal ImporteNoGravado = 0;

            foreach (var detalle in comprobante.Detalle)
            {
                detalle.Comprobante = comprobante; // Importante! Mantiene relacion.
                subtotal += detalle.Total;

                if (detalle.RubroCompra.PercepcionIIBB)
                {
                    percepcionesIIBB += detalle.Total;
                }
                else if (detalle.RubroCompra.PercepcionIVA)
                {
                    percepcionesIVA += detalle.Total;
                }
                else
                {
                    switch (detalle.TipoIva.AdditionalData)
                    {
                        case "10.5":
                            iva105 += detalle.IVA;
                            break;
                        case "21":
                            iva21 += detalle.IVA;
                            break;
                        case "27":
                            iva27 += detalle.IVA;
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
            }

            comprobante.Subtotal = decimal.Round(subtotal, 2, MidpointRounding.AwayFromZero);
            comprobante.IVA105 = decimal.Round(iva105, 2, MidpointRounding.AwayFromZero);
            comprobante.IVA21 = decimal.Round(iva21, 2, MidpointRounding.AwayFromZero);
            comprobante.IVA27 = decimal.Round(iva27, 2, MidpointRounding.AwayFromZero);
            decimal iva = comprobante.IVA21 + comprobante.IVA105 + comprobante.IVA27;
            comprobante.IVA = decimal.Round(iva, 2, MidpointRounding.AwayFromZero);
            comprobante.Total = decimal.Round(comprobante.Subtotal + comprobante.IVA, 2, MidpointRounding.AwayFromZero);
            comprobante.ImporteNoGravado = decimal.Round(ImporteNoGravado, 2, MidpointRounding.AwayFromZero);
            comprobante.ImporteExento = ImporteExento;
            comprobante.PercepcionesIIBB = percepcionesIIBB;
            comprobante.PercepcionesIVA = percepcionesIVA;
            comprobante.FechaCreacion = DateTime.Now;
            comprobante.FechaVencimiento = comprobante.Fecha.AddDays(Convert.ToInt32(comprobante.CondicionCompra.AdditionalData));

            // Validaciones correspondientes
            if (comprobante.IVA > 0 && (comprobante.Letra == "E" || comprobante.Letra == "C"))
                throw new BusinessException("Los comprobantes E y C, no discriminan IVA.");

            return comprobante;
        }

    }
}
