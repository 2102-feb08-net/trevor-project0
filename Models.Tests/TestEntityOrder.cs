using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Tests
{
    public class TestEntityOrder
    {
        [Fact]
        public void TestOrderConstructor()
        {
            Customer c = new Customer("Trevor", "Dunbar", "tdunbar@gmail.com", "123 cool street");
            Store s = new Store("Storename", "Annapolis", "Maryland");
            Order o = new Order(c, s);

            Assert.Equal(c, o.Customer);
            Assert.Equal(s, o.Store);
            Assert.Equal(0M, o.TotalPrice);
        }

        [Fact]
        public void TestOrderCalculateOrderTotal()
        {
            Order o = new Order();
            Product p = new Product("Apple", 1);
            o.Items = new Dictionary<Product, int>();
            o.Items.Add(p, 5);

            o.CalculateOrderTotal();

            Assert.Equal(5, o.TotalPrice);
        }

        /// <summary>
        /// Only checking to see that OrderTime gets updated here
        /// </summary>
        [Fact]
        public void TestOrderSubmitOrder()
        {
            Order o = new Order();
            o.Items = new Dictionary<Product, int>();
            Product p = new Product("Apple", 0.5M);

            Store s = new Store();
            s.Inventory = new Dictionary<Product, int>();

            o.Store = s;
            s.Inventory.Add(p, 5);
            o.Items.Add(p, 5);

            var today = DateTime.Now;
            o.SubmitOrder();
            Assert.Equal(today.Date, o.OrderTime.Date);
        }
    }
}
