using ECommerceSolution.DTOs.Product;
using ECommerceSolution.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(
            IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products =
                await _service.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var product =
                await _service.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(
                    "Product not found.");
            }

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            CreateProductDto dto)
        {
            var product =
                await _service.CreateAsync(dto);

            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            UpdateProductDto dto)
        {
            var updated =
                await _service.UpdateAsync(
                    id,
                    dto);

            if (!updated)
            {
                return NotFound(
                    "Product not found.");
            }

            return Ok(
                "Product updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var deleted =
                await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(
                    "Product not found.");
            }

            return Ok(
                "Product deleted successfully.");
        }
    }
}