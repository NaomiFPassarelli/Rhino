using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Bolos
{
    public class Empresa : ISecuredEntity
    {
        public virtual int Id { get; set; }

        //[Required(ErrorMessage = "Es Necesario una Razon Social")]
        //[DisplayName("Razón Social")]
        //public virtual string RazonSocial { get; set; }

        //[Required(ErrorMessage = "Es Necesario un CUIT")]
        //[RegularExpression(@"[0-9]{2}-[0-9]{8}-[0-9]{1}",
        //ErrorMessage = "El CUIT no es valido.")]
        //public virtual string CUIT { get; set; }

        //[Required(ErrorMessage = "Es Necesario un Domicilio")]
        //public virtual string Domicilio { get; set; }

        //[DisplayName("Código Postal")]
        //public virtual string CodigoPostal { get; set; }

        //[DoNotValidate]
        //[DisplayName("Provincia")]
        //public virtual Localizacion Localizacion { get; set; }

        //[Required(ErrorMessage = "Es Necesario un Telefono")]
        //[DisplayName("Teléfono")]
        //public virtual string Telefono { get; set; }

        //public virtual bool Activo { get; set; }
        
        
        //ES MAS INFORMACION ACERCA DE LA ORGANIZACION PERO QUE RHINO NO NECESITA
        //SOLO SIRVE PARA BOLOS
        //HAY UNA SOLA POR CADA ORGANIZACION, SERIA LO MISMO QUE LA ORGANIZACION 
        //PERO CON ALGUNOS DETALLES MAS

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        [Required(ErrorMessage= "Es Necesario el nombre del apoderado")]
        [DisplayName("Nombre del Apoderado")]
        public virtual string NombreApoderado { get; set; }
        [Required(ErrorMessage= "Es Necesario el apellido del apoderado")]
        [DisplayName("Apellido del Apoderado")]
        public virtual string ApellidoApoderado { get; set; }

        [DisplayName("Banco de Deposito")]
        [DoNotValidate]
        public virtual ComboItem BancoDeposito { get; set; }

        public Empresa()
        {
            //this.Activo = true;
        }

    }

}
