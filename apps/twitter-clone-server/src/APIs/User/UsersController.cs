using Microsoft.AspNetCore.Mvc;

namespace TwitterClone.APIs;

[ApiController()]
public class UsersController : UsersControllerBase
{
    public UsersController(IUsersService service)
        : base(service) { }
}
