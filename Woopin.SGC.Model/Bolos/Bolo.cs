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
    public class Bolo : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario un Domicilio")]
        public virtual string Domicilio { get; set; }

        [Required(ErrorMessage = "Es Necesario un Código Postal")]
        [DisplayName("Código Postal")]
        public virtual string CodigoPostal { get; set; }

        [DoNotValidate]
        [DisplayName("Provincia")]
        public virtual Localizacion Localizacion { get; set; }

        public virtual bool Activo { get; set; }
        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }
        [Required(ErrorMessage = "Es Necesario el nombre")]
        [DisplayName("Nombre")]
        public virtual string Nombre { get; set; }

        [Required(ErrorMessage = "Es Necesario la Denominación del Producto")]
        [DisplayName("Denominación del Producto")]
        public virtual string DenominacionProducto { get; set; }
        
        [Required(ErrorMessage = "Es Necesario la Denominación de la Película")]
        [DisplayName("Denominación de la Película")]
        public virtual string DenominacionPelicula { get; set; }

        [DisplayName("Fecha de liquidación")]
        public virtual DateTime FechaLiquidacion { get; set; }

        [DisplayName("Tope mínimo")]
        public virtual decimal TopeMinimo { get; set; }
        [DisplayName("Tope máximo")]
        public virtual decimal TopeMaximo { get; set; }

        [Required(ErrorMessage = "Es Necesario la denominación de la agencia")]
        [DisplayName("Denominación de la agencia")]
        public virtual string Agencia { get; set; }

        [Required(ErrorMessage = "Es Necesario la denominación del anunciante")]
        [DisplayName("Denominación del anunciante")]
        public virtual string Anunciante { get; set; }


        public Bolo()
        {
            this.Activo = true;
        }
    }
}
