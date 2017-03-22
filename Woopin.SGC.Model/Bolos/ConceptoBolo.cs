using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Contabilidad;

namespace Woopin.SGC.Model.Bolos
{
    //Lo que va a usar el liquidador - en cada liquidador detalle - pero con el valor especifico para ese
    //trabajador, para esa pelicula. etc
    //es como el rubro para el detalle del comprobante de compras - del area compras
 
    public class ConceptoBolo : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [DisplayName("Descripción")]
        public virtual string Descripcion { get; set; }
        public virtual string AdditionalDescription { get; set; }
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Porcentaje debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal? Porcentaje { get; set; }
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Valor debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal? Valor { get; set; }
        public virtual bool Suma { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        
        public virtual bool OnlyAutomatic { get; set; } //true se puede agregar solo automaticamente por el sistema

        //[Required(ErrorMessage = "La cuenta es requerida")]
        //[DoNotValidate]
        //public virtual Cuenta Cuenta { get; set; }

        //[DisplayName("Percepcion IIBB")]
        //public virtual bool PercepcionIIBB { get; set; }
        //[DisplayName("Percepcion IVA")]
        //public virtual bool PercepcionIVA { get; set; }
        public virtual bool Activo { get; set; }

        public ConceptoBolo()
        {
            this.Activo = true;
            this.OnlyAutomatic = false;
        }

        
    }
    
}
