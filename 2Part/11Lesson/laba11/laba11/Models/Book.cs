using System;
using System.Collections.Generic;

namespace laba11.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int Pages { get; set; }

    public int Year { get; set; }
}
