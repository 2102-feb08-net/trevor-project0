using System;
using System.Collections.Generic;

namespace Models
{
    public class Store
    {
        private static int _idSeed = 1111;
        public int ID {get; set;}
        public string Name {get;}
        public string Location {get;}
        public Dictionary<Product, int> Inventory {get; set;}
        public List<Customer> Customers {get; set;}
        public List<Order> OrderHistory {get; set;}

        public Store(string name, string city)
        {
            ID = _idSeed;
            _idSeed++;
            Name = name;
            Location = city;
            Inventory = new Dictionary<Product, int>();
            Customers = new List<Customer>();
            OrderHistory = new List<Order>();
        }

        public void AddToInventory(Product product, int quantity)
        {
            if(quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be less than 0.");
            }
            if(Inventory.ContainsKey(product))
            {
                Inventory[product] += quantity;
            }
            else
            {
                Inventory[product] = quantity;
            }
        }

        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }

        public Customer GetCustomerByName(string name)
        {
            foreach(var customer in Customers)
            {
                if(customer.Name == name)
                {
                    return customer;
                }
            }
            throw new Exception("No such customer.");
        }

        public Order GetOrderByID(int id)
        {
            foreach(var order in OrderHistory)
            {
                if(order.ID == id)
                {
                    return order;
                }
            }
            throw new Exception("No such order.");
        }
    }
}
