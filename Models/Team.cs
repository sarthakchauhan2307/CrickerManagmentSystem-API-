using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrickerManagmentSystem_API_.Models
{
    public partial class Team
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; } = null!;

        public string TeamDescription { get; set; } = null!;

        public int UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<MatchScore> MatchScores { get; set; } = new List<MatchScore>();

        [JsonIgnore]
        public virtual ICollection<Match> MatchTeamAs { get; set; } = new List<Match>();

        [JsonIgnore]
        public virtual ICollection<Match> MatchTeamBs { get; set; } = new List<Match>();

        [JsonIgnore]
        public virtual ICollection<Match> MatchWinnerTeams { get; set; } = new List<Match>();

        [JsonIgnore]
        public virtual ICollection<Player> Players { get; set; } = new List<Player>();

        [JsonIgnore]
        public virtual ICollection<PointTable> PointTables { get; set; } = new List<PointTable>();

        [JsonIgnore]
        public virtual ICollection<TeamWisePlayer> TeamWisePlayers { get; set; } = new List<TeamWisePlayer>();

        [JsonIgnore]
        public virtual User? User { get; set; } = null!;
    }
}
