using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportFixtureAndPointTable.Models;

public partial class Team
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int TeamId { get; set; }

    public string? TeamName { get; set; }

    public string? City { get; set; }

    public virtual ICollection<Fixture> FixtureAwayTeams { get; set; } = new List<Fixture>();

    public virtual ICollection<Fixture> FixtureHomeTeams { get; set; } = new List<Fixture>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
