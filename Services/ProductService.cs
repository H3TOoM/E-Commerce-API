using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using E_Commerce.Services.Base;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMainRepoistory<Product> _productRepository;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _productRepository = _unitOfWork.GetRepository<Product>();
        }

        // Get all products
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products =  await _productRepository.GetAllWithIncludeAsync(p => p.Category);
            if (products == null || !products.Any())
            {
                return products ?? Enumerable.Empty<Product>();
            }
            return products;

        }

        // Filter products by category
        public async Task<IEnumerable<Product>> FilterByCategoryAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentException("Category cannot be null or empty.");
            }

            var filteredProducts = await _productRepository.GetAllWithFilterAsync(
                p => p.Category.Name == category,
                p => p.Category
            );

            if (filteredProducts == null || !filteredProducts.Any())
            {
                return filteredProducts ?? Enumerable.Empty<Product>();
            }

            return filteredProducts;
        }

        // Get product by ID
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            return product;
        }


        // Add a new product
        public async Task<Product> AddProductAsync(ProductDto dto, IFormFile image)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Title) || dto.Price <= 0)
            {
                throw new ArgumentException("Invalid product data provided.");
            }


            // check if image is null or empty
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException("Image file is required.");
            }



            // check if category exists
            if (dto.CategoryId <= 0)
            {
                throw new ArgumentException("Invalid category ID provided.");
            }

            // create a unique image name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension( image.FileName );
            var imagePath = Path.Combine( "wwwroot", "images", fileName );
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // create a new product
            var product = new Product
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = $"/images/{fileName}",
                CategoryId = dto.CategoryId
            };


            // add the product to the repository
            await _productRepository.AddAsync(product);
            // save changes to the database
            await _unitOfWork.SaveChangesAsync();

            return product;

        }


        // Update an existing product
        public async Task<Product> UpdateProductAsync(int id, ProductDto dto, IFormFile image)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Title) || dto.Price <= 0)
            {
                throw new ArgumentException("Invalid product data provided.");
            }
            // check if product exists
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            // check if image is provided
            if (image != null && image.Length > 0)
            {
                // create a unique image name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine("wwwroot", "images", "products", fileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                existingProduct.ImageUrl = $"/images/products/{fileName}";
            }
            // update product properties
            existingProduct.Title = dto.Title;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.CategoryId = dto.CategoryId;
            // update the product in the repository
            await _productRepository.UpdateAsync(id, existingProduct);
            // save changes to the database
            await _unitOfWork.SaveChangesAsync();
            return existingProduct;
        }


        // Delete a product
        public async Task<string> DeleteProductAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid product ID provided.");
            }
            // check if product exists
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            // delete the product from the repository
            await _productRepository.DeleteAsync(id);
            // save changes to the database
            await _unitOfWork.SaveChangesAsync();
            return "Product deleted successfully.";
        }




    }
}
