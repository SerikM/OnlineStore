using Model.EF;
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
   public partial class FavouriteItem
   {
        [Key]
        public string ListId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
       
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
   }
}