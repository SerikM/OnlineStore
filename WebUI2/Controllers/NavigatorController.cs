using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI2.Models;

namespace WebUI2.Controllers
{

    /// <summary>
    /// Provides logic for navigation based on the category of the product required by the user
    /// </summary>
    public class NavigatorController : Controller
    {
        IRepository repo;
        public NavigatorController(IRepository repo)
        {
            this.repo = repo;
        }

        public PartialViewResult Menu(string currentCategory = null)
        {
            ViewBag.CurrentCategory = currentCategory;
            List<CategoryInfo> catModels = new List<CategoryInfo>();
            IEnumerable<string> categories = repo.Products
                                             .Select(x => x.Category)
                                             .Distinct()
                                             .OrderBy(x => x);

            foreach (var cat in categories)
            {
                catModels.Add(new CategoryInfo { Name = cat, SubCategories = new List<string>() });
            }


            foreach (var product in repo.Products)
            {
                foreach (var catModel in catModels)
                {
                    if (product.Category == catModel.Name)
                    {
                        catModel.SubCategories.Add(product.SubCategory);
                    }
                    catModel.SubCategories = catModel.SubCategories.Distinct().ToList();
                }

            }

            return PartialView(catModels);
        }
    }
}