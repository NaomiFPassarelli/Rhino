using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Woopin.SGC.Web.PDF
{
    public class PDFEvents : PdfPageEventHelper
    {
        public Image imageHeader { get; set; }
        public Image imageFooter { get; set; }
        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
        }

        // Agrega cabecera y/o footer al pdf
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            // cell height 
            float cellHeight = document.TopMargin;
            // PDF document size      
            Rectangle page = document.PageSize;
            PdfPTable head = null;
            PdfPCell c = null;


            if (this.imageHeader != null)
            {
                // create two column table
                head = new PdfPTable(1);
                head.TotalWidth = page.Width;
                // add image; PdfPCell() overload sizes image to fit cell
                c = new PdfPCell(imageHeader, true);
                c.HorizontalAlignment = Element.ALIGN_RIGHT;
                c.FixedHeight = cellHeight;
                c.Border = PdfPCell.NO_BORDER;
                head.AddCell(c);
                // since the table header is implemented using a PdfPTable, we call
                // WriteSelectedRows(), which requires absolute positions!
                head.WriteSelectedRows(
                  0, -1,  // first/last row; -1 flags all write all rows
                  0,      // left offset
                    // ** bottom** yPos of the table
                  page.Height - cellHeight + head.TotalHeight,
                  writer.DirectContent
                );
            }


            if (this.imageFooter != null)
            {
                head = new PdfPTable(1);
                head.TotalWidth = page.Width;
                imageFooter.ScaleAbsoluteWidth(page.Width);
                // add image; PdfPCell() overload sizes image to fit cell
                c = new PdfPCell(imageFooter, true);
                c.HorizontalAlignment = Element.ALIGN_RIGHT;
                c.FixedHeight = document.BottomMargin;
                head.WidthPercentage = 100;
                c.Border = PdfPCell.NO_BORDER;
                head.AddCell(c);
                // since the table header is implemented using a PdfPTable, we call
                // WriteSelectedRows(), which requires absolute positions!
                head.WriteSelectedRows(
                  0, -1,  // first/last row; -1 flags all write all rows
                  0,      // left offset
                    // ** bottom** yPos of the table
                  document.BottomMargin,
                  writer.DirectContent
                );
            }

        }


        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    } 
}