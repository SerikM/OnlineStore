using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI2.Models
{
    public class CartInfo
    {
        public ShoppingCart Cart
        {
            get;
            set;
        }

        public string ReturnUrl
        {
            get;
            set;
        }
    }
}