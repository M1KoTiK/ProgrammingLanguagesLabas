using System;
using System.Collections.Generic;

namespace laba9.Models;

public partial class BlogRecord
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string Message { get; set; } = null!;
}
