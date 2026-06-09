using ECommerceSolution.DTOs.Cart;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;

namespace ECommerceSolution.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;

        public CartService(
            ICartRepository cartRepo,
            IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        // =========================
        // GET CART
        // =========================
        public async Task<CartResponseDto> GetCartAsync(int userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null)
                return new CartResponseDto();

            var items = cart.CartItems.Select(x => new CartItemResponseDto
            {
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Price = x.Product.Price,
                Quantity = x.Quantity,
                TotalPrice = x.Product.Price * x.Quantity
            }).ToList();

            return new CartResponseDto
            {
                Items = items,
                GrandTotal = items.Sum(x => x.TotalPrice)
            };
        }

        // =========================
        // ADD TO CART (FIXED)
        // =========================
        public async Task AddToCartAsync(int userId, AddToCartDto dto)
        {
            var product = await _productRepo.GetByIdAsync(dto.ProductId);

            if (product == null)
                throw new Exception($"Product not found: {dto.ProductId}");

            if (dto.Quantity <= 0)
                throw new Exception("Quantity must be greater than 0");

            if (dto.Quantity > product.StockQuantity)
                throw new Exception(
                    $"Only {product.StockQuantity} items available in stock for {product.Name}");

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };

                await _cartRepo.AddCartAsync(cart);
                await _cartRepo.SaveChangesAsync();
            }

            var existingItem = await _cartRepo
                .GetCartItemAsync(cart.Id, dto.ProductId);

            if (existingItem != null)
            {
                int newQty = existingItem.Quantity + dto.Quantity;

                if (newQty > product.StockQuantity)
                    throw new Exception(
                        $"Cannot add more than stock. Available: {product.StockQuantity}");

                existingItem.Quantity = newQty;

                _cartRepo.UpdateCartItem(existingItem);
            }
            else
            {
                await _cartRepo.AddCartItemAsync(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            await _cartRepo.SaveChangesAsync();
        }

        // =========================
        // REMOVE FROM CART
        // =========================
        public async Task<bool> RemoveFromCartAsync(int userId, int productId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null)
                return false;

            var item = await _cartRepo.GetCartItemAsync(cart.Id, productId);

            if (item == null)
                return false;

            _cartRepo.RemoveCartItem(item);
            await _cartRepo.SaveChangesAsync();

            return true;
        }

        // =========================
        // UPDATE QUANTITY (FIXED)
        // =========================
        public async Task<bool> UpdateQuantityAsync(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than 0");

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null)
                return false;

            var item = await _cartRepo.GetCartItemAsync(cart.Id, productId);

            if (item == null)
                return false;

            var product = await _productRepo.GetByIdAsync(productId);

            if (product == null)
                throw new Exception("Product not found");

            if (quantity > product.StockQuantity)
                throw new Exception(
                    $"Only {product.StockQuantity} items available in stock for {product.Name}");

            item.Quantity = quantity;

            _cartRepo.UpdateCartItem(item);
            await _cartRepo.SaveChangesAsync();

            return true;
        }
    }
}