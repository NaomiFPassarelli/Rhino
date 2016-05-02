using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Model.Exceptions;
using Woopin.SGC.Model.Negocio;

namespace Woopin.SGC.Model.Common
{
    public class ReporteCitiItem
    {
        public DateTime Fecha { get; set; }
        public DateTime FechaVto { get; set; }
        public string TipoComprobante { get; set; }
        public string CodigoAfipComprobante { get; set; }
        public int IdTipoComprobante { get; set; }
        public string Letra { get; set; }
        public string Comprobante { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public decimal Total { get; set; }
        public decimal IVA { get; set; }
        public decimal NetoGravado21 { get; set; }
        public decimal IVA21 { get; set; }
        public decimal NetoGravado105 { get; set; }
        public decimal IVA105 { get; set; }
        public decimal NetoGravado27 { get; set; }
        public decimal IVA27 { get; set; }
        public decimal Exento { get; set; }
        public decimal NoGravado { get; set; }
        public string CodigoMoneda { get; set; }
        public decimal PercepcionesIVA { get; set; }
        public decimal PercepcionesIIBB { get; set; }
        public decimal Internos { get; set; }
        public decimal Cotizacion { get; set; }

        public string[] GetCitiRows(TipoReporteCiti tipo)
        {
            switch (tipo)
            {
                case TipoReporteCiti.Compras:
                    return new string[] { this.ToComprasRowString() };

                case TipoReporteCiti.Ventas:
                    return new string[] { this.ToVentasRowString() };

                case TipoReporteCiti.VentasAlicuotas:
                    return this.ToVentasAlicuotasString();

                case TipoReporteCiti.ComprasAlicuotas:
                    return this.ToComprasAlicuotasString();

                default:
                    throw new BusinessException("Reporte no implementado");
            }
        }

        #region Citi Compras
        public string ToComprasRowString()
        {
            string row = "";
            row += this.Fecha.ToString("yyyyMMdd");
            row += GetComprasTipoComprobante();
            if (this.Comprobante.Contains("-"))
            {
                row += Convert.ToInt32(this.Comprobante.Split('-')[0]).ToString("00000");
                row += Convert.ToInt32(this.Comprobante.Split('-')[1]).ToString("00000000000000000000");
            }
            else
            {
                row += "00000";
                row += Convert.ToInt64(this.Comprobante).ToString("00000000000000000000");
            }

            row += String.Format("{0,-16}",String.Empty);
            row += "80"; // Hardcoded Cuit value
            row += Convert.ToInt64(this.CUIT.Replace("-", "")).ToString("00000000000000000000");
            string razonSocial = this.RazonSocial.Length > 30 ? this.RazonSocial.Substring(0, 30) : this.RazonSocial;
            row += String.Format("{0,-30}", razonSocial); // Nombre del Proveedor!
            row += Convert.ToInt32(this.Total * 100).ToString("000000000000000");
            row += Convert.ToInt32(this.NoGravado * 100).ToString("000000000000000"); // Importe no gravado
            row += "000000000000000"; // Percep. a no categ.
            row += Convert.ToInt32(this.Exento * 100).ToString("000000000000000"); // Exentos
            row += Convert.ToInt32(this.PercepcionesIVA * 100).ToString("000000000000000");
            row += Convert.ToInt32(this.PercepcionesIIBB * 100).ToString("000000000000000");
            row += "000000000000000"; // Percepciones Municipales
            row += Convert.ToInt32(this.Internos * 100).ToString("000000000000000");
            row += CodigoMoneda;
            row += "0001000000"; // Tipo de cambio
            row += this.GetSumAlicuotas().ToString();
            row += GetCodigoOperacion(); // Codigo de Operacion
            row += "000000000000000"; // Credito Fiscal Computable 
            row += "000000000000000"; // Otros tributos
            row += "00000000000"; // CUIT Corredor
            row += String.Format("{0,-30}", String.Empty); // Corredor
            row += "000000000000000"; // Comision.
            return row;
        }

        public string[] ToComprasAlicuotasString()
        {
            List<string> result = new List<string>();
            string row = "";
            row += GetComprasTipoComprobante();
            if (this.Comprobante.Contains("-"))
            {
                row += Convert.ToInt32(this.Comprobante.Split('-')[0]).ToString("00000");
                row += Convert.ToInt32(this.Comprobante.Split('-')[1]).ToString("00000000000000000000");
            }
            else
            {
                row += "00000";
                row += Convert.ToInt64(this.Comprobante).ToString("00000000000000000000");
            }
            row += "80"; // Doc del CUIT
            row += Convert.ToInt64(this.CUIT.Replace("-", "")).ToString("00000000000000000000");

            string itemRow;
            if (this.IVA21 > 0)
            {
                itemRow = row;
                itemRow += (this.NetoGravado21 * 100).ToString("000000000000000");
                itemRow += "0005"; // Valor AFIP para 21%
                itemRow += (this.IVA21 * 100).ToString("000000000000000");
                result.Add(itemRow);
            }

            if (this.IVA27 > 0)
            {
                itemRow = row;
                itemRow += (this.NetoGravado27 * 100).ToString("000000000000000");
                itemRow += "0006"; // Valor AFIP para 27%
                itemRow += (this.IVA27 * 100).ToString("000000000000000");
                result.Add(itemRow);
            }

            if (this.IVA105 > 0)
            {
                itemRow = row;
                itemRow += (this.NetoGravado105 * 100).ToString("000000000000000");
                itemRow += "0004"; // Valor AFIP para 10,5%
                itemRow += (IVA105 * 100).ToString("000000000000000");
                result.Add(itemRow);
            }

            if (this.Exento > 0)
            {
                itemRow = row;
                itemRow += "000000000000000";
                itemRow += "0003"; // Valor AFIP para 0%
                itemRow += "000000000000000";
                result.Add(itemRow);
            }


            return result.ToArray();
        }
        #endregion

        #region Citi Ventas
        public string ToVentasRowString()
        {
            string row = "";
            row += this.Fecha.ToString("yyyyMMdd");
            row += GetVentasTipoComprobante();
            row += Convert.ToInt32(this.Comprobante.Split('-')[0]).ToString("00000");
            row += Convert.ToInt32(this.Comprobante.Split('-')[1]).ToString("00000000000000000000");
            row += Convert.ToInt32(this.Comprobante.Split('-')[1]).ToString("00000000000000000000");
            row += "80"; // Hardcoded Cuit value
            row += Convert.ToInt64(this.CUIT.Replace("-", "")).ToString("00000000000000000000");
            row += "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"; // Nombre del cliente!
            row += Convert.ToInt32(this.Total * 100).ToString("000000000000000");
            row += Convert.ToInt32(this.NoGravado * 100).ToString("000000000000000"); // No Gravado
            row += "000000000000000"; // Percep. a no categ.
            row += Convert.ToInt32(this.Exento * 100).ToString("000000000000000"); ; // Exentos
            row += Convert.ToInt32(this.PercepcionesIVA * 100).ToString("000000000000000");
            row += Convert.ToInt32(this.PercepcionesIIBB * 100).ToString("000000000000000");
            row += "000000000000000"; // Percepciones Municipales
            row += Convert.ToInt32(this.Internos * 100).ToString("000000000000000");
            row += CodigoMoneda;
            row += Convert.ToInt32(this.Cotizacion * 1000000).ToString("0000000000"); // TCC
            row += this.GetSumAlicuotas().ToString(); // Cantidad de alicuotas en Comprobante
            row += GetCodigoOperacion(); // Codigo Operacion
            row += "000000000000000"; // Otros tributos
            row += this.FechaVto.ToString("yyyyMMdd");
            return row;
        }
        public string[] ToVentasAlicuotasString()
        {
            List<string> result = new List<string>();
            string row = "";
            row += GetVentasTipoComprobante();
            row += Convert.ToInt32(this.Comprobante.Split('-')[0]).ToString("00000");
            row += Convert.ToInt32(this.Comprobante.Split('-')[1]).ToString("00000000000000000000");
            
            string itemRow;
            if (this.IVA21 > 0)
            {
                itemRow = row;
                itemRow += (this.NetoGravado21 * 100).ToString("000000000000000");
                itemRow += "0005"; // Valor AFIP para 21%
                itemRow += (this.IVA21 * 100).ToString("000000000000000");
                result.Add(itemRow);
            }

            if (this.IVA27 > 0)
            {
                itemRow = row;
                itemRow += (this.NetoGravado27 * 100).ToString("000000000000000");
                itemRow += "0006"; // Valor AFIP para 27%
                itemRow += (this.IVA27 * 100).ToString("000000000000000");
                result.Add(itemRow);
            }

            if (this.IVA105 > 0)
            {
                itemRow = row;
                itemRow += (this.NetoGravado105 * 100).ToString("000000000000000");
                itemRow += "0004"; // Valor AFIP para 10,5%
                itemRow += (IVA105 * 100).ToString("000000000000000");
                result.Add(itemRow);
            }

            if (this.Exento > 0)
            {
                itemRow = row;
                itemRow += "000000000000000";
                itemRow += "0003"; // Valor AFIP para 0%
                itemRow += "000000000000000";
                result.Add(itemRow);
            }


            return result.ToArray();
        }
        #endregion

        #region Helpers
        public int GetSumAlicuotas()
        {
            int ret = 0;
            if (IVA27 != 0) ret++;
            if (IVA105 != 0) ret++;
            if (IVA21 != 0) ret++;
            if (Exento != 0) ret++;
            return ret;
        }
        public string GetVentasTipoComprobante()
        {
            if (CodigoAfipComprobante != null) return Convert.ToInt32(this.CodigoAfipComprobante).ToString("000");
            switch (IdTipoComprobante)
            {
                case ComprobanteVentaHelper.Factura:
                    if (Letra == "A") return "001";
                    if (Letra == "B") return "006";
                    if (Letra == "B") return "011";
                    if (Letra == "M") return "051";
                    break;
                case ComprobanteVentaHelper.NotaCredito:
                    if (Letra == "A") return "003";
                    if (Letra == "B") return "008";
                    if (Letra == "B") return "013";
                    if (Letra == "M") return "053";
                    break;
                case ComprobanteVentaHelper.NotaDebito:
                    if (Letra == "A") return "002";
                    if (Letra == "B") return "007";
                    if (Letra == "B") return "012";
                    if (Letra == "M") return "052";
                    break;
                default:
                    throw new BusinessException("Tipo de Comprobante invalido.");
            }
            throw new BusinessException("Tipo de Comprobante invalido.");
        }

        public string GetComprasTipoComprobante()
        {
            if (CodigoAfipComprobante != null) return Convert.ToInt32(this.CodigoAfipComprobante).ToString("000");
            switch (IdTipoComprobante)
            {
                case ComprobanteCompraHelper.Factura:
                    if (Letra == "A") return "001";
                    if (Letra == "B") return "006";
                    if (Letra == "C") return "011";
                    if (Letra == "M") return "051";
                    break;
                case ComprobanteCompraHelper.NotaCredito:
                    if (Letra == "A") return "003";
                    if (Letra == "B") return "008";
                    if (Letra == "C") return "013";
                    if (Letra == "M") return "053";
                    break;
                case ComprobanteCompraHelper.NotaDebito:
                    if (Letra == "A") return "002";
                    if (Letra == "B") return "007";
                    if (Letra == "C") return "012";
                    if (Letra == "M") return "052";
                    break;
                case ComprobanteCompraHelper.Ticket:
                    return "083";
                case ComprobanteCompraHelper.TicketFactura:
                    if (Letra == "A") return "081";
                    if (Letra == "B") return "082";
                    if (Letra == "M") return "118";
                    break;
            }
            throw new BusinessException("Tipo de Comprobante invalido.");
        }

        public string GetCodigoOperacion()
        {
            if (Exento > 0) return "E";
            if (IVA > 0) return " ";
            if (NoGravado > 0) return "N";
            return " ";

            /*
             ---- Otras opciones -----
             Z- Importaciones de la zona franca.
             X- Importaciones del Exterior.
             E- Operaciones Exentas.
             N- No gravado.
             C- Operaciones de Canje
            */

        }
        #endregion
    }

    public enum TipoReporteCiti
    {
        Ventas = 1,
        VentasAlicuotas = 2,
        Compras = 3,
        ComprasAlicuotas = 4
    }
}
