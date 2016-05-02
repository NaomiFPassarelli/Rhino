using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woopin.SGC.Common.JSON;

namespace Woopin.SGC.Common.Json
{
    public class JsonHelper
    {
        public static string ToJson(object obj)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new LazyLoadingContractResolver()
            };

            var scriptSerializer = JsonSerializer.Create(settings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, obj);
                return sw.ToString();
            }
        }
    }
}
