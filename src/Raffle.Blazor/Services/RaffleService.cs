using Microsoft.EntityFrameworkCore;

namespace Raffle.Blazor.Services;

internal class RaffleService
{
    internal bool Authenticated { get; set; } = false;
    internal RaffleDto? RaffleDto { get; set; }
    internal RaffleUserDto? CurrentUser { get; set; }

    private static Random _random = new Random();
    private readonly KcGameOnContext _dbContext;
    private readonly string? _password;

    public RaffleService(KcGameOnContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _password = configuration.GetConnectionString("Password");
    }

    internal async Task InitAsync(int eventId)
    {
        var ticketsThatCanWin = new[] { "VIP", "BYOC", "VOL" };
        RaffleDto = new(await _dbContext
            .Waivers
            .Where(x =>
                x.EventId == eventId &&
                ticketsThatCanWin.Contains(x.Tt))
            .Select(x => new RaffleUserDto(x.Wondoor, x.Username, x.Tt!, x.TableId, x.EventId, x.WonDateTime))
            .ToListAsync());
    }

    internal void Authenticate(string password)
    {
        if (string.IsNullOrEmpty(_password) || _password != password)
            return;

        Authenticated = true;
    }

    internal void GetWinner()
    {
        if (RaffleDto is null)
            return;

        var elegibleUsers = RaffleDto
            .UserDtos
            .Where(x => x.Status == "0");

        if (!elegibleUsers.Any())
            return;

        var index = _random.Next(0, elegibleUsers.Count());
        CurrentUser = elegibleUsers.ToList()[index];
    }

    internal async Task Resolve(string status)
    {
        var goodStatuses = new[] { "1", "2", };
        if (!goodStatuses.Contains(status) || CurrentUser is null)
            return;

        var waiver = await _dbContext
            .Waivers
            .FindAsync(CurrentUser.TicketId);

        if (waiver is null)
            return;

        waiver.Wondoor = status;
        waiver.WonDateTime = DateTime.Now;
        await _dbContext.SaveChangesAsync();
        await InitAsync(CurrentUser.EventId);
        CurrentUser = null;
    }
}

internal record RaffleDto(
    List<RaffleUserDto> UserDtos);

internal record RaffleUserDto(
    string Status,
    string Username,
    string TicketType,
    int TicketId,
    int EventId,
    DateTime? WonAt);
