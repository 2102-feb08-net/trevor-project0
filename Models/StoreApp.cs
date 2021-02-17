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
            // These will be replaced with deserialization methods soon
            Inputter = new Inputter();
            Outputter = new Outputter();
            Stores = new List<Store>();
            Customers = new List<Customer>();
        }
        public void Run()
        {
            bool running = true;
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
                        try
                        {
                            currentStore = GetStoreByLocation(location, Stores);
                        }
                        catch(Exception)
                        {
                            Outputter.WriteLine("Store location does not exist!");
                        }
                    }
                    else
                    {
                        Outputter.Write("Enter a name for the store: ");
                        string storeName = Inputter.GetAnyInput();
                        Outputter.Write("Enter the store location: ");
                        string storeLocation = Inputter.GetStringInput();
                        Stores.Add(new Store(storeName, storeLocation));
                    }
                }
                int option = 0;
                while(!(option > 0 && option < 10))
                {
                    Outputter.WriteLine("Choose an option:\n[1] Add Customer\n[2] Search Customers\n[3] Place order for customer\n[4] Display order details\n[5] Display customer order history\n[6] Display store order history\n[7] Edit inventory\n[8] Change store locations\n[9] Quit");
                    option = Inputter.GetIntegerInput();
                }
                switch(option)
                {
                    case 1:
                        AddNewCustomer();
                        break;
                    case 2:
                        SearchCustomerByID();
                        break;
                    case 3:
                        PlaceNewOrder(currentStore);
                        break;
                    case 4:
                        DisplayOrderDetails(currentStore);
                        break;
                    case 5:
                        DisplayCustomerOrders();
                        break;
                    case 6:
                        currentStore.PrintOrderHistory();
                        break;
                    case 7:
                        EditInventory(currentStore);
                        break;
                    case 8:
                        currentStore = null;
                        break;
                    case 9:
                        running = false;
                        break;
                }
            }
        }

        public void AddNewCustomer()
        {
            Outputter.Write("Enter a name for new customer: ");
            string name = Inputter.GetStringInput();
            Outputter.Write("Enter an email for new customer: ");
            string email = Inputter.GetAnyInput();
            Outputter.Write("Enter an address for new customer: ");
            string address = Inputter.GetAnyInput();
            Customers.Add(new Customer(name, email, address));
            Outputter.WriteLine("Customer added successfully!");
        }

        public void SearchCustomerByID()
        {
            Outputter.Write("Enter a customer ID to search for: ");
            int id = Inputter.GetIntegerInput();
            try
            {
                Customer customer = GetCustomerByID(id);
                Outputter.WriteLine("Customer found!");
                Outputter.WriteLine("ID\tName\tEmail\tAddress");
                Outputter.WriteLine($"{customer.ID}\t{customer.Name}\t{customer.Email}\t{customer.Address}");
            }
            catch(Exception)
            {
                Outputter.WriteLine("Couldn't find customer with that ID.");
            }
        }

        public void PlaceNewOrder(Store store)
        {
            Outputter.Write("Enter a customer id to order for them: ");
            int id = Inputter.GetIntegerInput();
            try
            {
                Customer customer = GetCustomerByID(id);
                Order order = new Order(customer, store);
                bool ordering = true;
                while(ordering)
                {
                    int option = 0;
                    while(!(option > 0 && option < 6))
                    {
                        Outputter.WriteLine("Choose an action\n[1] Add item to cart\n[2] Remove item from cart\n[3] Show cart\n[4] Submit order\n[5] Cancel");
                        option = Inputter.GetIntegerInput();
                    }
                    switch(option)
                    {
                        case 1:
                            PrintInventory(store);
                            Outputter.Write("Enter an item to add to cart: ");
                            string item = Inputter.GetStringInput();
                            Outputter.Write("Enter how many you would like: ");
                            int quantity = Inputter.GetIntegerInput();
                            try
                            {
                                Product toAdd = store.GetProductByName(item);
                                order.Add(toAdd, quantity);
                                Outputter.WriteLine("Item added successfully!");
                            }
                            catch(Exception)
                            {
                                Outputter.WriteLine("Couldn't find product to add to cart.");
                            }
                            break;
                        case 2:
                            order.DisplayDetails();
                            Outputter.Write("Enter an item to remove from cart: ");
                            string item2 = Inputter.GetStringInput();
                            Outputter.Write("Enter how many you would like to remove: ");
                            int quantity2 = Inputter.GetIntegerInput();
                            try
                            {
                                Product toRemove = store.GetProductByName(item2);
                                order.Delete(toRemove, quantity2);
                                Outputter.WriteLine("Item removed successfully!");
                            }
                            catch(Exception)
                            {
                                Outputter.WriteLine("Couldn't find product to remove from cart.");
                            }
                            break;
                        case 3:
                            order.DisplayDetails();
                            break;
                        case 4:
                            try
                            {
                                order.SubmitOrder();
                                ordering = false;
                                Outputter.WriteLine("Order placed successfully!");
                            }
                            catch(Exception)
                            {
                                Outputter.WriteLine("Couldn't process order!");
                            }
                            break;
                        case 5:
                            ordering = false;
                            Outputter.WriteLine("Order cancelled.");
                            break;
                    }
                }
            }
            catch(Exception)
            {
                Outputter.WriteLine("Couldn't find customer with that ID.");
            }
        }

        public void DisplayOrderDetails(Store store)
        {
            Outputter.Write("Enter the order number to display: ");
            int orderNumber = Inputter.GetIntegerInput();
            try
            {
                Order order = store.GetOrderByID(orderNumber);
                order.DisplayDetails();
            }
            catch(Exception)
            {
                Outputter.WriteLine("Couldn't find an order with that ID.");
            }
        }

        public void DisplayCustomerOrders()
        {
            Outputter.Write("Enter a customer ID to display their orders: ");
            int id = Inputter.GetIntegerInput();
            try
            {
                Customer c = GetCustomerByID(id);
                c.PrintOrderHistory();
            }
            catch(Exception)
            {
                Outputter.WriteLine("Couldn't find customer with that ID.");
            }
        }

        public void EditInventory(Store store)
        {
            bool editting = true;
            while(editting)
            {
                int option = 0;
                while(!(option > 0 && option < 7))
                {
                    Outputter.WriteLine("Select an action:\n[1] Add a new product\n[2] Add inventory for existing product\n[3] Change price of existing product\n[4] Delete product from inventory\n[5] View Inventory\n[6] Quit back to main menu");
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
                        try
                        {
                            store.AddToInventory(new Product(name, price), quantity);
                            Outputter.WriteLine("Product added to inventory successfully!");
                        }
                        catch(ArgumentException)
                        {
                            Outputter.WriteLine("Couldn't add product to inventory.");
                        }
                        break;
                    case 2:
                        Outputter.Write("Enter a product ID: ");
                        try
                        {
                            Product p = store.GetProductByID(Inputter.GetIntegerInput());
                            Outputter.Write("How many do you want to add to inventory: ");
                            int quantity2 = Inputter.GetIntegerInput();
                            store.AddToInventory(p, quantity2);
                            Outputter.WriteLine("Product inventory added successfully!");
                        }
                        catch(Exception)
                        {
                            Outputter.WriteLine("Couldn't add inventory for product.");
                        }
                        break;
                    case 3:
                        Outputter.Write("Enter a product ID: ");
                        int id = Inputter.GetIntegerInput();
                        Outputter.Write("Enter a new price for the item: ");
                        double price2 = Inputter.GetDoubleInput();
                        try
                        {
                            store.UpdateItemPrice(id, price2);
                            Outputter.WriteLine("Price changed successfully!");
                        }
                        catch(Exception)
                        {
                            Outputter.WriteLine("Couldn't change price of product.");
                        }
                        break;
                    case 4:
                        Outputter.Write("Enter a product ID to remove: ");
                        int idRemove = Inputter.GetIntegerInput();
                        try
                        {
                            Product toRemove = store.GetProductByID(idRemove);
                            store.DeleteAll(toRemove);
                            Outputter.WriteLine("Product successfully removed from inventory!");
                        }
                        catch(Exception)
                        {
                            Outputter.WriteLine("Couldn't remove product from inventory!");
                        }
                        break;
                    case 5:
                        PrintInventory(store);
                        break;
                    case 6:
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
            throw new Exception();
        }

        public void PrintStoreLocations(List<Store> Stores)
        {
            if(Stores.Count == 0)
            {
                Outputter.WriteLine("No locations available!");
            }
            else
            {
                Outputter.WriteLine("Available Locations:");
                foreach(var store in Stores)
                {
                    Outputter.WriteLine(store.Location);
                }
            }
        }

        public void PrintInventory(Store store)
        {
            if(store.Inventory.Count == 0)
            {
                Outputter.WriteLine("Inventory is empty.");
            }
            else
            {
                foreach(var item in store.Inventory)
                {
                    Outputter.WriteLine($"{item.Key.Name} - ${item.Key.Price} - {item.Value} Available");
                }
            }
        }

        public Customer GetCustomerByID(int id)
        {
            foreach(var customer in Customers)
            {
                if(customer.ID == id)
                {
                    return customer;
                }
            }
            throw new Exception();
        }
    }
}