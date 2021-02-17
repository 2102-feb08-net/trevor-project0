using System;
using System.Collections.Generic;

namespace Models
{
    public class Product
    {
        private static int _idSeed = 1111;
        public int ID {get; set;}
        public string Name {get;}
        public double Price {get; set;}
        
        public Product(string name, double price)
        {
            ID = _idSeed;
            _idSeed++;
            Name = name;
            Price = price;
        }
    }
}
