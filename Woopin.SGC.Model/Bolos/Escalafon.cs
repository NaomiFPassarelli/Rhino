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
    public class Escalafon : ISecuredEntity
    {
        public virtual int Id { get; set; }
        //public virtual int NumeroReferencia { get; set; }
        [DisplayName("Descripción de la tarea")]
        [Required(ErrorMessage = "Es Necesario un Descripcion de la tarea")]
        public virtual string Descripcion { get; set; }

        public virtual bool Activo { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        [DisplayName("Salario")]
        [RegularExpression("^[0-9]+[0-9]*(.[0-9]{0,2})?$", ErrorMessage = "El Salario debe ser un número mayor a cero, puede contener el caracter punto (.) y con dos decimales")]
        public virtual decimal Salario { get; set; }

        //[DoNotValidate]
        //public virtual ComboItem Categoria { get; set; }
        [DisplayName("Vigencia Desde")]
        public virtual DateTime VigenciaDesde { get; set; }
        [DisplayName("Vigencia Hasta")]
        public virtual DateTime VigenciaHasta { get; set; }
        //[DisplayName("Marca de Vigencia")]
        //public virtual bool MarcaVigencia { get; set; }
        //[DisplayName("Resolución")]
        //public virtual string Resolucion { get; set; }
        public Escalafon()
        {
            this.Activo = true;
            //this.MarcaVigencia = false;
        }
    }
}
