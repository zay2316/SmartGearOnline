using Microsoft.EntityFrameworkCore;
using SmartGearOnline.Data;
using SmartGearOnline.Models;

namespace SmartGearOnline.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly SmartGearOnlineContext _context;

        public ProductRepository(SmartGearOnlineContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<bool> AddAsync(Product product)
        {
            _context.Products.Add(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}