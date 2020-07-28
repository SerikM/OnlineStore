using Model;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebUI2.App_Start;
using WebUI2.Binders;
using WebUI2.Infrastructure;

namespace WebUI2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(ShoppingCart), new CartModelBinder());      
        }

       protected void Application_Error(object sender, EventArgs args)
       {
            Server.ClearError();
            Response.Clear();
            if (isAjaxRequest())
            {
                Response.Write("error");
            }
            else
            {
                Response.RedirectToRoute("ErrorRoute", new { action="GlobalError"});
            }
      
           
        }

        private bool isAjaxRequest()
        {
            bool ajax = (Request["X-Requested-With"] == "XMLHttpRequest") ||
                 (Request.Headers["X-Requested-With"] == "XMLHttpRequest");

            return ajax;
        }



    }
}


