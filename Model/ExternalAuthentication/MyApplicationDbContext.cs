using Microsoft.AspNet.Identity.EntityFramework;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ExternalAuthentication
{
    public class MyApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public MyApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            // writes sql query details to out put window
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine("\n\rIdentity user-related info:************************************************** \n\r" + s + "\n\r");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShippingAddress>().ToTable("ShippingAddress");

            modelBuilder.Entity<FavouriteItem>().ToTable("FavouritesList");
        }

        public virtual DbSet<ShippingAddress> ShippingAddress { get; set; }

        public virtual DbSet<FavouriteItem> FavouritesList { get; set; }
    }
}
