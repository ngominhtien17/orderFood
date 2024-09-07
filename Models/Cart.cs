using System;
using System.Collections.Generic;

namespace OrderFood.Models;

public partial class Cart
{
    public int CardId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? UserId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
