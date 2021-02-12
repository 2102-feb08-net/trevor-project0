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

        public Customer(string name, string email, string address)
        {
            ID = _idSeed;
            _idSeed++;
            Name = name;
            Email = email;
            Address = address;
            Cart = new Order(this);
            OrderHistory = new List<Order>();
        }

        public void AddItemToCart(Product product)
        {
            Cart.Add(product);
        }

        public void RemoveItemFromCart(Product product)
        {
            Cart.Delete(product);
        }

        public void SubmitOrder()
        {
            if(Cart.Items.Count == 0)
            {
                return;
            }
            OrderHistory.Add(Cart);
            Cart = new Order(this);
        }

        public void PrintOrderHistory()
        {
            Console.WriteLine("ID\tItems\t\t\tTotal Price");
            Console.WriteLine("__________________________________________________________________________________________");
            foreach(var order in OrderHistory)
            {
                Console.WriteLine($"{order.ID}\t{order.GetItemNames()}\t\t\t${order.TotalPrice}");
            }
        }
    }
}
