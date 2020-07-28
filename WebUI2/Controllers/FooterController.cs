using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI2.Controllers
{
    public class FooterController : Controller
    {

        public  PartialViewResult FooterContent()
        {

            return PartialView();
        }
	}
}