using System;
using System.Collections.Generic;

namespace OrderFood.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
