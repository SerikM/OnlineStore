using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebUI2.Models
{
    public class LoginInfo
    {
        [Required(ErrorMessage="Please Enter Username")]
        public string Username { get; set; }
        
        [Required(ErrorMessage="Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Persist { get; set; }
    }

    public class LoginInfoNoPassword
    {
        [Required(ErrorMessage="Please Enter Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"^[a-z0-9_\+-]+(\.[a-z0-9_\+-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*\.([a-z]{2,4})$", ErrorMessage = "Email is invalid")]
        public string Email { get; set; }
       // [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is invalid")]
       
            

            }
}