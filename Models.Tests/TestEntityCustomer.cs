using System;
using Xunit;

namespace Models.Tests
{
    public class TestEntityCustomer
    {
        [Fact]
        public void TestCustomerConstructor()
        {
            string firstName = "Trevor";
            string lastName = "Dunbar";
            string email = "tdunbar123@yahoo.com";
            string address = "555 main street";
            Customer c = new Customer(firstName, lastName, email, address);

            Assert.Equal(firstName, c.FirstName);
            Assert.Equal(lastName, c.LastName);
            Assert.Equal(email, c.Email);
            Assert.Equal(address, c.Address);
        }
    }
}
