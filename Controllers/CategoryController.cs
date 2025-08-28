using E_Commerce.DTOs;
using E_Commerce.Services.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    [Authorize(Roles ="admin")]

    public class CategoryController : ControllerBase
    {

        // Inject category service
        private readonly ICategoryService _categoryService;
        public CategoryController( ICategoryService categoryService )
        {
            _categoryService = categoryService;
        }


        // Get All Categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound( "No categories found." );
            }
            return Ok( categories );
        }

        // Get Category By Id 
        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetCategoryById( int id )
        {
            var category = await _categoryService.GetCategoryByIdAsync( id );
            if (category == null)
            {
                return NotFound();
            }

            return Ok( category );
        }


        // Add New Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory( CategoryDto dto )
        {
            // check if the name is not empty
            if (string.IsNullOrWhiteSpace( dto.Name ))
            {
                return BadRequest( "Category name is required." );
            }


            var category = await _categoryService.CreateCategoryAsync( dto );
            return Ok( category );
        }


        // Get Category By Id and Update Category
        [HttpPut( "{id}" )]

        public async Task<IActionResult> UpdateCategory( int id, CategoryDto dto )
        {

            // fetch the category by id
            var category = await _categoryService.GetCategoryByIdAsync( id );
            // check if the category exists
            if (category == null)
            {
                return NotFound( "Category not found." );
            }
            // check if the name is not empty
            if (string.IsNullOrWhiteSpace( dto.Name ))
            {
                return BadRequest( "Category name is required." );
            }
            // update the category
            category.Name = dto.Name;
            await _categoryService.UpdateCategoryAsync( id, dto );

            // return the updated category
            return Ok( category );
        }

        // Delete Category
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteCategory( int id )
        {
            // fetch the category by id
            var category = await _categoryService.GetCategoryByIdAsync( id );
            // check if the category exists
            if (category == null)
            {
                return NotFound( "Category not found." );
            }
            // delete the category
            await _categoryService.DeleteCategoryAsync( id );
            return Ok( "Category deleted successfully." );
        }


    }
}
