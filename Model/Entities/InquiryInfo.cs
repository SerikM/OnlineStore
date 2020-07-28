using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model.Entities
{
    /// <summary>
    /// Holds data submitted by the user in order to send an inquiry
    /// </summary>
    public class InquiryInfo
    {

        public string Title { get; set; }
        
        [Required(ErrorMessage="First Name is Required")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Email Address is Required")]
        public string EmailAddress { get; set; }

        public string Telephone { get; set; }

        public string Country { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "Please, Create Some Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public string RecepientAddress { get; set; }

        public string RecepientName { get; set; }

        public string FullMessage { get; set; }

    }
}