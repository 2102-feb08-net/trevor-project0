using System;
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
                OrderTime = order.OrderTime
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

        public Dictionary<Product, int> GetOrderItems(int orderId)
        {
            OrderDAL query = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(p => p.Product)
                .First(o => o.Id == orderId);

            if (query != null)
            {
                var inventory = query.OrderItems.Select(
                    x => new KeyValuePair<Product, int>(
                    new Product(x.Product.Name, decimal.ToDouble(x.Product.Price)), x.Quantity)).ToList();
                return inventory.ToDictionary(x => x.Key, y => y.Value);
            }
            else
            {
                return new Dictionary<Product, int>();
            }
        }

        public Order GetOrderByID(int id)
        {
            var query = _context.Orders.Find(id);
            if(query != null)
            {
                return new Order
                {
                    ID = query.Id,
                    Store = GetStore(id),
                    Customer = GetCustomer(id),
                    TotalPrice = decimal.ToDouble(query.TotalPrice),
                    OrderTime = query.OrderTime,
                    Items = GetOrderItems(id)
                };
            }
            else
            {
                throw new Exception("Couldn't find order with that ID");
            }
        }

        public IEnumerable<Order> GetOrdersByCustomerID(int customerID)
        {
            var query = _context.Orders.Where(o => o.CustomerId == customerID).ToList();
            if(query != null)
            {
                return query.Select(o => new Order
                {
                    ID = o.Id,
                    Store = GetStore(o.Id),
                    Customer = GetCustomer(o.Id),
                    TotalPrice = decimal.ToDouble(o.TotalPrice),
                    OrderTime = o.OrderTime,
                    Items = GetOrderItems(o.Id)
                });
            }
            else
            {
                throw new Exception("Couldn't find any orders for customer with that ID");
            }
        }

        public IEnumerable<Order> GetOrdersByStoreID(int storeId)
        {
            var query = _context.Orders.Where(o => o.StoreId == storeId).ToList();
            if (query != null)
            {
                return query.Select(o => new Order
                {
                    ID = o.Id,
                    Store = GetStore(o.Id),
                    Customer = GetCustomer(o.Id),
                    TotalPrice = decimal.ToDouble(o.TotalPrice),
                    OrderTime = o.OrderTime,
                    Items = GetOrderItems(o.Id)
                });
            }
            else
            {
                throw new Exception("Couldn't find any orders for store with that ID");
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
                query.Quantity = quantity;
                _context.Update(query);
            }
            else
            {
                throw new Exception("Couldn't find order item to update");
            }
        }

        public Store GetStore(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if(order != null)
            {
                var store = _context.Stores.Find(order.StoreId);
                if(store != null)
                {
                    var query = _context.StoreItems.Where(si => si.StoreId == order.StoreId).ToList();
                    var inventory = query.Select(
                        x => new KeyValuePair<Product, int>(
                        new Product(x.Product.Name, decimal.ToDouble(x.Product.Price)), x.Quantity)).ToList();
                    return new Store
                    {
                        ID = store.Id,
                        Name = store.Name,
                        City = store.City,
                        State = store.State,
                        GrossProfit = decimal.ToDouble(store.Profit),
                        Inventory = inventory.ToDictionary(x => x.Key, y => y.Value)
                    };
                }
                else
                {
                    throw new Exception("Couldn't find store associated with order");
                }
            }
            else
            {
                throw new Exception("Couldn't find order with that ID");
            }
            
        }

        public Customer GetCustomer(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                var customer = _context.Customers.Find(order.CustomerId);
                if(customer != null)
                {
                    return new Customer
                    {
                        ID = customer.Id,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Address = customer.Address
                    };
                }
                else
                {
                    throw new Exception("Couldn't find customer associated with this order");
                }
            }
            else
            {
                throw new Exception("Couldn't find order with that ID");
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
    }
}
