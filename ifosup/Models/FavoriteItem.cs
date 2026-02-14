using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace ifosup.Models;

[Index(nameof(UserId), nameof(ExternalId), IsUnique = true)]
public sealed class FavoriteItem
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string ExternalId { get; set; } = default!;

    [MaxLength(120)]
    public string? CustomTitle { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public string? SnapshotJson { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
