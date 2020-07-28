using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    class Receipt
    {

        public DateTime SupplyDate
        {
            get;
            set;
        }

        public string ABN { get; set; }
        public string Products { get; set; }
        public double Price { get; set; }

    }
}
