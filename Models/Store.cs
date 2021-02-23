using System;
using System.Collections.Generic;

namespace Models
{
    public class Store
    {
        public Dictionary<Product, int> Inventory;

        public int ID {get; set;}
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal GrossProfit {get; set;}

        public Store(string name, string city, string state)
        {
            Name = name;
            City = city;
            State = state;
            Inventory = new Dictionary<Product, int>();
            GrossProfit = 0.0M;
        }

        public Store()
        {

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

        public void DeleteAll(Product product)
        {
            if(Inventory.ContainsKey(product))
            {
                Inventory.Remove(product);
            }
            else
            {
                throw new ArgumentException("Item doesn't exist in inventory");
            }
        }

        public bool OrderIsValid(Order order)
        {
            var items = order.Items;
            foreach(var listItem in items)
            {
                var key = GetKeyByID(listItem.Key.ID);
                if(!Inventory.ContainsKey(key))
                {
                    throw new Exception("Order item does not exist");
                }
                if(listItem.Value > Inventory[key])
                {
                    throw new Exception("Order item is requesting to buy too many");
                }
            }
            return true;
        }

        public void ProcessOrder(Order order)
        {
            var items = order.Items;
            foreach(var listItem in items)
            {
                var key = GetKeyByID(listItem.Key.ID);
                Inventory[key] -= listItem.Value;
            }
            GrossProfit += order.TotalPrice;
        }

        public Product GetKeyByID(int id)
        {
            foreach(var item in Inventory)
            {
                if(item.Key.ID == id)
                {
                    return item.Key;
                }
            }
            return null;
        }
    }
}
