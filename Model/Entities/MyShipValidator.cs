using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;




namespace Model.Entities
{
    /// <summary>
    /// This is a custom implementation of class ValidationAttribute.
    /// It validates data provided by the user in the "country" shipping details field
    /// </summary>
    public class CountryValidator : ValidationAttribute 
    {
        private string word = "Country Name";
        public CountryValidator() 
        {
            ErrorMessage = "Please select a country name";
        }

        public override bool IsValid(object value)
        {
            return !((string)value).Equals(word);
        }
    
    }


    public class PhoneValidator : ValidationAttribute
    {
      
        public PhoneValidator()
        {
            ErrorMessage = "Phone number is invalid";
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                Match match = Regex.Match(value.ToString(), @"^\d+$", RegexOptions.IgnoreCase);
                if ((match.Success) && (value.ToString().Length > 4)) return true;
                else return false;
            }
            return false;
        }
    }


    /// <summary>
    /// This is a custom implementation of class ValidationAttribute.
    /// It validates data provided by the user in the "street" shipping details field
    /// </summary>
    public class StreetTypeValidator : ValidationAttribute
    {
        private string word = "Street Type";
        public StreetTypeValidator()
        {
            ErrorMessage = "Please select a street type";
        }

        public override bool IsValid(object value)
        {
            return !((string)value).Equals(word);
        }

    }


}