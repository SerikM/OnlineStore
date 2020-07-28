using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{

    /// <summary>
    /// Holds all the logic required to process users' shopping activity on the site
    /// </summary>
   public class ShoppingCart
    {
        List<ProductLine> lineCollection = new List<ProductLine>();


        public void AddItem(Product product)
        {
            ProductLine line = lineCollection.Where(p => p.Products.First().ProductID == product.ProductID).FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add( new ProductLine(product) );                           
            }
            else
            {
                line.Products.AddLast(product);
            }
        }



        public void RemoveItem(Product product)
        {
            ProductLine line = lineCollection.Where(p => p.Products.First().ProductID == product.ProductID).FirstOrDefault();
            line.Products.RemoveLast();

            if (line.Quantity <= 0)
                lineCollection.Remove(line);
        }



        public decimal ComputeCartValue()
        {
            return lineCollection.Sum(p => p.Products.First().Price * p.Quantity);
        }


        public void ClearCart() 
        {
            lineCollection.Clear();        
        }


        public IEnumerable<ProductLine> ProductLines 
        {
            get { return lineCollection; }
        }





        public string GetProductsList()
        {
            string list = "";
            foreach (var line in lineCollection) 
            {
                foreach (var prod in line.Products) 
                {
                    list += prod.Name + ",\r\n";
                }
            }
            return list;
        }
    
   

       public int CartQuantity
       {
           get { return lineCollection.Sum(p => p.Quantity); }
       }

       public decimal ShippingCosts { get; set; }
    }



   public class ProductLine
    {
        public ProductLine(Product product) 
        {
            if (Products == null) 
            {
                Products = new LinkedList<Product>();
            }
            Products.AddLast(product);
        }

        public LinkedList<Product> Products { get; set; }
      
        public int Quantity { get { return Products.Count(); }  }
    }



}
