using System;
using System.Collections.Generic;

namespace Models
{
    public class Product
    {
        public int ID {get; set;}
        public string Name { get; set; }
        public double Price {get; set;}

        public Product()
        {

        }
        
        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public Product(int id, string name, double price)
        {
            ID = id;
            Name = name;
            Price = price;
        }

        public void UpdatePrice(double price)
        {
            if(price < 0)
            {
                throw new ArgumentException("Price cannot be less than 0.");
            }
            Price = price;
        }
    }
}
