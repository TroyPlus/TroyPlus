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

            #region Css files
            // Icons 
            bundles.Add(new StyleBundle("~/Content/themes/icons").Include(
                        "~/Content/themes/TroyPlus/css/icons.css"));
            // jQueryUI 
            //bundles.Add(new StyleBundle("~/Content/themes/jqueryUI").Include(
            //                "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.all.css"));

            bundles.Add(new StyleBundle("~/Content/themes/jqueryUI").Include(
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.core.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.resizable.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.selectable.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.accordion.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.autocomplete.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.button.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.dialog.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.slider.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.tabs.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.datepicker.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.progressbar.css",
                       "~/Content/themes/TroyPlus/css/sprflat-theme/jquery.ui.theme.css"));


            // Bootstrap stylesheets (included template modifications) 
            bundles.Add(new StyleBundle("~/Content/themes/bootstrap").Include(
                            "~/Content/themes/TroyPlus/css/bootstrap.css"));
            // Plugins stylesheets (all plugin custom css) 
            bundles.Add(new StyleBundle("~/Content/themes/plugins").Include(
                            "~/Content/themes/TroyPlus/css/plugins.css"));
            // Main stylesheets (template main css file) 
            bundles.Add(new StyleBundle("~/Content/themes/mainStyles").Include(
                            "~/Content/themes/TroyPlus/css/main.css"));
            // Custom stylesheets ( Put your own changes here ) 
            bundles.Add(new StyleBundle("~/Content/themes/custom").Include(
                            "~/Content/themes/TroyPlus/css/custom.css"));          

            #endregion

            #region Javascripts

            bundles.Add(new ScriptBundle("~/bundles/script/jquery").Include("~/Content/themes/TroyPlus/js/libs/jquery-2.1.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/script/jqueryUI").Include("~/Content/themes/TroyPlus/js/libs/jquery-ui-1.10.4.min.js"));

            // Load pace first -->
            bundles.Add(new ScriptBundle("~/bundles/script/jPace").Include("~/Content/themes/TroyPlus/plugins/core/pace/pace.min.js"));
            // Important javascript libs(put in all pages) -->
            bundles.Add(new ScriptBundle("~/bundles/script/jLibs").Include("~/Content/themes/TroyPlus/js/jquery-2.1.1.min.js"
                , "~/Content/themes/TroyPlus/js/jquery-ui-js.js",
                "~/Content/themes/TroyPlus/js/libs/jquery-ui-1.10.4.min.js"
                ));
            // UI Validation 
            bundles.Add(new ScriptBundle("~/bundles/script/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Bootstrap plugins -->
            bundles.Add(new ScriptBundle("~/bundles/script/bootstrap").Include("~/Content/themes/TroyPlus/js/bootstrap/bootstrap.js"));
            // Core plugins ( not remove ever) -->
            // Handle responsive view functions -->
            bundles.Add(new ScriptBundle("~/bundles/script/jRespond").Include("~/Content/themes/TroyPlus/js/jRespond.min.js"));
            // Custom scroll for sidebars,tables and etc. -->
            bundles.Add(new ScriptBundle("~/bundles/script/jSlimScrollBar").Include("~/Content/themes/TroyPlus/plugins/core/slimscroll/jquery.slimscroll.min.js"
                , "~/Content/themes/TroyPlus/plugins/core/slimscroll/jquery.slimscroll.horizontal.min.js"));
            // Resize text area in most pages -->
            bundles.Add(new ScriptBundle("~/bundles/script/jAutoSize").Include("~/Content/themes/TroyPlus/plugins/forms/autosize/jquery.autosize.js"));
            // Proivde quick search for many widgets -->
            bundles.Add(new ScriptBundle("~/bundles/script/jSearch").Include("~/Content/themes/TroyPlus/plugins/core/quicksearch/jquery.quicksearch.js"));
            // Bootbox confirm dialog for reset postion on panels -->
            bundles.Add(new ScriptBundle("~/bundles/script/jBootbox").Include("~/Content/themes/TroyPlus/plugins/ui/bootbox/bootbox.js"));


            // Common scripts for master data pages.
            bundles.Add(new ScriptBundle("~/bundles/script/commonScripts").Include(
                "~/Content/themes/TroyPlus/plugins/core/moment/moment.min.js",
                "~/Content/themes/TroyPlus/plugins/charts/sparklines/jquery.sparkline.js",
                "~/Content/themes/TroyPlus/plugins/charts/pie-chart/jquery.easy-pie-chart.js",
                "~/Content/themes/TroyPlus/plugins/forms/icheck/jquery.icheck.js",
                "~/Content/themes/TroyPlus/plugins/forms/tags/jquery.tagsinput.min.js",
                "~/Content/themes/TroyPlus/plugins/forms/tinymce/tinymce.min.js",
                "~/Content/themes/TroyPlus/plugins/tables/datatables/jquery.dataTables.min.js",
                "~/Content/themes/TroyPlus/plugins/tables/datatables/jquery.dataTablesBS3.js",
                "~/Content/themes/TroyPlus/plugins/tables/datatables/tabletools/ZeroClipboard.js",
                "~/Content/themes/TroyPlus/plugins/tables/datatables/tabletools/TableTools.js",
                "~/Content/themes/TroyPlus/plugins/misc/highlight/highlight.pack.js",
                "~/Content/themes/TroyPlus/plugins/misc/countTo/jquery.countTo.js",
                "~/Content/themes/TroyPlus/js/jquery.sprFlat.js",
                "~/Content/themes/TroyPlus/js/app.js",
                "~/Content/themes/TroyPlus/js/pages/data-tables.js",
                "~/Content/themes/TroyPlus/js/libs/jQuery-placeholder.js"
                ));

            #endregion
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;   
        }
    }
}
