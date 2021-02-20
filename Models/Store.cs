using System;
using System.Collections.Generic;

namespace Models
{
    public class Store
    {
        private static int _idSeed = 1110;

        private Dictionary<int, int> _inventory;

        public int ID {get; set;}
        public string Name { get; set; }
        public string Location { get; set; }
        public double GrossProfit {get; set;}

        public Store(string name, string location)
        {
            ID = ++_idSeed;
            Name = name;
            Location = location;
            _inventory = new Dictionary<int, int>();
            GrossProfit = 0.0;
        }

        public Store()
        {

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

        public bool OrderIsValid(Order order)
        {
            var items = order.GetItems();
            foreach(var listItem in items)
            {
                if(!_inventory.ContainsKey(listItem.Key) || listItem.Value > _inventory[listItem.Key])
                {
                    throw new Exception("Order item either does not exist or has too high quantity");
                }
            }
            return true;
        }

        public void ProcessOrder(Order order)
        {
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
