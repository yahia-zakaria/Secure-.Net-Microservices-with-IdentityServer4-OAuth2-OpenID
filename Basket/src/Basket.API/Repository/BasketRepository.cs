using System;
using System.Text.Json;
using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache redisCache;
        public BasketRepository(IDistributedCache redisCache)
        {
            this.redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }
        public async Task DeleteBasket(string username)
        {
            await redisCache.RemoveAsync(username);
        }

        public async Task<ShoppingCart> GetBasket(string username)
        {
            var basketString = await redisCache.GetStringAsync(username);
            var basket = JsonSerializer.Deserialize<ShoppingCart>(basketString); 
            return basket;
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            var basketString = JsonSerializer.Serialize(basket);
            await redisCache.SetStringAsync(basket.Username, basketString);
            return await GetBasket(basket.Username);
        }
    }
}