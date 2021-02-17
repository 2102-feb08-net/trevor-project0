using System;
using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        private static int _idSeed = 1110;
        public int ID {get; set;}
        public Dictionary<Product, int> Items {get; set;}
        public double TotalPrice {get; set;}
        public Customer Customer {get;}
        public Store Store {get; set;}
        public DateTime OrderTime {get; set;}

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
            if(Items.ContainsKey(product))
            {
                Items[product] += quantity;
            }
            else
            {
                Items[product] = quantity;
            }
            TotalPrice += product.Price*quantity;
        }

        public void Delete(Product product, int quantity)
        {
            if(Items.ContainsKey(product))
            {
                if(Items[product] >= quantity)
                {
                    Items[product] -= quantity;
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
            if(Items.Count == 0)
            {
                return;
            }
            foreach(var listItem in Items)
            {
                if(!Store.Inventory.ContainsKey(listItem.Key) || listItem.Value > Store.Inventory[listItem.Key])
                {
                    throw new Exception("Cannot process this order because item doesn't exist or quantity to order is too high.");
                }
            }
            //Verified order is OK for processing
            OrderTime = DateTime.Now;
            Customer.OrderHistory.Add(this);
            Store.OrderHistory.Add(this);
            foreach(var listItem in Items)
            {
                Store.Inventory[listItem.Key] -= listItem.Value;
            }
            
            Store.GrossProfit += TotalPrice;
        }

        public void DisplayDetails()
        {
            Console.WriteLine($"Order number: {ID}\nStore: {Store.Name}, {Store.Location}\nCustomer name: {Customer.Name}\nTimestamp: {OrderTime.Date.ToString("d")}");
            Console.WriteLine("Receipt\n________");
            foreach(var item in Items)
            {
                Console.WriteLine($"({item.Value}) {item.Key.Name} ${item.Key.Price*item.Value}");
            }
            Console.WriteLine($"Total: ${TotalPrice}");
        }
    }
}
