using System;
using System.Collections.Generic;

namespace OrderFood.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public string? ImageUrl { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
