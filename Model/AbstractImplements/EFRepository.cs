using Microsoft.AspNet.Identity;
using Model.Entities;
using Model.ExternalAuthentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Model.EF;
using System.Data.Entity.Validation;

namespace Model
{

    /// <summary>
    /// Interacts with the database. Serves as the main interface with the database ofr the application
    /// </summary>
    public class EFRepository : IRepository
    {
        TeethStoreEntities database = new TeethStoreEntities();
        ApplicationUserManager applicationUserManager= new ApplicationUserManager();


   

        public void AddColor(Color color)
        {
           database.Colors.Add(color);
           database.SaveChanges();
        }

        public IEnumerable<string> GetStreetTypes()
        {
            string[] list = {"Street Type", "Street", "Alley", "Approach","Arcade","Avenue","Boulevard","Brow","Bypass","Causeway","Circuit",	
                                 "Circus","Close","Copse","Corner","Cove","Court","Crescent","Drive","End","Esplanande",	
                                 "Flat","Freeway","Frontage","Gardens","Glade","Glen","Green","Grove","Heights","Highway",	
                                 "Lane","Link","Loop","Mall","Mews","Packet","Parade","Park","Parkway","Place","Promenade",	
                                 "Reserve","Ridge","Rise","Road","Row","Square","Strip","Tarn","Terrace","Thoroughfare",	
                                 "Track","Trunkway","View","Vista","Walk","Way","Walkway","Yard"};
            return list;
        }




        public IEnumerable<Country> GetAllCountries()
        {
            return database.Countries;
        }


        public IQueryable<Product> Products
        {
            get { return database.Products; }
        }


        public IQueryable<BackgroundImage> BackgroundImages 
        {
            get { return database.BackgroundImages; }
        }


        public int getTotalCategoryItems(string category)
        {

            if (category == null)
            {
                return (database.Products).Count();
            }
            else
            {
                return (database.Products.Where(p => p.Category == category)).Count();
            }
        }



        public bool IsDuplicateID(string id)
        {

            if (database.Orders.Any(p => p.RequestTxnId == id))
            {
                return true; // the table has a duplicate transaction id
            }
            else
            {
                return false; // there is no duplicate
            }
        }



        public void SaveToPendingOrders(Order order)
        {

        }



        public void SaveAllToDatabase(Order order)
        {
            try
            {
                database.Orders.Add(order);
                database.SaveChanges();
            }
            catch (Exception exc)
            {
                string error = exc.Message;
            }
        }



        public void SaveProduct(Product product)
         {
            //remove elements 
            database.ProductToSizes.RemoveRange(database.ProductToSizes.Where(p => p.ProductID == product.ProductID));
            //remove elements 
            database.ProductToColors.RemoveRange(database.ProductToColors.Where(p => p.ProductID == product.ProductID));
            foreach (var joint in product.ProductToSizes)
            {
                database.Entry(joint).State = System.Data.Entity.EntityState.Added;
            }

            foreach (var joint in product.ProductToColors)
            {
                database.Entry(joint).State = System.Data.Entity.EntityState.Added;
            }
         
            if (product.ProductID == 0)// add a new product if the id is 0
            {
                database.Products.Add(product);
            }
            else
            {
                product.ImageData = database.Products.Where(d => d.ProductID == product.ProductID).Select(p => p.ImageData).First();
                product.ImageMimeType = database.Products.Where(d => d.ProductID == product.ProductID).Select(p => p.ImageMimeType).First();

                database.Entry(product).State = System.Data.Entity.EntityState.Modified;
            }
               

           try
            {
                database.SaveChanges();
            }

            catch (Exception)
            {               
            }
        }




        public void SaveBckgr(BackgroundImage im) 
        {
            database.BackgroundImages.Add(im);
            database.SaveChanges();
        }




        public bool DeleteProduct(Product product)
        {
            try
            {
                
                database.ProductToSizes.RemoveRange(database.ProductToSizes.Where(p => p.ProductID == product.ProductID));
                database.ProductToColors.RemoveRange(database.ProductToColors.Where(p => p.ProductID == product.ProductID));
            
                
                database.Products.Remove(product);
                database.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }


        public bool DeleteProduct(int productId)
        {
            try
            {
                database.ProductToSizes.RemoveRange(database.ProductToSizes.Where(p => p.ProductID == productId));
                database.ProductToColors.RemoveRange(database.ProductToColors.Where(p => p.ProductID == productId));
                database.Products.Remove(database.Products.Where(p => p.ProductID == productId).First());
                database.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }








        public IEnumerable<string> GetSlidesImages()
        {
            return database.Slides.Select(p => p.ImageName);
        }




        // returns results of a search for a particular product 
        public IEnumerable<Product> GetSearchResults(string searchWords)
        {
            IEnumerable<Product> results = database.Products.Where(p => p.Name == searchWords);


            string[] delimiter = { " " };
            string[] words = searchWords.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

            IEnumerable<Product> results2;
            foreach (var word in words)
            {
                results2 = database.Products.Where(p => p.Name.Contains(word));
                results = results.Concat(results2);

            }


            return results.Distinct();
        }





        public List<string> GetListOfPastOrders(string id)
        {
            List<string> txnIds = new List<string>();

            List<Order> orders = database.Orders.Where(p => p.RequestPayerId == id).ToList();
            if (orders != null)
            {
                txnIds =
                  orders.Select(m => m.RequestTxnId).ToList();
                return txnIds;
            }

            return txnIds;

        }



        public Order GetOrder(string transId)
        {
            return database.Orders.Where(p => p.RequestTxnId == transId).FirstOrDefault();
        }

        public Order GetOrderByOrderId(int id) 
        {
            return database.Orders.Where(p => p.Id == id).FirstOrDefault();
        }


        public void SaveIncompleteOrder(Order order)
        {
            IncompleteOrder incOrder = new IncompleteOrder
            {
                CartNumberOfItems = order.CartNumberOfItems,
                CspContactNumber = order.CspContactNumber,
                CspPayerEmail = order.CspPayerEmail,
                CspShippingAddress = order.CspShippingAddress,
                PayPalPayerId = order.PayPalPayerId,
                RequestCurrency = order.RequestCurrency,
                RequestFirstName = order.RequestFirstName,
                RequestLastName = order.RequestLastName,
                RequestListOfProducts = order.RequestListOfProducts,
                RequestMcGross = order.Total,
                RequestNotes = order.RequestNotes,
                RequestPayerId = order.RequestPayerId,
                RequestPaymentDate = order.RequestPaymentDate,
                RequestPaymentStatus = order.RequestPaymentStatus,
                RequestPaymentType = order.RequestPaymentType,
                RequestPendingReason = order.RequestPendingReason,
                RequestReceiverId = order.RequestReceiverId,
                RequestResidenceCnt = order.RequestResidenceCnt,
                RequestTxnId = order.RequestTxnId,
                RequestTxnType = order.RequestTxnType
            };


            try
            {
                database.IncompleteOrders.Add(incOrder);
                database.SaveChanges();
            }
            catch (Exception exc)
            {

            }
        }



        public bool AddFavourite(string userName, int productId)
        {
            ApplicationUser user = applicationUserManager.FindByName(userName);
            user.FavouritesList.Add(new FavouriteItem
            {
                ListId = Convert.ToString(Guid.NewGuid()),
                ApplicationUser = user,
                ProductID = productId
            });
            try
            {
                applicationUserManager.Update(user);
                return true;
            }
          
            
            catch (Exception exc)
            {           
               string st = exc.InnerException.InnerException.Message;
            }
               return false;       
        }


        public bool RemoveFavourite(string userName, int productId)
        {
            MyApplicationDbContext ctx = new MyApplicationDbContext();
           
            ApplicationUser user = applicationUserManager.FindByName(userName);
            ctx.FavouritesList.Remove(ctx.FavouritesList.Where(p => p.UserId == user.Id && p.ProductID == productId).First());
            
            try
            {
                ctx.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException exc)
            {
                foreach (var err in exc.EntityValidationErrors)
                {
                    foreach (var errrr in err.ValidationErrors)
                    {
                        string st = errrr.ErrorMessage;
                    }
                }
                return false;
            }          
        }


        public ApplicationUser UpdateUserDetails(ShippingDetails model, string userName)
        {

            ApplicationUser appUser = applicationUserManager.FindByName(userName);
            
            
            string recId = appUser.ShippingAddressId;
            
            // if address record doesn't already exist create a new id for the addr record so that it is added to the datatable
            if (recId == null) 
            {  
                recId = Guid.NewGuid().ToString();
                
                ShippingAddress shipAddr = new ShippingAddress
                {
                    RecordId = recId,
                    Country = model.Country,
                    PostCode = model.PostCode,
                    State = model.State,
                    StreetName = model.StreetName,
                    StreetNumber = model.StreetNumber,
                    StreetType = model.StreetType,
                    Town = model.Town,
                    UnitNumber = model.UnitFlat
                };
                appUser.ShippingAddressId = shipAddr.RecordId;
                appUser.ShippingAddress.Add(shipAddr);
            }
           
            // otherwise use the old id and update the old record
            else
            {
                ShippingAddress sa = appUser.ShippingAddress.Where(p => p.RecordId == recId).First();
                sa.Country = model.Country;
                sa.PostCode = model.PostCode;
                sa.State = model.State;
                sa.StreetName = model.StreetName;
                sa.StreetNumber = model.StreetNumber;
                sa.StreetType = model.StreetType;
                sa.Town = model.Town;
                sa.UnitNumber = model.UnitFlat;
            }

            appUser.Name = model.Name;
            appUser.Surname = model.Surname;
            appUser.Email = model.Email;
            appUser.Phone = model.Phone;
                   
           applicationUserManager.Update(appUser);

          return appUser;
        }


        public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            return await applicationUserManager.FindAsync(loginInfo);
        }

        public async Task<IdentityResult> Create(ApplicationUser user)
        {
            return await applicationUserManager.CreateAsync(user);
        }

        public async Task<IdentityResult> AddLogin(string userId, UserLoginInfo userLoginInfo)
        {
            return await applicationUserManager.AddLoginAsync(userId, userLoginInfo);
        }


        public async Task AddToRole(string userId, string userRoleName)
        {
            await applicationUserManager.AddToRoleAsync(userId, userRoleName);
        }


        public async Task<ClaimsIdentity> CreateIdentity(ApplicationUser user, string authType)
        {
            return await applicationUserManager.CreateIdentityAsync(user, authType);
        }

        public async Task<ApplicationUser> Find(string userName, string password) 
        {
            return await applicationUserManager.FindAsync(userName, password);
        }

        public async Task<bool> IsInRole(string id, string adminRoleName)
        {
            return await applicationUserManager.IsInRoleAsync(id, adminRoleName);
        }



        public async Task<IdentityResult> Create(ApplicationUser user, string password) 
        {
            return  await applicationUserManager.CreateAsync(user, password);     
        }

        public ApplicationUser FindByName(string username)
        {
            return applicationUserManager.FindByName(username);
        }

        public void RemovePassword(string userId) 
        {
            applicationUserManager.RemovePassword(userId);
        }

        public void AddPassword(string userId, string newPassword) 
        {
            applicationUserManager.AddPassword(userId, newPassword);
        }

        public IQueryable<Size> Sizes 
        {
            get { return database.Sizes; }
        }

        public IQueryable<Color> Colors
        {
            get { return database.Colors; }
        }
    }
}


/*          
 * catch (DbEntityValidationException exc)
 *     {
 *     foreach (var err in exc.EntityValidationErrors)
 *     {
 *     foreach (var errrr in err.ValidationErrors)
 *     {
 *     string st = errrr.ErrorMessage;
 *     }
 *     }
 *      }*/