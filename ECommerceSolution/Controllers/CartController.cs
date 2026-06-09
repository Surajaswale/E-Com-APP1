using ECommerceSolution.DTOs.Cart;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // only logged-in users can use cart
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Helper: Get UserId from JWT
     

         private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID claim not found.");

        return int.Parse(userIdClaim.Value);
    }

    // =========================
    // GET CART
    // =========================
    [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();

            var cart = await _cartService.GetCartAsync(userId);

            return Ok(cart);
        }

        // =========================
        // ADD TO CART
        // =========================
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            var userId = GetUserId();

            await _cartService.AddToCartAsync(userId, dto);

            return Ok(new
            {
                message = "Product added to cart"
            });
        }

        // =========================
        // UPDATE QUANTITY
        // =========================
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuantity(
            int productId,
            int quantity)
        {
            var userId = GetUserId();

            var result = await _cartService
                .UpdateQuantityAsync(userId, productId, quantity);

            if (!result)
                return NotFound("Item not found in cart");

            return Ok("Quantity updated");
        }

        // =========================
        // REMOVE ITEM
        // =========================
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var userId = GetUserId();

            var result = await _cartService
                .RemoveFromCartAsync(userId, productId);

            if (!result)
                return NotFound("Item not found in cart");

            return Ok("Item removed from cart");
        }
    }
}