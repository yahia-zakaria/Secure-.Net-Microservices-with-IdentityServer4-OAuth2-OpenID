using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration config)
        {
            var dbClient = new MongoClient(config.GetValue<string>("DatabaseSettings:ConnectionString"));
            var db = dbClient.GetDatabase(config.GetValue<string>("DatabaseSettings:DatabaseName"));
            Products = db.GetCollection<Product>(config.GetValue<string>("DatabaseSettings:CollectionName"));
            CatalogContextSeed.SeedData(Products);
        }
         public IMongoCollection<Product> Products { get; }
    }
}