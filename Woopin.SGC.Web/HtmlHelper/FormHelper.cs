using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Web.HtmlHelper
{
    public static class FormHelper
    {
        public static MvcHtmlString etiquetaStatus<TModel>(this HtmlHelper<TModel> htmlHelper, string status, bool dialog)
        {
            TagBuilder divStatus = new TagBuilder("div");
            TagBuilder divTextStatus = new TagBuilder("div");
            if (dialog)
            {
                divStatus.AddCssClass("ribbon-wrapper ribbon-top-right");
                divTextStatus.AddCssClass("ribbon green");
                divStatus.Attributes.Add("style", "right: 1.6%; top: 3%; z-index: 1;");
            }
            else {
                divTextStatus.AddCssClass("label label-success");
                divTextStatus.Attributes.Add("style", "font-size: 13px; float: right; line-height: 20px;");
                divStatus.Attributes.Add("style", "float: right;");
            }
            divTextStatus.SetInnerText(status);
            divStatus.InnerHtml += divTextStatus.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(divStatus.ToString(TagRenderMode.Normal));
        }
    }
}
