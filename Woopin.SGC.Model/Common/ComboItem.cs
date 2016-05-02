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
    public class ComboItem
    {
        // Este es el valor de los combos. Con este se van a relacionar las entidades.
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "El valor es requerido")]
        [DisplayName("Valor")]
        public virtual string Data { get; set; }

        [DisplayName("Valor Opcional")]
        public virtual string AdditionalData { get; set; }
        public virtual bool Activo { get; set; }

        [Required(ErrorMessage = "El combo al que pertenece es requerido")]
        [DisplayName("Combo Nombre")]
        [DoNotValidateOnlyId]
        public virtual Combo Combo { get; set; }

        [DisplayName("Valor Afip")]
        public virtual string AfipData { get; set; }


        public ComboItem()
        {
            this.Activo = true;
        }
    }
}
