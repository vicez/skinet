using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            database = redis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var data = await database.StringGetAsync(id);
            
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var created = await database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket),  TimeSpan.FromDays(30));
            if(!created) return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}