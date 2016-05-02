using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Tesoreria
{
    public class HistorialCaja : ISecuredEntity
    {
        public virtual int Id { get; set; }
        public virtual Caja Caja { get; set; }
        public virtual decimal Importe { get; set; }
        [Required(ErrorMessage = "Es Necesario un Concepto")]
        public virtual string Concepto { get; set; }
        public virtual decimal Saldo { get; set; }
        public virtual DateTime Fecha { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        public HistorialCaja()
        {

        }

        public HistorialCaja(string Concepto, decimal Importe, decimal Saldo, Caja caja)
        {
            this.Concepto = Concepto;
            this.Importe = Importe;
            this.Saldo = Saldo;
            this.Caja = caja;
            this.Fecha = DateTime.Now;
        }
    }
}
