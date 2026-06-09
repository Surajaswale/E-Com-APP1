using ECommerceSolution.DTOs.Payment;
using ECommerceSolution.Entities;
using ECommerceSolution.Interfaces;

namespace ECommerceSolution.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentResponseDto>
            ProcessPaymentAsync(
                int userId,
                ProcessPaymentDto dto)
        {
            var order =
                await _orderRepository
                .GetByIdAsync(dto.OrderId);

            if (order == null)
                throw new Exception("Order not found");

            if (order.UserId != userId)
                throw new Exception("Unauthorized order");

            if (order.Status != OrderStatus.Pending)
                throw new Exception("Order already paid");

            var existingPayment =
                await _paymentRepository
                .GetByOrderIdAsync(dto.OrderId);

            if (existingPayment != null)
                throw new Exception("Payment already exists");

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                PaymentMethod = dto.PaymentMethod,
                TransactionId =
                    Guid.NewGuid().ToString()
            };

            await _paymentRepository
                .AddAsync(payment);

            order.Status = OrderStatus.Processing;

            await _paymentRepository
                .SaveChangesAsync();

            await _orderRepository
                .SaveChangesAsync();

            return new PaymentResponseDto
            {
                PaymentId = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod =
                    payment.PaymentMethod,
                TransactionId =
                    payment.TransactionId,
                PaymentDate =
                    payment.PaymentDate,
                Status =
                    payment.Status
            };
        }
    }
}