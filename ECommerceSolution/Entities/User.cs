using System.ComponentModel.DataAnnotations;

namespace ECommerceSolution.Entities
{
    //This is user entity
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "Customer";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Wishlist> Wishlists
        {
            get;
            set;
        }
 =
 new List<Wishlist>();

        public ICollection<Order> Orders
        {
            get;
            set;
        }


=
new List<Order>();

        public ICollection<Cart> Carts
        {
            get;
            set;
        }
=
new List<Cart>();
    }



}