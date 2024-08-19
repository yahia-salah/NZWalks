using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO;

public partial class HoldDatesUpdateDto
{
    public List<HoldDateUpdateDto> Updates { get; set; }
}
