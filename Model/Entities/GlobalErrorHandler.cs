using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Model.Entities
{
   public class GlobalErrorHandler : System.Web.HttpApplication
    {


        void Application_Error(object sender, EventArgs args) 
        {
            Response.Clear();
            if (isAjaxRequest())
            {
                Response.Write("{\"error\":\"some error\"}");
            }
            else
            {
            
                Response.Redirect("/Home/");
            }
            Server.ClearError();
        }

        private bool isAjaxRequest()
        {
           bool ajax = (Request["X-Requested-With"] == "XMLHttpRequest") ||
                (Request.Headers != null && Request.Headers["X-Requested-With"] == "MLHttpRequest");

           return ajax;
        }






    }
}
