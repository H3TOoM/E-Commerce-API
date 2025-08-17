using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Services.Base
{
    public interface IProductService
    {
        // CRUD operations
       
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task<IEnumerable<Product>> FilterByCategoryAsync(string category);

        Task<Product> AddProductAsync(ProductDto dto , IFormFile image);

        Task<Product> UpdateProductAsync(int id, ProductDto dto, IFormFile image);

        Task<string> DeleteProductAsync(int id);
    }
}
