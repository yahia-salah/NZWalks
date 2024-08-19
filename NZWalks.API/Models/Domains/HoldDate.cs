using System;
using System.Collections.Generic;

namespace NZWalks.API.Models.Domains;

public partial class HoldDate
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? BeginHold { get; set; }

    public DateTime? Release { get; set; }
}
