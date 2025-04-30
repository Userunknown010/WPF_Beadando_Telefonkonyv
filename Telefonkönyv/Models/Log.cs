using System;
using System.Collections.Generic;

namespace Telefonkönyv.Models;

public partial class Log
{
    public int LogId { get; set; }

    public int? UserId { get; set; }

    public string? Operation { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User? User { get; set; }
}
