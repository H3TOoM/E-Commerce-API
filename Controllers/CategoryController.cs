using E_Commerce.Data;
using E_Commerce.DTOs;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }


        // Get All Categories
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // Get Category By Id 
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }


        // Add New Category
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto dto)
        {
            // check if the name is not empty
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Category name is required.");
            }

            // check if the category already exists
            var category = new Category
            {
                Name = dto.Name
            };

            // Add the category to the database
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }


        // Get Category By Id and Update Category
        [Authorize]
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateCategory(int id, CategoryDto dto)
        {

            // fetch the category by id
            var category = await _context.Categories.FindAsync(id);
            // check if the category exists
            if (category == null)
            {
                return NotFound("Category not found.");
            }
            // check if the name is not empty
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Category name is required.");
            }
            // update the category
            category.Name = dto.Name;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            // return the updated category
            return Ok(category);
        }

        // Delete Category
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            // fetch the category by id
            var category = await _context.Categories.FindAsync(id);
            // check if the category exists
            if (category == null)
            {
                return NotFound("Category not found.");
            }
            // delete the category
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok("Category deleted successfully.");
        }


    }
}
