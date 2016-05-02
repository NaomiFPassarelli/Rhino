using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Common.Models
{
    public class WMail
    {
        public string Subject { get; set; }
        public string From { get; set; }
        public List<string> To { get; set; }
        public bool IsHtml { get; set; }
        public string Message { get; set; }
        public List<WMailAttachment> Attachments { get; set; }

        public WMail()
        {
            this.Attachments = new List<WMailAttachment>();
            this.To = new List<string>();
        }
    }


}
