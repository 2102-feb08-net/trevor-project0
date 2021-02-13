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
        public Store DefaultStore {get; set;}

        public Customer(string name, string email, string address, Store store)
        {
            ID = _idSeed;
            _idSeed++;
            Name = name;
            Email = email;
            Address = address;
            Cart = new Order(this, store);
            OrderHistory = new List<Order>();
            DefaultStore = store;
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
            OrderHistory.Add(Cart);
            Cart = new Order(this, DefaultStore);
        }

        public void PrintOrderHistory()
        {
            Console.WriteLine("ID\tItems\t\t\tTotal Price");
            Console.WriteLine("__________________________________________________________________________________________");
        }
    }
}
