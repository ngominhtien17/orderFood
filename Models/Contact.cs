using System;
using System.Collections.Generic;

namespace OrderFood.Models;

public partial class Contact
{
    public int ContatcId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User? EmailNavigation { get; set; }
}
