using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }


        // Get All Products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return Ok(products);
        }


        // Get Product By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Create New Product
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto dto, IFormFile image)
        {
            
            // check if the image is null
            if (image == null || image.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            // check category exists
            var ccategoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!ccategoryExists)
            {
                return BadRequest("Category does not exist.");
            }

            // create a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var imagePath = Path.Combine("wwwroot/images", fileName);

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
                CategoryId = dto.CategoryId,
                ImageUrl = $"/images/{fileName}" // Store the relative path
            };

            // add the product to the database
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }


        // Update Product
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductDto dto, IFormFile image)
        {
            // check if the category exists
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            // check if the image is null
            if (image != null && image.Length > 0)
            {
                // create a unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var imagePath = Path.Combine("wwwroot/images", fileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                product.ImageUrl = $"/images/{fileName}"; // Store the relative path
            }
            // update product properties
            product.Title = dto.Title;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }


        // Delete Product
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product deleted successfully." });
        }


        // filter by category
        [HttpGet("filterByCategory")]

        public async Task<IActionResult> filterByCategory([FromQuery] string name)
        {

            var filteredProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Category.Name == name)
                .ToListAsync();

            // check is filteredProducts is empty
            if (!filteredProducts.Any())
            {
                return Ok("No Products in this category");
            }

            // return result
            return Ok(filteredProducts);

        }

        // Order by price
        [HttpGet("orderByPrice")]
        public async Task<IActionResult> orderByPrice([FromQuery] string order = "asc")
        {

            // fetch products as a query
            var productsQuery = _context.Products.AsQueryable();

            // if user oreder desc
            if(order.ToLower() == "desc")
            {
                productsQuery = productsQuery.OrderByDescending(p => p.Price);
            }
            else
            {
                productsQuery = productsQuery.OrderBy(p => p.Price);
            }

            // change Query to list 
            var sortedProducts = await productsQuery.ToListAsync();


            // return result
            return Ok(sortedProducts);
        }



    }
}
