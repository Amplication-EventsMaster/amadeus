using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Infrastructure.Models;

[Table("Tweets")]
public class Tweet
{
    [StringLength(1000)]
    public string? Comment { get; set; }

    [StringLength(1000)]
    public string? Content { get; set; }

    [Required()]
    public DateTime CreatedAt { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    public List<Like>? Likes { get; set; } = new List<Like>();

    public List<Retweet>? Retweets { get; set; } = new List<Retweet>();

    [Required()]
    public DateTime UpdatedAt { get; set; }

    public string? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; } = null;
}
