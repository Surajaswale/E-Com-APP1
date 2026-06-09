namespace ECommerceSolution.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string TransactionId { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }
            = DateTime.UtcNow;

        public string Status { get; set; }
            = "Success";
    }
}