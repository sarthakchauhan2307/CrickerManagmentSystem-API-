using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CrickerManagmentSystem_API_.Models
{
    public partial class TeamWisePlayer
    {
        public int TeamWisePlayerId { get; set; }

        public int TeamId { get; set; }

        public int PlayerId { get; set; }

        public int UserId { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual Player? Player { get; set; } = null!;

        [JsonIgnore]
        public virtual Team? Team { get; set; } = null!;

        [JsonIgnore]
        public virtual User? User { get; set; } = null!;
    }
}
