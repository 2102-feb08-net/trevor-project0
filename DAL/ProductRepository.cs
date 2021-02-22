using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ProductRepository : IProductRepository
    {
        private readonly Project0Context _context;

        /// <summary>
        /// Ensures database connection is working properly
        /// </summary>
        /// <param name="context">Data source</param>
        public ProductRepository(Project0Context context)
        {
            _context = context ?? throw new ArgumentNullException("Error instantiating Store Repository");
        }
        public void AddProduct(Product product)
        {
            ProductDAL newProduct = new ProductDAL
            {
                Name = product.Name,
                Price = Convert.ToDecimal(product.Price)
            };
            _context.Add(newProduct);
        }

        public void DeleteProduct(Product product)
        {
            var query = _context.Products.Find(product.ID);
            if(query != null)
            {
                _context.Remove(query);
            }
            else
            {
                throw new Exception("Couldn't find product to delete");
            }
        }

        public Product GetProductByID(int id)
        {
            var query = _context.Products.Find(id);
            if(query != null)
            {
                return new Product
                {
                    ID = query.Id,
                    Name = query.Name,
                    Price = decimal.ToDouble(query.Price)
                };
            }
            else
            {
                throw new Exception("Couldn't find product with that ID");
            }
        }

        public Product GetProductByNameAndPrice(string name, decimal p)
        {
            var query = _context.Products.Where(x => x.Name.Contains(name) && x.Price == p).First();
            if(query != null)
            {
                return new Product
                {
                    ID = query.Id,
                    Name = query.Name,
                    Price = decimal.ToDouble(query.Price)
                };
            }
            else
            {
                throw new Exception("Couldn't find product with that name and price");
            }
        }

        public IEnumerable<Product> GetProducts(string search = null)
        {
            IQueryable<ProductDAL> query = _context.Products;
            if(search != null)
            {
                query = query.Where(p => p.Name.Contains(search));
            }
            if(query != null)
            {
                return query.Select(p => new Product
                {
                    ID = p.Id,
                    Name = p.Name,
                    Price = decimal.ToDouble(p.Price)
                });
            }
            else
            {
                throw new Exception("No products exist");
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var query = _context.Products.Find(product.ID);
            if(query != null)
            {
                query.Name = product.Name;
                query.Price = Convert.ToDecimal(product.Price);
                _context.Update(query);
            }
            else
            {
                throw new Exception("Couldn't find product to update");
            }
        }
    }
}
