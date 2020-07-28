using Model;
using Model.EF;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI2.Models
{
    public class ShippingInfo
    {

        private IRepository repo;

        public ShippingInfo(IRepository repo)
        {
            this.repo = repo;
        }


        public IEnumerable<Country> CountriesSelectList
        {
            get { return repo.GetAllCountries(); }
        }



        public IEnumerable<string> StreetTypeSelectData
        {
            get { return repo.GetStreetTypes(); }

        }


        public ShippingDetails ShipDet
        {
            get;
            set;
        }
    }
}