using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models;
using DAL;
using System.IO;

namespace UI
{
    public class StoreApp
    {
        public Inputter Inputter { get; }
        public Outputter Outputter { get; }
        public CustomerRepository CustomerRepo { get; set; }
        public StoreRepository StoreRepo { get; set; }
        public ProductRepository ProductRepo { get; set; }
        public OrderRepository OrderRepo { get; set; }


        public StoreApp()
        {
            string _connectionString = File.ReadAllText("C:/revature/project0-connection-string.txt");
            CustomerRepo = new CustomerRepository(_connectionString);
            StoreRepo = new StoreRepository(_connectionString);
            ProductRepo = new ProductRepository(_connectionString);
            OrderRepo = new OrderRepository(_connectionString);
            Inputter = new Inputter();
            Outputter = new Outputter();
        }
        public void Run()
        {
            bool running = true;
            Store currentStore = null;
            
            while(running)
            {
                while(currentStore == null)
                {
                    PrintStoreLocations();
                    Outputter.Write("Enter a store ID to shop from or type '0' to add a new location: ");
                    int location = Inputter.GetIntegerInput();
                    if(location != 0)
                    {
                        try
                        {
                            currentStore = StoreRepo.GetStoreByID(location);
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
                        Outputter.Write("Enter the stores city: ");
                        string storeCity = Inputter.GetStringInput();
                        Outputter.Write("Enter the stores state: ");
                        string storeState = Inputter.GetStringInput();
                        StoreRepo.AddStore(new Store(storeName, storeCity, storeState));
                        StoreRepo.Save();
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
                        PrintCustomerList();
                        PlaceNewOrder(currentStore);
                        break;
                    case 4:
                        DisplayOrderDetails();
                        break;
                    case 5:
                        DisplayCustomerOrders();
                        break;
                    case 6:
                        PrintStoreOrderHistory(currentStore);
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
            Outputter.Write("Enter first name for new customer: ");
            string firstName = Inputter.GetStringInput();
            Outputter.Write("Enter last name for new customer: ");
            string lastName = Inputter.GetStringInput();
            Outputter.Write("Enter an email for new customer: ");
            string email = Inputter.GetAnyInput();
            Outputter.Write("Enter an address for new customer: ");
            string address = Inputter.GetAnyInput();
            CustomerRepo.AddCustomer(new Customer(firstName, lastName, email, address));
            CustomerRepo.Save();
            Outputter.WriteLine("Customer added successfully!");
        }

        public void SearchCustomerByID()
        {
            Outputter.Write("Enter a customer ID to search for: ");
            int id = Inputter.GetIntegerInput();
            try
            {
                Customer customer = CustomerRepo.GetCustomerByID(id);
                Outputter.WriteLine("Customer found!");
                Outputter.WriteLine("ID\tName\tEmail\tAddress");
                Outputter.WriteLine($"{customer.ID}\t{customer.FirstName + " " + customer.LastName}\t{customer.Email}\t{customer.Address}");
            }
            catch(Exception e)
            {
                Outputter.WriteLine(e.Message);
            }
        }

        public void PlaceNewOrder(Store store)
        {
            Outputter.Write("Enter a customer id to order for them: ");
            int id = Inputter.GetIntegerInput();
            try
            {
                Customer customer = CustomerRepo.GetCustomerByID(id);
                bool ordering = true;
                Dictionary<Product, int> cart = new Dictionary<Product, int>();
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
                            PrintInventory(store.ID, cart);
                            Outputter.Write("Enter an item to add to cart: ");
                            int item = Inputter.GetIntegerInput();
                            Outputter.Write("Enter how many you would like: ");
                            int quantity = Inputter.GetIntegerInput();
                            try
                            {
                                Product p = StoreRepo.GetProductFromInventory(item, store.ID);
                                if (cart.ContainsKey(p))
                                {
                                    cart[p] += quantity;
                                }
                                else
                                {
                                    cart[p] = quantity;
                                }
                                Outputter.WriteLine("Item added successfully!");
                            }
                            catch(Exception)
                            {
                                Outputter.WriteLine("Couldn't find product to add to cart or too much quantity trying to be added.");
                            }
                            break;
                        case 2:
                            PrintCart(cart);
                            Outputter.Write("Enter an item to remove from cart: ");
                            int item2 = Inputter.GetIntegerInput();
                            Outputter.Write("Enter how many you would like to remove: ");
                            int quantity2 = Inputter.GetIntegerInput();
                            try
                            {
                                Product p = GetProductFromStoreByID(store, item2);
                                foreach(var cartItem in cart)
                                {
                                    if(cartItem.Key.ID == p.ID)
                                    {
                                        if(cartItem.Value <= quantity2)
                                        {
                                            cart.Remove(cartItem.Key);
                                            Outputter.WriteLine("Item removed successfully!");
                                        }
                                        else
                                        {
                                            cart[cartItem.Key] -= quantity2;
                                            Outputter.WriteLine($"Removed {quantity2} {p.Name}'s from cart successfully!");
                                        }
                                    }
                                }
                            }
                            catch(Exception)
                            {
                                Outputter.WriteLine("Couldn't find product to remove from cart.");
                            }
                            break;
                        case 3:
                            PrintCart(cart);
                            break;
                        case 4:
                            try
                            {
                                Order order = new Order(customer, store);
                                order.Items = cart;
                                order.CalculateOrderTotal();
                                order.SubmitOrder();
                                StoreRepo.UpdateStore(order.Store);
                                StoreRepo.ProcessInventoryForOrder(order.Store, cart);
                                StoreRepo.Save();
                                ordering = false;
                                OrderRepo.AddOrder(order);
                                OrderRepo.Save();
                                order = OrderRepo.GetMostRecentOrder();
                                foreach(var orderItem in cart)
                                {
                                    OrderRepo.AddOrderItem(orderItem.Key, order, orderItem.Value);
                                }
                                OrderRepo.Save();
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

        public void DisplayOrderDetails()
        {
            Outputter.Write("Enter the order number to display: ");
            int orderNumber = Inputter.GetIntegerInput();
            try
            {
                Order order = OrderRepo.GetOrderByID(orderNumber);
                PrintOrder(order);
            }
            catch(Exception)
            {
                Outputter.WriteLine("Couldn't find an order with that ID.");
            }
        }

        public void PrintOrder(Order order)
        {
            Outputter.WriteLine($"Order number: {order.ID}\nStore: {order.Store.Name}, {order.Store.City}, {order.Store.State}\nCustomer name: {order.Customer.FirstName} {order.Customer.LastName}\nTimestamp: {order.OrderTime.Date.ToString("d")}");
            Outputter.WriteLine("Receipt\n________");
            foreach(var item in order.Items)
            {
                Outputter.WriteLine($"{item.Key.ID} - ({item.Value}) {item.Key.Name} ${item.Key.Price*item.Value}");
            }
            Outputter.WriteLine("________");
            order.CalculateOrderTotal();
            Outputter.WriteLine($"Total: ${order.TotalPrice}");
        }

        public void PrintCart(Dictionary<Product, int> cart)
        {
            foreach (var item in cart)
            {
                Outputter.WriteLine($"{item.Key.ID} - ({item.Value}) {item.Key.Name} ${item.Key.Price * item.Value}");
            }
            Outputter.WriteLine("________");
        }

        public void DisplayCustomerOrders()
        {
            Outputter.Write("Enter a customer ID to display their orders: ");
            int id = Inputter.GetIntegerInput();
            try
            {
                Customer c = CustomerRepo.GetCustomerByID(id);
                PrintCustomerOrderHistory(c);
            }
            catch(Exception)
            {
                Outputter.WriteLine("Couldn't find customer with that ID.");
            }
        }

        public void PrintCustomerOrderHistory(Customer customer)
        {
            List<Order> orderHistory = OrderRepo.GetOrdersByCustomerID(customer.ID).ToList();
            if(orderHistory.Count == 0)
            {
                Outputter.WriteLine("Customer has 0 orders!");
            }
            else
            {
                Outputter.WriteLine("ID\tDate of Order\t\t\tTotal Price");
                Outputter.WriteLine("__________________________________________________________________________________________");
                foreach(var order in orderHistory)
                {
                    Outputter.WriteLine($"{order.ID}\t{order.OrderTime.Date.ToString("d")}\t\t\t${order.TotalPrice}");
                }
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
                    Outputter.WriteLine("Select an action:\n[1] Add a new product\n[2] Add inventory for existing product\n[3] Delete product from inventory\n[4] View Inventory\n[5] Quit back to main menu");
                    option = Inputter.GetIntegerInput();
                }
                switch(option)
                {
                    case 1:
                        Outputter.Write("Enter a product name: ");
                        string name = Inputter.GetStringInput();
                        Outputter.Write("Enter a price: ");
                        decimal price = Inputter.GetDecimalInput();
                        Outputter.Write("How many do you want to add to inventory: ");
                        int quantity = Inputter.GetIntegerInput();
                        try
                        {
                            Product toAdd = new Product(name, price);
                            ProductRepo.AddProduct(toAdd);
                            ProductRepo.Save();
                            toAdd = ProductRepo.GetProductByNameAndPrice(name, Convert.ToDecimal(price)); // This is necessary to get the ID of the product we just added
                            store.AddToInventory(toAdd, quantity); // Don't think I need this
                            StoreRepo.AddToInventory(toAdd, store, quantity);
                            StoreRepo.Save();
                            Outputter.WriteLine("Product added to inventory successfully!");
                        }
                        catch(ArgumentException)
                        {
                            Outputter.WriteLine("Couldn't add product to inventory.");
                        }
                        break;
                    case 2:
                        PrintInventory(store.ID);
                        Outputter.Write("Enter a product ID: ");
                        try
                        {
                            int productID = Inputter.GetIntegerInput();
                            Product p = GetProductFromStoreByID(store, productID);
                            Outputter.Write("How many do you want to add to inventory: ");
                            int quantity2 = Inputter.GetIntegerInput();
                            if(quantity2 > 0)
                            {
                                StoreRepo.UpdateItemQuantity(p, store, quantity2);
                                StoreRepo.Save();
                                Outputter.WriteLine("Product inventory added successfully!");
                            }
                            else
                            {
                                Outputter.WriteLine("Can't add 0 or negative inventory amount");
                            }
                        }
                        catch(Exception)
                        {
                            Outputter.WriteLine("Couldn't add inventory for product.");
                        }
                        break;
                    case 3:
                        PrintInventory(store.ID);
                        Outputter.Write("Enter a product ID to remove: ");
                        int idRemove = Inputter.GetIntegerInput();
                        try
                        {
                            Product remove = GetProductFromStoreByID(store, idRemove);
                            StoreRepo.RemoveItemFromInventory(remove, store);
                            StoreRepo.Save();
                            Outputter.WriteLine("Product successfully removed from inventory!");
                        }
                        catch(Exception)
                        {
                            Outputter.WriteLine("Couldn't remove product from inventory!");
                        }
                        break;
                    case 4:
                        PrintInventory(store.ID);
                        break;
                    case 5:
                        editting = false;
                        StoreRepo.Save();
                        ProductRepo.Save();
                        break;
                }
            }
        }

        public void PrintStoreLocations()
        {
            List<Store> s = StoreRepo.GetStores().ToList();
            if(s.Count == 0)
            {
                Outputter.WriteLine("No locations available!");
            }
            else
            {
                Outputter.WriteLine("Available Locations:");
                foreach(var store in s)
                {
                    Outputter.WriteLine($"{store.ID} - {store.Name} in {store.City}, {store.State}");
                }
            }
        }

        public void PrintInventory(int storeID)
        {
            Store store = StoreRepo.GetStoreByID(storeID);
            if(store.Inventory.Count == 0)
            {
                Outputter.WriteLine("Inventory is empty.");
            }
            else
            {
                Outputter.WriteLine("ID\t\tName\t\tPrice\t\tQuantity");
                foreach(var item in store.Inventory)
                {
                    Outputter.WriteLine($"{item.Key.ID}\t\t{item.Key.Name}\t\t${item.Key.Price}\t\t{item.Value} Available");
                }
            }
        }

        public void PrintInventory(int storeID, Dictionary<Product, int> cart)
        {
            Store store = StoreRepo.GetStoreByID(storeID);
            if(store.Inventory.Count == 0)
            {
                Outputter.WriteLine("Inventory is empty.");
            }
            else
            {
                Outputter.WriteLine("ID\t\tName\t\tPrice\t\tQuantity");
                foreach(var item in store.Inventory)
                {
                    int inOrder = 0;
                    foreach(var cartItem in cart)
                    {
                        if(cartItem.Key.ID == item.Key.ID)
                        {
                            inOrder = cartItem.Value;
                        }
                    }
                    Outputter.WriteLine($"{item.Key.ID}\t\t{item.Key.Name}\t\t${item.Key.Price}\t\t{item.Value-inOrder} Available");
                }
            }
        }

        public Product GetProductFromStoreByID(Store store, int id)
        {
            foreach (var item in store.Inventory)
            {
                if (item.Key.ID == id)
                {
                    return item.Key;
                }
            }
            throw new Exception("Item not found.");
        }

        public void PrintStoreOrderHistory(Store store)
        {
            List<Order> orderHistory = OrderRepo.GetOrdersByStoreID(store.ID).ToList();
            if(orderHistory.Count == 0)
            {
                Outputter.WriteLine("Store has no order history.");
            }
            else
            {
                Outputter.WriteLine("ID\tDate of Order\t\t\tTotal Price\t\t\tCustomer");
                Outputter.WriteLine("__________________________________________________________________________________________");
                foreach(var order in orderHistory)
                {
                    Outputter.WriteLine($"{order.ID}\t{order.OrderTime.Date.ToString("d")}\t\t\t${order.TotalPrice}\t\t\t{order.Customer.FirstName} {order.Customer.LastName}");
                }
            }
        }

        public void PrintCustomerList()
        {
            List<Customer> customers = CustomerRepo.GetCustomers().ToList();
            if(customers.Count == 0)
            {
                Outputter.WriteLine("No customers in database.");
            }
            else
            {
                Outputter.WriteLine("ID\tName\t\t\tEmail\t\t\tAddress");
                Outputter.WriteLine("________________________________________________");
                foreach(var customer in customers)
                {
                    Outputter.WriteLine($"{customer.ID}\t{customer.FirstName + " " + customer.LastName}\t\t\t{customer.Email}\t\t\t{customer.Address}");
                }
            }
        }
    }
}