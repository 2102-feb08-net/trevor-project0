using System;
using System.Collections.Generic;

namespace Models
{
    public class Customer
    {
        private static int _idSeed = 1110;
        public int ID {get; set;}
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Customer()
        {

        }

        public Customer(string name, string email, string address)
        {
            ID = ++_idSeed;
            Name = name;
            Email = email;
            Address = address;
        }
    }
}
