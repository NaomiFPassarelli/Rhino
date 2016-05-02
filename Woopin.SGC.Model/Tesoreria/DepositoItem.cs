using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Ventas;

namespace Woopin.SGC.Model.Tesoreria
{
    public class DepositoItem
    {
        public virtual int Id { get; set; }

        public virtual Cheque Cheque { get; set; }
    }
}
