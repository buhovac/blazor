using System.Text.Json;
using ifosup.Data;
using ifosup.Models;
using Microsoft.EntityFrameworkCore;

namespace ifosup.Services;

public sealed class FavoritesService
{
    private readonly ApplicationDbContext _db;

    public FavoritesService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<FavoriteItem>> GetForUserAsync(string userId, CancellationToken ct = default)
    {
        return await _db.FavoriteItems
            .AsNoTracking()
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAtUtc)
            .ToListAsync(ct);
    }

    public async Task<bool> AddAsync(
        string userId,
        string externalId,
        string? customTitle,
        string? note,
        object? snapshotObject = null,
        CancellationToken ct = default)
    {
        var exists = await _db.FavoriteItems
            .AnyAsync(f => f.UserId == userId && f.ExternalId == externalId, ct);

        if (exists) return false;

        var fav = new FavoriteItem
        {
            UserId = userId,
            ExternalId = externalId,
            CustomTitle = customTitle,
            Note = note,
            SnapshotJson = snapshotObject is null ? null : JsonSerializer.Serialize(snapshotObject),
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.FavoriteItems.Add(fav);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> UpdateAsync(
        string userId,
        int favoriteId,
        string? customTitle,
        string? note,
        CancellationToken ct = default)
    {
        var fav = await _db.FavoriteItems
            .FirstOrDefaultAsync(f => f.Id == favoriteId && f.UserId == userId, ct);

        if (fav is null) return false;

        fav.CustomTitle = customTitle;
        fav.Note = note;

        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> RemoveAsync(string userId, int favoriteId, CancellationToken ct = default)
    {
        var fav = await _db.FavoriteItems
            .FirstOrDefaultAsync(f => f.Id == favoriteId && f.UserId == userId, ct);

        if (fav is null) return false;

        _db.FavoriteItems.Remove(fav);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
