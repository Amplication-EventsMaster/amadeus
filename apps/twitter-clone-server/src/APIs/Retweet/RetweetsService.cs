using TwitterClone.Infrastructure;

namespace TwitterClone.APIs;

public class RetweetsService : RetweetsServiceBase
{
    public RetweetsService(TwitterCloneDbContext context)
        : base(context) { }
}
