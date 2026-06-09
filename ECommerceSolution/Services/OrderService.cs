using ECommerceSolution.DTOs.Order;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;

namespace ECommerceSolution.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderResponseDto> CheckoutAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty");

            var products = await _productRepository.GetAllAsync();

            var productMap = products.ToDictionary(p => p.Id);

            foreach (var item in cart.CartItems)
            {
                if (!productMap.ContainsKey(item.ProductId))
                    throw new Exception($"Product not found: {item.ProductId}");

                var product = productMap[item.ProductId];

                if (product.StockQuantity < item.Quantity)
                {
                    throw new Exception(
                        $"Insufficient stock for {product.Name}. Available: {product.StockQuantity}");
                }
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = 0,
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in cart.CartItems)
            {
                var product = productMap[item.ProductId];

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });

                product.StockQuantity -= item.Quantity;

                _productRepository.Update(product);

                total += product.Price * item.Quantity;
            }

            order.TotalAmount = total;

            await _orderRepository.AddAsync(order);

            foreach (var item in cart.CartItems.ToList())
            {
                _cartRepository.RemoveCartItem(item);
            }

            await _orderRepository.SaveChangesAsync();

            return MapOrder(order);
        }

        public async Task<List<OrderResponseDto>> GetMyOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);

            return orders
                .Select(MapOrder)
                .ToList();
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(
            int orderId,
            int userId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                return null;

            if (order.UserId != userId)
                return null;

            return MapOrder(order);
        }

        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders
                .Select(MapOrder)
                .ToList();
        }

        public async Task UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(dto.OrderId);

            if (order == null)
                throw new Exception("Order not found");

            if (!Enum.TryParse<OrderStatus>(
                dto.Status,
                true,
                out var newStatus))
            {
                throw new Exception("Invalid status");
            }

            // Business Rules

            if (order.Status == OrderStatus.Cancelled)
            {
                throw new Exception(
                    "Cancelled order cannot be updated.");
            }

            if (order.Status == OrderStatus.Delivered)
            {
                throw new Exception(
                    "Delivered order cannot be updated.");
            }

            order.Status = newStatus;

            _orderRepository.Update(order);

            await _orderRepository.SaveChangesAsync();
        }

        private static OrderResponseDto MapOrder(Order order)
        {
            return new OrderResponseDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                OrderDate = order.OrderDate,

                Items = order.OrderItems
                    .Select(i => new OrderItemResponseDto
                    {
                        ProductName = i.Product != null
                            ? i.Product.Name
                            : $"Product #{i.ProductId}",

                        Quantity = i.Quantity,

                        UnitPrice = i.UnitPrice,

                        TotalPrice = i.UnitPrice * i.Quantity
                    })
                    .ToList()
            };
        }
    }
}