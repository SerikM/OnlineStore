using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;


namespace Model.Entities
{
    /// <summary>
    /// Processes and holds data about products recently viewed by the customer
    /// </summary>
    public class RecentlyViewed
    {
        private LinkedList<Product> recentlyViewed = new LinkedList<Product>();
        private int maxSize = 9;

        public void AddRecentProduct(Product product) 
        {
            recentlyViewed.AddFirst(product);
            
            if (recentlyViewed.Count() > maxSize)
            {
                // remove the last element if there are too many in the sequence
                recentlyViewed.RemoveLast();
            }
        
        }


      public  IEnumerable<Product> GetRecentViews() 
        {
            return recentlyViewed;
        }



      public bool ContainsProduct(Product product)
      {
          if (recentlyViewed.Where(p => p.ProductID == product.ProductID).FirstOrDefault() == null)
              return false;

          else return true;

      }

      public void AddProductAndMove(Product product)
      {
          // remove the product with the given id and then add it at the beginning
          recentlyViewed.Remove(recentlyViewed.Where(p => p.ProductID == product.ProductID).FirstOrDefault());
          recentlyViewed.AddFirst(product);

      }
    }
}
