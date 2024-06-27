using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Infrastructure.Models;

[Table("Followers")]
public class Follower
{
    [Required()]
    public DateTime CreatedAt { get; set; }

    public string? FollowerId { get; set; }

    [ForeignKey(nameof(FollowerId))]
    public Follower? Follower { get; set; } = null;

    public List<Follower>? Followers { get; set; } = new List<Follower>();

    [StringLength(1000)]
    public string? Following { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
