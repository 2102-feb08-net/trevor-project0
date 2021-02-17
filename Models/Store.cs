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
        public List<Order> OrderHistory {get; set;}

        public Store(string name, string city)
        {
            ID = _idSeed;
            _idSeed++;
            Name = name;
            Location = city;
            Inventory = new Dictionary<Product, int>();
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

        public Product GetProductByID(int id)
        {
            foreach(var item in Inventory)
            {
                if(item.Key.ID == id)
                {
                    return item.Key;
                }
            }
            throw new ArgumentException("Item doesn't exist in the inventory.");
        }

        public void UpdateItemPrice(int id, double newPrice)
        {
            foreach(var item in Inventory)
            {
                if(item.Key.ID == id)
                {
                    item.Key.Price = newPrice;
                }
            }
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
