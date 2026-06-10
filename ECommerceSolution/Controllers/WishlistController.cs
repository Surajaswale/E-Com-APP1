using ECommerceSolution.DTOs.Wishlist;
using ECommerceSolution.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(
            IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost]
        public async Task<IActionResult>
            AddToWishlist(
                AddWishlistDto dto)
        {
            var userId = int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

            await _wishlistService
                .AddToWishlistAsync(
                    userId,
                    dto);

            return Ok(
                "Product added to wishlist.");
        }

        [HttpGet]
        public async Task<IActionResult>
            GetWishlist()
        {
            var userId = int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

            var wishlist =
                await _wishlistService
                .GetWishlistAsync(userId);

            return Ok(wishlist);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult>
            RemoveFromWishlist(
                int productId)
        {
            var userId = int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

            await _wishlistService
                .RemoveFromWishlistAsync(
                    userId,
                    productId);

            return Ok(
                "Product removed from wishlist.");
        }
    }
}