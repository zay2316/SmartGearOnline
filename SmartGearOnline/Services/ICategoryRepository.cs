using SmartGearOnline.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<bool> AddAsync(Category category);
    Task<bool> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int id);
}