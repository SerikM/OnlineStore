using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebUI2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                             "ErrorRoute",
                             "ErrorHandler/{action}",
                             new { controller = "ErrorHandler" }
                           );


            routes.MapRoute(
                "Default", 
                "{controller}/{action}",
                new
                   {
                     controller = "Home",
                     action = "Home"
                  });

        }
    }
}

