using System;
using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        private static int _idSeed = 1111;
        public int ID {get; set;}
        public List<Product> Items {get; set;}
        public double TotalPrice {get; set;}
        public Customer Customer {get;}

        public Order(Customer customer)
        {
            ID = _idSeed;
            _idSeed++;
            Items = new List<Product>();
            TotalPrice = 0.0;
            Customer = customer;
        }

        public void Add(Product product)
        {
            Items.Add(product);
            TotalPrice += product.Price;
        }

        public void Delete(Product product)
        {
            Items.Remove(product);
            TotalPrice -= product.Price;
        }

        public string GetItemNames()
        {
            string ret = "";
            foreach(var item in Items)
            {
                ret += item.Name + ", ";
            }
            return ret;
        }
    }
}
