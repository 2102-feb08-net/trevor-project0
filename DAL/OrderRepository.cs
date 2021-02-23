﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Project0Context _context;

        /// <summary>
        /// Ensures database connection is working properly
        /// </summary>
        /// <param name="context">Data source</param>
        public OrderRepository(Project0Context context)
        {
            _context = context ?? throw new ArgumentNullException("Error instantiating Store Repository");
        }
        public void AddOrder(Order order)
        {
            OrderDAL newOrder = new OrderDAL
            {
                StoreId = order.Store.ID,
                CustomerId = order.Customer.ID,
                TotalPrice = Convert.ToDecimal(order.TotalPrice),
                OrderTime = order.OrderTime,
            };
            _context.Add(newOrder);
        }

        public void AddOrderItem(Product product, Order order, int quantity)
        {
            OrderItemDAL toAdd = new OrderItemDAL
            {
                //Order item Id is auto incrementing so no need to instantiate here
                OrderId = order.ID,
                ProductId = product.ID,
                Quantity = quantity
            };
            _context.Add(toAdd);
        }

        public Order GetOrderByID(int id)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(p => p.Product)
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .First(o => o.Id == id);
            if(query != null)
            {
                var inventory = query.OrderItems.Select(
                    x => new KeyValuePair<Product, int>(
                    new Product(x.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList();
               
                return new Order
                {
                    ID = query.Id,
                    Store = new Store
                    {
                        ID = query.Store.Id,
                        Name = query.Store.Name,
                        City = query.Store.City,
                        State = query.Store.State,
                        GrossProfit = query.Store.Profit,
                        Inventory = query.Store.StoreItems.Select(
                                x => new KeyValuePair<Product, int>(
                                new Product(x.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList().ToDictionary(x => x.Key, y => y.Value)
                    },
                    Customer = new Customer
                    {
                        ID = query.Customer.Id,
                        FirstName = query.Customer.FirstName,
                        LastName = query.Customer.LastName,
                        Email = query.Customer.Email,
                        Address = query.Customer.Address
                    },
                    TotalPrice = query.TotalPrice,
                    OrderTime = query.OrderTime,
                    Items = inventory.ToDictionary(x => x.Key, y => y.Value)
                };
            }
            else
            {
                throw new Exception("Couldn't find order with that ID");
            }
        }

        public IEnumerable<Order> GetOrdersByCustomerID(int customerID)
        {
            List<Order> orders = new List<Order>();
            var query = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(p => p.Product)
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .Where(o => o.CustomerId == customerID).ToList();
            if(query != null)
            {
                foreach(var order in query)
                {
                    var items = order.OrderItems.Select(
                    x => new KeyValuePair<Product, int>(
                    new Product(x.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList();
                    orders.Add(new Order
                    {
                        ID = order.Id,
                        Store = new Store
                        {
                            ID = order.Store.Id,
                            Name = order.Store.Name,
                            City = order.Store.City,
                            State = order.Store.State,
                            GrossProfit = order.Store.Profit,
                            Inventory = order.Store.StoreItems.Select(
                                x => new KeyValuePair<Product, int>(
                                new Product(x.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList().ToDictionary(x => x.Key, y => y.Value)
                        },
                        Customer = new Customer
                        {
                            ID = order.Customer.Id,
                            FirstName = order.Customer.FirstName,
                            LastName = order.Customer.LastName,
                            Email = order.Customer.Email,
                            Address = order.Customer.Address
                        },
                        TotalPrice = order.TotalPrice,
                        OrderTime = order.OrderTime,
                        Items = items.ToDictionary(x => x.Key, y => y.Value)
                    });
                }
                return orders;
            }
            else
            {
                throw new Exception("Couldn't find any orders for customer with that ID");
            }
        }

        public IEnumerable<Order> GetOrdersByStoreID(int storeId)
        {
            List<Order> orders = new List<Order>();
            var query = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(p => p.Product)
                .Include(o => o.Store)
                .Include(o => o.Customer)
                .Where(o => o.StoreId == storeId).ToList();
            if (query != null)
            {
                foreach (var order in query)
                {
                    var items = order.OrderItems.Select(
                    x => new KeyValuePair<Product, int>(
                    new Product(x.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList();
                    orders.Add(new Order
                    {
                        ID = order.Id,
                        Store = new Store
                        {
                            ID = order.Store.Id,
                            Name = order.Store.Name,
                            City = order.Store.City,
                            State = order.Store.State,
                            GrossProfit = order.Store.Profit,
                            Inventory = order.Store.StoreItems.Select(
                                x => new KeyValuePair<Product, int>(
                                new Product(x.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList().ToDictionary(x => x.Key, y => y.Value)
                        },
                        Customer = new Customer
                        {
                            ID = order.Customer.Id,
                            FirstName = order.Customer.FirstName,
                            LastName = order.Customer.LastName,
                            Email = order.Customer.Email,
                            Address = order.Customer.Address
                        },
                        TotalPrice = order.TotalPrice,
                        OrderTime = order.OrderTime,
                        Items = items.ToDictionary(x => x.Key, y => y.Value)
                    });
                }
                return orders;
            }
            else
            {
                throw new Exception("Couldn't find any orders for customer with that ID");
            }
        }

        public void RemoveOrderItem(Product product, Order order)
        {
            var query = _context.OrderItems.Where(oi => oi.ProductId == product.ID && oi.OrderId == order.ID).First();
            if (query != null)
            {
                _context.Remove(query);
            }
            else
            {
                throw new Exception("Couldn't find order item to remove");
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateOrderItemQuantity(Product product, Order order, int quantity)
        {
            var query = _context.OrderItems.Where(oi => oi.ProductId == product.ID && oi.OrderId == order.ID).First();
            if(query != null)
            {
                query.Quantity += quantity;
                _context.Update(query);
            }
            else
            {
                throw new Exception("Couldn't find order item to update");
            }
        }

        public void UpdateOrder(Order order)
        {
            OrderDAL toUpdate = _context.Orders.Find(order.ID);
            if(toUpdate != null)
            {
                toUpdate.TotalPrice = Convert.ToDecimal(order.TotalPrice);
                toUpdate.OrderTime = order.OrderTime;
                _context.Update(toUpdate);
            }
            else
            {
                throw new Exception("Couldn't find order to update");
            }
        }

        public Order GetMostRecentOrder()
        {
            var query = _context.Orders.OrderByDescending(o => o.Id).First();
            if(query != null)
            {
                var inventory = query.OrderItems.Select(
                    x => new KeyValuePair<Product, int>(
                    new Product(x.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList();

                return new Order
                {
                    ID = query.Id,
                    Store = new Store
                    {
                        ID = query.Store.Id,
                        Name = query.Store.Name,
                        City = query.Store.City,
                        State = query.Store.State,
                        GrossProfit = query.Store.Profit,
                        Inventory = query.Store.StoreItems.Select(
                                x => new KeyValuePair<Product, int>(
                                new Product(x.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList().ToDictionary(x => x.Key, y => y.Value)
                    },
                    Customer = new Customer
                    {
                        ID = query.Customer.Id,
                        FirstName = query.Customer.FirstName,
                        LastName = query.Customer.LastName,
                        Email = query.Customer.Email,
                        Address = query.Customer.Address
                    },
                    TotalPrice = query.TotalPrice,
                    OrderTime = query.OrderTime,
                    Items = inventory.ToDictionary(x => x.Key, y => y.Value)
                };
            }
            else
            {
                throw new Exception("No orders available");
            }
        }
    }
}
