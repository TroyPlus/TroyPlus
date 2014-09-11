using System.Web;
using System.Web.Optimization;

namespace Troy.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerydata").Include(
                        "~/Scripts/jquery.dataTables.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstraptable").Include(
                        "~/Scripts/dataTables.bootstrap.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/Site").Include(
                                               "~/Content/Site.css"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                                               "~/Content/bootstrap-switch.css",
                                               "~/Content/bootstrap.css",
                                               "~/Content/responsive.css",
                                               "~/Content/custom.css"));

            bundles.Add(new StyleBundle("~/Content/jquerycustom").Include("~/Content/jquery-ui-1.10.4.custom.min.css"));

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
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));


            bundles.Add(new StyleBundle("~/bundles/themes/TroyPlus").Include(
                "~/Content/themes/TroyPlus/css/less/sidebar.less",
                "~/Content/themes/TroyPlus/css/less/bootstrap.less",
                "~/Content/themes/TroyPlus/css/less/custom-variables.less",
                "~/Content/themes/TroyPlus/css/less/app.less",
                "~/Content/themes/TroyPlus/css/less/plugins/highlight.less",
                "~/Content/themes/TroyPlus/css/bootstrap.css",
                "~/Content/themes/TroyPlus/css/plugins.css",
                "~/Content/themes/TroyPlus/css/main.css",
                "~/Content/themes/TroyPlus/css/custom.css",
                "~/Content/themes/TroyPlus/css/icons.css",
                "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.all.css",
                "~/Content/themes/TroyPlus/plugins/misc/highlight/styles/"));

            // Wrapbootstrap
            bundles.Add(new ScriptBundle("~/bundles/scripts/TroyPlus").
                Include(
            "~/Content/themes/TroyPlus/plugins/core/quicksearch/jquery.quicksearch.js",
            "~/Content/themes/TroyPlus/plugins/misc/highlight/highlight.pack.js",
            "~/Content/themes/TroyPlus/js/html5shiv.js",
            "~/Content/themes/TroyPlus/js/bootstrap/bootstrap.js",
            "~/Content/themes/TroyPlus/js/jquery.sprFlat.js",
            "~/Content/themes/TroyPlus/js/jRespond.min.js",
            "~/Content/themes/TroyPlus/js/bootstrap/bootstrap.js",
            "~/Content/themes/TroyPlus/plugins/core/moment/moment.min.js",
             "~/Content/themes/TroyPlus/js/app.js",
             "~/Content/themes/TroyPlus/plugins/core/pace/pace.min.js",
             "~/Content/themes/TroyPlus/plugins/core/slimscroll/jquery.slimscroll.min.js",
             "~/Content/themes/TroyPlus/plugins/core/slimscroll/jquery.slimscroll.horizontal.min.js",
             "~/Content/themes/TroyPlus/plugins/forms/autosize/jquery.autosize.js",
             "~/Content/themes/TroyPlus/plugins/ui/bootbox/bootbox.js",
             "~/Content/themes/TroyPlus/plugins/misc/countTo/jquery.countTo.js",
             "~/Content/themes/TroyPlus/plugins/forms/icheck/jquery.icheck.js",
             "~/Content/themes/TroyPlus/js/jRespond.min.js",
             "~/Content/themes/TroyPlus/plugins/forms/tinymce/tinymce.min.js",
             "~/Content/themes/TroyPlus/plugins/forms/tags/jquery.tagsinput.min.js",
             "~/Content/themes/TroyPlus/plugins/core/moment/moment.min.js",
             "~/Content/themes/TroyPlus/plugins/forms/tags/jquery.tagsinput.min.js",
             "~/Content/themes/TroyPlus/plugins/tables/datatables/jquery.dataTables.min.js",
            "~/Content/themes/TroyPlus/plugins/tables/datatables/jquery.dataTablesBS3.js",
            "~/Content/themes/TroyPlus/plugins/tables/datatables/tabletools/ZeroClipboard.js",
            "~/Content/themes/TroyPlus/plugins/tables/datatables/tabletools/TableTools.js"
             ));
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            //BundleTable.EnableOptimizations = true;
        }
    }
}
