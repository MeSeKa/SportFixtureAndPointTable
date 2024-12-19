using System;
using System.Collections.Generic;

namespace SportFixtureAndPointTable.Models;

public partial class MatchResult
{
    public int Id { get; set; }

    public int? FixtureId { get; set; }

    public int? HomeScore { get; set; }

    public int? AwayScore { get; set; }

    public virtual Fixture? Fixture { get; set; }
}
