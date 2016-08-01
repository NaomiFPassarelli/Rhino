using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Cooperativa
{
    public class Asociado : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Nombre")]
        public virtual string Nombre { get; set; }

        [Required(ErrorMessage = "Es Necesario un Apellido")]
        public virtual string Apellido { get; set; }

        [DisplayName("Dirección")]
        public virtual string Direccion { get; set; }

        [DisplayName("Número")]
        public virtual string Numero { get; set; }
        public virtual string Piso { get; set; }
        public virtual string Departamento { get; set; }

        [DisplayName("Codigo Postal")]
        public virtual string CodigoPostal { get; set; }

        [DoNotValidate]
        public virtual Localizacion Localizacion { get; set; }

        [DoNotValidate]
        public virtual ComboItem Nacionalidad { get; set; }

        //[DisplayName("Estado Civil")]
        //[DoNotValidate]
        //public virtual ComboItem EstadoCivil { get; set; }

        //[DoNotValidate]
        //public virtual ComboItem Sexo { get; set; }

        public virtual string Telefono { get; set; }

        //[DataType(DataType.EmailAddress)]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
        //ErrorMessage = "El email no es valido.")]
        //public virtual string Email { get; set; }

        [Required(ErrorMessage = "Es Necesario un CUIT")]
        [RegularExpression(@"[0-9]{2}-[0-9]{8}-[0-9]{1}",
        ErrorMessage = "El CUIT no es valido.")]
        public virtual string CUIT { get; set; }

        [Required(ErrorMessage = "Es Necesario un DNI")]
        [RegularExpression(@"[0-9]{8}", ErrorMessage = "El DNI no es valido.")]
        public virtual string DNI { get; set; }

        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        //[DisplayName("Cooperativa Bruto Mensual")]
        //[RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Cooperativa Bruto Mensual debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        //public virtual decimal? CooperativaBrutoMensual { get; set; }

        //[DisplayName("Cooperativa Bruto Hora")]
        //[RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Cooperativa Bruto Hora debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        //public virtual decimal? CooperativaBrutoHora { get; set; }

        public virtual DateTime FechaCreacion { get; set; }

        [DisplayName("Fecha de Ingreso")]
        [Required(ErrorMessage = "Es Necesario una Fecha de Ingreso")]
        public virtual DateTime FechaIngreso { get; set; }
        [DisplayName("Fecha de Egreso")]
        public virtual DateTime? FechaEgreso { get; set; }

        [DisplayName("Fecha de Acta Ingreso")]
        public virtual DateTime? FechaActaIngreso { get; set; }
        
        //[DisplayName("Fecha de Antiguedad Reconocidad")]
        //public virtual DateTime? FechaAntiguedadReconocida { get; set; }

        [DisplayName("Fecha de Nacimiento")]
        public virtual DateTime? FechaNacimiento { get; set; }

        //[DoNotValidate]
        //public virtual ComboItem Categoria { get; set; }

        //[DoNotValidate]
        //public virtual ComboItem Tarea { get; set; }

        //[DoNotValidate]
        //public virtual ComboItem Sindicato { get; set; }

        //[DisplayName("Obra Social")]
        //[DoNotValidate]
        //public virtual ComboItem ObraSocial { get; set; }
        
        //[DisplayName("Banco de Deposito")]
        //[DoNotValidate]
        //public virtual ComboItem BancoDeposito { get; set; }

        //[DisplayName("Beneficiario Obra Social")]
        //[RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Beneficiario de la Obra Social debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        //public virtual decimal? BeneficiarioObraSocial { get; set; }
        [DisplayName("Número de Acta del Alta")]
        public virtual int? ActaAlta { get; set; } // nro
        [DisplayName("Número de Acta de Baja")]
        public virtual int? ActaBaja { get; set; } // nro
        [DisplayName("Cantidad de Cuotas")]
        [Required(ErrorMessage = "Es Necesario una Cantidad de Cuotas")]
        public virtual int CantidadCuotas { get; set; }
        [DisplayName("Importe de cada cuota")]
        [Required(ErrorMessage = "Es Necesario un Importe de cada cuota")]
        public virtual decimal ImporteCuota { get; set; }
        public virtual bool AbonoTotalmente { get; set; }
        public virtual int CantidadCuotasAbonadas { get; set; }
        
        public Asociado()
        {
            this.Activo = true;
            this.AbonoTotalmente = false;
            this.FechaCreacion = DateTime.Now;
        }
    }
}
