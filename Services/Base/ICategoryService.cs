using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Services.Base
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);

        Task<Category> CreateCategoryAsync(CategoryDto dto);

        Task<Category> UpdateCategoryAsync(int id, CategoryDto dto);

        Task<string> DeleteCategoryAsync(int id);
    }
}
