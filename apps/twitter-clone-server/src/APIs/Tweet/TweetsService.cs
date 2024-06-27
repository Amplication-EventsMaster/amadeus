using TwitterClone.Infrastructure;

namespace TwitterClone.APIs;

public class TweetsService : TweetsServiceBase
{
    public TweetsService(TwitterCloneDbContext context)
        : base(context) { }
}
