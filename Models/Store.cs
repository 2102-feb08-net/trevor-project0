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
        public double GrossProfit {get; set;}
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
            GrossProfit = 0.0;
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

        public Customer GetCustomerByID(int id)
        {
            foreach(var customer in Customers)
            {
                if(customer.ID == id)
                {
                    return customer;
                }
            }
            throw new Exception("No such customer.");
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

        public void ProcessOrder(Order order)
        {
            //Check if it's a valid order first by going through items and ensuring quantities are within store limits
            foreach(var listItem in order.Items)
            {
                if(!Inventory.ContainsKey(listItem.Key) || listItem.Value > Inventory[listItem.Key])
                {
                    throw new Exception("Cannot process this order because item doesn't exist or quantity to order is too high.");
                }
            }
            //We checked all items to ensure the order is safe to process
            foreach(var listItem in order.Items)
            {
                Inventory[listItem.Key] -= listItem.Value;
            }
            order.SetOrderTime();
            OrderHistory.Add(order);
            GrossProfit += order.TotalPrice;
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

        public Product GetProductByName(string name)
        {
            foreach(var item in Inventory)
            {
                if(item.Key.Name == name)
                {
                    return item.Key;
                }
            }
            throw new ArgumentException("Item doesn't exist in the inventory.");
        }

        public void PrintInventory()
        {
            foreach(var item in Inventory)
            {
                Console.WriteLine($"{item.Key.Name} - ${item.Key.Price} - {item.Value} Available");
            }
        }

        public void PrintOrderHistory()
        {
            Console.WriteLine("ID\tDate of Order\t\t\tTotal Price\t\t\tCustomer");
            Console.WriteLine("__________________________________________________________________________________________");
            foreach(var order in OrderHistory)
            {
                Console.WriteLine($"{order.ID}\t{order.OrderTime.Date.ToString("d")}\t\t\t{order.TotalPrice}\t\t\t{order.Customer.Name}");
            }
        }
    }
}
