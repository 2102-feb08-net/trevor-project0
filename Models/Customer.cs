using System;
using System.Collections.Generic;

namespace Models
{
    public class Customer
    {
        private static int _idSeed = 1110;
        public int ID {get; set;}
        public string Name {get;}
        public string Email {get;}
        public string Address {get;}
        public List<Order> OrderHistory {get; set;}

        public Customer(string name, string email, string address)
        {
            ID = ++_idSeed;
            Name = name;
            Email = email;
            Address = address;
            OrderHistory = new List<Order>();
        }
    }
}
