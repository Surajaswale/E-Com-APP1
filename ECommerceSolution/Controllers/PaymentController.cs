using System.Security.Claims;
using ECommerceSolution.DTOs.Payment;
using ECommerceSolution.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(
            IPaymentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult>
            ProcessPayment(
                ProcessPaymentDto dto)
        {
            var userId = int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier)!);

            var result =
                await _service
                .ProcessPaymentAsync(
                    userId,
                    dto);

            return Ok(result);
        }
    }
}