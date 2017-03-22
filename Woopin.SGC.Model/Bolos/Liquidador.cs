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
using Woopin.SGC.Model.Exceptions;

namespace Woopin.SGC.Model.Bolos
{
    public class Liquidador : ISecuredEntity
    {
        public virtual int Id { get; set; }
        [DisplayName("N° Referencia")]
        public virtual int NumeroReferencia { get; set; }

        [Required(ErrorMessage = "Es Necesario el Trabajador")]
        [DoNotValidateOnlyId]
        public virtual Trabajador Trabajador { get; set; }

        //Pelicula
        [Required(ErrorMessage = "Es Necesario el Bolo")]
        [DoNotValidateOnlyId]
        public virtual Bolo Bolo { get; set; }

        [DisplayName("Observación")]
        public virtual string Observacion { get; set; }

        [Required(ErrorMessage = "La Fecha Desde es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Desde")]
        public virtual DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "La Fecha Hasta es requerida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Hasta")]
        public virtual DateTime FechaHasta { get; set; }

        [DisplayName("Fecha Creación")]
        public virtual DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "Es Necesario un Número de Recibo")]
        [DisplayName("Número")]
        public virtual string Numero { get; set; }

        //[DisplayName("IVA 21%")]
        //public virtual decimal IVA21 { get; set; }

        //[DisplayName("IVA 10,5%")]
        //public virtual decimal IVA105 { get; set; }

        //[DisplayName("IVA 27%")]
        //public virtual decimal IVA27 { get; set; }

        //[DisplayName("Importe Exento")]
        //public virtual decimal ImporteExento { get; set; }

        //[DisplayName("Importe No Gravado")]
        //public virtual decimal ImporteNoGravado { get; set; }
        public virtual decimal Subtotal { get; set; }
        public virtual decimal IVA { get; set; }
        public virtual decimal Total { get; set; }
        //public virtual decimal TotalPagado { get; set; }
        //public virtual decimal PercepcionesIIBB { get; set; }
        //public virtual decimal PercepcionesIVA { get; set; }
        public virtual IList<DetalleLiquidador> Detalle { get; set; }

        [Required(ErrorMessage = "Es Necesario un Período")]
        [DisplayName("Período")]
        public virtual string Periodo { get; set; }
        

        //[DoNotValidate]
        //public virtual Usuario Usuario { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public virtual bool Activo { get; set; }
        [DisplayName("Dias Normales")]
        public virtual int DiasNormales { get; set; }
        [Required(ErrorMessage = "Es Necesario una Fecha de Último Depósito")]
        [DisplayName("Fecha Último Depósito")]
        public virtual DateTime FechaUltimoDeposito { get; set; }
        public Liquidador() 
        {
            this.Activo = true;
            this.FechaCreacion = DateTime.Now;
        }

    }
}
