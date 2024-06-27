using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Infrastructure.Models;

[Table("Users")]
public class User
{
    [Required()]
    public DateTime CreatedAt { get; set; }

    public string? Email { get; set; }

    [StringLength(256)]
    public string? FirstName { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    [StringLength(256)]
    public string? LastName { get; set; }

    public List<Like>? Likes { get; set; } = new List<Like>();

    [Required()]
    public string Password { get; set; }

    public List<Retweet>? Retweets { get; set; } = new List<Retweet>();

    [Required()]
    public string Roles { get; set; }

    public List<Tweet>? Tweets { get; set; } = new List<Tweet>();

    [Required()]
    public DateTime UpdatedAt { get; set; }

    [Required()]
    public string Username { get; set; }
}
