using System;
using System.Collections.Generic;

namespace Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Customer me = new Customer("Trevor", "tdunbar123@yahoo.com", "28 Mayo Avenue");
            Product apple = new Product("Apple", .50);
            me.AddItemToCart(apple);
            me.AddItemToCart(apple);
            me.SubmitOrder();
            me.PrintOrderHistory();
        }
    }
}