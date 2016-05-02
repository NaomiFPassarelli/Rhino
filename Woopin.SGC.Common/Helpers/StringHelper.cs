using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woopin.SGC.Common.Helpers
{
    public static class StringHelper
    {

        public static string ReplaceUTF8Characters(this string str)
        {
            var returnString = str;

            byte[] bytes = Encoding.Default.GetBytes(returnString);
            returnString = Encoding.UTF8.GetString(bytes);
           
            return returnString;
        }

        public static string CleanFileName(this string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }
}
