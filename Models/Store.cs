using System;
using System.Collections.Generic;

namespace Models
{
    public class Store
    {
        private static int _idSeed = 1110;

        private Dictionary<Product, int> _inventory;

        public int ID {get; set;}
        public string Name {get;}
        public string Location {get;}
        public double GrossProfit {get; set;}
        //public Dictionary<Product, int> Inventory {get; set;} // make private field
        public List<Order> OrderHistory {get; set;}

        public Store(string name, string city)
        {
            ID = ++_idSeed;
            Name = name;
            Location = city;
            _inventory = new Dictionary<Product, int>();
            OrderHistory = new List<Order>();
            GrossProfit = 0.0;
        }

        public void AddToInventory(Product product, int quantity)
        {
            if(quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be less than 0.");
            }
            if(_inventory.ContainsKey(product))
            {
                _inventory[product] += quantity;
            }
            else
            {
                _inventory[product] = quantity;
            }
        }

        public void DeleteAll(Product product)
        {
            if(_inventory.ContainsKey(product))
            {
                _inventory.Remove(product);
            }
            else
            {
                throw new ArgumentException("Item doesn't exist in inventory");
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
            throw new Exception("Order not found.");
        }

        public Product GetProductByName(string name)
        {
            foreach(var item in _inventory)
            {
                if(item.Key.Name == name)
                {
                    return item.Key;
                }
            }
            throw new Exception("Item not found.");
        }

        public Product GetProductByID(int id)
        {
            foreach(var item in _inventory)
            {
                if(item.Key.ID == id)
                {
                    return item.Key;
                }
            }
            throw new Exception("Item not found.");
        }

        public void UpdateItemPrice(int id, double newPrice)
        {
            if(newPrice < 0)
            {
                throw new ArgumentException("New price cannot be less than 0!");
            }
            foreach(var item in _inventory)
            {
                if(item.Key.ID == id)
                {
                    item.Key.Price = newPrice;
                    return;
                }
            }
            throw new Exception("Couldn't find item to update.");
        }

        public bool OrderIsValid(Order order)
        {
            var items = order.GetItems();
            foreach(var listItem in items)
            {
                if(_inventory.ContainsKey(listItem.Key) || listItem.Value > _inventory[listItem.Key])
                {
                    return false;
                }
            }
            return true;
        }

        public void ProcessOrder(Order order)
        {
            OrderHistory.Add(order);
            var items = order.GetItems();
            foreach(var listItem in items)
            {
                _inventory[listItem.Key] -= listItem.Value;
            }
            GrossProfit += order.TotalPrice;
        }

        public Dictionary<Product, int> GetInventory()
        {
            return _inventory;
        }
    }
}
