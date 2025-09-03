using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CrickerManagmentSystem_API_.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public int TeamId { get; set; }

    public string PlayerName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Nationality { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public int UserId { get; set; }
    [NotMapped]
    public IFormFile? File { get; set; }

    public string? Image { get; set; }
    [JsonIgnore]

    public virtual Team? Team { get; set; } = null!;
    [JsonIgnore]

    public virtual ICollection<TeamWisePlayer> TeamWisePlayers { get; set; } = new List<TeamWisePlayer>();
    [JsonIgnore]

    public virtual User? User { get; set; } = null!;
}
