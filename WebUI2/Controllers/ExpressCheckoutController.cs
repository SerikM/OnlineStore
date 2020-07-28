using Model;
using Model.Abstracts;
using Model.EF;
using Model.Entities;
using Model.ExternalAuthentication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;



namespace WebUI2.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpressCheckoutController : Controller
    {
        string reqKey = "reqExpressCheckout";
        IRepository repo;
        IMailProcessor mp;
        private string failMessage = "Unfortunately we were unable to complete your transaction.\n\r" +
                                     "Please contact us on one of the following numbers:\n\r" +
                                      ConfigurationManager.AppSettings["MyContactNumber"] + " or\n\r" +
                                      ConfigurationManager.AppSettings["MyMobContactNumber"]; 
        
        public ExpressCheckoutController(IRepository repo, IMailProcessor mp) 
        {
            this.repo = repo;
            this.mp = mp;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // following code is used for PayPal's Eexpress Checkout functionality
        // detailed tutorial on express checkout is at https://developer.paypal.com/webapps/developer/docs/classic/express-checkout/ht_ec-singleItemPayment-curl-etc/
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        
        


        public RedirectResult SetExpressCheckout()
        {

            CartShipPayInfo csp = (CartShipPayInfo)Session["CartShipPayInfo"];
            ExprPayPalRequest req = new ExprPayPalRequest()
            {
                // live credentials
                Password = "BUAEUEJ8VAL68RPE",
                PayPalMerchantUsername = "tyrkamotchka_api1.mail.ru",
                Signature = "AcQ.7s0YrYjFbaBoVNy6bGaoP-VpAkRQHqL0If7cnR.3iZZFIjV8BG6h",
               
                Endpoint = "https://api-3t.paypal.com/nvp/", //nvp production
                //req.Endpoint = "https://api-3t.sandbox.paypal.com/nvp/"; // nvp     sandbox                
                Method = "SetExpressCheckout",// METHOD
                // set all costs and products details from cart info
                ProductsList = csp.ShoppingCart.ProductLines,
                Total = Convert.ToString(csp.ShoppingCart.ComputeCartValue() + csp.ShoppingCart.ShippingCosts),
                SubTotal = Convert.ToString(csp.ShoppingCart.ComputeCartValue()),//
                ShippingCost = Convert.ToString(csp.ShoppingCart.ShippingCosts),
                currency = "AUD",
                //  set urls
                //ReturnUrl = "http://localhost:33966/ExpressCheckout/GetExpressCheckoutDetails",
                CancelUrl = "http://localhost:33966/Cart/RedirectAfterCancel",
                ReturnUrl = "https://stesha.com.au/ExpressCheckout/GetExpressCheckoutDetails",
                //CancelUrl = "https://stesha.com.au/Cart/RedirectAfterCancel",
                Version = 93,
                PaymentAction = "SALE"
            };




            try
            {
                // make a call to PayPal and read answer
                string resp = SendRequest(req.Endpoint, req.ToString());
                req.Token = ParseToken(resp, "TOKEN");
            }
            catch (Exception)
            {
                TempData["Fail"] = failMessage;
                return Redirect("_ErrorDisplay");
            }
      
            // string url = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + req.Token;
            string url = "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + req.Token;
            Session[reqKey] = req;

            return Redirect(url);
        }






        public ActionResult GetExpressCheckoutDetails()
        {
            CartShipPayInfo csp = (CartShipPayInfo)Session["CartShipPayInfo"];
            // after the payment is confirmed get the payer's details
            ExprPayPalRequest req = (ExprPayPalRequest)Session[reqKey];
            req.Method = "GetExpressCheckoutDetails";
       
            try
            {
                //send data to pay pal server  and read response
                string resp = SendRequest(req.Endpoint, req.ToString());
                string ack = ParseToken(resp, "ACK");
                            
                // verify success and token
                if (ack.Equals("Success") && (req.Token.Equals(ParseToken(resp, "TOKEN"))))
                {
                    req.PayerId = (Session["Profile"] == null) ? csp.ShippingDetails.Email :
                                                            Convert.ToString(((ApplicationUser)Session["Profile"]).Id);
                    req.PayPalPayerId = ParseToken(resp, "PAYERID");
                    req.Total = ParseToken(resp, "PAYMENTREQUEST_0_AMT");
                    req.PayerEmail = csp.ShippingDetails.Email;  
                    req.PayerName = ParseToken(resp, "FIRSTNAME");
                    req.PayerLastName = ParseToken(resp, "LASTNAME");
                    req.ShiptoName = csp.ShippingDetails.Name + " " + csp.ShippingDetails.Surname;
                    req.ShipAddress = csp.ShippingDetails.GetAddress();
                    req.TransTime = Convert.ToString(DateTime.Parse(ParseToken(resp, "TIMESTAMP")));
                    Session[reqKey] = req;

                    return new RedirectResult("/ExpressCheckout/DoExpressCheckoutPayment");
                }
                else
                {
                    TempData["Fail"] = failMessage;
                    return View("_ErrorDisplay");  
                }
            }
            catch(Exception) 
            {
                TempData["Fail"] = failMessage;
                return View("_ErrorDisplay");  
            }
            
        }






        
        public ViewResult DoExpressCheckoutPayment()
        {
            try
            {
               ExprPayPalRequest req = (ExprPayPalRequest)Session[reqKey];
               req.Method = "DoExpressCheckoutPayment";

               string resp = SendRequest(req.Endpoint, req.ToString());

               string ack = ParseToken(resp, "ACK");
               req.TransId = ParseToken(resp, "PAYMENTINFO_0_TRANSACTIONID");
               req.TransType = ParseToken(resp, "PAYMENTINFO_0_TRANSACTIONTYPE");
               req.PaymentStatus = ParseToken(resp, "PAYMENTINFO_0_PAYMENTSTATUS");

               // send mail to customer
               if (ack.Equals("Success") && (req.Token.Equals(ParseToken(resp, "TOKEN"))))
               {
                   Order order = CopyExpressCheckoutReqToOrder(req);
                   repo.SaveAllToDatabase(order);

                   sendCustomerEmail(order);
                   sendAdminEmail(order);

                   CartShipPayInfo csp = (CartShipPayInfo)Session["CartShipPayInfo"];
                   csp.ShoppingCart.ClearCart();
                   TempData["Confirm"] = "Payment Successful";
                   return View("ConfirmPayment", order);
               }
            }
            catch (Exception)
            {
            }
            TempData["Fail"] = failMessage;
            return View("_ErrorDisplay");
        }












        private Order CopyExpressCheckoutReqToOrder(ExprPayPalRequest req)
        {
            CartShipPayInfo csp = (CartShipPayInfo)Session["CartShipPayInfo"];

            Order order = new Order
            {
                CspPayerEmail = req.PayerEmail,
                CspShippingAddress = req.ShipAddress,
                RequestCurrency = req.currency,
                RequestFirstName = req.PayerName,
                RequestLastName = req.PayerLastName,
                RequestPayerId = req.PayerId,
                RequestListOfProducts = req.Products,
                PayPalPayerId = req.PayPalPayerId,
                RequestPaymentDate = req.TransTime,
                CspContactNumber = csp.ShippingDetails.Phone,
                CartNumberOfItems = Convert.ToString(csp.ShoppingCart.ProductLines.Sum(p => p.Quantity)),
                RequestPaymentStatus = req.PaymentStatus,
                RequestTxnId = req.TransId,
                RequestTxnType = req.TransType,
                RequestReceiverId = req.PayPalMerchantUsername,
                Total = req.Total,
                ShiptoName = req.ShiptoName,
                ShippingCost = req.ShippingCost,
                SubTotal = req.SubTotal
            };
            order.OrderedProducts = GetOrderedProducts(req.ProductsList, order.Id);

            return order;
        }

        private ICollection<OrderedProduct> GetOrderedProducts(IEnumerable<ProductLine> prodLines, int orderId)
        {
            ICollection<OrderedProduct> prods = new LinkedList<OrderedProduct>();
            foreach(var line in prodLines)
            {
               foreach(var prod in line.Products)
               {
                   prods.Add(new OrderedProduct 
                   {
                     Category = prod.Category,
                     Description = prod.Description,
                     ExternalLink = prod.ExternalLink,
                     LongDescription = prod.LongDescription,
                     Name = prod.Name,
                     NordStromId = prod.NordstromId,
                     Price = prod.Price,
                     ProductId = prod.ProductID,
                     SelectedColor = prod.SelectedColour,
                     SelectedSize = prod.SelectedSize,
                     SubCategory = prod.SubCategory,
                     OrderId = orderId
                   });
               }
            }
            
            return prods;
        }




        private string ParseToken(string p, string key)
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


    
        private string SendRequest(string endpoint, string payLoad)
        {
            // Create the request
            HttpWebRequest request =
              (HttpWebRequest)WebRequest.Create(endpoint);
            // Set values for the request
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = payLoad.Length;
            // Write the request
            StreamWriter writer =
            new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(RuntimeHelpers.GetObjectValue(payLoad));
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
        



        private void sendCustomerEmail(Order order)
        {
            mp.SendMail(new InquiryInfo
            {
                Subject = "Order Confirmation",
                Message = "",
                RecepientAddress = order.CspPayerEmail,
                RecepientName = order.RequestFirstName,
                FullMessage = "Thanks for purchasing from Stesha Pty Ltd!\n\rFollowing are details of your order: \n\r" 
                + OrderToString(order)
            });
        }




        private void sendAdminEmail(Order order)
        {
           // send mail to admin
            mp.SendMail(new InquiryInfo
            {
                Subject = "Order Notification",
                Message = "an order has been placed",
                RecepientAddress = ConfigurationManager.AppSettings["AdminEmail"],
                RecepientName = ConfigurationManager.AppSettings["MyBusinessName"],
                FullMessage = "Following are details of the new order: \n\r" + OrderToString(order) 
            });
        }

        private string OrderToString(Order order)
        {
            StringBuilder bld = new StringBuilder();
            bld.Append("Order status - " + order.RequestPaymentStatus + "\n\r");
            bld.Append("Order number - " + order.Id + "\n\r");
            bld.Append("Customer name - " + order.ShiptoName + "\n\r" );
            bld.Append("Transaction date - " + order.RequestPaymentDate + "\n\r" );
            bld.Append("Items purchased - " + order.RequestListOfProducts + "\n\r");
            bld.Append("Seller - " + ConfigurationManager.AppSettings["MyBusinessName"] + "\n\r");
            
            return bld.ToString();
        }





    }
}


// code below is used to get shipping info from paypal response
                    //req.ShipAddress = ParseToken(payPalResponse, "SHIPTOSTREET") + " " +
                    //   ParseToken(payPalResponse, "SHIPTOCITY") + " " +
                    //   ParseToken(payPalResponse, "SHIPTOSTATE") + " " +
                    //   ParseToken(payPalResponse, "SHIPTOZIP") + " " +
                    //   ParseToken(payPalResponse, "SHIPTOCOUNTRYNAME");
