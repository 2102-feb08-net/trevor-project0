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
        /// Get the product items mapped to quantities for an order
        /// </summary>
        /// <param name="order">Order to grab items for</param>
        /// <returns>Dictionary of Product-Quantity pairs</returns>
        Dictionary<Product, int> GetOrderItems(Order order);

        /// <summary>
        /// Commit changes to database
        /// </summary>
        void Save();
    }
}
