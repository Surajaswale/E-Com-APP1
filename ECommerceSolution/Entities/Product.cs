using ECommerceSolution.Entities;
using ECommerceSolution.Entities;
public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public ICollection<Wishlist> Wishlists
    {
        get;
        set;
    }
   =
   new List<Wishlist>();

    public ICollection<CartItem> CartItems { get; set; }
        = new List<CartItem>();
}