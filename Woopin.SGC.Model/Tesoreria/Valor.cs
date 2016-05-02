using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Tesoreria
{
    public class Valor : ISecuredEntity
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        public virtual string Nombre { get; set; }
        
        [Required(ErrorMessage = "El tipo de Valor es requerido")]
        [DisplayName("Tipo de Valor")]
        [DoNotValidateOnlyId]
        public virtual ComboItem TipoValor { get; set; }

        [Required(ErrorMessage = "La Moneda es requerida")]
        [DoNotValidateOnlyId]
        public virtual Moneda Moneda { get; set; }

        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }


        public Valor()
        {
            this.Activo = true;
        }
    }


    public static class TipoValor
    {
        public const string TarjetaCredito = "Tarjeta de Credito";
        public const string Efectivo = "Efectivo";
        public const string Cheque = "Cheque";
        public const string ChequePropio = "Cheque Propio";
        public const string Retencion = "Retencion";
        public const string Transferencia = "Transferencia";
    }
}
