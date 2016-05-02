using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Models;

namespace Woopin.SGC.Services.Common
{
    public class EmailerService
    {
        private SmtpClient SmtpClient { get; set; }

        public EmailerService()
        {
            this.SmtpClient = new SmtpClient(ConfigurationManager.AppSettings["smtp"].ToString());
            SmtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            SmtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["usuario"].ToString(), ConfigurationManager.AppSettings["password"].ToString());
            SmtpClient.EnableSsl = true;
        }

        public void SendEmail(WMail mail)
        {
            MailMessage mmail = new MailMessage();
            mmail.IsBodyHtml = mail.IsHtml;
            mmail.Subject = mail.Subject;
            mmail.Bcc.Add("naomi.passarelli@woopin.com.ar");
            foreach(var to in mail.To)
            {
                mmail.To.Add(to);
            }
            mmail.From = new MailAddress(ConfigurationManager.AppSettings["usuario"].ToString(), mail.From);
            if (mail.IsHtml)
            {
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Message, null, "text/html");
                mmail.AlternateViews.Add(htmlView);
            }
            else
            {
                mmail.Body = mail.Message;
            }

            foreach(var at in mail.Attachments)
            {
                Attachment newAttach = new Attachment(at.FilePath);
                if (at.HtmlObjID != null)
                {
                    newAttach.ContentDisposition.Inline = true;
                    newAttach.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    newAttach.ContentId = at.HtmlObjID;
                    newAttach.ContentType.MediaType = "image/png";
                    newAttach.ContentType.Name = "Logo.png";
                }
                
                mmail.Attachments.Add(newAttach);
            }

            this.SmtpClient.Send(mmail);
            mmail.Dispose();
        }
    }
}
