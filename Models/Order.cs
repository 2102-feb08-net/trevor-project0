using System;
using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        private static int _idSeed = 1110;

        private Dictionary<int, int> _items;

        public int ID {get; set;}
        public double TotalPrice {get; set;}
        public Customer Customer { get; set; }
        public Store Store {get; set;}
        public DateTime OrderTime {get; set;}

        public Order(Customer customer, Store store)
        {
            ID = ++_idSeed;
            _items = new Dictionary<int, int>();
            TotalPrice = 0.0;
            Customer = customer;
            Store = store;
        }

        public void Add(int productID, int quantity)
        {
            var storeItems = Store.GetInventory();
            if(quantity > storeItems[productID])
            {
                throw new Exception("Cannot more than what is available.");
            }
            if(_items.ContainsKey(productID))
            {
                _items[productID] += quantity;
            }
            else
            {
                _items[productID] = quantity;
            }
        }

        public void Delete(int productID, int quantity)
        {
            if(_items.ContainsKey(productID))
            {
                if(_items[productID] >= quantity)
                {
                    _items[productID] -= quantity;
                }
                else
                {
                    throw new ArgumentException("Quantity to remove cannot be higher than quantity in the cart.");
                }
            }
            else
            {
                throw new ArgumentException("Product to remove does not exist in cart");
            }
        }

        public void CalculateOrderTotal(List<Product> Products)
        {
            TotalPrice = 0;
            if(_items.Count > 0)
            {
                foreach(var item in _items)
                {
                    foreach(var product in Products)
                    {
                        if(item.Key == product.ID)
                        {
                            TotalPrice += product.Price*item.Value;
                        }
                    }
                }
            }
        }

        public void SubmitOrder()
        {
            //Make sure order meets criteria to be processed
            if(_items.Count == 0)
            {
                return;
            }
            if(Store.OrderIsValid(this))
            {
                //Verified order is OK for processing
                OrderTime = DateTime.Now;
                Customer.OrderHistory.Add(this);
                Store.ProcessOrder(this);
            }
        }

        public Dictionary<int, int> GetItems()
        {
            return _items;
        }
    }
}
