using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Ventas
{
    public class Talonario : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Es Necesario una Descripción")]
        [DisplayName("Descripción")]
        public virtual string Descripcion { get; set; }

        [DisplayName("Inicio de Actividad")]
        public virtual DateTime? InicioActividad { get; set; }

        [Required(ErrorMessage = "Es necesario el prefijo")]
        [RegularExpression("^[0-9]{0,4}$", ErrorMessage = "Prefijo invalido")]
        public virtual string Prefijo { get; set; }

        [DisplayName("Punto de Venta")]
        [RegularExpression("^[0-9]{0,4}$", ErrorMessage = "Punto de venta invalido")]
        public virtual int? PuntoVenta { get; set; }

        [DisplayName("Certificado Afip")]
        public virtual string CertificadoPath { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public virtual bool Activo { get; set; }

        public Talonario()
        {
            this.Activo = true;
        }


        public virtual void Validate(bool hasFile, bool isFE)
        {
            if (!isFE)
            {
                this.PuntoVenta = null;
                this.CertificadoPath = null;
            }

            if (isFE && (this.PuntoVenta == null || (this.CertificadoPath == null && !hasFile)))
            {
                throw new Model.Exceptions.ValidationException("Es necesario el certificado emitido por la AFIP y el punto de venta.");
            }
        }

    }
}
