using System;
using System.Collections.Generic;

namespace SportFixtureAndPointTable.Models;

public partial class Fixture
{
    public int Id { get; set; }

    public int? HomeTeamId { get; set; }

    public int? AwayTeamId { get; set; }

    public DateOnly? MatchDate { get; set; }

    public virtual Team? AwayTeam { get; set; }

    public virtual Team? HomeTeam { get; set; }

    public virtual MatchResult? MatchResult { get; set; } 
}
