using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tests
{
    public class TestEntityStore
    {
        [Fact]
        public void TestStoreConstructor()
        {
            string name = "My store";
            string city = "Annapolis";
            string state = "Maryland";

            Store s = new Store(name, city, state);

            Assert.Equal(name, s.Name);
            Assert.Equal(city, s.City);
            Assert.Equal(state, s.State);
        }

        [Fact]
        public void TestStoreOrderIsValid()
        {
            Order o = new Order();
            Product apple = new Product(1, "Apple", 0.5M);
            o.Items = new Dictionary<Product, int>();
            o.Items.Add(apple, 5);

            Store s = new Store();
            s.Inventory = new Dictionary<Product, int>();
            s.Inventory.Add(apple, 5);

            Assert.True(s.OrderIsValid(o));
        }

        [Fact]
        public void TestStoreOrderTooHighQuantity()
        {
            Order o = new Order();
            Product apple = new Product(1, "Apple", 0.5M);
            o.Items = new Dictionary<Product, int>();
            o.Items.Add(apple, 6);

            Store s = new Store();
            s.Inventory = new Dictionary<Product, int>();
            s.Inventory.Add(apple, 5);

            Assert.Throws<Exception>(() => s.OrderIsValid(o));
        }

        [Fact]
        public void TestStoreOrderProductDoesNotExist()
        {
            Order o = new Order();
            Product apple = new Product(1, "Apple", 0.5M);
            Product banana = new Product(2, "Banana", 0.3M);
            o.Items = new Dictionary<Product, int>();
            o.Items.Add(apple, 6);

            Store s = new Store();
            s.Inventory = new Dictionary<Product, int>();
            s.Inventory.Add(banana, 6);

            Assert.Throws<Exception>(() => s.OrderIsValid(o));
        }

        /// <summary>
        /// When we call process order we have already validated 
        /// the order so we just have to make sure profit is being
        /// adjusted properly and inventory values are being changed.
        /// </summary>
        [Fact]
        public void TestStoreProcessOrder()
        {
            Order o = new Order();
            Product apple = new Product(1, "Apple", 0.5M);
            Product banana = new Product(2, "Banana", 0.3M);
            o.Items = new Dictionary<Product, int>();
            o.Items.Add(apple, 6);
            o.Items.Add(banana, 10);
            o.TotalPrice = 6;

            Store s = new Store();
            s.Inventory = new Dictionary<Product, int>();
            s.Inventory.Add(apple, 20);
            s.Inventory.Add(banana, 20);
            s.ProcessOrder(o);

            Assert.Equal(6, s.GrossProfit);
            Assert.Equal(14, s.Inventory[apple]);
            Assert.Equal(10, s.Inventory[banana]);
        }
    }
}
