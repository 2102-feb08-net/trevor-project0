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
            throw new NotImplementedException();
        }

        public void AddToInventory(Product product, Store store, int quantity)
        {
            throw new NotImplementedException();
        }

        public Dictionary<Product, int> GetInventory(Store store)
        {
            throw new NotImplementedException();
        }

        public Store GetStoreByID(int id)
        {
            throw new NotImplementedException();
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
                Location = s.City + ", " + s.State,
                GrossProfit = decimal.ToDouble(s.Profit)
            });
        }

        public void RemoveItemFromInventory(Product product, Store store)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateItemAmount(Product product, Store store, int quantity)
        {
            throw new NotImplementedException();
        }

        public void UpdateStore(Store store)
        {
            throw new NotImplementedException();
        }
    }
}
