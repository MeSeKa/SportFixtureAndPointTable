using System;
using System.Collections.Generic;

namespace SportFixtureAndPointTable.Models;

public partial class Player
{
    public int Id { get; set; }

    public string? PlayerName { get; set; }

    public int? TeamId { get; set; }

    public string? Position { get; set; }

    public virtual Team? Team { get; set; }
}
