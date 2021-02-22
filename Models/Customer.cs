using System;
using System.Collections.Generic;

namespace Models
{
    public class Customer
    {
        private static int _idSeed = 1110;
        public int ID {get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Customer()
        {

        }

        public Customer(string firstName, string lastName, string email, string address)
        {
            ID = ++_idSeed;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
        }
    }
}
