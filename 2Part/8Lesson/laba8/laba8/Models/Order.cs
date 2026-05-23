using System;
using System.Collections.Generic;

namespace laba8.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string OrderDetails { get; set; } = null!;

    public string DeliveryAddress { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
