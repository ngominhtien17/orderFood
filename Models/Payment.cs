using System;
using System.Collections.Generic;

namespace OrderFood.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string? Name { get; set; }

    public string? CardNo { get; set; }

    public string? ExpiryDate { get; set; }

    public int? CvvNo { get; set; }

    public string? Address { get; set; }

    public string? PaymentMode { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
