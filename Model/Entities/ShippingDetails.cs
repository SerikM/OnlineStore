using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Model.Entities
{

   /// <summary>
   /// Holds all the shipping details entered by the customer
   /// </summary>
   public class ShippingDetails
    {
     


       public string GetAddress() 
       {
           StringBuilder bld = new StringBuilder();
           bld.Append(UnitFlat + ", ");
           bld.Append(StreetNumber + ", ");
           bld.Append(StreetName + " ");
           bld.Append(StreetType + ", ");
           bld.Append(Town + ", ");
           bld.Append(State + ", ");
           bld.Append(Country + ", ");
           bld.Append(PostCode);
           return bld.ToString();
       }

        [Required(ErrorMessage="Please enter your name")]
        [DataType(DataType.Text)]
        public string Name
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please enter your family name")]
        public string Surname
        {
            get;
            set;
        }


        [Required(ErrorMessage="PLease enter street name")]
        public string StreetName
        {
            get;
            set;
        }


        [Required(ErrorMessage="Please enter a city name")]
        public string Town
        {
            get;
            set;
        }

       

       [Required(ErrorMessage="Please enter a state name")]
        public string State
        {
            get;
            set;
        }




        [CountryValidator]
        [Required(ErrorMessage="Please enter a country name")]
        public string Country
        {
            get;
            set;
        }

        
      

       [Required(ErrorMessage = "Please enter your email")]      
       [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage="Email is invalid")]
        public string Email
        {
            get;
            set;
        }


      
       [PhoneValidator]
       [Required(ErrorMessage = "Please enter your phone number")]
       public string Phone
        {
            get;
            set;
        }

       public string UnitFlat { get; set; }





       [StreetTypeValidator]
       [Required(ErrorMessage="Please selet street type")]
       public string StreetType { get; set; }


       [Required(ErrorMessage = "Please enter street number")]
       public string StreetNumber { get; set; }



       [DataType(DataType.PostalCode, ErrorMessage="Poscode is invalid")]
       [Required(ErrorMessage = "Please enter your postcode")]
       public string PostCode { get; set; }


   }
}
