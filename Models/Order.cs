using System;
using System.Collections.Generic;

namespace OrderFood.Models;

public partial class Order
{
    public int OrderDetailsId { get; set; }

    public string? OrderNo { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? UserId { get; set; }

    public bool? Status { get; set; }

    public int? PaymentId { get; set; }

    public DateTime? OrderDate { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
