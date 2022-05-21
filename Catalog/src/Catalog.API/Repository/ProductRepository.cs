using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var result = await _context.Products.DeleteOneAsync(filter);
              return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<Product> GetProductById(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            return await _context.Products.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string categoryName)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
               return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            var result = await _context.Products.ReplaceOneAsync(filter, product); 
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}