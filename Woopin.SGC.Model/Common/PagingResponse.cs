using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Model.Common
{
    public class PagingResponse<T>
    {
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public IList<T> Records { get; set; }
        public int TotalRecords { get; set; }
        public dynamic userdata { get; set; }
    }
}
