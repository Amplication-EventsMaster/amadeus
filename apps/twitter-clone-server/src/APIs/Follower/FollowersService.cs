using TwitterClone.Infrastructure;

namespace TwitterClone.APIs;

public class FollowersService : FollowersServiceBase
{
    public FollowersService(TwitterCloneDbContext context)
        : base(context) { }
}
