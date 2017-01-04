using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Contabilidad
{
    public class LibroMayor
    {
        public virtual DateTime Fecha { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        public virtual string Leyenda { get; set; }
        public virtual string Modulo { get; set; }
        public virtual Ejercicio Ejercicio { get; set; }
        [DisplayName("N° Referencia")]
        public virtual int NumeroReferencia { get; set; }
        public virtual decimal Debe { get; set; }
        public virtual decimal Haber { get; set; }
        public virtual int NumeroCheque { get; set; }
        public virtual TipoOperacion TipoOperacion { get; set; }
        public virtual int ComprobanteAsociado { get; set; }
    }
}
