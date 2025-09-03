using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrickerManagmentSystem_API_.Models
{
    public partial class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Mobile { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public bool? IsAdmin { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<MatchScore> MatchScores { get; set; } = new List<MatchScore>();

        [JsonIgnore]
        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

        [JsonIgnore]
        public virtual ICollection<Player> Players { get; set; } = new List<Player>();

        [JsonIgnore]
        public virtual ICollection<PointTable> PointTables { get; set; } = new List<PointTable>();

        [JsonIgnore]
        public virtual ICollection<TeamWisePlayer> TeamWisePlayers { get; set; } = new List<TeamWisePlayer>();

        [JsonIgnore]
        public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
