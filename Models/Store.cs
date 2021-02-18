using System;
using System.Collections.Generic;

namespace Models
{
    public class Store
    {
        private static int _idSeed = 1110;

        private Dictionary<int, int> _inventory;

        public int ID {get; set;}
        public string Name {get;}
        public string Location {get;}
        public double GrossProfit {get; set;}
        public List<Order> OrderHistory {get; set;}

        public Store(string name, string city)
        {
            ID = ++_idSeed;
            Name = name;
            Location = city;
            _inventory = new Dictionary<int, int>();
            OrderHistory = new List<Order>();
            GrossProfit = 0.0;
        }

        public void AddToInventory(int productID, int quantity)
        {
            if(quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be less than 0.");
            }
            if(_inventory.ContainsKey(productID))
            {
                _inventory[productID] += quantity;
            }
            else
            {
                _inventory[productID] = quantity;
            }
        }

        public void DeleteAll(int productID)
        {
            if(_inventory.ContainsKey(productID))
            {
                _inventory.Remove(productID);
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

        public int GetProductByID(int id) // COME BACK TO THIS METHOD. MIGHT NOT BE USEFUL ANYMORE
        {
            foreach(var item in _inventory)
            {
                if(item.Key == id)
                {
                    return item.Key;
                }
            }
            throw new Exception("Item not found.");
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

        public Dictionary<int, int> GetInventory()
        {
            return _inventory;
        }
    }
}
