using System;
using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        private static int _idSeed = 1110;

        public Dictionary<Product, int> Items;

        public int ID {get; set;}
        public double TotalPrice {get; set;}
        public Customer Customer { get; set; }
        public Store Store {get; set;}
        public DateTime OrderTime {get; set;}

        public Order()
        {

        }

        public Order(Customer customer, Store store)
        {
            ID = ++_idSeed;
            Items = new Dictionary<Product, int>();
            TotalPrice = 0.0;
            Customer = customer;
            Store = store;
        }

        public void Add(Product product, int quantity)
        {
            var storeItems = Store.Inventory;
            if(quantity > storeItems[product])
            {
                throw new Exception("Cannot more than what is available.");
            }
            if(Items.ContainsKey(product))
            {
                Items[product] += quantity;
            }
            else
            {
                Items[product] = quantity;
            }
        }

        public void Delete(Product product, int quantity)
        {
            if(Items.ContainsKey(product))
            {
                if(Items[product] >= quantity)
                {
                    Items[product] -= quantity;
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
            if(Items.Count > 0)
            {
                foreach(var item in Items)
                {
                    foreach(var product in Products)
                    {
                        if(item.Key == product)
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
            if(Items.Count == 0)
            {
                return;
            }
            if(Store.OrderIsValid(this))
            {
                //Verified order is OK for processing
                OrderTime = DateTime.Now;
                Store.ProcessOrder(this);
            }
        }
    }
}
