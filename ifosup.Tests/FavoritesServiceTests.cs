using ifosup.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ifosup.Tests;

public sealed class FavoritesServiceTests
{
    [Fact]
    public async Task AddAsync_CreatesFavorite_ForUser()
    {
        using var factory = new TestDbFactory();
        await using var db = factory.CreateDbContext();

        var service = new FavoritesService(db);

        var ok = await service.AddAsync(
            userId: "user-a",
            externalId: "openmeteo:brussels:hourly:2026-02-16T12:00",
            customTitle: "Brussels 12:00",
            note: "5.2 °C"
        );

        Assert.True(ok);

        var all = await db.FavoriteItems.ToListAsync();
        Assert.Single(all);
        Assert.Equal("user-a", all[0].UserId);
        Assert.Equal("Brussels 12:00", all[0].CustomTitle);
    }

    [Fact]
    public async Task AddAsync_DuplicateExternalId_ForSameUser_ReturnsFalse()
    {
        using var factory = new TestDbFactory();
        await using var db = factory.CreateDbContext();

        var service = new FavoritesService(db);

        var id = "openmeteo:brussels:hourly:2026-02-16T12:00";

        var first = await service.AddAsync("user-a", id, "t1", "n1");
        var second = await service.AddAsync("user-a", id, "t2", "n2");

        Assert.True(first);
        Assert.False(second);

        var count = await db.FavoriteItems.CountAsync();
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task GetForUserAsync_ReturnsOnlyUsersFavorites()
    {
        using var factory = new TestDbFactory();
        await using var db = factory.CreateDbContext();

        var service = new FavoritesService(db);

        await service.AddAsync("user-a", "id-1", "a1", "n1");
        await service.AddAsync("user-a", "id-2", "a2", "n2");
        await service.AddAsync("user-b", "id-3", "b3", "n3");

        var aFavs = await service.GetForUserAsync("user-a");
        var bFavs = await service.GetForUserAsync("user-b");

        Assert.Equal(2, aFavs.Count);
        Assert.Single(bFavs);

        Assert.All(aFavs, f => Assert.Equal("user-a", f.UserId));
        Assert.All(bFavs, f => Assert.Equal("user-b", f.UserId));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesOnlyIfOwner()
    {
        using var factory = new TestDbFactory();
        await using var db = factory.CreateDbContext();

        var service = new FavoritesService(db);

        await service.AddAsync("user-a", "id-1", "old", "old-note");

        var fav = await db.FavoriteItems.SingleAsync();

        var okOwner = await service.UpdateAsync("user-a", fav.Id, "new", "new-note");
        Assert.True(okOwner);

        var reloaded = await db.FavoriteItems.SingleAsync();
        Assert.Equal("new", reloaded.CustomTitle);
        Assert.Equal("new-note", reloaded.Note);

        var okNotOwner = await service.UpdateAsync("user-b", fav.Id, "hacked", "hacked");
        Assert.False(okNotOwner);

        var reloaded2 = await db.FavoriteItems.SingleAsync();
        Assert.Equal("new", reloaded2.CustomTitle);
        Assert.Equal("new-note", reloaded2.Note);
    }

    [Fact]
    public async Task RemoveAsync_DeletesOnlyIfOwner()
    {
        using var factory = new TestDbFactory();
        await using var db = factory.CreateDbContext();

        var service = new FavoritesService(db);

        await service.AddAsync("user-a", "id-1", "t", "n");

        var fav = await db.FavoriteItems.SingleAsync();

        var notOwner = await service.RemoveAsync("user-b", fav.Id);
        Assert.False(notOwner);

        Assert.Equal(1, await db.FavoriteItems.CountAsync());

        var owner = await service.RemoveAsync("user-a", fav.Id);
        Assert.True(owner);

        Assert.Equal(0, await db.FavoriteItems.CountAsync());
    }
}
