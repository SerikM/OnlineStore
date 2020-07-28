using Microsoft.AspNet.Identity;
using Model.Entities;
using Model.ExternalAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model
{
    public interface IRepository
    {

        IQueryable<Size> Sizes { get; }
        IQueryable<Color> Colors { get; }
        IQueryable<Product> Products { get; }
        IQueryable<BackgroundImage> BackgroundImages { get; }

        int getTotalCategoryItems(string currentCategory);

        bool IsDuplicateID(string p);

        void SaveToPendingOrders(Order pendingOrder);

        void SaveAllToDatabase(Order order);

        void SaveProduct(Product product);

        bool DeleteProduct(Product product);


        IEnumerable<string> GetSlidesImages();

        IEnumerable<Product> GetSearchResults(string searchWords);



        Order GetOrder(string transId);

        Order GetOrderByOrderId(int id);

        IEnumerable<Country> GetAllCountries();


        IEnumerable<string> GetStreetTypes();

        void SaveIncompleteOrder(Order order);

        List<string> GetListOfPastOrders(string id);

        ApplicationUser UpdateUserDetails(ShippingDetails ShipDet, string p);



        Task<ApplicationUser> FindAsync(UserLoginInfo userLoginInfo);

        Task<IdentityResult> Create(ApplicationUser user);

        Task<IdentityResult> AddLogin(string userId, UserLoginInfo userLoginInfo);

        Task AddToRole(string p, string userRoleName);

        Task<ClaimsIdentity> CreateIdentity(ApplicationUser user, string authenticationType);

        Task<ApplicationUser> Find(string userName, string password);

        Task<bool> IsInRole(string id, string adminRoleName);

        Task<IdentityResult> Create(ApplicationUser user, string password);

        ApplicationUser FindByName(string username);

        void RemovePassword(string userId);

        void AddPassword(string userId, string newPassword);





        bool  AddFavourite(string userName, int productId);

        bool DeleteProduct(int productId);

        bool RemoveFavourite(string userName, int productId );

        void SaveBckgr(BackgroundImage im);



        void AddColor(Color color);

        
    }

}