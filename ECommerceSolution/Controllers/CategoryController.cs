using ECommerceSolution.DTOs.Category;
using ECommerceSolution.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(
            ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories =
                await _service.GetAllAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var category =
                await _service.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(
                    "Category not found.");
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            CreateCategoryDto dto)
        {
            var result =
                await _service.CreateAsync(dto);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateCategoryDto dto)
        {
            var updated =
                await _service
                .UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound(
                    "Category not found.");
            }

            return Ok(
                "Category updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var deleted =
                await _service
                .DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(
                    "Category not found.");
            }

            return Ok(
                "Category deleted successfully.");
        }
    }
}