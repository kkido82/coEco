using System.Web;
using System.Web.Optimization;
using AspNetBundling;

namespace CoEco.BO.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/vendor").Include(
            "~/Scripts/jquery-2.1.4.min.js",
            "~/Scripts/tinymce.min.js",
            "~/Scripts/bootstrap.js",
            "~/Scripts/app.js",
            "~/Scripts/jquery.mCustomScrollbar.concat.min.js",
            "~/Scripts/respond.js",
           "~/Scripts/breeze.debug.js",
            "~/Scripts/angular.js",
            "~/Scripts/angular-gravatar.js",
            "~/Scripts/angular-ui/ui-bootstrap.js",
            "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
            "~/Scripts/moment.js",
            "~/Scripts/daterangepicker.js",
            "~/Scripts/angular-ui-router.js",
            "~/Scripts/angular-animate.js",
            "~/Scripts/angular-messages.js",
            "~/Scripts/angular-local-storage.js",
            "~/Scripts/breeze.bridge.angular.js",
            "~/Scripts/loading-bar.js",
            "~/Scripts/angular-touch.js",
            "~/Scripts/angular-slider.js",
            "~/Scripts/angular-sanitize.js",
            "~/Scripts/angular-toastr.tpls.js",
            "~/Scripts/angular-aria.js",
            "~/Scripts/aside-menu.js",
            "~/Scripts/angular-file-upload.js",
            "~/Scripts/tinymce.js",
            "~/Scripts/ng-quick-date.js",
            "~/Scripts/angular-daterangepicker.js",
            "~/Scripts/scrollbars.js",
            "~/Scripts/vkbeautify.js"
          ));

            bundles.Add(new ScriptWithSourceMapBundle("~/bundles/app").Include(
                "~/App/app.min.js"
));

            bundles.Add(new StyleBundle("~/Content/styles").Include(
          "~/Content/bootstrap.css",
          "~/Content/site.css",
          "~/Content/angular-dropdownMultiselect.css"));
        }
    }
}