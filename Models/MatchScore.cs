using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrickerManagmentSystem_API_.Models;

public partial class MatchScore
{
    public int ScoreId { get; set; }

    public int MatchId { get; set; }

    public int TeamId { get; set; }

    public int Runs { get; set; }

    public int Wicket { get; set; }

    public int Over { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public int UserId { get; set; }
    [JsonIgnore]

    public virtual Match? Match { get; set; } = null!;
    [JsonIgnore]

    public virtual Team? Team { get; set; } = null!;
    [JsonIgnore]

    public virtual User? User { get; set; } = null!;
}
