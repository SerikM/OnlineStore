using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using WebUI2.Models;
using System.Configuration;
using System.Data.SqlClient;
using WebUI2.Binders;
using Model.Entities;
using Model.ExternalAuthentication;
using System.Threading.Tasks;
using Model.EF;
using System.Net.Http;
using System.Net;
using System.Web.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections;





namespace WebUI2.Controllers
{


    /// <summary>
    /// Contains business logic necessary to display, update and manage products information to the user.
    /// Also processes products search requests
    /// </summary>
    public class ProductsController : Controller
    {
        private IRepository repository;
        public int pageSize = 4;


        public ProductsController(IRepository repository)
        {
            this.repository = repository;
        }



        /// <summary>
        /// retrieves products data from the database and sends all necessary info for display to the view
        /// </summary>
        public ViewResult List(string currentCategory, string currentSubCategory)
        {
            IQueryable<Product> products = (repository.Products)
                                 .Where(p => currentCategory == null || p.Category == currentCategory)
                                 .Where(p => currentSubCategory == null || p.SubCategory == currentSubCategory)
                                 .OrderBy(p => p.ProductID)
                                 .Take(pageSize);

            PagingInfo info = new PagingInfo
            {
                Products = (IEnumerable<Product>)products,
                PageSize = pageSize,
                Category = currentCategory,
                SubCategory = currentSubCategory
            };
            return View(info);
        }








        public ActionResult AjaxList(string currentCategory, string subCategory, int skipSize, int addSize = 0)
        {
            IQueryable<Product> products = (repository.Products)
                                 .Where(p => currentCategory == "" || p.Category == currentCategory)
                                 .Where(p => subCategory == "" || p.SubCategory == subCategory)
                                 .OrderBy(p => p.ProductID)
                                 .Skip(skipSize)
                                 .Take(addSize);



            return PartialView("_ProductsBundle", products);
        }





        public FileContentResult GetImage(int productId)
        {
            Product product = repository.Products.First(p => p.ProductID == productId);
            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }



        public ViewResult ShowDetailedDescription(int productId, string returnUrl)
        {
            if (returnUrl.Contains("/Products/AjaxList"))
            {
                returnUrl = "/Products/List";
            }

            ViewData["returnUrl"] = returnUrl;
            Product product = repository.Products.First(p => p.ProductID == productId);

            // create an instance of the recently viewed class  - to store the products viewed
            RecentlyViewed recentViews =
                (RecentlyViewed)(new RecentViewsModelBinder()).BindModel(this.ControllerContext, new ModelBindingContext());


            // make sure only one instance of each product is added to the view
            if (!recentViews.ContainsProduct(product))
            {
                recentViews.AddRecentProduct(product);
            }
            else
            {
                recentViews.AddProductAndMove(product);
            }

            IEnumerable<int> sizeIds = product.ProductToSizes.Select(p => p.SizeId);
            IEnumerable<int> colorIds = product.ProductToColors.Select(p => p.ColorId);

            ProductInfo inf = new ProductInfo {
                                                 SizeHolders = repository.
                                                 Sizes.Where(p => sizeIds.Contains(p.Id)).
                                                 Select(d => d.Value).ToArray(),
                                                 Product = product,
                                                 ColorHolders = repository.
                                                 Colors.Where(p => colorIds.Contains(p.ColorId)).
                                                 Select(d => d.Value).ToArray()
                                              };
            return View(inf);
        }





        public ActionResult SearchDatabase(string searchWord)
        {
            SearchData data = new SearchData();
            data.Results = repository.GetSearchResults(searchWord);
            return PartialView("_SearchResults", data.Results);
        }



        [HttpGet]
        public JsonResult AddFavourite(int productId)
        {
            ApplicationUser user = (ApplicationUser)Session["Profile"];


            if (repository.AddFavourite(user.UserName, productId))
            {
                Session["Profile"] = repository.FindByName(user.UserName);
                var jsonData = new AddFavModel { Result = "success", UserName = user.UserName };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = new AddFavModel { Result = "fail", UserName = user.UserName };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
        }//


        public ViewResult WishList(string UserName, string returnUrl = "/Products/List") 
        {  
           ApplicationUser user = repository.FindByName(UserName);
           IEnumerable<FavouriteItem> wishList = user.FavouritesList.Select(p => p);
           ViewBag.Data = returnUrl; 
           return View(wishList);
        }


        [HttpGet]
        public PartialViewResult GetWishListString()
        {
            ApplicationUser user = (ApplicationUser)Session["Profile"];
            IEnumerable<FavouriteItem> wishList = user.FavouritesList.Select(p => p);
            ViewBag.returnUrl = "/Account/Personal";
     
            return PartialView("_WishListInner", wishList);
        }



        public ViewResult DeleteFromWishList(string userName, int productId) 
        {
            repository.RemoveFavourite(userName, productId);
            IEnumerable<FavouriteItem> wishList = repository.FindByName(userName).FavouritesList.Select(p => p);
            Session["Profile"] = repository.FindByName(userName);
            ViewBag.Data = "/Products/List";
            return View("WishList", wishList);
        }


        public String GetProdName(int productId) 
        {
            return repository.Products.Where(p => p.ProductID == productId).First().Name;
        }

    }
}