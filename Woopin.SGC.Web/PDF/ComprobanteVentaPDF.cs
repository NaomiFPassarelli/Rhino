using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Common.Helpers;

namespace Woopin.SGC.Web.PDF
{
    public class ComprobanteVentaPDF
    {
        public ComprobanteVenta Comprobante { get; set; }
        public string Filename { get; set; }
        public string OutputPath { get; set; }
        public ComprobanteVentaPDF(ComprobanteVenta comprobante)
        {
            this.Comprobante = comprobante;
            this.Filename = this.Comprobante.GetLetraNumero() + ".pdf";
            this.OutputPath = HttpContext.Current.Server.MapPath("~/App_Data/Reportes/" +  this.Filename);
        }

        public MemoryStream GenereatePDF()
        {
            if (Comprobante.Letra == "A")
            {
                return this.GeneratePDFcmpA();
            }else if(Comprobante.Letra == "B")
            {
                return this.GeneratePDFcmpB();
            }else // Armar para E
            {
                //return new MemoryStream();
                return this.GeneratePDFcmpE();
            }
        }
        public MemoryStream GeneratePDFcmpA()
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = this.Comprobante.GetLetraNumero();
            document.Info.Author = "";
            document.Info.Subject = "";
            PdfPage page = document.AddPage();
            page.Orientation = PdfSharp.PageOrientation.Portrait;
            page.Size = PdfSharp.PageSize.A4;
            XFont font_nro = new XFont("Verdana", 16, XFontStyle.Regular);
            XFont font_14 = new XFont("Verdana", 14, XFontStyle.Regular);
            XFont font_subtitulo = new XFont("Verdana", 13, XFontStyle.Regular);
            XFont font_12 = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont font_normal = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont font_normal_bold = new XFont("Verdana", 12, XFontStyle.Bold);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);
            int currentY = 0;
            int currentX = 0;

            //gfx.DrawString(this.Comprobante.Numero.Split('-')[1], font_nro, XBrushes.Black, 413, 112, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Fecha.ToString("dd/MM/yyyy"), font_14, XBrushes.Black, 380, 148, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.RazonSocial, font_subtitulo, XBrushes.Black, 96, 240, XStringFormats.Default);
            string Direccion = this.Comprobante.Cliente.Direccion + this.Comprobante.Cliente.Numero + " - " + this.Comprobante.Cliente.Localizacion.Nombre;
            gfx.DrawString(Direccion, font_subtitulo, XBrushes.Black, 96, 268, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.CategoriaIva.Abreviatura, font_subtitulo, XBrushes.Black, 76, 303, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.CondicionVenta.Data, font_subtitulo, XBrushes.Black, 160, 322, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.CUIT, font_14, XBrushes.Black, 356, 298, XStringFormats.Default);

            currentY = 360;
            foreach (var item in this.Comprobante.Detalle)
            {
                string cantidad = "";
                if (Math.Ceiling(item.Cantidad) > item.Cantidad)
                {
                    cantidad = item.Cantidad.ToString("0.00");
                }
                else
                {
                    cantidad = ((int)item.Cantidad).ToString();
                }
                gfx.DrawString(cantidad, font_12, XBrushes.Black, 30, currentY, XStringFormats.Default);
                gfx.DrawString(item.Descripcion, font_12, XBrushes.Black, 70, currentY, XStringFormats.Default);
                gfx.DrawString(item.PrecioUnitario.ToStringArCurrency(), font_12, XBrushes.Black, 380, currentY, XStringFormats.Default);
                gfx.DrawString(item.Total.ToStringArCurrency(), font_12, XBrushes.Black, 465, currentY, XStringFormats.Default);
                currentY += 14;
            }
            foreach (var item in this.Comprobante.Observaciones)
            {
                gfx.DrawString(item.Descripcion, font_12, XBrushes.Black, 70, currentY, XStringFormats.Default);
                currentY += 13;
            }

            gfx.DrawString(this.Comprobante.Subtotal.ToStringArCurrency(), font_normal, XBrushes.Black, 55, 720, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Subtotal.ToStringArCurrency(), font_normal, XBrushes.Black, 260, 720, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.IVA21.ToStringArCurrency(), font_normal, XBrushes.Black, 365, 720, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Total.ToStringArCurrency(), font_normal, XBrushes.Black, 475, 720, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.GetLetraNumero(), font_normal, XBrushes.Black, 300, 783, XStringFormats.Default);

            // Devuelvo el stream para responder con el pdf
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            return stream;
        }


        public MemoryStream GeneratePDFcmpB()
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = this.Comprobante.GetLetraNumero();
            document.Info.Author = "";
            document.Info.Subject = "";
            PdfPage page = document.AddPage();
            page.Orientation = PdfSharp.PageOrientation.Portrait;
            page.Size = PdfSharp.PageSize.A4;
            XFont font_nro = new XFont("Verdana", 16, XFontStyle.Regular);
            XFont font_14 = new XFont("Verdana", 14, XFontStyle.Regular);
            XFont font_subtitulo = new XFont("Verdana", 13, XFontStyle.Regular);
            XFont font_12 = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont font_normal = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont font_normal_bold = new XFont("Verdana", 12, XFontStyle.Bold);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);
            int currentY = 0;
            int currentX = 0;

            //gfx.DrawString(this.Comprobante.Numero.Split('-')[1], font_nro, XBrushes.Black, 392, 105, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Fecha.ToString("dd/MM/yyyy"), font_14, XBrushes.Black, 380, 140, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.RazonSocial, font_subtitulo, XBrushes.Black, 96, 228, XStringFormats.Default);
            string Direccion = this.Comprobante.Cliente.Direccion + this.Comprobante.Cliente.Numero + " - " + this.Comprobante.Cliente.Localizacion.Nombre;
            gfx.DrawString(Direccion, font_subtitulo, XBrushes.Black, 96, 258, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.CategoriaIva.Abreviatura, font_subtitulo, XBrushes.Black, 76, 288, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.CondicionVenta.Data, font_subtitulo, XBrushes.Black, 155, 313, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.CUIT, font_14, XBrushes.Black, 356, 290, XStringFormats.Default);

            currentY = 360;
            foreach (var item in this.Comprobante.Detalle)
            {
                string cantidad = "";
                if (Math.Ceiling(item.Cantidad) > item.Cantidad)
                {
                    cantidad = item.Cantidad.ToString("0.00");
                }
                else
                {
                    cantidad = ((int)item.Cantidad).ToString();
                }
                gfx.DrawString(cantidad, font_12, XBrushes.Black, 30, currentY, XStringFormats.Default);
                gfx.DrawString(item.Descripcion, font_12, XBrushes.Black, 70, currentY, XStringFormats.Default);
                gfx.DrawString(item.PrecioUnitario.ToStringArCurrency(), font_12, XBrushes.Black, 380, currentY, XStringFormats.Default);
                gfx.DrawString(item.Total.ToStringArCurrency(), font_12, XBrushes.Black, 465, currentY, XStringFormats.Default);
                currentY += 14;
            }
            foreach (var item in this.Comprobante.Observaciones)
            {
                gfx.DrawString(item.Descripcion, font_12, XBrushes.Black, 70, currentY, XStringFormats.Default);
                currentY += 13;
            }

            //gfx.DrawString(this.Comprobante.Subtotal.ToString("C"), font_normal, XBrushes.Black, 20, 720, XStringFormats.Default);
            //gfx.DrawString(this.Comprobante.Subtotal.ToString("C"), font_normal, XBrushes.Black, 250, 720, XStringFormats.Default);
            //gfx.DrawString(this.Comprobante.IVA21.ToString("C"), font_normal, XBrushes.Black, 374, 720, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Total.ToStringArCurrency(), font_normal, XBrushes.Black, 475, 720, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.GetLetraNumero(), font_normal, XBrushes.Black, 300, 783, XStringFormats.Default);

            // Devuelvo el stream para responder con el pdf
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            return stream;
        }


        public MemoryStream GeneratePDFcmpE()
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = this.Comprobante.GetLetraNumero();
            document.Info.Author = "";
            document.Info.Subject = "";
            PdfPage page = document.AddPage();
            page.Orientation = PdfSharp.PageOrientation.Portrait;
            page.Size = PdfSharp.PageSize.A4;
            page.Height = XUnit.FromMillimeter(276); //782 pt
            XFont font_nro = new XFont("Verdana", 16, XFontStyle.Regular);
            XFont font_14 = new XFont("Verdana", 14, XFontStyle.Regular);
            XFont font_subtitulo = new XFont("Verdana", 13, XFontStyle.Regular);
            XFont font_12 = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont font_normal = new XFont("Verdana", 12, XFontStyle.Regular);
            XFont font_normal_bold = new XFont("Verdana", 12, XFontStyle.Bold);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);
            int currentY = 0;
            int currentX = 0;

            //gfx.DrawString(this.Comprobante.Numero.Split('-')[1], font_nro, XBrushes.Black, 400, 80, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Fecha.ToString("dd/MM/yyyy"), font_14, XBrushes.Black, 390, 90, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.RazonSocial, font_subtitulo, XBrushes.Black, 76, 168, XStringFormats.Default);
            string Direccion = this.Comprobante.Cliente.Direccion + this.Comprobante.Cliente.Numero + " - " + this.Comprobante.Cliente.Localizacion.Nombre;
            gfx.DrawString(Direccion, font_subtitulo, XBrushes.Black, 76, 205, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.CategoriaIva.Abreviatura, font_subtitulo, XBrushes.Black, 76, 238, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.CondicionVenta.Data, font_subtitulo, XBrushes.Black, 140, 255, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Cliente.CUIT, font_14, XBrushes.Black, 362, 238, XStringFormats.Default);

            currentY = 300;
            foreach (var item in this.Comprobante.Detalle)
            {
                string cantidad = "";
                if (Math.Ceiling(item.Cantidad) > item.Cantidad)
                {
                    cantidad = item.Cantidad.ToString("0.00");
                }
                else
                {
                    cantidad = ((int)item.Cantidad).ToString();
                }
                gfx.DrawString(cantidad, font_12, XBrushes.Black, 20, currentY, XStringFormats.Default);
                gfx.DrawString(item.Descripcion, font_12, XBrushes.Black, 64, currentY, XStringFormats.Default);
                gfx.DrawString(item.PrecioUnitario.ToStringArCurrency(), font_12, XBrushes.Black, 392, currentY, XStringFormats.Default);
                gfx.DrawString(item.Total.ToStringArCurrency(), font_12, XBrushes.Black, 485, currentY, XStringFormats.Default);
                currentY += 14;
            }
            foreach (var item in this.Comprobante.Observaciones)
            {
                gfx.DrawString(item.Descripcion, font_12, XBrushes.Black, 54, currentY, XStringFormats.Default);
                currentY += 13;
            }

            gfx.DrawString(this.Comprobante.Subtotal.ToStringArCurrency(), font_normal, XBrushes.Black, 20, 672, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Subtotal.ToStringArCurrency(), font_normal, XBrushes.Black, 250, 672, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.IVA21.ToStringArCurrency(), font_normal, XBrushes.Black, 374, 672, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.Total.ToStringArCurrency(), font_normal, XBrushes.Black, 492, 672, XStringFormats.Default);
            gfx.DrawString(this.Comprobante.GetLetraNumero(), font_normal, XBrushes.Black, 225, 723, XStringFormats.Default);

            // Devuelvo el stream para responder con el pdf
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            return stream;
        }

    }
}