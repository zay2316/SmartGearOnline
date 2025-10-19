using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SmartGearOnline.Models;
using SmartGearOnline.Services;  
using SmartGearOnline.Hubs;

namespace SmartGearOnline.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly IHubContext<ProductHub> _hubContext;
        private readonly ILogger<ProductService> _logger;

        private const string CacheKey = "ProductList";

        public ProductService(
            IProductRepository repository,
            IMemoryCache cache,
            IHubContext<ProductHub> hubContext,
            ILogger<ProductService> logger)
        {
            _repository = repository;
            _cache = cache;
            _hubContext = hubContext;
            _logger = logger;
        }

        // GET: All Products 
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            if (!_cache.TryGetValue(CacheKey, out IEnumerable<Product>? products))
            {
                _logger.LogInformation("Cache miss: loading products from database.");
                products = await _repository.GetAllAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(CacheKey, products, cacheOptions);
            }
            else
            {
                _logger.LogInformation("Cache hit: returning products from memory cache.");
            }

            return products!;
        }

        // GET: Single Product
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // CREATE
        public async Task<bool> AddAsync(Product product)
        {
            var result = await _repository.AddAsync(product);
            if (result)
            {
                InvalidateCache();
                await NotifyClientsAsync();
            }
            return result;
        }

        // UPDATE
        public async Task<bool> UpdateAsync(Product product)
        {
            var result = await _repository.UpdateAsync(product);
            if (result)
            {
                InvalidateCache();
                await NotifyClientsAsync();
            }
            return result;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (result)
            {
                InvalidateCache();
                await NotifyClientsAsync();
            }
            return result;
        }

        // Cache invalidation
        private void InvalidateCache()
        {
            _cache.Remove(CacheKey);
            _logger.LogInformation("Product cache invalidated.");
        }

        // Notify clients in real-time
        private async Task NotifyClientsAsync()
        {
            await _hubContext.Clients.All.SendAsync("ProductListUpdated");
            _logger.LogInformation("SignalR clients notified about product list change.");
        }
    }
}
