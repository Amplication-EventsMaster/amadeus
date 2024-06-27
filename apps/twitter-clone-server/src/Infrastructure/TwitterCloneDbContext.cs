using Microsoft.EntityFrameworkCore;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.Infrastructure;

public class TwitterCloneDbContext : DbContext
{
    public TwitterCloneDbContext(DbContextOptions<TwitterCloneDbContext> options)
        : base(options) { }

    public DbSet<Follower> Followers { get; set; }

    public DbSet<Tweet> Tweets { get; set; }

    public DbSet<Retweet> Retweets { get; set; }

    public DbSet<Like> Likes { get; set; }

    public DbSet<User> Users { get; set; }
}
