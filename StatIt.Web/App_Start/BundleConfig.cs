using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace StatIt.Web.App_Start
{
    public class BundleConfig
    {
         // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.css",
                     "~/Content/bootstrap-datepicker3.css",
                     "~/Content/StatIt.css"));

            bundles.Add(new ScriptBundle("~/bundles/presentation").Include(
                     "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                     "~/Scripts/respond.js",
                     "~/Scripts/knockout-3.0.0.js",
                     "~/Scripts/globalize/globalize.js",
                     "~/Scripts/dx.chartjs.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

        }
    }
}