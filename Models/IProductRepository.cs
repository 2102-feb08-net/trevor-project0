using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IProductRepository
    {
        /// <summary>
        /// Get a list of products or search by name
        /// </summary>
        /// <param name="search">Optional parameter to search products by name</param>
        /// <returns>List of products</returns>
        IEnumerable<Product> GetProducts(string search = null);

        /// <summary>
        /// Get a product by its ID
        /// </summary>
        /// <param name="id">Primary key of product</param>
        /// <returns>Product</returns>
        Product GetProductByID(int id);

        /// <summary>
        /// Add a product to the database
        /// </summary>
        /// <param name="product">Product to add</param>
        void AddProduct(Product product);

        /// <summary>
        /// Update the price or name of a product in the database
        /// </summary>
        /// <param name="product">Product to update</param>
        void UpdateProduct(Product product);

        /// <summary>
        /// Delete a product from the database
        /// </summary>
        /// <param name="product">Product to delete</param>
        void DeleteProduct(Product product);

        /// <summary>
        /// Commit changes to database
        /// </summary>
        void Save();
    }
}
