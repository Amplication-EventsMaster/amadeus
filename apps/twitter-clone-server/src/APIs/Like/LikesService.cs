using TwitterClone.Infrastructure;

namespace TwitterClone.APIs;

public class LikesService : LikesServiceBase
{
    public LikesService(TwitterCloneDbContext context)
        : base(context) { }
}
