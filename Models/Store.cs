using System;

namespace Models
{
    public class Store
    {
        private static int _idSeed = 1111;
        public int ID {get; set;}
        public string Name {get;}
        public string Location {get;}
        public Dictionary<Product, int> Inventory {get; set;}
        public List<Customer> Customers {get; set;}
        public List<Order> OrderHistory {get; set;}

        public Store(string name, string city)
        {
            ID = _idSeed;
            _idSeed++;
            Name = name;
            Location = city;
            Inventory = new Dictionary<Product, int>();
            Customers = new List<Customer>();
            OrderHistory = new List<Order>();
        }
    }
}
