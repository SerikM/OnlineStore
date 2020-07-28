using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI2.Models
{
    public class ProductInfo
    {
        public Product Product { get; set; }

       // public IEnumerable<Size> Sizes { get; set; }

        public string[] SizeHolders { get; set; }

        public string[] ColorHolders { get; set; }

        public int CurrentPage { get; set; }
    }
}