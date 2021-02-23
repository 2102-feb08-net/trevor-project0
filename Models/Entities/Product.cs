using System;
using System.Collections.Generic;

namespace Models
{
    public class Product
    {
        public int ID {get; set;}
        public string Name { get; set; }
        public decimal Price
        {
            get { return Price; } 
            set
            {
                if(value < 0)
                {
                    throw new ArgumentException("Price cannot be negative");
                }
                Price = value;
            }
        }

        public Product()
        {

        }
        
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public Product(int id, string name, decimal price)
        {
            ID = id;
            Name = name;
            Price = price;
        }

        public void UpdatePrice(decimal price)
        {
            if(price < 0)
            {
                throw new ArgumentException("Price cannot be less than 0.");
            }
            Price = price;
        }
    }
}
