using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.APIs;

[ApiController()]
public class FollowersController : FollowersControllerBase
{
    public FollowersController(IFollowersService service)
        : base(service) { }
}
