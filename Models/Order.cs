using System;
using System.Collections.Generic;
//
// TODO Remove all dependency on Customer and Store class objects. All order modification and submission logic should be performed here.
//
namespace Models
{
    public class Order
    {
        private static int _idSeed = 1111;
        public int ID {get; set;}
        public Dictionary<Product, int> Items {get; set;}
        public double TotalPrice {get; set;}
        public Customer Customer {get;}
        public Store Store {get; set;}
        public DateTime OrderTime {get; set;}

        public Order(Customer customer, Store store)
        {
            ID = _idSeed;
            _idSeed++;
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

        public void SetOrderTime()
        {
            OrderTime = DateTime.Now;
        }

        public void SubmitOrder()
        {
            if(Items.Count == 0)
            {
                return;
            }
            try
            {
                Store.ProcessOrder(this);
                Customer.OrderHistory.Add(this);
            }
            catch
            {
                throw new Exception("Could not process order.");
            }
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
