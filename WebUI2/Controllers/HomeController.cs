using Model;
using Model.Abstracts;
using Model.EF;
using Model.Entities;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebUI2.Binders;
using WebUI2.Models;

namespace WebUI2.Controllers
{
    /// <summary>
    /// Provides for navigation among main pages of the website and some basic functionality
    /// for such pages as Home, ContactUs and AboutUs
    /// </summary>
    public class HomeController : Controller
    {
        private IRepository repository;
        private IMailProcessor mailProcessor;
      


        public HomeController(IRepository repository, IMailProcessor processor)
        {
            this.repository = repository;
            this.mailProcessor = processor;           
        }



        public ActionResult Home()
        {
            IEnumerable<string> imagesNames = repository.GetSlidesImages();
            return View(imagesNames);
        }


        [HttpPost]
        public ActionResult ContactUs(InquiryInfo inq)
        {
            if (ModelState.IsValid)
            {
                ProcessInquiry(inq);
                TempData["Confirm"] = "Your Message has been successfully sent";
                return View();
            }
            else
            {
                TempData["Fail"] = "Unfortunately submission of your inquiry failed";
                return View();
            }
        }


        private void ProcessInquiry(InquiryInfo inq)
        {
            inq.RecepientAddress = "tyrkamotchka@mail.ru";
            inq.RecepientName = "Krystsina Kasnikouskaya";
            inq.FullMessage = "Hello, my name is " + inq.Title + " " + inq.FirstName + " " + inq.LastName;
            inq.FullMessage += ". I would like to inquire about " + inq.Subject;
            inq.FullMessage += ". Following are my contact details : Contact number - ";
            inq.FullMessage += inq.Telephone;
            inq.FullMessage += "; Email address - ";
            inq.FullMessage += inq.EmailAddress;
            inq.FullMessage += "; Country of residence - ";
            inq.FullMessage += inq.Country;
            inq.FullMessage += ". The inquiry: " + inq.Message;

            mailProcessor.SendMail(inq);
        }



        public ActionResult AboutUs()
        {
            return View(new InquiryInfo());
        }











        public PartialViewResult ViewsSummary()
        {
            RecentlyViewed views =
               (RecentlyViewed)(new RecentViewsModelBinder()).BindModel(this.ControllerContext, new ModelBindingContext());

            return PartialView(views.GetRecentViews());
        }





        public FileContentResult GetBackImage()
        {
            BackgroundImage im = repository.BackgroundImages.Last();
            if (im != null)
            {
                return File(im.ImageData, im.ImageMimeType);
            }
            else
            {
                return null;
            }
        }




        public ActionResult PrintReceipt(int id)
        {
            Order order = repository.GetOrderByOrderId(id);
            PDFFactory fact = new PDFFactory(order, Server);
            fact.BuildFile(Response);
            return File(fact.BuildFile(Response), "application/pdf","filename.pdf");
        }

        
        public ActionResult ContactUs()
        {
            return View();
        }




    }
}