using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    class StoreRepository : Models.IStoreRepository
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
                Id = store.ID,
                Name = store.Name,
                City = store.City,
                State = store.State,
                Profit = Convert.ToDecimal(store.GrossProfit),
            };
            _context.Add(newStore);
        }

        public void AddToInventory(Product product, Store store, int quantity)
        {
            ProductDAL newProduct = new ProductDAL
            {
                Id = product.ID,
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

        public Dictionary<Product, int> GetInventory(int storeId)
        {
            StoreDAL query = _context.Stores
                .Include(s => s.StoreItems)
                    .ThenInclude(p => p.Product)
                .First(s => s.Id == storeId);

            if(query != null)
            {
                var inventory = query.StoreItems.Select(
                    x => new KeyValuePair<Product, int>(
                    new Product(x.Product.Name, decimal.ToDouble(x.Product.Price)), x.Quantity)).ToList();
                return inventory.ToDictionary(x => x.Key, y => y.Value);
            }
            else
            {
                return new Dictionary<Product, int>();
            }
        }

        public Store GetStoreByID(int id)
        {
            StoreDAL query = _context.Stores.Find(id);
            if(query != null)
            {
                return new Store
                {
                    ID = query.Id,
                    Name = query.Name,
                    City = query.City,
                    State = query.State,
                    GrossProfit = decimal.ToDouble(query.Profit),
                    Inventory = GetInventory(id)
                };
            }
            else
            {
                throw new Exception("Could not locate store with this ID");
            }
        }

        public IEnumerable<Store> GetStores(string search = null)
        {
            IQueryable<StoreDAL> query = _context.Stores;
            if(search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }
            return query.Select(s => new Store
            {
                ID = s.Id,
                Name = s.Name,
                City = s.City,
                State = s.State,
                GrossProfit = decimal.ToDouble(s.Profit),
                Inventory = GetInventory(s.Id)
            });
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
