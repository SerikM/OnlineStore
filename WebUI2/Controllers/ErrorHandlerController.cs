using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI2.Controllers
{
    public class ErrorHandlerController : Controller
    {
        
        public ViewResult GlobalError()
        {
            TempData["Fail"] = "Ooops! Something just went wrong.";
            return View("_ErrorDisplay");
        }
    }
}