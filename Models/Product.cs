using System;

namespace Models
{
    public class Product
    {
        private int _idSeed = 1111;
        public int ID {get; set;}
        public string Name {get;}
        public double Price {get; set;}
        
        public Product(string name, int price)
        {
            ID = _idSeed;
            _idSeed++;
            Name = name;
            Price = price;
        }
    }
}
