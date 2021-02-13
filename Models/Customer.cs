using System;
using System.Collections.Generic;

namespace Models
{
    public class Customer
    {
        private static int _idSeed = 1111;
        public int ID {get; set;}
        public string Name {get;}
        public string Email {get;}
        public string Address {get;}
        public Order Cart {get; set;}
        public List<Order> OrderHistory {get; set;}
        public Store Store {get; set;}

        public Customer(string name, string email, string address, Store store)
        {
            ID = _idSeed;
            _idSeed++;
            Name = name;
            Email = email;
            Address = address;
            Cart = new Order(this, store);
            OrderHistory = new List<Order>();
            Store = store;
        }

        public void AddItemToCart(Product product, int quantity)
        {
            Cart.Add(product, quantity);
        }

        public void RemoveItemFromCart(Product product, int quantity)
        {
            Cart.Delete(product, quantity);
        }

        public void SubmitOrder()
        {
            if(Cart.Items.Count == 0)
            {
                return;
            }
            try
            {
                Store.ProcessOrder(Cart);
                OrderHistory.Add(Cart);
                Cart = new Order(this, Store);
            }
            catch
            {
                throw new Exception("Could not process order.");
            }
        }

        public void PrintCart()
        {
            Cart.DisplayDetails();
        }

        public void PrintOrderHistory()
        {
            Console.WriteLine("ID\tDate of Order\t\t\tTotal Price");
            Console.WriteLine("__________________________________________________________________________________________");
            foreach(var order in OrderHistory)
            {
                Console.WriteLine($"{order.ID}\t{order.OrderTime.Date.ToString("d")}\t\t\t{order.TotalPrice}");
            }
        }
    }
}
