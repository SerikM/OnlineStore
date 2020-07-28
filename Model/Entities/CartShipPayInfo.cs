using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Entities
{
    public class CartShipPayInfo
    {
        public ShoppingCart ShoppingCart
        {
            get;
            set;
        }
        public ShippingDetails ShippingDetails
        {
            get;
            set;
        }




       
    }
}