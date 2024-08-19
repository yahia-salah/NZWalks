using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO;

public partial class HoldDateUpdateDto
{
	public string Name { get; set; }

    public DateTime? BeginHold { get; set; }

	public DateTime? Release { get; set; }
}
