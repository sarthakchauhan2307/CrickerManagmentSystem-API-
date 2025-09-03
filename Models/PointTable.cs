using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrickerManagmentSystem_API_.Models;

public partial class PointTable
{
    public int TeamId { get; set; }

    public int MatchPlayed { get; set; }

    public int Win { get; set; }

    public int Loss { get; set; }

    public int PointTableId { get; set; }

    public int Points { get; set; }

    public int UserId { get; set; }

    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    [JsonIgnore]
    public virtual Team? Team { get; set; } = null!;
    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
