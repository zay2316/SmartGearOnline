using SmartGearOnline.Data;
using SmartGearOnline.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartGearOnline.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SmartGearOnlineContext _context;

        public CategoryRepository(SmartGearOnlineContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
