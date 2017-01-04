using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;

namespace Woopin.SGC.Model.Ventas
{
    public class ObservComprobanteVenta
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Es Necesario una Descripción")]
        public virtual string Descripcion { get; set; }


        public ObservComprobanteVenta() 
        {
 
        }
    }
}
