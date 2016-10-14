using System.Web.Optimization;

namespace ClarabridgeChallenge.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Scripts Bundles
            bundles.Add(new ScriptBundle("~/ContentScript/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/ContentScript/angularJS").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-sanitize.js",
                "~/Scripts/ngDialog.js"));

            bundles.Add(new ScriptBundle("~/ContentScript/ckeditor").Include(
                "~/Scripts/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/ContentScript/signalR").Include(
                "~/Scripts/jquery.signalR-{version}.js",
                "~/signalr/hubs"));

            bundles.Add(new ScriptBundle("~/ModalsAndModules/Base").Include(
                "~/Modules/Base/*.js",
                "~/Modules/Directives/*.js"));

            bundles.Add(new ScriptBundle("~/ModalsAndModules/messageBox").Include(
                "~/Modules/messageBox.js"));

            bundles.Add(new ScriptBundle("~/ModalsAndModules/pressRelease").Include(
                "~/Modules/pressRelease.js"));


            //CSS LESS Bundles
            bundles.Add(new LessBundle("~/ContentLess/angularJS").Include(
                "~/Content/css/ngDialog/ngDialog.css",
                "~/Content/css/ngDialog/themes/ngDialog-theme-default.css"));

            bundles.Add(new LessBundle("~/ContentLess/Clarabridge").Include(
                "~/Content/css/clarabridge/master.css"));

            bundles.Add(new LessBundle("~/ContentLess/indexLess").Include(
                "~/Content/css/variables.less",
                "~/Content/css/index.less",
                "~/Content/css/dialog.less",
                "~/Content/css/messageBox.less",
                "~/Content/themes/base/jquery-ui.css",
                "~/Content/themes/base/jquery.datetimepicker.css"));

            bundles.Add(new LessBundle("~/ContentLess/pressReleaseLess").Include(
                "~/Content/css/pressRelease.less"));

            bundles.Add(new LessBundle("~/Fonts/MontserratLess").Include(
                "~/Content/fonts/Montserrat.less"));

        }
    }
}
