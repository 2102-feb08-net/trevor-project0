using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Access an order by its order id
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns></returns>
        Order GetOrderByID(int id);

        /// <summary>
        /// Get a list of orders for a store.
        /// </summary>
        /// <param name="storeId">Foreign key for store</param>
        /// <returns>List of orders for a store</returns>
        IEnumerable<Order> GetOrdersByStoreID(int storeId);

        /// <summary>
        /// Get a list of orders for a customer
        /// </summary>
        /// <param name="customerID">Foreign key for customer</param>
        /// <returns>List of orders for a customer</returns>
        IEnumerable<Order> GetOrdersByCustomerID(int customerID);

        /// <summary>
        /// Adds a new order to the database
        /// </summary>
        /// <param name="order">Order to add</param>
        void AddOrder(Order order);

        /// <summary>
        /// Update a given order in the database
        /// </summary>
        /// <param name="order">Order to update</param>
        void UpdateOrder(Order order);

        /// <summary>
        /// Adds a new product to an order
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <param name="order">Order to add product to</param>
        void AddOrderItem(Product product, Order order, int quantity);

        /// <summary>
        /// Get the store associated with an order
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns></returns>
        public Store GetStore(int orderId);

        /// <summary>
        /// Get the customer associated with an order
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns></returns>
        public Customer GetCustomer(int orderId);

        /// <summary>
        /// Update order item information including item name, price or quantity
        /// </summary>
        /// <param name="product">Product on order to edit</param>
        /// <param name="order">Order to edit</param>
        /// <param name="quantity">Optional number to edit quantity on the order</param>
        void UpdateOrderItemQuantity(Product product, Order order, int quantity);

        /// <summary>
        /// Remove a product from an order
        /// </summary>
        /// <param name="product">Product to remove</param>
        /// <param name="order">Order to remove from</param>
        void RemoveOrderItem(Product product, Order order);

        /// <summary>
        /// Commit changes to database
        /// </summary>
        void Save();
    }
}
