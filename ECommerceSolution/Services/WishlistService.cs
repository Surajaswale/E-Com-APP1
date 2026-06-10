    using ECommerceSolution.DTOs.Wishlist;
    using ECommerceSolution.Entities;
    using ECommerceSolution.Interfaces;

    namespace ECommerceSolution.Services
    {
        public class WishlistService : IWishlistService
        {
            private readonly IWishlistRepository _wishlistRepository;
            private readonly IProductRepository _productRepository;

            public WishlistService(
                IWishlistRepository wishlistRepository,
                IProductRepository productRepository)
            {
                _wishlistRepository = wishlistRepository;
                _productRepository = productRepository;
            }

            public async Task AddToWishlistAsync(
                int userId,
                AddWishlistDto dto)
            {
                var product =
                    await _productRepository
                    .GetByIdAsync(dto.ProductId);

                if (product == null)
                {
                    throw new Exception(
                        "Product not found.");
                }

                var existingWishlistItem =
                    await _wishlistRepository
                    .GetWishlistItemAsync(
                        userId,
                        dto.ProductId);

                if (existingWishlistItem != null)
                {
                    throw new Exception(
                        "Product already exists in wishlist.");
                }

                var wishlistItem =
                    new Wishlist
                    {
                        UserId = userId,
                        ProductId = dto.ProductId
                    };

                await _wishlistRepository
                    .AddAsync(wishlistItem);

                await _wishlistRepository
                    .SaveChangesAsync();
            }

            public async Task<List<WishlistResponseDto>>
                GetWishlistAsync(
                    int userId)
            {
                var wishlistItems =
                    await _wishlistRepository
                    .GetUserWishlistAsync(userId);

                return wishlistItems
                    .Select(w => new WishlistResponseDto
                    {
                        WishlistId = w.Id,
                        ProductId = w.ProductId,
                        ProductName = w.Product?.Name ?? "",
                        Price = w.Product?.Price ?? 0,
                        ImageUrl = w.Product?.ImageUrl,
                        AddedAt = w.AddedAt
                    })
                    .ToList();
            }

            public async Task RemoveFromWishlistAsync(
                int userId,
                int productId)
            {
                var wishlistItem =
                    await _wishlistRepository
                    .GetWishlistItemAsync(
                        userId,
                        productId);

                if (wishlistItem == null)
                {
                    throw new Exception(
                        "Wishlist item not found.");
                }

                _wishlistRepository
                    .Remove(wishlistItem);

                await _wishlistRepository
                    .SaveChangesAsync();
            }
        }
    }