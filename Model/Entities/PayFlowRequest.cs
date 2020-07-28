using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Model.Entities
{
    /// <summary>
    /// Holds all data required to make a call to the PayPal PayFlow server
    /// </summary>
    public class PayFlowRequest
    {
        
        public string Endpoint { get; set; }

        public string Vendor { get; set; }

        public string User { get; set; }

        public string Partner { get; set; }

        public string Currency { get; set; }


        public string Pwd { get; set; }


        public string Amt { get; set; }

        public string Trxtype { get; set; }

        public string Tender { get; set; }




        public string BuildPayload()
        {
            string payLoad = "USER=" + User;
            payLoad += "&PWD=" + Pwd;
            payLoad += "&PARTNER=" + Partner;
            payLoad += "&VENDOR=" + Vendor;
            payLoad += "&AMT=" + Amt;
            payLoad += "&CURRENCY=" + Currency;
            payLoad += "&TRXTYPE=" + Trxtype;
            payLoad += "&TENDER=" + Tender;
            payLoad += "&TIMEOUT=" + Timeout;
            payLoad += "&HOSTADDRESS=" + HostAddress;
            payLoad += "&HOSTPORT=" + Hostport;
            payLoad += "&VERBOSITY=" + Verbosity;
            payLoad += "&ITEMS=" + Items;

            payLoad += "&ACCT=" + Acct;
            payLoad += "&CVV2=" + SecCode; 
            payLoad += "&EXPDATE=" + ExpDate;

            return payLoad;
        }






        public string Timeout { get; set; }

        public string HostAddress { get; set; }

        public string Acct { get; set; }

        public string SecCode { get; set; }

        public string ExpDate { get; set; }

        public string Verbosity { get; set; }

        public string Hostport { get; set; }

        public int Items { get; set; }
    }
}
