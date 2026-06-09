namespace ECommerceSolution.DTOs.Cart
{
    public class CartResponseDto
    {
        public List<CartItemResponseDto> Items
        {
            get;
            set;
        }
        =
        new();

        public decimal GrandTotal
        {
            get;
            set;
        }
    }
}