using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Woopin.SGC.Web.HtmlHelper
{
    public static class SecurityHelper
    {
        public static MvcHtmlString SecuredActionLI<TModel>(this HtmlHelper<TModel> htmlHelper,string permission,string action,string controller,string area)
        {
            MvcHtmlString returnHtml = null;

            return returnHtml;
        }
        
    }
}