using Model;
using Model.EF;
using Model.Entities;
using Model.ExternalAuthentication;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebUI2.Models;

namespace WebUI2.Controllers
{

    /// <summary>
    /// Prepares a credit card payment reuest to be sent to the PayPal's PayFlow payment gateway.
    /// Verifies the response received from the PayPal server and stores order information in the database
    /// </summary>
    public class PayFlowDirectController : Controller
    {
      IRepository repo;
     

       public PayFlowDirectController(IRepository repo)
       {
          this.repo = repo;
       }



        // use following test details for FDMS Nashvillle processor 4111111111111111 (a 4 and fifteen 1’s), expiration date 12/15, card security code 123.




        public ActionResult Set(CreditCardInfo card)
        {
            CartShipPayInfo csp = (CartShipPayInfo)Session["CartShipPayInfo"];
            PayFlowRequest req = SetReq(csp, card);

            string payLoad = req.BuildPayload();        
            string response = SendReq(req.Endpoint, payLoad, BuildHeaders());
            string result = GetValue(response, "RESULT");
            Order order = CopyDirectPaymentToOrder(response);

           if (result.Equals("0") && GetValue(response,"CVV2MATCH") == "Y")
            {
                TempData["Confirm"] = "The payment has been successfully processed";
                
                repo.SaveAllToDatabase(order);
                csp.ShoppingCart.ClearCart();

                return View("ConfirmPayment", order);
            }
            else 
            {
                TempData["Fail"] = "Payment failed";
                repo.SaveIncompleteOrder(order);
                return View("_ErrorDisplay");
            }
     

       }

        private PayFlowRequest SetReq(CartShipPayInfo csp, CreditCardInfo card)
        { 
            PayFlowRequest req = new PayFlowRequest
            {
                Pwd = "1812war347813",
                Vendor = "stesha",
                User = "stesha",
                Partner = "VSA",//id of the gateway provider
                //live endpoint
                // Endpoint = "https://payflowpro.paypal.com",
                //test endpoint
                Endpoint = "https://pilot-payflowpro.paypal.com",
                Amt = Convert.ToString(csp.ShoppingCart.ComputeCartValue()), // amount of sale with two decimal places
                Currency = "AUD",
                Trxtype = "S",                  // type of transaction S-for sale
                Tender = "C",                   // method of payment C - for credit card
                Timeout = "300",
                Verbosity = "HIGH",
                Hostport = "443",
               
                // temp values
                SecCode = card.Verification,
                Acct = card.Account,           // creit card number
                ExpDate = "0719"//card.months + card.years

                ///additional possible settings
                ///SILENTTRAN=TRUE // for silent redirect
            };

            return req;
        }






          private NameValueCollection BuildHeaders()
           {
              string uniqueId = genId();
              
              NameValueCollection col = new NameValueCollection()
               {
                  {"X-VPS-REQUEST-ID", uniqueId},// id used to verify transaction is not duplicate (page 157, PayFlow Gateway Deveoper guide) 
                  {"X-VPS-CLIENT-TIMEOUT", "400"}              
               };      
       
               return col;
           }





      public string SendReq(string endpoint, string payLoad, NameValueCollection headers)
        {
            // Create the request
            HttpWebRequest request =
                  (HttpWebRequest)WebRequest.Create(endpoint);
            // Set values for the request
        
            request.Method = "POST";
            request.ContentType = "text/name value";
            request.ContentLength = payLoad.Length;
            request.Headers.Add(headers);

            // Write the request
            StreamWriter writer =
            new StreamWriter(request.GetRequestStream());
            writer.Write(payLoad);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("utf-8");
            StreamReader reader = new StreamReader(responseStream, encoding);
            string responseStr = "";

            while (!reader.EndOfStream)
            {
                responseStr += reader.ReadLine();
            }

            return responseStr;
        }



    
        // generates a random unique ID for use in the initial CREATESECURETOKEN=Y request
        protected string genId()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 16)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return "MySecTokenID-" + result; //add a prefix to avoid confusion with the "SECURETOKEN"
        }

    




    private string GetValue(string p, string key)
        {
            p = Convert.ToString(HttpUtility.UrlDecode(p));

            string[] delimiters = { "=", "&" };
            string[] words = p.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Equals(key))
                    return words[i + 1];
            }

            return null;
        }





        private Order CopyDirectPaymentToOrder(string resp)
        {
           CartShipPayInfo csp = (CartShipPayInfo)Session["CartShipPayInfo"];
           string customerId = (Session["Profile"] == null) ? csp.ShippingDetails.Email : 
                                                    Convert.ToString(((ApplicationUser)Session["Profile"]).Id);          

            Order order = new Order
            {
                CspPayerEmail = csp.ShippingDetails.Email,
                CspShippingAddress = csp.ShippingDetails.GetAddress(),
                RequestCurrency = GetValue(resp, "CURRENCY"),
                RequestFirstName = csp.ShippingDetails.Name,
                RequestLastName = csp.ShippingDetails.Surname,
                Total = csp.ShoppingCart.ComputeCartValue().ToString("c"),
                RequestPayerId = customerId,
                RequestTxnType = "Sale",
                RequestReceiverId = "Stesha Pty Ltd",
                RequestListOfProducts = csp.ShoppingCart.GetProductsList(),
                RequestPaymentDate = GetValue(resp, "TRANSTIME") == null ? DateTime.Now.ToString() : GetValue(resp, "TRANSTIME"),
                CspContactNumber = ((csp.ShippingDetails.Phone == null) ? "not provided" : csp.ShippingDetails.Phone),
                CartNumberOfItems = Convert.ToString(csp.ShoppingCart.ProductLines.Sum(p => p.Quantity)),
                RequestPaymentStatus = GetValue(resp, "RESPMSG"),
                RequestTxnId = GetValue(resp, "PNREF"),
            };


            return order;
        }
	





    
    }
}




    




