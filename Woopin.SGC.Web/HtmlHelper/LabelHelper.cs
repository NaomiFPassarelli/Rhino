using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Woopin.SGC.Common.HtmlModel;
using Woopin.SGC.Model.Common;
using Woopin.SGC.Model.Tesoreria;

namespace Woopin.SGC.Web.HtmlHelper
{
    public static class LabelHelper
    {
        public static MvcHtmlString LabelStatus<TModel>(this HtmlHelper<TModel> htmlHelper, string status, int statusNumber, bool dialog)
        {
            TagBuilder divStatus = new TagBuilder("div");
            TagBuilder divTextStatus = new TagBuilder("div");
            string colorLabelRibbon = null;
            string colorLabel = null;

            switch (statusNumber) 
            {
                case -1://Anulada
                    colorLabelRibbon = "red";
                    colorLabel = "danger";
                    break;
                case 0:
                    //Borrador
                    colorLabelRibbon = "grey";
                    colorLabel = "default";
                    break;
                case 1:
                case 2:
                case 3:
                    //Creada
                    //Cartera
                    //emitida
                    colorLabelRibbon = "blue";
                    colorLabel = "primary";
                    break;
                case 4:
                    //cancelando
                    colorLabelRibbon = "yellow";
                    colorLabel = "yellow";
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 13:
                    //Pagada
                    //Cobrada
                    //Recibida
                    //Entregada
                    //Depositada
                    //Imputado
                    colorLabelRibbon = "green";
                    colorLabel = "success";
                    break;
                case 11:
                case 12:
                    //Vencida
                    //Devuelta
                    colorLabelRibbon = "red";
                    colorLabel = "danger";
                    break;

            }
            
            if (dialog)
            {
                divStatus.AddCssClass("ribbon-wrapper ribbon-top-right");
                divTextStatus.AddCssClass("ribbon " + colorLabelRibbon);
                divStatus.Attributes.Add("style", "right: 1.6%; top: 3%; z-index: 1;");
            }
            else {
                divTextStatus.AddCssClass("label label-" + colorLabel);
                divTextStatus.Attributes.Add("style", "font-size: 13px; float: right; line-height: 20px;");
                divStatus.Attributes.Add("style", "float: right;");
            }
            divTextStatus.SetInnerText(status);
            divStatus.InnerHtml += divTextStatus.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(divStatus.ToString(TagRenderMode.Normal));
        }
    }
}
