using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Model.Tesoreria
{
    public class MovimientoFondo : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "La Cuenta Bancaria es requerida")]
        [DisplayName("Cuenta Bancaria")]
        [DoNotValidateOnlyId]
        public virtual CuentaBancaria CuentaBancaria { get; set; }

        [DisplayName("Cuenta Destino")]
        [DoNotValidate]
        public virtual CuentaBancaria CuentaDestino { get; set; }

        [DisplayName("Caja")]
        [DoNotValidate]
        public virtual Caja Caja { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        [DoNotValidateOnlyId]
        public virtual ComboItem Movimiento { get; set; } //1 Deposito 0 Extraccion (en value del comboitem)

        [Required(ErrorMessage = "La Fecha de realizacion es requerida")]
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Realizacion")]
        public virtual DateTime Fecha { get; set; }
        
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [IsDateAfter("Fecha", ErrorMessage = "La fecha de final debe ser posterior a la de inicio")]
        [DisplayName("Fecha de Acreditacion")]
        public virtual Nullable<DateTime> FechaAcredita { get; set; } //Solo en caso de ser Deposito

        public virtual string Concepto { get; set; }

        [Required(ErrorMessage = "El Importe es requerido")]
        [RegularExpression("^[0-9]+(.[0-9]+)?$", ErrorMessage = "El importe debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales ")]
        public virtual decimal Importe { get; set; }

        public virtual DateTime FechaCreacion { get; set; }

        public virtual Asiento Asiento { get; set; }
        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        public MovimientoFondo()
        { 
        }
    }
}
