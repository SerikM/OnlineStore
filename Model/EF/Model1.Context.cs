﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TeethStoreEntities : DbContext
    {
        public TeethStoreEntities()
            : base("name=TeethStoreEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BackgroundImage> BackgroundImages { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<IncompleteOrder> IncompleteOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderedProduct> OrderedProducts { get; set; }
        public virtual DbSet<Slide> Slides { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductToColor> ProductToColors { get; set; }
        public virtual DbSet<ProductToSize> ProductToSizes { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
    }
}
