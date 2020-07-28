using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebUI2.Models
{
    public class PagingInfo
    {
        public IEnumerable<Product> Products { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public int CurrentPage { get; set; }
    }
}
