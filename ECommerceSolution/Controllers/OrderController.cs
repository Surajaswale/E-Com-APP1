using ECommerceSolution.DTOs.Order;
using ECommerceSolution.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.CheckoutAsync(userId);

            return Ok(result);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.GetMyOrdersAsync(userId);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _service.GetOrderByIdAsync(id, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _service.GetAllOrdersAsync();
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusDto dto)
        {
            await _service.UpdateOrderStatusAsync(dto);
            return Ok("Order status updated successfully");
        }
    }
}