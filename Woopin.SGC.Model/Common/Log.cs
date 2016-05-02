using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.Validations;

namespace Woopin.SGC.Model.Common
{
    public class Log : ISecuredEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Thread { get; set; }
        public virtual string Level { get; set; }
        public virtual string Logger { get; set; }
        public virtual string Message { get; set; }
        public virtual string Exception { get; set; }

        [DoNotValidate]
        public virtual Usuario Usuario { get; set; }

        [DoNotValidate]
        public virtual Organizacion Organizacion { get; set; }

        public Log()
        {

        }
    }

    public enum LogType
    {
        Error,
        Debug,
        Info,
        Warning
    }
}
