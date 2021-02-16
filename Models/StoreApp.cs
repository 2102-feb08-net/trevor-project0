using System;
using System.Collections.Generic;

namespace Models
{
    public class StoreApp
    {
        public Inputter Inputter {get;}
        public Outputter Outputter {get;}
        public List<Store> Stores {get; set;}
        public List<Customer> Customers {get; set;}

        public StoreApp()
        {
            Inputter = new Inputter();
            Outputter = new Outputter();
            Stores = new List<Store>();
            Customers = new List<Customer>();
        }
        public void Run()
        {
            bool running = true;
            //Test material
            //Store store = new Store("Giant foods", "Annapolis");
            //Product apple = new Product("Apple", 0.5);
            //Product cereal = new Product("Cheerios", 3);
            //Product milk = new Product("Milk", 4);
            //Product steak = new Product("Steak", 12);
            //store.AddToInventory(apple, 20);
            //store.AddToInventory(cereal, 15);
            //store.AddToInventory(milk, 30);
            //store.AddToInventory(steak, 5);
            //Stores.Add(store);
            Store currentStore = null;

            while(running)
            {
                while(currentStore == null)
                {
                    PrintStoreLocations(Stores);
                    Outputter.Write("Enter a store location to shop from or type 'new' to add a new location: ");
                    string location = Inputter.GetStringInput();
                    if(location.ToLower() != "new")
                    {
                        currentStore = GetStoreByLocation(location, Stores);
                    }
                    else
                    {
                        Outputter.Write("Enter a name for the store: ");
                        string storeName = Inputter.GetStringInput();
                        Outputter.Write("Enter the store location: ");
                        string storeLocation = Inputter.GetStringInput();
                        Stores.Add(new Store(storeName, storeLocation));
                    }
                }
                int option = 0;
                while(!(option > 0 && option < 8))
                {
                    Outputter.WriteLine("Choose an option:\n[1] Add Customer\n[2] Search Customers\n[3] Place order for customer\n[4] Display order details\n[5] Display customer order history\n[6] Display store order history\n[7] Edit inventory\n[8] Change store locations");
                    option = Inputter.GetIntegerInput();
                }
                switch(option)
                {
                    case 1:
                        Customer c = AddNewCustomer(currentStore);
                        Customers.Add(c);
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
                        EditInventory(currentStore);
                        break;
                    case 8:
                        currentStore = null;
                        break;
                }
            }
        }

        public Customer AddNewCustomer(Store store)
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
            return c;
        }

        public void SearchCustomerByName(Store store)
        {
            Console.Write("Enter a name to search customers: ");
            string search = Console.ReadLine();
            Customer find = store.GetCustomerByName(search);
            Console.WriteLine("Customer found!");
            Console.WriteLine("Name\tEmail\tAddress");
            Console.WriteLine($"{find.ID}\t{find.Name}\t{find.Email}\t{find.Address}");
        }

        public void PlaceNewOrder(Store store)
        {
            Console.Write("Enter a customer id to order for them: ");
            string input = Console.ReadLine();
            int id = int.Parse(input);
            Customer customer = store.GetCustomerByID(id);
            bool ordering = true;
            while(ordering)
            {
                int option = 0;
                while(!(option > 0 && option < 5))
                {
                    Console.WriteLine("Choose an action\n[1] Add item to cart\n[2] Remove item from cart\n[3] Show cart\n[4] Submit order\n");
                    input = Console.ReadLine();
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

        public void DisplayOrderDetails(Store store)
        {
            Console.Write("Enter the order number to display: ");
            int orderNumber = int.Parse(Console.ReadLine());
            Order order = store.GetOrderByID(orderNumber);
            order.DisplayDetails();
        }

        public void DisplayCustomerOrders(Store store)
        {
            Console.Write("Enter a name of a customer to display their orders: ");
            string name = Console.ReadLine();
            Customer customer = store.GetCustomerByName(name);
            customer.PrintOrderHistory();
        }

        public void DisplayStoreOrders(Store store)
        {
            store.PrintOrderHistory();
        }

        public void EditInventory(Store store)
        {
            bool editting = true;
            while(editting)
            {
                int option = 0;
                while(!(option > 0 && option < 5))
                {
                    Outputter.WriteLine("Select an action:\n[1] Add a new product\n[2] Add inventory for existing product\n[3] Change price of existing product\n[4] Quit back to main menu");
                    option = Inputter.GetIntegerInput();
                }
                switch(option)
                {
                    case 1:
                        Outputter.Write("Enter a product name: ");
                        string name = Inputter.GetStringInput();
                        Outputter.Write("Enter a price: ");
                        double price = Inputter.GetDoubleInput();
                        Outputter.Write("How many do you want to add to inventory: ");
                        int quantity = Inputter.GetIntegerInput();
                        Product p = new Product(name, price);
                        store.AddToInventory(p, quantity);
                        Outputter.WriteLine("Product added to inventory successfully!");
                        break;
                    case 2:
                        Outputter.Write("Enter a product ID: ");
                        Product p2 = store.GetProductByID(Inputter.GetIntegerInput());
                        Outputter.Write("How many do you want to add to inventory: ");
                        int quantity2 = Inputter.GetIntegerInput();
                        store.AddToInventory(p2, quantity2);
                        Outputter.WriteLine("Product inventory added successfully!");
                        break;
                    case 3:
                        Outputter.Write("Enter a product ID: ");
                        Product p3 = store.GetProductByID(Inputter.GetIntegerInput());
                        Outputter.Write("Enter a new price for the item: ");
                        double price2 = Inputter.GetDoubleInput();
                        p3.ChangePrice(price2);
                        store.UpdateItemPrice(p3);
                        Outputter.WriteLine("Price changed successfully!");
                        break;
                    case 4:
                        editting = false;
                        break;
                }
            }
        }

        public Store GetStoreByLocation(string location, List<Store> Stores)
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

        public void PrintStoreLocations(List<Store> Stores)
        {
            foreach(var store in Stores)
            {
                Console.WriteLine(store.Location);
            }
        }
    }
}