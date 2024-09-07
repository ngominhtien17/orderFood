using System;
using System.Collections.Generic;

namespace OrderFood.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Username { get; set; }

    public string? Mobile { get; set; }

    public string Email { get; set; } = null!;

    public string? Address { get; set; }

    public string? PostCode { get; set; }

    public string? Password { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

    public virtual ICollection<Contact> Contacts { get; } = new List<Contact>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
