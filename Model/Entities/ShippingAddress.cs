using Model.ExternalAuthentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public partial class ShippingAddress
    {
        [Key]
        public string RecordId { get; set; }
        
        [Required]
        public string UserId { get; set; }
        public string UnitNumber { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public override string ToString()
        {
            return UnitNumber + " " + StreetNumber + " " + StreetName + " " + StreetType + ", " + Town + ", " + State + ", " + Country + ", " + PostCode;
        }
    }
}
