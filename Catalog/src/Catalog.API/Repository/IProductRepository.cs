using Catalog.API.Entities;

namespace Catalog.API.Repository
{
    public interface IProductRepository
    {
         Task<IEnumerable<Product>> GetProducts();
         Task<Product> GetProductById(string id);
         Task<Product> GetProductByName(string name);
         Task<IEnumerable<Product>> GetProductsByCategory(string categoryName);
         Task CreateProduct(Product product);
         Task<bool> UpdateProduct(Product product);
         Task<bool> DeleteProduct(string id);
    }
}