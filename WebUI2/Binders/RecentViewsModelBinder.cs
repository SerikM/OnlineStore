using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI2.Binders
{
    public class RecentViewsModelBinder : IModelBinder  {

      private string key = "recentViews";


        public object BindModel(ControllerContext contrContext, ModelBindingContext modContext)
        {
            RecentlyViewed recView = (RecentlyViewed)contrContext.HttpContext.Session[key];
            if(recView == null)
            {
               recView = new RecentlyViewed();
               contrContext.HttpContext.Session[key] = recView;
            }
            return recView;
        }

    }
}





