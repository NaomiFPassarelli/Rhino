using System.Web;
using System.Web.Optimization;

namespace Woopin.SGC.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/metro.js",
                        "~/Scripts/common.functions.js",
                        "~/Scripts/dialog.functions.js",
                        "~/Scripts/form.functions.js",
                        "~/Scripts/jquery.validations.extensions.js",
                        "~/Scripts/ui.functions.js",
                        "~/Scripts/accounting.js",
                        "~/Scripts/Hoe/hoe.js",
                        "~/Scripts/nanoscroller/jquery.nanoscroller.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqgrid").Include(
                "~/Scripts/jqgrid/jquery.jqGrid.src.js",
                "~/Scripts/jqgrid/i18n/grid.locale-es.js",
                "~/Scripts/grid.functions.js",
                "~/Scripts/export.excel.functions.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                        "~/Scripts/select2/select2.js",
                        "~/Scripts/select2/locale/select2_locale_es.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/metro-bootstrap.css",
                        "~/Content/font-awesome.css",
                        "~/SCripts/nanoscroller/nanoscroller.css",
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/hoe").Include(
                        "~/Scripts/Hoe/hoe-overlay-effect.css",
                        "~/Scripts/Hoe/hoe-shrink-effect.css",
                        "~/Scripts/Hoe/hoe-push-effect.css",
                        "~/Scripts/Hoe/hoe-rightsidebar.css",
                        "~/Scripts/Hoe/hoe-horizontal-navigation.css",
                        "~/Scripts/Hoe/hoe-theme-color.css",
                        "~/Scripts/Hoe/extra.css",
                        "~/Scripts/Hoe/hoe.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css"));
        }
    }
}