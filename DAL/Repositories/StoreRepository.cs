using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class StoreRepository : Models.IStoreRepository
    {
        private readonly DbContextOptions<Project0Context> _options;

        /// <summary>
        /// Ensures database connection is working properly
        /// </summary>
        /// <param name="context">Data source</param>
        public StoreRepository(string connectionString)
        {
            _options = new DbContextOptionsBuilder<Project0Context>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public void AddStore(Store store)
        {
            using var _context = new Project0Context(_options);
            var newStore = new StoreDAL
            {
                Name = store.Name,
                City = store.City,
                State = store.State,
                Profit = store.GrossProfit,
            };
            _context.Add(newStore);
            _context.SaveChanges();
        }

        public void AddToInventory(Product product, Store store, int quantity)
        {
            using var _context = new Project0Context(_options);
            //Check store and product exist
            StoreDAL s = _context.Stores.Find(store.ID);
            ProductDAL p = _context.Products.Find(product.ID);
            if(s == null || p == null)
            {
                throw new Exception("Product or store does not exist in database");
            }
            StoreItemDAL newInventoryItem = new StoreItemDAL
            {
                //Id is auto incrementing, so no need to instantiate one here
                StoreId = store.ID,
                ProductId = product.ID,
                Quantity = quantity
            };
            _context.Add(newInventoryItem);
            _context.SaveChanges();
        }

        public Product GetProductFromInventory(int productID, int storeID)
        {
            using var _context = new Project0Context(_options);
            var query = _context.StoreItems
                .Include(s => s.Product)
                .Where(s => s.ProductId == productID && s.StoreId == storeID).First();
            if(query != null)
            {
                return new Product
                {
                    ID = query.ProductId,
                    Name = query.Product.Name,
                    Price = query.Product.Price
                };
            }
            else
            {
                throw new Exception("Couldn't find product in that store");
            }
        }

        public Store GetStoreByID(int id)
        {
            using var _context = new Project0Context(_options);
            StoreDAL query = _context.Stores
                .Include(s => s.StoreItems)
                    .ThenInclude(p => p.Product)
                 .First(s => s.Id == id);

            if(query != null)
            {
                var inventory = query.StoreItems.Select(
                    x => new KeyValuePair<Product, int>(
                    new Product(x.Product.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList();
               
                return new Store
                {
                    ID = query.Id,
                    Name = query.Name,
                    City = query.City,
                    State = query.State,
                    GrossProfit = query.Profit,
                    Inventory = inventory.ToDictionary(x => x.Key, y => y.Value)
                };
            }
            else
            {
                throw new Exception("Could not locate store with this ID");
            }
        }

        public IEnumerable<Store> GetStores(string search = null)
        {
            using var _context = new Project0Context(_options);
            List<Store> stores = new List<Store>();
            IQueryable<StoreDAL> query = _context.Stores
                .Include(s => s.StoreItems)
                    .ThenInclude(p => p.Product);
            if(search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }
            foreach(var store in query)
            {
                var inventory = store.StoreItems.Select(
                    x => new KeyValuePair<Product, int>(
                    new Product(x.Id, x.Product.Name, x.Product.Price), x.Quantity)).ToList();
                stores.Add(new Store
                {
                    ID = store.Id,
                    Name = store.Name,
                    City = store.City,
                    State = store.State,
                    GrossProfit = store.Profit,
                    Inventory = inventory.ToDictionary(x => x.Key, y => y.Value)
                });
            }
            return stores;
        }

        public void RemoveItemFromInventory(Product product, Store store)
        {
            using var _context = new Project0Context(_options);
            var query = _context.StoreItems.Where(x => x.ProductId == product.ID && x.StoreId == store.ID).First();
            if(query != null)
            {
                _context.Remove(query);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Couldn't find product in store inventory");
            }
        }

        public void UpdateItemQuantity(Product product, Store store, int quantity)
        {
            using var _context = new Project0Context(_options);
            var query = _context.StoreItems.Where(p => p.ProductId == product.ID && p.StoreId == store.ID).First();
            if(query != null)
            {
                query.Quantity += quantity;
                _context.Update(query);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Could not locate store to update");
            }
        }

        public void UpdateStore(Store store)
        {
            using var _context = new Project0Context(_options);
            var query = _context.Stores
                .Include(s => s.StoreItems)
                    .ThenInclude(p => p.Product).First(s => s.Id == store.ID);
            if(query != null)
            {
                query.Name = store.Name;
                query.City = store.City;
                query.State = store.State;
                query.Profit = store.GrossProfit;
                _context.Update(query);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Could not locate store to update");
            }
        }

        public void ProcessInventoryForOrder(Store store, Dictionary<Product, int> cart)
        {
            using var _context = new Project0Context(_options);
            var query = _context.StoreItems
                .Include(s => s.Product)
                .Where(s => s.StoreId == store.ID);

            foreach(var storeItem in query)
            {
                foreach(var cartItem in cart)
                {
                    if(storeItem.ProductId == cartItem.Key.ID)
                    {
                        storeItem.Quantity -= cartItem.Value;
                        break;
                    }
                }
                _context.Update(storeItem);
                _context.SaveChanges();
            }
        }
    }
}
