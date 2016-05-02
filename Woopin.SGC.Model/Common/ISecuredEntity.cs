using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{
    public interface ISecuredEntity
    {
        Organizacion Organizacion { get; set; }
    }
}
