using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;
using Woopin.SGC.Model.Common;

namespace Woopin.SGC.Model.Sueldos
{
    public class Recibo : ISecuredEntity
    {
        public virtual int Id { get; set; }

        public virtual Empleado Empleado { get; set; }
        //[DoNotValidate]
        //del empleado porque si se llega a modificar el sind, obra social o bco deposito del mismo
            //despues el recibo va a tener informacion erronea
        public virtual string Sindicato { get; set; }

        [DisplayName("Obra Social")]
        //[DoNotValidate]
        public virtual string ObraSocial { get; set; }
        [DisplayName("Banco de Deposito")]
        //[DoNotValidate]
        public virtual string BancoDeposito { get; set; }
        public virtual Organizacion Organizacion { get; set; }
        //public virtual IList<Adicional> Adicionales { get; set; }
        public virtual IList<AdicionalRecibo> AdicionalesRecibo { get; set; }
        [DisplayName("Total Remunerativo")]
        public virtual decimal TotalRemunerativo { get; set; }
        [DisplayName("Total No Remunerativo")]
        public virtual decimal TotalNoRemunerativo { get; set; }
        [DisplayName("Total Descuento")]
        public virtual decimal TotalDescuento { get; set; }
        public virtual decimal Total { get; set; }
        public virtual DateTime FechaCreacion { get; set; }
        [DisplayName("Fecha Inicio")]
        public virtual DateTime FechaInicio { get; set; }
        [DisplayName("Fecha Fin")]
        public virtual DateTime FechaFin { get; set; }
        public virtual DateTime Periodo { get; set; }
        public virtual string Observacion { get; set; }
        [DisplayName("Numero Referencia")]
        public virtual int NumeroReferencia { get; set; }
        public virtual decimal DomicilioEmpresa { get; set; }

        //Estado - creado, pagado, habria que ver si se puede anular
        //Asiento

        [DisplayName("Tipo de Recibo")]
        public virtual ComboItem TipoRecibo { get; set; }

        public Recibo()
        {

        }
    }
}
