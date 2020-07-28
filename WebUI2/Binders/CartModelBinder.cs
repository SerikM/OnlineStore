using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI2.Binders
{
    /// <summary>
    /// Processes requests for the shopping cart object - retrieves data from the session state 
    /// and returns ShoppingCart type
    /// </summary>
    public class CartModelBinder : IModelBinder
    {
        private string sessionKey = "cart";


        public object BindModel(ControllerContext controllerContext, ModelBindingContext modBindingContext) 
        {
            ShoppingCart cart = (ShoppingCart)controllerContext.HttpContext.Session[sessionKey];
            if (cart == null) 
            {
                cart = new ShoppingCart();     
                controllerContext.HttpContext.Session[sessionKey] = cart;
             }
            return cart;
        }
    }
}