using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebUI2
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include("~/Scripts/jquery-2.1.4.min.js")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js")
                .Include("~/Scripts/MyScripts.js")
                .Include("~/Scripts/ShippingFormScripts.js")
                .Include("~/Scripts/CardFormValidation.js")
                .Include("~/Scripts/PopupDialogScript.js")
                .Include("~/Scripts/SearchProductAjax.js")
                .Include("~/Scripts/LazyLoading.js"));

            bundles.Add(new StyleBundle("~/bundles/styles") 
                .Include("~/Content/SiteCSS.min.css"));

            //admin bundles
            bundles.Add(new ScriptBundle("~/bundles/adminScripts")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.min.js"));
            bundles.Add(new StyleBundle("~/bundles/adminStyles")
                .Include("~/Content/SiteCSS.min.css"));
        }
    }
}

