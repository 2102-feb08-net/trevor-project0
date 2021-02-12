using System;

namespace Models
{
    public class Customer
    {
        private static int _idSeed = 1111;
        public int ID {get; set;}
        public string Name {get;}
        public string Email {get;}
        public string Address {get;}
        public Order Cart {get; set;}
        public List<Order> OrderHistory {get; set;}

        public Customer(string name, string email, string address)
        {
            ID = _idSeed;
            _idSeed++;
            Name = name;
            Email = email;
            Address = address;
            Cart = new Order(this);
            OrderHistory = new List<Order>();
        }
    }
}
