using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Woopin.SGC.Common.Models;
using Woopin.SGC.Services;
using Woopin.SGC.Services.Common;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;
using Woopin.SGC.Model.Ventas;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Web.PDF;
using System.Web.Mvc;
using System.Web.UI;
using Woopin.SGC.Common.App.Logging;
using Woopin.SGC.Model.Exceptions;


namespace Woopin.SGC.Web.Scheduler
{
    public class ComprobanteVentaJobs
    {
        private readonly ISystemService systemService;
        private readonly IVentasService ventasService;

        public ComprobanteVentaJobs(ISystemService systemService, IVentasService ventasService)
        {
            this.systemService = systemService;
            this.ventasService = ventasService;
        }

        public void EnviarComprobante(int IdComprobante, JobHeader header)
        {
            // Initialize job sessiondata
            this.systemService.InitializeSessionData(header);

            ComprobanteVenta comprobante = this.ventasService.GetComprobanteVentaCompleto(IdComprobante);
            
            WMail mail = new WMail();
            mail.To = new List<string>();
            string EmailCC = comprobante.MailCobro;

            if (EmailCC == null) 
                throw new ValidationException("No posee email para destinatario");

            mail.To.AddRange(EmailCC.Split(';').ToList());
            mail.Subject = "Envio de Comprobante";
            mail.From = comprobante.Organizacion.RazonSocial;
            mail.IsHtml = true;
            string path = AppDomain.CurrentDomain.BaseDirectory + "EmailTemplates\\ComprobanteVenta.html";
            StreamReader streamReader = new StreamReader(path);
            string message = streamReader.ReadToEnd();
            streamReader.Close();
            message = message.Replace("@@ClienteRazonSocial@@", comprobante.Cliente.RazonSocial);
            message = message.Replace("@@OrganizacionRazonSocial@@", comprobante.Organizacion.RazonSocial);
            message = message.Replace("@@MailOrganizacion@@", comprobante.Organizacion.Email);
            mail.Message = message;
            
            mail.Attachments = new List<WMailAttachment>();

            WMailAttachment archivo = new WMailAttachment();
            
            string NombreArchivo = comprobante.GetLetraNumero() + ".pdf";

            string RutaArchivo = AppDomain.CurrentDomain.BaseDirectory + "\\App_Data\\ComprobantesVenta\\" + comprobante.Organizacion.Id.ToString() + "\\" + NombreArchivo;
            archivo.FilePath = RutaArchivo;
            mail.Attachments.Add(archivo);

            EmailerService service = new EmailerService();
            service.SendEmail(mail);

            this.ventasService.UpdateEnvioEmail(IdComprobante);

        }
    }
}