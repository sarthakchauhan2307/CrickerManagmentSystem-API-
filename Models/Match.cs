using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrickerManagmentSystem_API_.Models;

public partial class Match
{
    public int MatchId { get; set; }

    public int TeamAid { get; set; }

    public int TeamBid { get; set; }

    public DateOnly MatchDate { get; set; }

    public DateTime StartTime { get; set; }

    public string Venue { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int? WinnerTeamId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public int UserId { get; set; }
    [JsonIgnore]
    public virtual ICollection<MatchScore> MatchScores { get; set; } = new List<MatchScore>();
    [JsonIgnore]

    public virtual Team? TeamA { get; set; } = null!;
    [JsonIgnore]

    public virtual Team? TeamB { get; set; } = null!;
    [JsonIgnore]

    public virtual User? User { get; set; } = null!;
    [JsonIgnore]

    public virtual Team? WinnerTeam { get; set; }
}
