using Model;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI2.Models
{
    public class AjaxPagingInfo
    {

        public IEnumerable<Product> Products { get; set; }

        public int TotalItems { get; set; }
        public int PageSize { get; set; }

        public string Category { get; set; }

        public int SkipSize { get; set; }


    }
}