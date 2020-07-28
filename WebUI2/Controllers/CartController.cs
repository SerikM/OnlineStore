using Model;
using Model.Abstracts;
using Model.EF;
using Model.Entities;
using Model.ExternalAuthentication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebUI2.Models;
using WebUI2.SearchAddress;




namespace WebUI2.Controllers
{

    /// <summary>
    /// Processes all user actions related to their cart and items placed in the cart.
    /// Prepares data necessary for checkout and performs address and model validation
    /// </summary>
    public class CartController : Controller
    {
        IRepository repo;
        IPaymentProcessor paymentProcessor;
        IMailProcessor mailSender;



        public CartController(IRepository repo, IPaymentProcessor paymentProcessor, IMailProcessor mailSender)
        {
            this.repo = repo;
            this.paymentProcessor = paymentProcessor;
            this.mailSender = mailSender;
        }




        // we get the cart object from the cartModelBinder
        public String AddItemAjax(ShoppingCart cart, int productId, string returnUrl, string returnUrl2, string size, string color)
        {
            Product product = repo.Products.FirstOrDefault(p => p.ProductID == productId);
            product.SelectedSize =( size ?? repo.Sizes.First().Value);
            product.SelectedColour = (color ?? repo.Colors.First().Value);
            cart.AddItem(product);
            string str = "Your Cart:  " + cart.ProductLines.Sum(x => x.Quantity) + " item(s): " +
            cart.ComputeCartValue().ToString("c");

            return str;
        }




        public RedirectResult AddItem(ShoppingCart cart, int productId, string returnUrl, string returnUrl2)
        {
            Product product = repo.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            cart.AddItem(product);

            if (returnUrl != null)
            { return Redirect(returnUrl); }

            else
            { return Redirect(returnUrl2); }
        }



        public RedirectToRouteResult RemoveItem(ShoppingCart cart, int productId, string returnUrl)
        {
            Product product = repo.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            cart.RemoveItem(product);

            return RedirectToAction("Index", new { returnUrl });
        }


        public ViewResult Index(ShoppingCart cart, string returnUrl)
        {
            CartInfo cartInfo = new CartInfo();
            cartInfo.Cart = cart;
            cartInfo.ReturnUrl = returnUrl;
            return View(cartInfo);
        }

        public PartialViewResult CartSummary(ShoppingCart cart)
        {
            return PartialView(cart);
        }





        public ViewResult Checkout(ShoppingCart cart)
        {
            string countryName = RegionInfo.CurrentRegion.DisplayName;

            if (cart.ProductLines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry your cart is empty");
            }

            if (ModelState.IsValid)
            {
                // this message will be used by popup dialog window
                ViewData["message"] = "Would you like to update your details?";
                // user is logged in - use their details
                if (Session["Profile"] != null)
                {
                    // initialize objects to be passed to the view
                    ShippingInfo shipInfo = InitializeModel(repo);
                    return View(shipInfo);
                }
                else
                {   // if the user is not registered return a blank class
                    return View(new ShippingInfo(repo) { ShipDet = new ShippingDetails { Country = countryName } });
                }
                

            }
            else
            {
                CartInfo inf = new CartInfo
                {
                    Cart = cart,
                    ReturnUrl = "/"
                };
                return View("Index", inf);
            }
        }





        private ShippingInfo InitializeModel(IRepository repo)
        {

            ApplicationUser prof = (ApplicationUser)Session["Profile"];

            ShippingInfo shipInfo = new ShippingInfo(repo)
            {
                ShipDet = new ShippingDetails()
                {
                    Name = prof.Name,
                    Surname = prof.Surname,
                    Email = prof.Email,
                    Phone = prof.Phone
                }
            };


            if (prof.ShippingAddressId != null)
            {
                ShippingAddress adr = prof.ShippingAddress.Where(p => p.RecordId == prof.ShippingAddressId).First();
                // if address data is present fill the model

                shipInfo.ShipDet.UnitFlat = adr.UnitNumber;
                shipInfo.ShipDet.Town = adr.Town;
                shipInfo.ShipDet.StreetType = adr.StreetType;
                shipInfo.ShipDet.StreetNumber = adr.StreetNumber;
                shipInfo.ShipDet.StreetName = adr.StreetName;
                shipInfo.ShipDet.State = adr.State;
                shipInfo.ShipDet.PostCode = adr.PostCode;
                shipInfo.ShipDet.Country = adr.Country;
            }

            return shipInfo;
        }





        [HttpPost]
        public ViewResult ProceedToPay(ShoppingCart cart, ShippingDetails ShipDet, string updateUser)
        {
            
            if (ModelState.IsValid)
            {
                CartShipPayInfo cspInfo = new CartShipPayInfo();
                // prepare for credit card payment
                PrepareCreditCardFields();

                if (updateUser == null ? false : updateUser.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                {
                    // updates the current user details and stores it in the session state
                    Session["Profile"] = repo.UpdateUserDetails(ShipDet, User.Identity.Name);
                }
                cspInfo.ShoppingCart = cart;
                cspInfo.ShippingDetails = ShipDet;
                Session["CartShipPayInfo"] = cspInfo; // save the whole order data class into session state
                ViewData["cartValue"] = cart.ComputeCartValue().ToString("c");

                return View(new CreditCardInfo());
            }
            else
            {
                return View("Checkout", new ShippingInfo(repo) { ShipDet = ShipDet });
            }
        }








        private void PrepareCreditCardFields()
        {
            IEnumerable<string> list = new List<string>
              {
                "month",
                "01",
                "02" ,
                "03" ,
                "04" ,
                "05" ,
                "06" ,
                "07" ,
                "08" ,
                "09" ,
                "10" ,
                "11" ,
                "12" 
              };



            IEnumerable<string> list2 = new List<string>
            {
                "year",
                "2014",
                "2015",
                "2016",
                "2017",
                "2018",
                "2019",
                "2020",
                "2021",
                "2022",
                "2023",
                "2024",
                "2025" 
        };


            ViewData["months"] = new SelectList(list, list.First());
            ViewData["years"] = new SelectList(list2, list2.First());
        }




        public ViewResult RedirectAfterCancel()
        {
            return View();
        }








        public PartialViewResult GetShipAddr(string StreetNumber, string StreetName, string StreetType,
                                                 string PostCode, string Town, string State, string Country)
        {
            AddressSearchClient client = new AddressSearchClient();
            AddressResult result = client.findAddress(StreetNumber, StreetName, StreetType, Town, PostCode, null);


            if (result.numRecs != 0)
            {
                return PartialView("_GetShipAddr", result.addresses);
            }
            else return PartialView("_GetShipAddr", result.addresses);
        }



        public ViewResult GetTermsAndConditions()
        {
            return View();
        }

        public ViewResult GetPrivacyPolicy()
        {
            return View();
        }

    }
}

