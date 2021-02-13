using System;
using System.Collections.Generic;

namespace Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool running = true;
            //Test material
            Store store = new Store("Giant foods", "Annapolis");
            Product apple = new Product("Apple", 0.5);
            Product cereal = new Product("Cheerios", 3);
            Product milk = new Product("Milk", 4);
            Product steak = new Product("Steak", 12);
            store.AddToInventory(apple, 20);
            store.AddToInventory(cereal, 15);
            store.AddToInventory(milk, 30);
            store.AddToInventory(steak, 5);
            List<Store> Stores = new List<Store>();
            Stores.Add(store);
            Store currentStore = null;
            while(running)
            {
                while(currentStore == null)
                {
                    PrintStoreLocations(Stores);
                    Console.Write("Enter a store location to shop from: ");
                    string location = Console.ReadLine();
                    currentStore = GetStoreByLocation(location, Stores);
                }
                int option = 0;
                while(!(option > 0 && option < 8))
                {
                    Console.WriteLine("Choose an option:\n[1] Add Customer\n[2] Search Customers\n[3] Place order for customer\n[4] Display order details\n[5] Display customer order history\n[6] Display store order history\n[7] Change store locations");
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
                switch(option)
                {
                    case 1:
                        AddNewCustomer(currentStore);
                        break;
                    case 2:
                        SearchCustomerByName(currentStore);
                        break;
                    case 3:
                        PlaceNewOrder(currentStore);
                        break;
                    case 4:
                        DisplayOrderDetails(currentStore);
                        break;
                    case 5:
                        DisplayCustomerOrders(currentStore);
                        break;
                    case 6:
                        DisplayStoreOrders(currentStore);
                        break;
                    case 7:
                        currentStore = null;
                        break;
                }
            }
        }

        public static void AddNewCustomer(Store store)
        {
            Console.Write("Enter a name for new customer: ");
            string name = Console.ReadLine();
            Console.Write("Enter an email for new customer: ");
            string email = Console.ReadLine();
            Console.Write("Enter an address for new customer: ");
            string address = Console.ReadLine();
            Customer c = new Customer(name, email, address, store);
            store.AddCustomer(c);
            Console.WriteLine("Customer added successfully!");
        }

        public static void SearchCustomerByName(Store store)
        {
            Console.Write("Enter a name to search customers: ");
            string search = Console.ReadLine();
            Customer find = store.GetCustomerByName(search);
            Console.WriteLine("Customer found!");
            Console.WriteLine("Name\tEmail\tAddress");
            Console.WriteLine($"{find.Name}\t{find.Email}\t{find.Address}");
        }

        public static void PlaceNewOrder(Store store)
        {
            Console.Write("Enter a customer name to order for them: ");
            string name = Console.ReadLine();
            Customer customer = store.GetCustomerByName(name);
            bool ordering = true;
            while(ordering)
            {
                int option2 = 0;
                while(!(option2 > 0 && option2 < 5))
                {
                    Console.WriteLine("Choose an action\n[1] Add item to cart\n[2] Remove item from cart\n[3] Show cart\n[4] Submit order\n");
                    string input = Console.ReadLine();
                    try
                    {
                        option2 = int.Parse(input);
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }
                switch(option2)
                {
                    case 1:
                        store.PrintInventory();
                        Console.Write("Enter an item to add to cart: ");
                        string item = Console.ReadLine();
                        Console.Write("Enter how many you would like: ");
                        int quantity = int.Parse(Console.ReadLine());
                        Product toAdd = store.GetProductByName(item);
                        customer.AddItemToCart(toAdd, quantity);
                        Console.WriteLine("Item added successfully!");
                        break;
                    case 2:
                        customer.PrintCart();
                        Console.Write("Enter an item to remove from cart: ");
                        string item2 = Console.ReadLine();
                        Console.Write("Enter how many you would like to remove: ");
                        int quantity2 = int.Parse(Console.ReadLine());
                        Product toRemove = store.GetProductByName(item2);
                        customer.RemoveItemFromCart(toRemove, quantity2);
                        Console.WriteLine("Item removed successfully!");
                        break;
                    case 3:
                        customer.PrintCart();
                        break;
                    case 4:
                        ordering = false;
                        customer.SubmitOrder();
                        Console.WriteLine("Order placed successfully!");
                        break;
                }
            }
        }

        public static void DisplayOrderDetails(Store store)
        {
            Console.Write("Enter the order number to display: ");
            int orderNumber = int.Parse(Console.ReadLine());
            Order order = store.GetOrderByID(orderNumber);
            order.DisplayDetails();
        }

        public static void DisplayCustomerOrders(Store store)
        {
            Console.Write("Enter a name of a customer to display their orders: ");
            string name = Console.ReadLine();
            Customer customer = store.GetCustomerByName(name);
            customer.PrintOrderHistory();
        }

        public static void DisplayStoreOrders(Store store)
        {
            store.PrintOrderHistory();
        }

        public static Store GetStoreByLocation(string location, List<Store> Stores)
        {
            foreach(var store in Stores)
            {
                if(store.Location == location)
                {
                    return store;
                }
            }
            Console.WriteLine("Store location does not exist.");
            return null;
        }

        public static void PrintStoreLocations(List<Store> Stores)
        {
            foreach(var store in Stores)
            {
                Console.WriteLine(store.Location);
            }
        }
    }
}