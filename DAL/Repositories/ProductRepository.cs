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
        private readonly DbContextOptions<Project0Context> _options;

        /// <summary>
        /// Ensures database connection is working properly
        /// </summary>
        /// <param name="context">Data source</param>
        public ProductRepository(string connectionString)
        {
            _options = new DbContextOptionsBuilder<Project0Context>()
                .UseSqlServer(connectionString)
                .Options;
        }
        public void AddProduct(Product product)
        {
            using var _context = new Project0Context(_options);
            ProductDAL newProduct = new ProductDAL
            {
                Name = product.Name,
                Price = Convert.ToDecimal(product.Price)
            };
            _context.Add(newProduct);
            _context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            using var _context = new Project0Context(_options);
            var query = _context.Products.Find(product.ID);
            if(query != null)
            {
                _context.Remove(query);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Couldn't find product to delete");
            }
        }

        public Product GetProductByID(int id)
        {
            using var _context = new Project0Context(_options);
            var query = _context.Products.Find(id);
            if(query != null)
            {
                return new Product
                {
                    ID = query.Id,
                    Name = query.Name,
                    Price = query.Price
                };
            }
            else
            {
                throw new Exception("Couldn't find product with that ID");
            }
        }

        public Product GetProductByNameAndPrice(string name, decimal p)
        {
            using var _context = new Project0Context(_options);
            var query = _context.Products.Where(x => x.Name.Contains(name) && x.Price == p).First();
            if(query != null)
            {
                return new Product
                {
                    ID = query.Id,
                    Name = query.Name,
                    Price = query.Price
                };
            }
            else
            {
                throw new Exception("Couldn't find product with that name and price");
            }
        }

        public IEnumerable<Product> GetProducts(string search = null)
        {
            using var _context = new Project0Context(_options);
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
                    Price = p.Price
                });
            }
            else
            {
                throw new Exception("No products exist");
            }
        }

        public void UpdateProduct(Product product)
        {
            using var _context = new Project0Context(_options);
            var query = _context.Products.Find(product.ID);
            if(query != null)
            {
                query.Name = product.Name;
                query.Price = Convert.ToDecimal(product.Price);
                _context.Update(query);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Couldn't find product to update");
            }
        }
    }
}
