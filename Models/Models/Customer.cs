using System;
using System.Linq;
using System.Collections.Generic;

namespace Models
{
    public class Customer
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _address;
        public int ID {get; set;}
        public string FirstName 
        {
            get { return _firstName; }
            set
            {
                if (value.All(Char.IsLetter))
                {
                    _firstName = value;
                }
                else
                {
                    throw new ArgumentException("First name must only contain letters");
                }
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (value.All(Char.IsLetter))
                {
                    _lastName = value;
                }
                else
                {
                    throw new ArgumentException("Last name must only contain letters");
                }
            }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                foreach(char c in value)
                {
                    if(!Char.IsLetterOrDigit(c) && !Char.IsWhiteSpace(c))
                    {
                        throw new ArgumentException("Address can't contain special characters");
                    }
                }
                _address = value;
            }
        }

        public Customer()
        {

        }

        public Customer(string firstName, string lastName, string email, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
        }
    }
}
