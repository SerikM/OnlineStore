using Model;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebUI2.Models;

namespace WebUI2.Controllers
{

    /// <summary>
    /// Provides functionality required for the administration of the products catalogue
    /// and addition or deletion of registered users
    /// </summary>

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IRepository repo;
        private const int pageSize = 10;

        public AdminController(IRepository repo)
        {
            this.repo = repo;          
        }



        public ViewResult Index(int page = 1)
        {
            PagingInfo pageInf = new PagingInfo
            {
                    PageSize = pageSize,
                    Products = repo.Products
                    .OrderBy(p => p.ProductID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                    TotalItems = repo.Products.Count(),
                    CurrentPage = page
            };
            return View(pageInf);
        }


        public ViewResult Edit(int ProductID, int page = 1)
        {
            Product product = repo.Products.FirstOrDefault(p => p.ProductID == ProductID);
            ProductInfo prInf = new ProductInfo
            {
                CurrentPage = page,
                Product = product,
                SizeHolders = repo.Sizes.Select(p => p.Value).ToArray(),
                ColorHolders = repo.Colors.Select(p => p.Value).ToArray()
            };
                 
            return View(prInf);
        }




        [HttpPost]
        public ViewResult Edit(ProductInfo prInf, HttpPostedFileBase image, int page = 1)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    prInf.Product.ImageMimeType = image.ContentType;
                    prInf.Product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(prInf.Product.ImageData, 0, image.ContentLength);
                }
                AddSizes(prInf.SizeHolders, ref prInf);
                AddColors(prInf.ColorHolders, ref prInf);
                repo.SaveProduct(prInf.Product);
                TempData["Confirm"] = $"{prInf.Product.Name} has been saved";
               
                return View("Index", new PagingInfo
                {
                    PageSize = pageSize,
                    Products = repo.Products
                    .OrderBy(p => p.ProductID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                    TotalItems = repo.Products.Count(),
                    CurrentPage = page
                }
                          );
            }
            else
            {
                // validation hasn't succeeded - return to the edit view
                return View(prInf);
            }

        }

        private void AddColors(string[] colorHolders, ref ProductInfo prInf)
        {
            if (colorHolders != null)
            {
                foreach (var color in colorHolders)
                {
                    //add color
                    prInf.Product.ProductToColors.Add(
                                   new ProductToColor
                                   {
                                       ColorId = repo.Colors.First(p => p.Value == color).ColorId,
                                       ProductID = prInf.Product.ProductID
                                   });
                }
            }
        }




        private void AddSizes(string[] sizeHolders, ref ProductInfo prInf)
        {
            if (sizeHolders != null)
            {
                foreach (var size in sizeHolders)
                {
                    //add sizes
                    prInf.Product.ProductToSizes.Add(
                                   new ProductToSize
                                   {
                                       SizeId = repo.Sizes.First(p => p.Value == size).Id,
                                       ProductID = prInf.Product.ProductID
                                   });
                }

            }
        }





        [HttpPost]
        public ActionResult Delete(int ProductID)
        {
            Product product = repo.Products.FirstOrDefault(p => p.ProductID == ProductID);

            if (repo.DeleteProduct(product))
            {
                TempData["Confirmation"] = $"{product.Name} has been deleted";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Fail"] = "Failed to delete product";
                return RedirectToAction("Index");
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }



        public void InsertCountries()
        {

            string[] countries = {"Country Name","Afghanistan","Akrotiri","Albania", "Algeria", "American Samoa",
           "Andorra", "Angola","Anguilla","Antarctica", "Antigua and Barbuda","Argentina","Armenia", "Aruba",
           "Ashmore and Cartier Islands","Australia","Austria","Azerbaijan","Bahamas", "Bahrain", "Bangladesh", 
           "Barbados","Bassas da India","Belarus","Belgium","Belize", "Benin", "Bermuda", "Bhutan",
"Bolivia","Bosnia and Herzegovina", "Botswana", "Bouvet Island","Brazil","British Indian Ocean Territory",
"British Virgin Islands","Brunei","Bulgaria", "Burkina Faso","Burma","Burundi","Cambodia","Cameroon","Canada",
"Cape Verde","Cayman Islands","Central African Republic","Chad","Chile","China","Christmas Island",
"Clipperton Island","Cocos (Keeling) Islands","Colombia","Comoros", "Democratic Republic of Congo",
"Republic of the Cook Islands","Coral Sea Islands","Costa Rica","Cote d Ivoire","Croatia","Cuba",
"Cyprus","Czech Republic","Denmark","Dhekelia","Djibouti","Dominica","Dominican Republic","Ecuador",
"Egypt","El Salvador","Equatorial Guinea","Eritrea","Estonia","Ethiopia","Europa Island","Falkland Islands (Islas Malvinas)",
"Faroe Islands","Fiji","Finland","France","French Guiana","French Polynesia","French Southern and Antarctic Lands",
"Gabon","Gambia","The Gaza Strip","Georgia","Germany","Ghana","Gibraltar","Glorioso Islands","Greece",
"Greenland","Grenada","Guadeloupe","Guam","Guatemala","Guernsey","Guinea","Guinea-Bissau","Guyana",
"Haiti","Heard Island and McDonald Islands","Holy See (Vatican City)","Honduras","Hong Kong","Hungary",
"Iceland","India","Indonesia","Iran","Iraq","Ireland","Isle of Man","Israel","Italy","Jamaica",
"Jan Mayen","Japan","Jersey","Jordan","Juan de Nova Island","Kazakhstan","Kenya","Kiribati","Korea North",
"Korea South","Kuwait","Kyrgyzstan","Laos","Latvia","Lebanon","Lesotho","Liberia","Libya","Liechtenstein",
"Lithuania","Luxembourg","Macau","Macedonia","Madagascar","Malawi","Malaysia","Maldives","Mali",
"Malta","Marshall Islands","Martinique","Mauritania","Mauritius","Mayotte","Mexico","Federated States of Micronesia",
"Moldova","Monaco","Mongolia","Montserrat","Morocco","Mozambique","Namibia","Nauru","Navassa Island",
"Nepal","Netherlands","Netherlands Antilles","New Caledonia","New Zealand","Nicaragua","Niger",
"Nigeria","Niue","Norfolk Island","Northern Mariana Islands","Norway","Oman","Pakistan","Palau",
"Panama","Papua New Guinea","Paracel Islands","Paraguay","Peru","Philippines","Pitcairn Islands",
"Poland","Portugal","Puerto Rico","Qatar","Reunion","Romania","Russia","Rwanda","Saint Helena",
"Saint Kitts and Nevis","Saint Lucia","Saint Pierre and Miquelon","Saint Vincent and the Grenadines",
"Samoa","San Marino","Sao Tome and Principe","Saudi Arabia","Senegal","Serbia and Montenegro",
"Seychelles","Sierra Leone","Singapore","Slovakia","Slovenia","Solomon Islands","Somalia",
"South Africa","South Georgia and the South Sandwich Islands","Spain","Spratly Islands","Sri Lanka",
"Sudan","Suriname","Svalbard","Swaziland","Sweden","Switzerland","Syria","Taiwan","Tajikistan",
"Tanzania","Thailand","Timor-Leste","Togo","Tokelau","Tonga","Trinidad and Tobago","Tromelin Island",
"Tunisia","Turkey","Turkmenistan","Turks and Caicos Islands","Tuvalu","Uganda","Ukraine",
"United Arab Emirates","United Kingdom","United States","Uruguay","Uzbekistan","Vanuatu","Venezuela",
"Vietnam","Virgin Islands","Wake Island","Wallis and Futuna","West Bank","Western Sahara","Yemen",
"Zambia","Zimbabwe"};



            TeethStoreEntities database = new TeethStoreEntities();

            foreach (String country in countries)
            {
                database.Countries.Add(new Country { Name = country });
            }
            database.SaveChanges();
        }



        public ActionResult UploadBackgroundImage(HttpPostedFileBase image) 
        {

            if (image != null)
            {
                BackgroundImage im = new BackgroundImage
                {
                    ImageMimeType = image.ContentType,
                    ImageData = new byte[image.ContentLength]
                };
                image.InputStream.Read(im.ImageData, 0, image.ContentLength);


                repo.SaveBckgr(im);
                TempData["Confirm"] = "The image has been saved";
            }
            else 
                TempData["Fail"] = "Failed to save image";
            
                
            return View("Index", new PagingInfo
              {
                 PageSize = pageSize,
                 Products = repo.Products
                 .OrderBy(p => p.ProductID)
                 .Take(pageSize),
                 TotalItems = repo.Products.Count()
              }
              );
            
        }





    }
}
