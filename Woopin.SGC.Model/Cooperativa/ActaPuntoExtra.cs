using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Sueldos;

namespace Woopin.SGC.Model.Cooperativa
{
    public class ActaPuntoExtra
    {
        public virtual int Id { get; set; }
        public virtual string Encabezado { get; set; }
        public virtual string Detalle { get; set; }
        [DoNotValidate]
        public virtual Acta Acta { get; set; }

        public ActaPuntoExtra()
        {
        }

    }
}
