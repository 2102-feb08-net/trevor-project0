using System;
using System.Collections.Generic;

namespace Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool running = true;
            while(running)
            {
                int option = 0;
                while(!(option > 0 && option < 7))
                {
                    Console.WriteLine("Choose an option:\n[1] Add Customer\n[2] Search Customers\n[3] Place order for customer\n[4] Display order details\n[5] Display customer order history\n[6] Display store order history");
                    string input = Console.ReadLine();
                    try
                    {
                        option = int.Parse(input);
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }
                Console.WriteLine("yay");
            }
        }
    }
}