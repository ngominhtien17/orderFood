using OrderFood.Models;

namespace OrderFood.ModelViews
{
    public class CartItem
    {
        public Product product { get; set; }
        public int amount { get; set; }
        public decimal price => amount * product.Price.Value;
    }
}
