using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Woopin.SGC.Web.PDF
{
    public static class HtmlToPDF
    {
        public static string Generate(string html, string filename, string folder)
        {
            return HtmlToPDF.Generate(html, filename,folder, null, null);
        }

        public static string Generate(string html, string filename, string folder, Image header, Image footer)
        {
            string OutputPath = HttpContext.Current.Server.MapPath("~/App_Data/" + folder);
            if (!Directory.Exists(OutputPath))
                Directory.CreateDirectory(OutputPath);
            OutputPath = Path.Combine(OutputPath, filename);
            int headerMargin = header != null ? 80 : 0;
            int footerMargin = footer != null ? 132 : 0;
            Document document = new Document(PageSize.A4, 0, 0, headerMargin, footerMargin);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(OutputPath, FileMode.Create));
            document.Open();
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(html));
            document.Close();
            return OutputPath;
        }

    }
}