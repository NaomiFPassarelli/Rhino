using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;

namespace Woopin.SGC.Model.Common
{
    public class CategoriaIVA
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "La Abreviatura es requerida")]
        public virtual string Abreviatura { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido")]
        public virtual string Nombre { get; set; }
        public virtual bool Discrimina { get; set; }
        [DisplayName("Liquida Internos")]
        public virtual bool LiquidaInternos { get; set; }

        //Combo Comprobantes
        [Required(ErrorMessage = "La Letra de Comprobante es requerida")]
        [DisplayName("Letra para Compras")]
        [DoNotValidateOnlyId]
        public virtual ComboItem LetraCompras { get; set; }

        [Required(ErrorMessage = "La Letra de Comprobante es requerida")]
        [DisplayName("Letra para Ventas")]
        [DoNotValidateOnlyId]
        public virtual ComboItem LetraVentas { get; set; }

        [DisplayName("Exento de Iva")]
        public virtual bool ExentoIva { get; set; }

        //Combo Responsabilidades
        [Required(ErrorMessage = "La Responsabilidad Afip es requerida")]
        [DisplayName("Responsabilidad Afip")]
        public virtual string ResponsabilidadAfip { get; set; }

        public virtual bool Activo { get; set; }
        public virtual bool Predeterminado { get; set; }

        public CategoriaIVA()
        {
            this.Activo = true;
        }
    }
}
