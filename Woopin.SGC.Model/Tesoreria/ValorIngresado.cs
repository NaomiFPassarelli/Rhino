using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Model.Tesoreria
{
    public class ValorIngresado : ISecuredEntity
    {
        public virtual int Id { get; set; }
        public virtual int IdEntidadExt { get; set; }
        public virtual Valor Valor { get; set; }
        public virtual int NumeroReferencia { get; set; }
        [Required(ErrorMessage = "Es Necesario una Descripcion")]
        public virtual string Descripcion { get; set; }
        public virtual string NumeroOperacion { get; set; }
        public virtual decimal Importe { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        public virtual Cuenta CuentaContable { get; set; }
        public virtual TipoIngreso TipoIngreso { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        public ValorIngresado()
        {
            FechaCreacion = DateTime.Now;
        }
    }



    public enum TipoIngreso
    {
        Egreso = -1,
        Ingreso = 1
    }
}
