using Model;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI2.Models
{
    public class SearchData
    {
        public IEnumerable<Product> Results
        {
            get;
            set;
        }



    }
}