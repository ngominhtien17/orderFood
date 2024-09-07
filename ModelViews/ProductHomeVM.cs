using OrderFood.Models;

namespace OrderFood.ModelViews
{
    public class ProductHomeVM
    {
        public Category category { get; set; }
        public List<Product> IsProducts { get; set; }
    }
}
