using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Common.HtmlModel
{
    public class SelectComboItem
    {
        public virtual int id { get; set; }
        public virtual string text { get; set; }
        public virtual string additionalData { get; set; }
        public virtual bool selected { get; set; }

        public SelectComboItem()
        {
            this.selected = false;
        }
    }
}
