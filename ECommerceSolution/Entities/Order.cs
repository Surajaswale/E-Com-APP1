namespace ECommerceSolution.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderItem> OrderItems
        {
            get;
            set;
        }
        =
        new List<OrderItem>();
    }
}