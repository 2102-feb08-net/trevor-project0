﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IStoreRepository
    {
        /// <summary>
        /// Returns a list of all stores.
        /// </summary>
        /// <param name="search">Search for store with some name</param>
        /// <returns>A list of Stores</returns>
        List<Store> GetStores(string search = null);
        
        /// <summary>
        /// Get a store by its Primary key in the database.
        /// </summary>
        /// <param name="id">Store primary key</param>
        /// <returns>Store</returns>
        Store GetStoreByID(int id);

        /// <summary>
        /// Adds a new store to the database along with associated inventory/products.
        /// </summary>
        /// <param name="store">Store to be added</param>
        void AddStore(Store store);

        /// <summary>
        /// Updates a store along with its inventory and order history
        /// </summary>
        /// <param name="store"></param>
        void UpdateStore(Store store);

        /// <summary>
        /// Adds a new item to the store inventory.
        /// </summary>
        /// <param name="toAdd">Product to add</param>
        /// <param name="store">Store to add product to</param>
        /// <param nam="quantity">Number to add to inventory</param>
        void AddToInventory(Product product, Store store, int quantity);

        /// <summary>
        /// Get a product from the store inventory by ID
        /// </summary>
        /// <param name="productID">Product ID</param>
        /// <param name="storeID">Store ID</param>
        /// <returns>Product</returns>
        Product GetProductFromInventory(int productID, int storeID);

        /// <summary>
        /// Updates the quantity on a product for a store
        /// </summary>
        /// <param name="update">Product to update</param>
        /// <param name="store">Store to update inventory</param>
        /// <param name="quantity">Number to add to inventory</param>
        void UpdateItemQuantity(Product product, Store store, int quantity);

        /// <summary>
        /// Remove an item from a stores inventory
        /// </summary>
        /// <param name="product">Product to be removed</param>
        /// <param name="store">Store to be updated</param>
        void RemoveItemFromInventory(Product product, Store store);
    }
}
