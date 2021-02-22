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
        private readonly Project0Context _context;

        /// <summary>
        /// Ensures database connection is working properly
        /// </summary>
        /// <param name="context">Data source</param>
        public StoreRepository(Project0Context context)
        {
            _context = context ?? throw new ArgumentNullException("Error instantiating Store Repository");
        }

        public void AddStore(Store store)
        {
            var newStore = new StoreDAL
            {
                Name = store.Name,
                City = store.City,
                State = store.State,
                Profit = Convert.ToDecimal(store.GrossProfit),
            };
            _context.Add(newStore);
        }

        public void AddToInventory(Product product, Store store, int quantity)
        {
            //Check store and product exist
            StoreDAL s = _context.Stores.Find(store.ID);
            ProductDAL p = _context.Products.Find(product.ID);
            if(s == null || p == null)
            {
                throw new Exception("Product or store does not exist in database");
            }
            ProductDAL newProduct = new ProductDAL
            {
                Name = product.Name,
                Price = Convert.ToDecimal(product.Price)
            };
            StoreItemDAL newInventoryItem = new StoreItemDAL
            {
                //Id is auto incrementing, so no need to instantiate one here
                StoreId = store.ID,
                ProductId = product.ID,
                Quantity = quantity
            };
            _context.Add(newProduct);
            _context.Add(newInventoryItem);
        }

        public Store GetStoreByID(int id)
        {
            StoreDAL query = _context.Stores
                .Include(s => s.StoreItems)
                    .ThenInclude(p => p.Product)
                 .First(s => s.Id == id);

            if(query != null)
            {
                var inventory = query.StoreItems.Select(
                    x => new KeyValuePair<Product, int>(
                    new Product(x.Product.Id, x.Product.Name, decimal.ToDouble(x.Product.Price)), x.Quantity)).ToList();
               
                return new Store
                {
                    ID = query.Id,
                    Name = query.Name,
                    City = query.City,
                    State = query.State,
                    GrossProfit = decimal.ToDouble(query.Profit),
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
                    new Product(x.Id, x.Product.Name, decimal.ToDouble(x.Product.Price)), x.Quantity)).ToList();
                stores.Add(new Store
                {
                    ID = store.Id,
                    Name = store.Name,
                    City = store.City,
                    State = store.State,
                    GrossProfit = decimal.ToDouble(store.Profit),
                    Inventory = inventory.ToDictionary(x => x.Key, y => y.Value)
                });
            }
            return stores;
        }

        public void RemoveItemFromInventory(Product product, Store store)
        {
            var query = _context.StoreItems.Where(x => x.ProductId == product.ID && x.StoreId == store.ID).First();
            if(query != null)
            {
                _context.Remove(query);
            }
            else
            {
                throw new Exception("Couldn't find product in store inventory");
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateItemQuantity(Product product, Store store, int quantity)
        {
            var query = _context.StoreItems.Where(p => p.ProductId == product.ID && p.StoreId == store.ID).First();
            if(query != null)
            {
                query.Quantity = quantity;
                _context.Update(query);
            }
            else
            {
                throw new Exception("Could not locate store to update");
            }
        }

        public void UpdateStore(Store store)
        {
            var query = _context.Stores.Find(store.ID);
            if(query != null)
            {
                query.Name = store.Name;
                query.City = store.City;
                query.State = store.State;
                query.Profit = Convert.ToDecimal(store.GrossProfit);
                _context.Update(query);
            }
            else
            {
                throw new Exception("Could not locate store to update");
            }
        }
    }
}
