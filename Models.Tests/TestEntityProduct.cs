using System;
using Xunit;

namespace Models.Tests
{
    public class TestEntityProduct
    {
        [Fact]
        public void TestProductConstructor()
        {
            string name = "Apple";
            decimal price = 0.5M;

            Product p = new Product(name, price);

            Assert.Equal(name, p.Name);
            Assert.Equal(price, p.Price);
        }

        [Fact]
        public void TestProductConstructor2()
        {
            int ID = 1;
            string name = "banana";
            decimal price = 1;

            Product p = new Product(ID, name, price);

            Assert.Equal(ID, p.ID);
            Assert.Equal(name, p.Name);
            Assert.Equal(price, p.Price);
        }

        [Fact]
        public void TestProductInvalidPrice()
        {
            string name = "Pizza";
            decimal price = -1;

            Assert.Throws<ArgumentException>(() => new Product(name, price));
        }
    }
}
