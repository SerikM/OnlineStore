using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using Model.Entities;


namespace Model.ExternalAuthentication
{

    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string RecentViewsId { get; set; }
        public string ShippingAddressId { get; set; }
        public virtual ICollection<ShippingAddress> ShippingAddress { get; set; }
        public virtual ICollection<FavouriteItem> FavouritesList { get; set; }
    }
}