using Model.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Model.Entities
{
    public class ExprPayPalRequest
    {
        [HiddenInput(DisplayValue = false)]
        public string Password { get; set; }

        public string Products { get; set; }

        public string PayPalMerchantUsername { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Signature { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Endpoint { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Method { get; set; }

        [Display(Name = "Amount for Payment")]
        public string Total { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string currency { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string CancelUrl { get; set; }


        [HiddenInput(DisplayValue = false)]
        public int Version { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string PaymentAction { get; set; }




        public override string ToString()
        {
            string payLoad = "USER=" + PayPalMerchantUsername;
            payLoad += "&PWD=" + Password;
            payLoad += "&SIGNATURE=" + Signature;
            payLoad += "&METHOD=" + Method;
            payLoad += "&VERSION=" + Version;
            payLoad += "&PAYMENTREQUEST_0_PAYMENTACTION=" + PaymentAction;           
            payLoad += "&PAYMENTREQUEST_0_CURRENCYCODE=" + currency;
            payLoad += "&RETURNURL=" + ReturnUrl;
            payLoad += "&CANCELURL=" + CancelUrl;
            payLoad += "&TOKEN=" + Token;
            payLoad += "&PAYERID=" + PayPalPayerId;
            payLoad += "&LASTNAME=" + PayerLastName;
            payLoad += "&FIRSTNAME=" + PayerName;
           
            payLoad += GetProductsDescriptionString();
            payLoad += GetCostsString();
            return payLoad;
        }

        private string GetCostsString()
        {
            return "&PAYMENTREQUEST_0_SHIPPINGAMT=" + ShippingCost +
                   "&PAYMENTREQUEST_0_AMT=" + Total;
        }

        private string GetProductsDescriptionString()
        {
            string payLoad = "";
            
            int index = 0;
            foreach (var line in ProductsList) 
            {
                payLoad += "&L_PAYMENTREQUEST_0_NAME" + index + "="   + line.Products.First().Name;
                payLoad += "&L_PAYMENTREQUEST_0_NUMBER" + index + "=" + line.Products.First().ProductID;
                
                // provide details of size and colors
                string descrStr = "";
                foreach (Product product in line.Products) 
                {
                   descrStr += " Color - " + line.Products.First().SelectedColour + ";   " + 
                                                "Size - " + line.Products.First().SelectedSize + ";   ";
                }
                payLoad += "&L_PAYMENTREQUEST_0_DESC" + index + "=" + descrStr;              
                payLoad += "&L_PAYMENTREQUEST_0_AMT" + index + "=" + line.Products.First().Price;
                payLoad += "&L_PAYMENTREQUEST_0_QTY" + index + "=" + line.Quantity;
                payLoad += "&PAYMENTREQUEST_0_ITEMAMT=" + SubTotal;

                index++; 
            }
            return payLoad;
        }





        [HiddenInput(DisplayValue = false)]
        public string Token { get; set; }


        [Display(Name = "Payer Email")]
        public string PayerEmail { get; set; }

        [Display(Name = "Payer Name")]
        public string PayerName { get; set; }

        [Display(Name = "Payer Family Name")]
        public string PayerLastName { get; set; }

        [Display(Name = "Shipment Recepient")]
        public string ShiptoName { get; set; }

        [Display(Name = "Shipping Address")]
        public string ShipAddress { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string PayerId { get; set; }

        public string TransTime { get; set; }

        public string TransId { get; set; }

        public string TransType { get; set; }

        public string Account { get; set; }

        public string CreditCardType { get; set; }

        public string Verification { get; set; }



        public string ExpirationDate { get; set; }

        public string PaymentStatus { get; set; }

        public string PayPalPayerId { get; set; }

        public IEnumerable<ProductLine> ProductsList { get; set; }

        public string SubTotal { get; set; }

        public string ShippingCost { get; set; }

        
    }
}




