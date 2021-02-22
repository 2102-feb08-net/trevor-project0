using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Grab a list of all customers or search by name
        /// </summary>
        /// <param name="name">Optional parameter to search by name</param>
        /// <returns>List of customers</returns>
        IEnumerable<Customer> GetCustomers(string firstName = null, string lastName = null);

        /// <summary>
        /// Get a customer by ID
        /// </summary>
        /// <param name="id">Primary key for customer</param>
        /// <returns>Customer</returns>
        Customer GetCustomerByID(int id);

        /// <summary>
        /// Add a new customer to database
        /// </summary>
        /// <param name="customer">Customer to add</param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Update a customer in the database along with order history
        /// </summary>
        /// <param name="customer">Customer to update</param>
        void UpdateCustomer(Customer customer);

        /// <summary>
        /// Commit changes to database
        /// </summary>
        void Save();
    }
}
