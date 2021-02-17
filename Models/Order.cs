using System;
using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        private static int _idSeed = 1110;

        private Dictionary<Product, int> _items;

        public int ID {get; set;}
        //public Dictionary<Product, int> Items {get; set;} // make this private field
        public double TotalPrice {get; set;}
        public Customer Customer {get;}
        public Store Store {get; set;}
        public DateTime OrderTime {get; set;}

        public Order(Customer customer, Store store)
        {
            ID = ++_idSeed;
            _items = new Dictionary<Product, int>();
            TotalPrice = 0.0;
            Customer = customer;
            Store = store;
        }

        public void Add(Product product, int quantity)
        {
            if(_items.ContainsKey(product))
            {
                _items[product] += quantity;
            }
            else
            {
                _items[product] = quantity;
            }
            TotalPrice += product.Price*quantity;
        }

        public void Delete(Product product, int quantity)
        {
            if(_items.ContainsKey(product))
            {
                if(_items[product] >= quantity)
                {
                    _items[product] -= quantity;
                    TotalPrice -= product.Price*quantity;
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

        public Dictionary<Product, int> GetItems()
        {
            return _items;
        }
    }
}
