using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Model.Entities
{
    public class CustomerInfo
    {

       


        [Required(ErrorMessage="Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        //[MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage="Password and Confirmation Password do not match")]
        public string ConfirmPassword { get; set; }
        
        

            
        [DataType(DataType.EmailAddress, ErrorMessage="Email address has incorrect format")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
      

    }
}
