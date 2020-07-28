using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI2.Models
{
    public class CreditCardInfo
    {
        [Required(ErrorMessage="Credit card number is required")]
        [RegularExpression(@"^\d+$",ErrorMessage="Credit card number should only contain digits")]
        public string Account
        {
           get;set;
        }

        [Required(ErrorMessage="Verification is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Entered verification code is not valid.")]   
        public string Verification
        {
            get;
            set;
        }


        [Required(ErrorMessage="Card type is required")]
        public string CardType
        {           
            get;
            set;
        }



        [Required(ErrorMessage = "Card expiration month is required")]
        public string months
        {
            get;
            set;
        }



        [Required(ErrorMessage = "Card expiration year is required")]
        public string years
        {
            get;
            set;
        }

    }
}



                   
                

