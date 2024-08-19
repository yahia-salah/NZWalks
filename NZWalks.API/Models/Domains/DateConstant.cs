using System;
using System.Collections.Generic;

namespace NZWalks.API.Models.Domains;

public partial class DateConstant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Value { get; set; }

    public bool? IsSystemDate { get; set; }
}
