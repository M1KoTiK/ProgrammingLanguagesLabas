using System;
using System.Collections.Generic;

namespace laba8.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
