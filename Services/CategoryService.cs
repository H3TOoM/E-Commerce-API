using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using E_Commerce.Services.Base;

namespace E_Commerce.Services
{
    public class CategoryService : ICategoryService
    {
        // Inject Main repository
        private readonly IMainRepoistory<Category> _mainRepoistory;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IMainRepoistory<Category> mainRepoistory, IUnitOfWork unitOfWork)
        {
            _mainRepoistory = mainRepoistory;
            _unitOfWork = unitOfWork;
        }

        public async Task<Category> CreateCategoryAsync( CategoryDto dto )
        {
            var category = new Category
            {
                Name = dto.Name
            };
            
            await _mainRepoistory.AddAsync( category );
            await _unitOfWork.SaveChangesAsync();

            if (category == null)
            {
                throw new KeyNotFoundException("Category could not be created.");
            }

            return category;

        }

        public async Task<string> DeleteCategoryAsync( int id )
        {
            var category = await _mainRepoistory.GetByIdAsync( id );
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            await _mainRepoistory.DeleteAsync( id );
            await _unitOfWork.SaveChangesAsync();

            return "Category deleted successfully.";
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories = await _mainRepoistory.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                throw new KeyNotFoundException("No categories found.");
            }

            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync( int id )
        {
            var category = await _mainRepoistory.GetByIdAsync( id );
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            return category;
        }

        public async Task<Category> UpdateCategoryAsync( int id, CategoryDto dto )
        {
            var category = await _mainRepoistory.GetByIdAsync( id );
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            category.Name = dto.Name;
            await _mainRepoistory.UpdateAsync(id, category );
            await _unitOfWork.SaveChangesAsync();

            return category;
        }
    }
}
