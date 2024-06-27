using TwitterClone.Infrastructure;

namespace TwitterClone.APIs;

public class UsersService : UsersServiceBase
{
    public UsersService(TwitterCloneDbContext context)
        : base(context) { }
}
