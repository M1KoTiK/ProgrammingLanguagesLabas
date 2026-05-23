using System;
using System.Collections.Generic;

namespace laba12.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public decimal Balance { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
}
