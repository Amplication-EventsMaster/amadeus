using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Infrastructure.Models;

[Table("Retweets")]
public class Retweet
{
    [Required()]
    public DateTime CreatedAt { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    public string? TweetId { get; set; }

    [ForeignKey(nameof(TweetId))]
    public Tweet? Tweet { get; set; } = null;

    [Required()]
    public DateTime UpdatedAt { get; set; }

    public string? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; } = null;
}
