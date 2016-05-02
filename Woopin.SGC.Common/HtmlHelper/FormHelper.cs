using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Woopin.SGC.Common.HtmlModel;

namespace Woopin.SGC.Common.HtmlHelper
{
    public static class FormHelper
    {
        public static MvcHtmlString DropDownListForWAD<T, TValue>(this HtmlHelper<T> html, Expression<Func<T, TValue>> expression, SelectCombo combo, string placeholder, string requiredval, object htmlAttributes = null)
        {
            TagBuilder select = new TagBuilder("select");
            select.Attributes.Add("data-val", "true");
            select.Attributes.Add("data-val-required", requiredval);
            var inner = "<option value=''>" + placeholder + "</option>";
            foreach(var item in combo.Items)
            {
                if (item.selected)
                {
                    inner += "<option value='" + item.id + "' data-ad='" + item.additionalData + "'selected>" + item.text + "</option>";
                }
                else {
                    inner += "<option value='" + item.id + "' data-ad='" + item.additionalData + "'>" + item.text + "</option>";                
                }
            }

            if (htmlAttributes != null)
            {
                var attributes = System.Web.Mvc.HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                select.MergeAttributes(attributes);
            }
            select.InnerHtml = inner;
            return MvcHtmlString.Create(select.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty, TEnum>(this HtmlHelper<TModel> htmlHelper,Expression<Func<TModel, TProperty>> expression, TEnum selectedValue)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
                                        .Cast<TEnum>();
            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem()
                                                {
                                                    Text = value.ToString(),
                                                    Value = value.ToString(),
                                                    Selected = (value.Equals(selectedValue))
                                                };
            return SelectExtensions.DropDownListFor(htmlHelper, expression, items);
        }

        public static MvcHtmlString EnumDropDownList<TModel, TProperty, TEnum>(this HtmlHelper<TModel> htmlHelper, string inputName, TEnum enumType)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
                                        .Cast<TEnum>();
            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem()
                                                {
                                                    Text = value.ToString(),
                                                    Value = value.ToString()
                                                };
            return SelectExtensions.DropDownList(htmlHelper, inputName, items);
        }


    }
}
