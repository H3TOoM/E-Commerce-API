using E_Commerce.DTOs;
using E_Commerce.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        // Inject Product service
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        // Get All Products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);
        }


        // Get Product By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
           var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }

        // Create New Product
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto dto, IFormFile image)
        {
            var product = await _productService.AddProductAsync(dto, image);
            if (product == null)
            {
                return BadRequest("Failed to create product. Please check the provided data.");
            }
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);

        }


        // Update Product
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto dto, IFormFile image)
        {
            var product = await _productService.UpdateProductAsync(id, dto, image);
            if (product == null)
            {
                return NotFound("Product not found or failed to update.");
            }
            return Ok(product);
        }


        // Delete Product
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
           var result = await _productService.DeleteProductAsync(id);
            if (result == null)
            {
                return NotFound("Product not found or failed to delete.");
            }
            return Ok(result);
        }


        // filter by category
        [HttpGet("filterByCategory")]

        public async Task<IActionResult> filterByCategory([FromQuery] string name)
        {

            var filteredProducts = await _productService.FilterByCategoryAsync(name);
    
            if (filteredProducts == null || !filteredProducts.Any())
            {
                return NotFound("No products found for the specified category.");
            }

            return Ok(filteredProducts);

        }

   


    }
}
