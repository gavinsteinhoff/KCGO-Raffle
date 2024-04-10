using System;
using System.Collections.Generic;

namespace Raffle.Blazor;

/// <summary>
/// to track who and when the waivers were signed.  This should be done each year.
/// </summary>
public partial class Waiver
{
    public int TableId { get; set; }

    public string Username { get; set; } = null!;

    public string SignedName { get; set; } = null!;

    public string? Tt { get; set; }

    public DateTime Datetime { get; set; }

    public bool? IsMinor { get; set; }

    public int EventId { get; set; }

    /// <summary>
    /// 0 = available to win, 1= won, 2= epic fail
    /// </summary>
    public string Wondoor { get; set; } = null!;

    public string Wonloyalty { get; set; } = null!;

    /// <summary>
    /// 0 = nonactive, player dropped from tournament.
    /// </summary>
    public int Active { get; set; }

    public DateTime? WonDateTime { get; set; }
}
