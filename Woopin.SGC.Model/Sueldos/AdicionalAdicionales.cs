//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Woopin.SGC.Common.Validations;
//using Woopin.SGC.Model.Common;

//namespace Woopin.SGC.Model.Sueldos
//{
//    public class AdicionalAdicionales : ISecuredEntity
//    //public class AdicionalAdicionales
//    {
//        public virtual int Id { get; set; }
//        //[DoNotValidate]
//        //public virtual Recibo Recibo { get; set; }
//        //[DoNotValidate]
//        //public virtual Adicional Adicional { get; set; }
//        //public virtual IList<Adicional> Adicionales { get; set; }
//        //[DoNotValidateOnlyId]
//        //public virtual Adicional? AdicionalSobre { get; set; }
//        public virtual bool Default { get; set; }

//        [DoNotValidate]
//        public virtual Organizacion Organizacion { get; set; }
//        public AdicionalAdicionales()
//        {
//            this.Default = false;
//        }
//    }
//}





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
    public class AdicionalAdicionales : ISecuredEntity
    {
        public virtual int Id { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        [DoNotValidate]
        public virtual Recibo Recibo { get; set; }
        [DoNotValidateOnlyId]
        public virtual Adicional Adicional { get; set; }
        //[DoNotValidateOnlyId]
        //public virtual IList<Adicional> Adicionales { get; set; }
        [DoNotValidateOnlyId]
        public virtual Adicional AdicionalSobre { get; set; }
        public virtual bool EsDefault { get; set; }

        public AdicionalAdicionales()
        {
            this.EsDefault = false;
        }
    }
}
